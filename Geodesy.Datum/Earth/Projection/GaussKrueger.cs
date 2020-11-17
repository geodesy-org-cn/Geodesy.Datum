using System;
using Newtonsoft.Json;
using Geodesy.Datum.Coordinate;
using System.Collections.Generic;

namespace Geodesy.Datum.Earth.Projection
{
    /// <summary>
    /// The Gauss-Kruger projection system using Transverse Mercator projections to map the world into 
    /// numerous standard belts that are six degrees wide. The standard Manifold Gauss-Kruger projection 
    /// is also known as the Pulkovo 1942 Gauss-Kruger projection.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class GaussKrueger : MapProjection
    {
        // defalut parameters of Gauss-Kruger projection
        private const double False_Easting = 500000.0;
        private const double False_Northing = 0.0;
        private const double Scale_Factor = 1.0;
        private const double Zone_Width = 6.0;

        #region Constructors
        /// <summary>
        /// Create a Gauss-Krueger projection instant with default ellipsoid.
        /// </summary>
        public GaussKrueger()
            : this(Settings.Ellipsoid.a, Settings.Ellipsoid.ivf)
        { }

        /// <summary>
        /// Create a Gauss-Krueger projection instant.
        /// </summary>
        /// <param name="ellipsoid">earth ellipsoid</param>
        /// <param name="zone">zone width, default value 6.</param>
        public GaussKrueger(Ellipsoid ellipsoid, double zone = Zone_Width)
            : this(ellipsoid.a, ellipsoid.ivf, zone)
        { }

        /// <summary>
        /// Create a Gauss-Krueger projection instant.
        /// </summary>
        /// <param name="a">semi-major of ellispid</param>
        /// <param name="ivf">inverse flattening of ellipsoid</param>
        /// <param name="zone">zone width, default value 6.</param>
        public GaussKrueger(double a, double ivf, double zone = Zone_Width)
            : this(new Dictionary<ProjectionParameter, double>{
                { ProjectionParameter.Semi_Major, a },
                { ProjectionParameter.Inverse_Flattening, ivf },
                { ProjectionParameter.Width_Of_Zone, zone },
                { ProjectionParameter.Scale_Factor, Scale_Factor },
                { ProjectionParameter.False_Easting, False_Easting },
                { ProjectionParameter.False_Northing, False_Northing }})
        { }

        /// <summary>
        /// Create a Gauss-Krueger projection instant
        /// </summary>
        /// <param name="parameters">projection parameters</param>
        public GaussKrueger(Dictionary<ProjectionParameter, double> parameters)
            : base(parameters)
        {
            // set other properties
            Identifier = new Identifier(typeof(GaussKrueger));
            Surface = ProjectionSurface.Cylindrical;
            Property = ProjectionProperty.Conformal;
            Orientation = ProjectionOrientation.Transverse;

            CheckParameters(ProjectionParameter.False_Easting, ProjectionParameter.Width_Of_Zone);
            if (double.IsNaN(FalseNorthing))
            {
                SetParameter(ProjectionParameter.False_Northing, False_Northing);
            }
            if (double.IsNaN(ScaleFactor))
            {
                SetParameter(ProjectionParameter.Scale_Factor, Scale_Factor);
            }
        }
        #endregion

        #region direct solution and reverse solution
        /// <summary>
        /// converts Gauss-Kruger projection geodetic coordinates to geodetic coordinates.
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lng">longitude</param>
        /// <param name="northing">northing</param>
        /// <param name="easting">false easting</param>
        public override void Forward(Latitude lat, Longitude lng, out double northing, out double easting)
        {
            // If the center meridian is not definited, compute it.
            int number = GetZoneNumber(lng, WidthOfZone);
            Angle dL = lng - (CenteralMaridian ?? GetCenteralMeridian(number, WidthOfZone));

            // compute the natural coordinate
            Bl_xy(lat.Radians, dL.Radians, out northing, out easting);

            // natural coordinate to false 
            easting += FalseEasting;
            if (number > 0) easting += number * 1E6;
            northing += FalseNorthing;
        }

        /// <summary>
        /// converts Gauss-Kruger projection geodetic coordinates to geodetic coordinates.
        /// </summary>
        /// <param name="lat">latitude in radian</param>
        /// <param name="dL">anle difference to center meridian in radian</param>
        /// <param name="north">natural northing</param>
        /// <param name="east">natural easting</param>
        private void Bl_xy(double lat, double dL, out double north, out double east)
        {
            double tanB = Math.Tan(lat);
            double sinB = Math.Sin(lat);
            double tan2 = tanB * tanB;
            double tan4 = tan2 * tan2;
            double cosB = Math.Cos(lat);
            double m = cosB * dL;

            double es = SquaredEccentricity;
            double N = SemiMajor / Math.Sqrt(1 - es * sinB * sinB);
            double eta2 = es / (1 - es) * Math.Pow(cosB, 2);

            // northing
            north = m * m / 2 + (5 - tan2 + 9 * eta2 + 4 * eta2 * eta2) * Math.Pow(m, 4) / 24 + (61 - 58 * tan2 + tan4) * Math.Pow(m, 6) / 720;
            north = GetMeridionalDistance(lat) + N * tanB * north;
            north *= ScaleFactor;

            east = N * (m + (1 - tan2 + eta2) * Math.Pow(m, 3) / 6 + (5 - 18 * tan2 + tan4 + 14 * eta2 - 58 * tan2 * eta2) * Math.Pow(m, 5) / 120);
            east *= ScaleFactor;
        }

        /// <summary>
        /// converts geodetic coordinates to Gauss-Kruger projection coordinates.
        /// </summary>
        /// <param name="northing">northing</param>
        /// <param name="easting">easting</param>
        /// <param name="lat">latitude</param>
        /// <param name="lng">longitude</param>
        public override void Reverse(double northing, double easting, out Latitude lat, out Longitude lng)
        {
            //求带号以判断是否是自然坐标，并将其换算成自然坐标
            int number = (int)Math.Floor(easting * 1E-6);
            if (number > 0) easting -= number * 1E+6;

            easting = (easting - FalseEasting) / ScaleFactor;
            northing = (northing - FalseNorthing) / ScaleFactor;
            xy_bl(northing, easting, out double B, out double dL);

            lat = new Latitude(B, Angle.DataStyle.Radians);

            // Center meridian has been definited?
            lng = CenteralMaridian ?? GetCenteralMeridian(number, WidthOfZone);
            lng += new Angle(dL, Angle.DataStyle.Radians);
            lng.Normalize();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="north"></param>
        /// <param name="east"></param>
        /// <param name="lat"></param>
        /// <param name="dL"></param>
        private void xy_bl(double north, double east, out double lat, out double dL)
        {
            double Bf = GetMeridionalLatitude(north);

            double es = SquaredEccentricity;
            double es2 = es / (1 - es);
            double cosB = Math.Cos(Bf);
            double tanB = Math.Tan(Bf);
            double sinB = Math.Sin(Bf);

            double V2 = 1 + es2 * cosB * cosB;
            double eta2 = es2 * cosB * cosB;
            double tan2 = tanB * tanB;
            double tmp = east / SemiMajor * Math.Sqrt(1 - es * sinB * sinB);

            // latitude
            double B = V2 * tanB / 2 * (Math.Pow(tmp, 2) - (5 + 3 * tan2 + eta2 - 9 * eta2 * tan2) * Math.Pow(tmp, 4) / 12
                          + (61 + 90 * tan2 + 45 * tan2 * tan2) * Math.Pow(tmp, 6) / 360);
            lat = Bf - B;

            // longitude
            dL = 1 / cosB * (tmp - (1 + 2 * tan2 + eta2) * Math.Pow(tmp, 3) / 6
                   + (5 + 28 * tan2 + 24 * tan2 * tan2 + 6 * eta2 + 8 * eta2 * tan2) * Math.Pow(tmp, 5) / 120);
        }
        #endregion

        #region static methods
        /// <summary>
        /// Get the centeral meridian
        /// </summary>
        /// <param name="lng">longitude</param>
        /// <param name="zone">zone width, 3.0 or 6.0</param>
        /// <returns>angle of centeral meridian</returns>
        public static Longitude GetCenteralMeridian(Longitude lng, double zone)
        {
            int number = GetZoneNumber(lng, zone);

            return (number > 0) ? GetCenteralMeridian(number, zone) : null;
        }

        /// <summary>
        /// Get the centeral meridian
        /// </summary>
        /// <param name="number">zone number</param>
        /// <param name="zone">zone width, 3.0 or 6.0</param>
        /// <returns>angle of centeral meridian</returns>
        public static Longitude GetCenteralMeridian(int number, double zone)
        {
            if (number < 1) return Longitude.Zero;

            Longitude lng = null;
            if (zone == 6.0)
            {
                lng = new Longitude(number * 6 - 3);
            }
            else if (zone == 3.0)
            {
                lng = new Longitude(number * 3);
            }

            return lng;
        }

        /// <summary>
        /// Get the zone number of projection
        /// </summary>
        /// <param name="lng">longitude</param>
        /// <param name="zone">zone width</param>
        /// <returns>if zone type invalid, return -1. Else retrun the zone number</returns>
        public static int GetZoneNumber(Longitude lng, double zone)
        {
            // change longitude to (0, 2π)
            lng.Normalize();

            int number;
            if (zone == 6.0)
            {
                number = (int)Math.Ceiling(lng.Degrees / zone);
            }
            else if (zone == 3.0)
            {
                number = (int)Math.Ceiling((lng.Degrees - zone / 2) / zone);
                if (number * zone == lng.Degrees - zone / 2) number += 1;
            }
            else
            {
                number = 0;
            }

            return number;
        }

        /// <summary>
        /// get zone number from plane coordinate
        /// </summary>
        /// <param name="easting">easting of plane coordinate</param>
        /// <returns>zone number. if easting is natural, return 0.</returns>
        public static int GetZoneNumber(double easting)
        {
            return (int)Math.Floor(easting * 1E-6);
        }

        /// <summary>
        /// convert false coordinate to natural coordinate
        /// </summary>
        /// <param name="easting">easting value, false or natural</param>
        /// <returns>natural coordinate</returns>
        public static double GetNaturalCoord(double easting)
        {
            int number = (int)Math.Floor(easting * 1E-6);

            if (number > 0)
            {
                return easting - number * 1E+6 - False_Easting;
            }
            else
            {
                return easting;
            }
        }
        #endregion

        #region 邻带换算
        /// <summary>
        /// project to west neighbor zone
        /// </summary>
        /// <param name="northing">northing coordinate</param>
        /// <param name="easting">easting coordinate, false or natural</param>
        /// <param name="westNorthing">northing in west zone, natural coordinate</param>
        /// <param name="westEasting">easting in west zone, natural coordinate</param>
        public void ToWestZone(double northing, double easting, out double westNorthing, out double westEasting)
        {
            Reverse(northing, easting, out Latitude lat, out Longitude lng);

            double zone = WidthOfZone;
            int number = GetZoneNumber(lng, zone);
            Angle dL = lng - (CenteralMaridian ?? GetCenteralMeridian(number, zone));

            Bl_xy(lat.Radians, (dL + new Angle(zone)).Radians, out westNorthing, out westEasting);
        }

        /// <summary>
        /// project to east neighbor zone
        /// </summary>
        /// <param name="northing">northing coordinate</param>
        /// <param name="easting">easting coordinate, false or natural</param>
        /// <param name="eastNorthing">northing in east zone, natural coordinate</param>
        /// <param name="eastEasting">easting in east zone, natural coordinate</param>
        public void ToEastZone(double northing, double easting, out double eastNorthing, out double eastEasting)
        {
            Reverse(northing, easting, out Latitude lat, out Longitude lng);

            double zone = WidthOfZone;
            int number = GetZoneNumber(lng, zone);
            Angle dL = lng - (CenteralMaridian ?? GetCenteralMeridian(number, zone));

            Bl_xy(lat.Radians, (dL - new Angle(zone)).Radians, out eastNorthing, out eastEasting);
        }

        /// <summary>
        /// Convert the coordinate in six degree zone to three degree zone
        /// </summary>
        /// <param name="northing">northing in six degree zone</param>
        /// <param name="easting">easting in six degree zone</param>
        /// <param name="newNorthing">northing in three degree zone</param>
        /// <param name="newEasting">easting in three degree zone</param>
        public void SixZoneToThree(double northing, double easting, out double newNorthing, out double newEasting)
        {
            int number = (int)Math.Floor(easting * 1E-6);
            if (number > 0)
            {
                easting -= number * 1E+6;
            }
            else
            {
                newNorthing = double.NaN;
                newEasting = double.NaN;
                return;
            }

            easting -= FalseEasting;
            northing -= FalseNorthing;
            xy_bl(northing, easting, out double lat, out double dL);

            if (Math.Abs(dL) < 1.5 * Math.PI / 180)
            {
                number = 2 * number - 1;
                newEasting = easting + (number - 1) * 1e6;
                newNorthing = northing;
                return;
            }

            if (dL > 0)
            {
                number *= 2;
                dL -= 3 * Math.PI / 180;
            }
            else if (dL < 0)
            {
                number = (number - 1) * 2;
                dL += 3 * Math.PI / 180;
            }

            Bl_xy(lat, dL, out newNorthing, out newEasting);
            newEasting += number * 1e6 + FalseEasting;
            newNorthing += FalseNorthing;
        }

        /// <summary>
        /// Convert the coordinate in three degree zone to six degree zone
        /// </summary>
        /// <param name="northing">northing in three degree zone</param>
        /// <param name="easting">easting in three degree zone</param>
        /// <param name="newNorthing">northing in six degree zone</param>
        /// <param name="newEasting">easting in six degree zone</param>
        public void ThreeZoneToSix(double northing, double easting, out double newNorthing, out double newEasting)
        {
            int number = (int)Math.Floor(easting * 1E-6);
            if (number > 0)
            {
                easting -= number * 1E+6;
            }
            else
            {
                newNorthing = double.NaN;
                newEasting = double.NaN;
                return;
            }

            if (number % 2 == 0)
            {
                easting -= number * 1e6 + FalseEasting;
                northing -= FalseNorthing;
                xy_bl(northing, easting, out double lat, out double dL);

                if (dL < 0)
                {
                    number /= 2;
                    dL += 3 * Math.PI / 180;
                }
                else if (dL > 0)
                {
                    number = (number + 1) / 2;
                    dL -= 3 * Math.PI / 180;
                }
                Bl_xy(lat, dL, out newNorthing, out newEasting);

                newEasting = number * 1e6 + FalseEasting;
                newNorthing += FalseNorthing;
            }
            else
            {
                newNorthing = northing;
                // change the zone number
                newEasting = easting - (number / 2 - 1) * 1e6;
            }
        }
        #endregion
    }
}
