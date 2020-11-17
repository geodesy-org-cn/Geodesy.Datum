using System;
using Newtonsoft.Json;
using Geodesy.Datum.Coordinate;
using System.Collections.Generic;

namespace Geodesy.Datum.Earth.Projection
{
    /// <summary>
    /// The universal polar stereographic (UPS) coordinate system is used in conjunction with 
    /// the universal transverse Mercator (UTM) coordinate system to locate positions on the 
    /// surface of the earth. The UPS coordinate system uses a metric-based cartesian grid laid 
    /// out on a conformally projected surface. UPS covers the Earth's polar regions, specifically 
    /// the areas north of 84°N and south of 80°S, which are not covered by the UTM grids.
    /// </summary>
    /// <remarks>
    /// Universal Polar Stereographic (FOR UTM POLAR REGION). REFERENCE TEC-SR-7 US ARMY 1996
    /// https://github.com/Tronald/CoordinateSharp/blob/master/CoordinateSharp/UTM_MGRS/UPS.cs
    /// </remarks>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class UPS : PolarStereographic
    {
        /// <summary>
        /// The scale factor at each pole is adjusted to 0.994 so that the latitude of true scale is 
        /// 81.11451786859362545° (about 81° 06' 52.3") North and South. The scale factor inside the 
        /// regions at latitudes higher than this parallel is too small, whereas the regions at latitudes 
        /// below this line have scale factors that are too large, reaching 1.0016 at 80° latitude.
        /// </summary>
        private const double Scale = 0.994;
        private const double False_Easting = 2000000;
        private const double False_Northing = 2000000;
        private const double True_Scale_Latitude = 81.11451786859362545;

        private const double Min_Easting = 0;
        private const double Max_Easting = 4000000;
        private const double Min_Northing = 0;
        private const double Max_Northing = 4000000;

        private char _latBand;
        private readonly char _hemisphere;

        /// <summary>
        /// Create a UPS projection object.
        /// </summary>
        /// <param name="ellipsoid">EarthEllipsoid data model for earth</param>
        public UPS()
            : this(Settings.Ellipsoid)
        { }

        /// <summary>
        /// Create a UPS projection object.
        /// </summary>
        /// <param name="ellipsoid">EarthEllipsoid data model for earth</param>
        public UPS(Ellipsoid ellipsoid)
            : base(new Dictionary<ProjectionParameter, double> {
                { ProjectionParameter.Semi_Major, ellipsoid.a },
                { ProjectionParameter.Inverse_Flattening, ellipsoid.ivf },
                { ProjectionParameter.Central_Meridian, 0 },
                { ProjectionParameter.Latitude_Of_Origin, True_Scale_Latitude },
                { ProjectionParameter.Scale_Factor, Scale },
                { ProjectionParameter.False_Easting, False_Easting },
                { ProjectionParameter.False_Northing, False_Northing} })
        {
            Identifier = new Identifier(typeof(UPS));
            _hemisphere = (OriginLatitude.Degrees > 0) ? 'N' : 'S';
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ellipsoid"></param>
        /// <param name="hemisphere"></param>
        public UPS(Ellipsoid ellipsoid, char hemisphere)
            : this(ellipsoid)
        {
            hemisphere = char.ToUpper(hemisphere);
            if (hemisphere != 'N' && hemisphere != 'S')
            {
                throw new GeodeticException("Hemisphere is invalid. It must be 'N' or 'S'.");
            }

            _hemisphere = hemisphere;
            double lat0 = Math.Abs(OriginLatitude.Degrees) * (_hemisphere == 'N' ? 1 : -1);
            SetParameter(ProjectionParameter.Latitude_Of_Origin, lat0);
        }

        /// <summary>
        /// Get the hemisphere character ('N' for Northern, 'S' for Southern)
        /// </summary>
        public char Hemisphere => _hemisphere;

        /// <summary>
        /// Get the latitudinal band character ('Y', 'Z', or 'A', 'B')
        /// </summary>
        public char LatBand => _latBand;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ups"></param>
        /// <param name="east"></param>
        /// <param name="north"></param>
        /// <returns></returns>
        private bool Decode(string ups, out double east, out double north)
        {
            east = 0;
            north = 0;

            ups = ups.ToUpper().Replace(" ", "").Replace("M", "").Replace("E", "").Replace("N", "");

            try
            {
                _latBand = ups.ToCharArray()[0];

                int len = (ups.Length - 1) / 2;
                east = Convert.ToDouble(ups.Substring(1, len));
                north = Convert.ToDouble(ups.Substring(len + 1));
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// converts geodetic coordinates to Albers equal-area conic projection coordinates.
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lng">longitude</param>
        /// <param name="northing">northing</param>
        /// <param name="easting">easting</param>
        public override void Forward(Latitude lat, Longitude lng, out double northing, out double easting)
        {
            //RESET FOR CALCS
            if (lng.Degrees == -180) lng.Degrees = 180;

            double es = SquaredEccentricity;
            double e = Math.Sqrt(es);

            //LAT LONG TO UPS
            //TEC-SR-7 US ARMY Corps of Engineers CONVERSIONS TEXT PAGE 99 STEP 0
            double c = SemiMajor / Math.Sqrt(1 - es);
            double K = 2 * c * Math.Pow((1 - e) / (1 + e), e / 2);

            double rLat = Math.Abs(lat.Radians);
            double KTan = K * Math.Tan(Math.PI / 4 - rLat / 2);
            double r = KTan * Math.Pow((1 + e * Math.Sin(rLat)) / (1 - e * Math.Sin(rLat)), e / 2);

            double lambda = lng.Radians - CenteralMaridian.Radians;
            easting = FalseEasting + ScaleFactor * r * Math.Sin(lambda);

            if (lat.Degrees > 0) { northing = FalseNorthing - ScaleFactor * r * Math.Cos(lambda); }
            else { northing = FalseNorthing + ScaleFactor * r * Math.Cos(lambda); }

            double dLat = lat.Degrees;
            double dLng = lng.Degrees;

            if (dLat >= 0)
            {
                if (dLng >= 0 || dLng == -180 || dLat == 90) { _latBand = 'Z'; }
                else { _latBand = 'Y'; }
            }
            else
            {
                if (dLng >= 0 || dLng == -180 || dLat == -90) { _latBand = 'B'; }
                else { _latBand = 'A'; }
            }
        }

        /// <summary>
        /// converts Lambert equal-area conic projection coordinates to geodetic coordinates.
        /// </summary>
        /// <param name="northing">northing</param>
        /// <param name="easting">easting</param>
        /// <param name="lat">latitude</param>
        /// <param name="lng">longitude</param>
        public override void Reverse(double northing, double easting, out Latitude lat, out Longitude lng)
        {
            //STEP 1          
            double east = (easting - FalseEasting) / ScaleFactor;
            double north = (northing - FalseNorthing) / ScaleFactor;

            double rLng = Math.PI + Math.Atan(east / north) * (_hemisphere == 'S' ? 1 : -1);
            lng = (double.IsNaN(rLng) ? new Longitude(0) : Longitude.FromRadians(rLng));

            // SET TO 0 or it will equate to 180
            if (north >= 0 && east == 0 && _hemisphere == 'S')
            {
                lng = new Longitude(0);
            }

            // SET TO 0 or it will equate to 180
            if (north < 0 && east == 0 && _hemisphere == 'N')
            {
                lng = new Longitude(0);
            }

            lng += CenteralMaridian;
            lng.Normalize();

            //STEP 2
            if (north == 0) north = 1;
            double temp = Math.PI + Math.Atan(east / north) * (_hemisphere == 'S' ? 1 : -1);

            //STEP 3
            double a = SemiMajor;
            double es = SquaredEccentricity;
            double e = Math.Sqrt(es);
            double b = a * Math.Sqrt(1 - es);
            double K = (2 * Math.Pow(a, 2) / b) * Math.Pow(((1 - e) / (1 + e)), (e / 2));

            //STEP 4
            double kCos = K * Math.Abs(Math.Cos(temp));
            double q = Math.Log(Math.Abs(north) / kCos) / Math.Log(Math.E) * -1;

            //STEP 5
            double rLat = 2 * Math.Atan(Math.Pow(Math.E, q)) - Math.PI / 2;
            double rB = Math.PI / 2;

            while (Math.Abs(rLat - rB) > Settings.Epsilon5)
            {
                if (double.IsInfinity(rLat)) break;
                rB = rLat;

                //STEP 6
                double sLat = Math.Sin(rLat);
                double bracket = (1 + sLat) / (1 - sLat) * Math.Pow((1 - e * sLat) / (1 + e * sLat), e);
                double fLat = -q + 1 / 2.0 * Math.Log(bracket);
                double fLat2 = (1 - Math.Pow(e, 2)) / ((1 - Math.Pow(e, 2) * Math.Pow(sLat, 2)) * Math.Cos(rLat));

                //STEP 7
                rLat -= fLat / fLat2;
            }
            if (!double.IsInfinity(rLat)) rB = rLat;

            //NaN signals poles
            lat = double.IsNaN(rB) ? new Latitude(90) : Latitude.FromRadians(rB);
            if (_hemisphere == 'S') lat = -lat;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <returns></returns>
        public string ToUps(Latitude lat, Longitude lng)
        {
            Forward(lat, lng, out double north, out double east);
            return _latBand.ToString() + string.Format("{0:0}{1:0}", east, north);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ups"></param>
        /// <param name="easting"></param>
        /// <param name="northing"></param>
        public void FromUps(string ups, out double easting, out double northing)
        {
            Decode(ups, out easting, out northing);

            easting = (easting - FalseEasting) / ScaleFactor;
            northing = (northing - FalseNorthing) / ScaleFactor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ups"></param>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        public void FromUps(string ups, out Latitude lat, out Longitude lng)
        {
            Decode(ups, out double east, out double north);
            Validate(ups);

            Reverse(north, east, out lat, out lng);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ups"></param>
        /// <returns></returns>
        public bool Validate(string ups)
        {
            Decode(ups, out double east, out double north);

            if (_hemisphere != 'N' && _hemisphere != 'S')
            {
                throw new GeodeticException("UPS hemisphere is error.");
            }

            if (_latBand != 'Y' && _latBand != 'Z' && _latBand != 'A' && _latBand != 'B')
            {
                throw new GeodeticException("UPS pole zone is error.");
            }

            if (east < Min_Easting || east > Max_Easting)
            {
                throw new GeodeticException("Easting value is outside of valid range.");
            }

            if (north < Min_Northing || north > Max_Northing)
            {
                throw new GeodeticException("Northing value is outside of valid range.");
            }

            return true;
        }
    }
}
