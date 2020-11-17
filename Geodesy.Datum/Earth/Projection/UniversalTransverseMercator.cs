using System;
using Newtonsoft.Json;
using Geodesy.Datum.Coordinate;
using System.Collections.Generic;

namespace Geodesy.Datum.Earth.Projection
{
    /// <summary>
    /// The UTM (Universal Transverse Mercator) class contains methods to parse and format between UTM coordinate strings
    /// and their geodetic (longitude and latitude) equivalents. A UTM object is defined only in terms of the EarthEllipsoid
    /// data model against which projections are made. 
    /// </summary>
    /// <remarks>
    /// reference: https://github.com/OpenSextant/geodesy/blob/master/src/main/java/org/opensextant/geodesy/UTM.java
    /// </remarks>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class UniversalTransverseMercator : TransverseMercator
    {
        /// <summary>
        /// EPSG Identifier
        /// </summary>
        public static readonly Identifier UTM = new Identifier("EPSG", "9824", "Transverse Mercator Zoned Grid System", "UTM");

        #region Static Class Constants
        private const double Max_North_Lat = +84.5;
        private const double Min_South_Lat = -80.5;
        private const double False_Easting = 500000.0;
        private const double False_Northing = 10000000.0;

        private const double Min_Easting = 100000.0;
        private const double Max_Easting = 900000.0;
        private const double Min_Northing = 0.0;
        private const double Max_Northing = 10000000.0;
        private const double Scale = 0.9996;
        private const double Equator = 0.0;

        private const double Rounding_POS = 1E+6;
        private const double Rouding_NEG = 1E-6;

        private static readonly Longitude V31_Centrel_Meridian = new Longitude(1.5);
        private static readonly Longitude X31_Centrel_Meridian = new Longitude(4.5);
        private static readonly Longitude V32_Centrel_Meridian = new Longitude(7.5);
        private static readonly Longitude X37_Centrel_Meridian = new Longitude(37.5);

        private static readonly int V31_Max_Northing = (int)Math.Round(
                        GeodeticConversion(new Latitude(63, 59, 59.99), new Longitude(0.0)).Northing);
        private static readonly int V32_Max_Northing = (int)Math.Round(
                        GeodeticConversion(new Latitude(63, 59, 59.99), new Longitude(3.0)).Northing);
        private static readonly int X31_X37_Max_Northing = (int)Math.Round(
                        GeodeticConversion(new Latitude(83, 59, 59.99), new Longitude(0.0)).Northing);
        private static readonly int X33_X35_Max_Northing = (int)Math.Round(
                        GeodeticConversion(new Latitude(83, 59, 59.99), new Longitude(21.0)).Northing);
        #endregion

        /// <summary>
        /// UTM Longitudinal Zone number (1 to 60)
        /// </summary>
        protected int _lngZone;

        /// <summary>
        /// Hemisphere char ('N' for Northern, 'S' for Southern)
        /// </summary>
        protected char _hemisphere;

        /// <summary>
        /// UTM Lat Band char('C' to 'X', not including 'I' or 'O')
        /// </summary>
        protected char _latBand;

        #region Contructors
        /// <summary>
        /// Create a UTM projection object.
        /// </summary>
        public UniversalTransverseMercator()
            : this(Settings.Ellipsoid)
        { }

        /// <summary>
        /// Create a UTM projection object.
        /// </summary>
        /// <param name="ellipsoid">Ellipsoid data model for earth</param>
        public UniversalTransverseMercator(Ellipsoid ellipsoid)
            : base(new Dictionary<ProjectionParameter, double> {
                { ProjectionParameter.Semi_Major, ellipsoid.a },
                { ProjectionParameter.Inverse_Flattening, ellipsoid.ivf },
                { ProjectionParameter.Latitude_Of_Origin, Equator },
                { ProjectionParameter.Scale_Factor, Scale },
                { ProjectionParameter.False_Easting, False_Easting },
                { ProjectionParameter.False_Northing, False_Northing} })
        {
            Identifier = UTM;
            Surface = ProjectionSurface.Cylindrical;
            Property = ProjectionProperty.Conformal;
            Orientation = ProjectionOrientation.Transverse;
        }

        /// <summary>
        /// Convert UTM parameters to equivalent geodetic Coordinate (lng-lat) position, compute lat band.
        /// </summary>
        /// <param name="ellipsoid">Ellipsoid data model for earth</param>
        /// <param name="lngZone">UTM longitudinal zone (1 to 60)</param>
        /// <param name="hemisphere">character 'N' for Northern or 'S' for Southern hemisphere</param>
        public UniversalTransverseMercator(Ellipsoid ellipsoid, int lngZone, char hemisphere)
            : this(ellipsoid)
        {
            _lngZone = lngZone;
            _hemisphere = hemisphere;
        }

        /// <summary>
        /// Create a UTM projection instant
        /// </summary>
        /// <param name="parameters">projection parameters</param>
        private UniversalTransverseMercator(Dictionary<ProjectionParameter, double> parameters)
            : base(parameters)
        { }
        #endregion

        #region properties
        /// <summary>
        /// Get the UTM longitudinal zone (1 to 60) for this UTM object.
        /// </summary>
        public int LngZone => _lngZone;

        /// <summary>
        /// Get the UTM latitudinal band character ('C' to 'X', not including 'I' or 'O') for this UTM object.
        /// </summary>
        public char LatBand => _latBand;

        /// <summary>
        /// Get the UTM hemisphere character ('N' for Northern, 'S' for Southern) for this UTM object.
        /// </summary>
        public char Hemisphere => _hemisphere;
        #endregion

        #region public methods
        /// <summary>
        /// This method converts from projected coordinates (lng-lat) to UTM parameters.
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lng">longitude</param>
        /// <param name="northing">northing</param>
        /// <param name="easting">easting</param>
        public override void Forward(Latitude lat, Longitude lng, out double northing, out double easting)
        {
            char latBand = GetLatBand(lat);
            int lonZone = GetLngZone(lng, latBand);

            // Set the central meridian
            Longitude cm = GetCentralMeridian(lonZone, latBand);
            SetParameter(ProjectionParameter.Central_Meridian, cm.Degrees);
            base.Forward(lat, lng, out double north, out double east);

            easting = east + FalseEasting;
            // Correct northing for latitudes barely south of the equator
            if ((north < 0.0) && (_hemisphere == 'N')) north = 0.0;
            northing = (north < 0.0) ? north + FalseNorthing : north;
        }

        /// <summary>
        /// This method converts UTM projection (zone, hemisphere, easting and northing) coordinates to geodetic
        /// (longitude and latitude) coordinates, based on the current EarthEllipsoid model.
        /// </summary>
        /// <param name="northing">northing</param>
        /// <param name="easting">easting</param>
        /// <param name="lat">latitude</param>
        /// <param name="lng">longitude</param>
        public override void Reverse(double northing, double easting, out Latitude lat, out Longitude lng)
        {
            // Validate input parameters
            ValidateLonZone(_lngZone);
            ValidateHemisphere(_hemisphere);
            ValidateEasting(easting);
            ValidateNorthing(northing);

            // set nominal central meridian & adjust false values to regain signed offsets
            double east = easting - FalseEasting;
            double north = (_hemisphere == 'S') ? northing - FalseNorthing : northing;

            // Un-project to geodetic coordinates, assume no special zone override necessary
            double cm = 6.0 * _lngZone + ((_lngZone >= 31) ? -183.0 : 177.0);
            SetParameter(ProjectionParameter.Central_Meridian, cm);
            base.Reverse(north, east, out lat, out lng);

            // Determine lat band and validate cell combo (lng zones 32, 34, & 36 are illegal in band 'X')
            _latBand = GetLatBand(lat);
            // if (validate) 
            ValidateZoneAndBand(_lngZone, _latBand);

            // Determine if special zone override makes it necessary to un-project again with adjustments
            if (_hemisphere == 'N')
            {
                // Use the un-projected northing latitude band to determine special case lng zone CM values.
                // Un-project again if central meridian has changed due to special zones
                double ocm = GetCentralMeridian(_lngZone, _latBand).Degrees;
                if (ocm != cm)
                {
                    SetParameter(ProjectionParameter.Central_Meridian, ocm);
                    base.Reverse(north, east, out lat, out lng);
                }
            }
        }

        /// <summary>
        /// This method returns the minimum northing value (in meters) for the specified latitude band.
        /// </summary>
        /// <param name="latBand">UTM latitude band character ('C' to 'X", not including 'I' or 'O')</param>
        /// <returns>minimum northing value for the specified latitude band</returns>
        public int GetMinNorthing(char latBand)
        {
            if (latBand < 'C' || latBand > 'X' || latBand == 'I' || latBand == 'O') return -1;

            Longitude centLon = Longitude.FromDegrees(-3.0);
            Longitude edgeLon = Longitude.FromDegrees(-6.0);

            // min Northings are inclusive (northing must be greater than or equal to this min)
            // min Northings for Northern Hemisphere are along a central meridian, but for
            // the Southern Hemisphere, they occur at the edges of a lonZone (+/- 3 deg from CM)
            Longitude lng = (latBand < 'N') ? edgeLon : centLon;
            Latitude minLat = GetMinLatitude(latBand);

            double minNorthing = GeodeticConversion(minLat, lng).Northing;
            return (int)Math.Round(minNorthing);
        }

        /// <summary>
        /// This method returns the maximum northing value (in meters) for the specified latitude band.
        /// </summary>
        /// <param name="lngZone">UTM Longitudinal Zone (1 to 60)</param>
        /// <param name="latBand">UTM latitude band character ('C' to 'X", not including 'I' or 'O')</param>
        /// <returns>maximum northing value for the specified latitude band</returns>
        public int GetMaxNorthing(int lngZone, char latBand)
        {
            double maxNorthing;
            Longitude lng;
            Longitude centLng = new Longitude(-3.0);
            Longitude edgeLng = new Longitude(-6.0);

            if (latBand == 'M')
            {
                maxNorthing = False_Northing;
            }
            else
            {
                lng = (latBand < 'N') ? centLng : edgeLng;
                Latitude maxLat = GetMaxLatitude(latBand);

                maxNorthing = GeodeticConversion(maxLat, lng).Northing;
            }

            // See if we need to override for special exception cells
            if (latBand == 'V')
            {
                if (lngZone == 31) maxNorthing = V31_Max_Northing;
                else if (lngZone == 32) maxNorthing = V32_Max_Northing;
            }
            else if (latBand == 'X')
            {
                if ((lngZone == 31) || (lngZone == 37)) maxNorthing = X31_X37_Max_Northing;
                else if ((lngZone == 33) || (lngZone == 35)) maxNorthing = X33_X35_Max_Northing;
            }

            return (int)Math.Round(maxNorthing);
        }
        #endregion

        #region Static Class Methods
        /// <summary>
        /// This method determines the UTM longitudinal zone number for a given lng (in decimal degrees).
        /// </summary>
        /// <param name="lng">degrees of longitude to map to UTM longitudinal zone number</param>
        /// <param name="latBand">UTM latitude band character ('C' to 'X", not including 'I' or 'O')</param>
        /// <returns>int for longitudinal zone number (1 to 60)</returns>
        public static int GetLngZone(Longitude lng, char latBand)
        {
            double lngDeg = lng.Degrees;

            // Round value to correct for accumulation of numerical error in UTM projection
            lngDeg = Math.Floor(Math.Round(lngDeg * Rounding_POS) * Rouding_NEG);

            // Normalize the longitude in degrees if necessary
            while (lngDeg < -180.0) lngDeg += 360.0;
            while (lngDeg >= +180.0) lngDeg -= 360.0;

            // Compute nominal zone value for most cases
            int lngZone = 1 + (int)((lngDeg + 180.0) / 6.0);

            // Five zones have special boundaries to be checked, override nominal if necessary
            if (latBand == 'V')
            {
                if ((3.0 <= lngDeg) && (lngDeg < 12.0)) lngZone = 32;
            }
            else if (latBand == 'X')
            {
                if ((0.0 <= lngDeg) && (lngDeg < 9.0)) lngZone = 31;
                else if ((9.0 <= lngDeg) && (lngDeg < 21.0)) lngZone = 33;
                else if ((21.0 <= lngDeg) && (lngDeg < 33.0)) lngZone = 35;
                else if ((33.0 <= lngDeg) && (lngDeg < 42.0)) lngZone = 37;
            }

            return lngZone;
        }

        /// <summary>
        /// This method determines the minimum longitude for a given lng zone and lat band.
        /// </summary>
        /// <param name="lngZone">UTM Longitudinal Zone (1 to 60)</param>
        /// <param name="latBand">UTM latitude band character ('C' to 'X", not including 'I' or 'O')</param>
        /// <returns>minimum longitude for this UTM cell (lng zone and lat band)</returns>
        public static Longitude GetMinLongitude(int lngZone, char latBand)
        {
            double lngDeg = (lngZone * 6.0) - 186.0;

            // Four zones have min lng overrides
            if ((latBand == 'V') && (lngZone == 32))
            {
                lngDeg = 3.0;
            }
            else if (latBand == 'X')
            {
                if (lngZone == 33) lngDeg = 9.0;
                else if (lngZone == 35) lngDeg = 21.0;
                else if (lngZone == 37) lngDeg = 33.0;
            }

            return new Longitude(lngDeg);
        }

        /// <summary>
        /// This accessor method gets the appropriate central meridian for the given UTM lng zone
        /// and lat band.
        /// </summary>
        /// <param name="lngZone">UTM Longitudinal Zone (1 to 60)</param>
        /// <param name="latBand">UTM latitude band character ('C' to 'X", not including 'I' or 'O')</param>
        /// <returns>Longitude of appropriate central meridian for the given UTM cell (zone and band)</returns>
        public static Longitude GetCentralMeridian(int lngZone, char latBand)
        {
            Longitude cm = new Longitude(6.0 * lngZone + ((lngZone >= 31) ? -183.0 : 177.0));

            if (latBand == 'V')
            {
                if (lngZone == 31) cm = V31_Centrel_Meridian;
                else if (lngZone == 32) cm = V32_Centrel_Meridian;
            }
            else if (latBand == 'X')
            {
                if (lngZone == 31) cm = X31_Centrel_Meridian;
                else if (lngZone == 37) cm = X37_Centrel_Meridian;
            }

            return cm;
        }

        /// <summary>
        /// This method determines the maximum longitude for a given lng zone & lat band.
        /// </summary>
        /// <param name="lngZone">UTM Longitudinal Zone (1 to 60)</param>
        /// <param name="latBand">UTM latitude band character ('C' to 'X", not including 'I' or 'O')</param>
        /// <returns>longitude for this UTM cell (lng zone and lat band)</returns>
        public static Longitude GetMaxLongitude(int lngZone, char latBand)
        {
            double lngDeg = (lngZone * 6.0) - 180.0;

            // Four zones have max lng overrides
            if ((latBand == 'V') && (lngZone == 31))
            {
                lngDeg = 3.0;
            }
            else if (latBand == 'X')
            {
                if (lngZone == 31) lngDeg = 9.0;
                else if (lngZone == 33) lngDeg = 21.0;
                else if (lngZone == 35) lngDeg = 33.0;
            }

            return new Longitude(lngDeg);
        }

        /// <summary>
        /// This method determines the UTM latitudinal band char for a given lat (in decimal degrees).
        /// </summary>
        /// <param name="lat">degrees of latitude to map to UTM latitude band character</param>
        /// <returns>character representing latitude band ('C' to 'X', skipping 'I' and 'O')</returns>
        public static char GetLatBand(Latitude lat)
        {
            double latDeg = lat.Degrees;

            // validate that latitude is within proper range (allow half degree overlap with UPS)
            if ((latDeg < Min_South_Lat) || (Max_North_Lat < latDeg))
            {
                throw new GeodeticException("Latitude value '" + latDeg + "' is out of legal range (-80 deg to 84 deg) for UTM");
            }

            // Round value to correct for accumulation of numerical error in UTM projection
            latDeg = Math.Floor(Math.Round(latDeg * Rounding_POS) * Rouding_NEG);

            // Restrict calculated index to 20 8 deg-wide bands between -80 deg and +80 deg
            int i;
            if (latDeg < -80.0)
            {
                i = 0;                          // correct for extra 'C' band range
            }
            else if (80.0 <= latDeg)
            {
                i = 19;                        // correct for extra 'X' band range
            }
            else
            {
                i = (int)(10.0 + (latDeg / 8.0));              // index values for all others
            }

            char latBand = (char)('C' + i);
            if (i > 10)
            {
                latBand = (char)((int)latBand + 2);
            }
            else if (i > 5)
            {
                latBand = (char)((int)latBand + 1);             // adjust for missing 'I' and 'O'
            }

            return latBand;
        }

        /// <summary>
        /// This method returns the inclusive minimum latitude for the given UTM latitude band char 
        /// (extra half degree of allowance not included).
        /// </summary>
        /// <param name="latBand">UTM latitude band character ('C' to 'X", not including 'I' or 'O')</param>
        /// <returns>minimum latitude for the given band</returns>
        public static Latitude GetMinLatitude(char latBand)
        {
            ValidateLatBand(latBand);

            int i = (int)latBand - (int)'C';
            if (latBand > 'N')
            {
                i -= 2;
            }
            else if (latBand > 'H')
            {
                i -= 1;             // adjust for missing 'I' and 'O'
            }

            return new Latitude(8.0 * (i - 10.0));
        }

        /// <summary>
        /// This method returns the exclusive maximum latitude for the given UTM latitude band char 
        /// (extra half degree of allowance not included).
        /// </summary>
        /// <param name="latBand">UTM latitude band character ('C' to 'X", not including 'I' or 'O')</param>
        /// <returns>latitude for the given band</returns>
        public static Latitude GetMaxLatitude(char latBand)
        {
            ValidateLatBand(latBand);

            int i = (int)latBand - (int)'C';

            if (latBand > 'N')
            {
                i -= 2;
            }
            else if (latBand > 'H')
            {
                i -= 1;             // adjust for missing 'I' and 'O'
            }

            double w = (latBand == 'X') ? 12.0 : 8.0;   // Band 'X' has extra 4.0 deg N

            return new Latitude((8.0 * (i - 10.0)) + w);
        }

        /// <summary>
        /// This method determines the hemisphere ('N' or 'S') for the given lat band.
        /// </summary>
        /// <param name="latBand">UTM latitude band character ('C' to 'X", not including 'I' or 'O')</param>
        /// <returns>hemisphere character ('N' for Northern, or 'S' for Southern)</returns>
        public static char GetHemisphere(char latBand)
        {
            ValidateLatBand(latBand);

            return (latBand < 'N') ? 'S' : 'N';
        }

        /// <summary>
        /// This method tests longitudinal zone to see if it is valid (by itself).
        /// </summary>
        /// <param name="lngZone">UTM Longitudinal Zone (1 to 60)</param>
        /// <returns></returns>
        public static bool ValidateLonZone(int lngZone)
        {
            return lngZone > 1 && 60 > lngZone;
        }

        /// <summary>
        /// This method tests latitudinal zone to see if it is valid (by itself).
        /// </summary>
        /// <param name="latBand">UTM Latitudinal Band ('C' to 'X', but not 'I' or 'O')</param>
        /// <returns></returns>
        public static bool ValidateLatBand(char latBand)
        {
            return !((latBand < 'C') || ('X' < latBand) || (latBand == 'I') || (latBand == 'O'));
        }

        /// <summary>
        /// This method tests longitudinal zone and latitudinal band to make sure they are
        /// consistent together.
        /// </summary>
        /// <param name="lngZone">UTM Longitudinal Zone (1 to 60)</param>
        /// <param name="latBand">UTM Latitudinal Band ('C' to 'X', but not 'I' or 'O')</param>
        /// <returns></returns>
        public static bool ValidateZoneAndBand(int lngZone, char latBand)
        {
            if ((latBand == 'X') && ((lngZone == 32) || (lngZone == 34) || (lngZone == 36)))
            {
                return false;
            }
            else
            {
                return ValidateLonZone(lngZone) && ValidateLatBand(latBand);
            }
        }

        /// <summary>
        /// This method tests hemisphere character to see if it is valid (by itself).
        /// </summary>
        /// <param name="hemisphere">UTM Hemisphere character ('N' for North, 'S' for South)</param>
        /// <returns></returns>
        protected static bool ValidateHemisphere(char hemisphere)
        {
            return hemisphere != 'N' || hemisphere != 'S';
        }

        /// <summary>
        /// This method tests easting value to see if it is within allowed positive numerical range.
        /// </summary>
        /// <param name="easting">UTM Easting value in meters</param>
        /// <returns></returns>
        protected static bool ValidateEasting(double easting)
        {
            return easting > Min_Easting && Max_Easting > easting;
        }

        /// <summary>
        /// This method tests northing value to see if it is within allowed positive numerical range.
        /// </summary>
        /// <param name="northing">UTM Northing value in meters</param>
        /// <returns></returns>
        protected static bool ValidateNorthing(double northing)
        {
            return northing > Min_Northing && Max_Northing > northing;
        }

        /// <summary>
        /// Convert the LatLng to Projected NorthEast in GRS80 ellipsoid.
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lng">longitude</param>
        /// <returns>projected coordinate</returns>
        private static ProjectedCoord GeodeticConversion(Latitude lat, Longitude lng)
        {
            Dictionary<ProjectionParameter, double> parameters = new Dictionary<ProjectionParameter, double> {
                { ProjectionParameter.Semi_Major, Settings.Ellipsoid.a },
                { ProjectionParameter.Inverse_Flattening, Settings.Ellipsoid.ivf },
                { ProjectionParameter.Latitude_Of_Origin, Equator },
                { ProjectionParameter.Scale_Factor, Scale },
                { ProjectionParameter.False_Easting, False_Easting },
                { ProjectionParameter.False_Northing, False_Northing }};

            UniversalTransverseMercator utm = new UniversalTransverseMercator(parameters);
            utm.Forward(lat, lng, out double northing, out double easting);
            return new ProjectedCoord(northing, easting);
        }
        #endregion
    }
}