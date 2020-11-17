using System;
using Newtonsoft.Json;
using Geodesy.Datum.Coordinate;
using System.Collections.Generic;

namespace Geodesy.Datum.Earth.Projection
{
    /// <summary>
    /// The TransverseMercator class implements a map projection of the same name, and is used by the Universal 
    /// Transverse Mercator (UTM) coordinate encoding system for points in the middle Latitudes around the globe.
    /// </summary>
    /// <remarks>
    /// reference: https://github.com/OpenSextant/geodesy/blob/master/src/main/java/org/opensextant/geodesy/TransverseMercator.java
    /// </remarks>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class TransverseMercator : MapProjection
    {
        /// <summary>
        /// EPSG Identifier
        /// </summary>
        public static readonly Identifier TMERC = new Identifier("EPSG", "9807", "Transverse Mercator", "TMERC");

        // Constants
        private const double Max_Lat = 89.99 * Math.PI / 180;
        private const double Max_Delta_Lon = 9.0 * Math.PI / 180;
        private const double Min_Easting = -40000000.0;
        private const double Max_Easting = +40000000.0;
        private const double Min_Northing = -20000000.0;
        private const double Max_Northing = +20000000.0;
        private const double Min_Scale = 0.3;
        private const double Max_Scale = 3.0;

        #region constructors
        /// <summary>
        /// 
        /// </summary>
        public TransverseMercator()
            : this(new Dictionary<ProjectionParameter, double>
            {
                { ProjectionParameter.Semi_Major, Settings.Ellipsoid.a },
                { ProjectionParameter.Inverse_Flattening, Settings.Ellipsoid.ivf },
            })
        { }

        /// <summary>
        /// Create an instance of transverse mercator projection.
        /// </summary>
        /// <param name="parameters">projection parameters</param>
        public TransverseMercator(Dictionary<ProjectionParameter, double> parameters)
            : base(parameters)
        {
            Identifier = TMERC;
            Surface = ProjectionSurface.Cylindrical;
            Property = ProjectionProperty.Conformal;
            Orientation = ProjectionOrientation.Transverse;

            CheckParameters(ProjectionParameter.Latitude_Of_Origin, ProjectionParameter.Scale_Factor);
            if (ScaleFactor > Max_Scale || ScaleFactor < Min_Scale)
            {
                throw new GeodeticException("Projection parameter '" + ProjectionParameter.Scale_Factor.ToString() + "' is error.");
            }
        }
        #endregion

        /// <summary>
        /// converts geodetic (latitude and longitude) coordinates to Transverse Mercator projection(easting and northing)
        /// coordinates, according to the current ellipsoid and Transverse Mercator projection coordinates.
        /// </summary>
        /// <param name="lng">longitude</param>
        /// <param name="lat">latiude</param>
        /// <param name="easting">easting</param>
        /// <param name="northing">northing</param>
        public override void Forward(Latitude lat, Longitude lng, out double northing, out double easting)
        {
            double rB = lat.Radians;
            if (Math.Abs(rB) > Max_Lat)
            {
                throw new GeodeticException("Latitude is too close to a Pole.");
            }

            // Delta Longitude
            double dL = (lng - CenteralMaridian).Radians;
            if (Math.Abs(dL) > Math.PI / 2)
            {
                throw new GeodeticException("Longitude is more than 90 deg from central meridian.");
            }

            if (Math.Abs(dL) > Max_Delta_Lon)
            {
                throw new GeodeticException("The point is too far from the center meridian.");
            }

            // Recognize near zero delta condition and correct
            if (Math.Abs(dL) < 2E-10) dL = 0.0;

            // CoSine of latitude 
            double c = Math.Cos(rB);
            double c2 = c * c;
            double c3 = c2 * c;
            double c5 = c3 * c2;
            double c7 = c5 * c2;

            // static readonly ant - TranMerc_ebs *c *c
            double es = SquaredEccentricity;
            double eta = es / (1 - es) * c2;
            double eta2 = eta * eta;
            double eta3 = eta2 * eta;
            double eta4 = eta3 * eta;

            // Sine of latitude
            double sin = Math.Sin(rB);

            // Radius of curvature in the prime vertical   
            double sn = GetN(rB);

            // Tangent of latitude
            double tan = Math.Tan(rB);
            double tan2 = tan * tan;
            double tan3 = tan2 * tan;
            double tan4 = tan3 * tan;
            double tan5 = tan4 * tan;
            double tan6 = tan5 * tan;

            // True Meridional distance    
            double tmd = GetMeridionalDistance(rB);
            // True Meridional distance for latitude of origin 
            double tmdo = GetMeridionalDistance(OriginLatitude.Radians);

            // Term in coordinate conversion formula - GP to Y 
            double t1, t2, t3, t4, t5, t6, t7, t8, t9;

            // Northing 
            double scale = ScaleFactor;
            t1 = (tmd - tmdo) * scale;
            t2 = sn * sin * c * scale / 2.0;
            t3 = sn * sin * c3 * scale * (5.0 - tan2 + 9.0 * eta + 4.0 * eta2) / 24.0;

            t4 = sn * sin * c5 * scale * (61.0 - 58.0 * tan2 + tan4 + 270.0 * eta - 330.0 * tan2 * eta
                    + 445.0 * eta2 + 324.0 * eta3 - 680.0 * tan2 * eta2 + 88.0 * eta4
                    - 600.0 * tan2 * eta3 - 192.0 * tan2 * eta4) / 720.0;

            t5 = sn * sin * c7 * scale * (1385.0 - 3111.0 * tan2 + 543.0 * tan4 - tan6) / 40320.0;

            northing = t1 + Math.Pow(dL, 2.0) * t2 + Math.Pow(dL, 4.0) * t3
                          + Math.Pow(dL, 6.0) * t4 + Math.Pow(dL, 8.0) * t5;

            // Easting 
            t6 = sn * c * scale;
            t7 = sn * c3 * scale * (1.0 - tan2 + eta) / 6.0;
            t8 = sn * c5 * scale * (5.0 - 18.0 * tan2 + tan4 + 14.0 * eta - 58.0 * tan2 * eta
                    + 13.0 * eta2 + 4.0 * eta3 - 64.0 * tan2 * eta2 - 24.0 * tan2 * eta3) / 120.0;
            t9 = sn * c7 * scale * (61.0 - 479.0 * tan2 + 179.0 * tan4 - tan6) / 5040.0;

            easting = dL * t6 + Math.Pow(dL, 3.0) * t7 + Math.Pow(dL, 5.0) * t8 + Math.Pow(dL, 7.0) * t9;
        }

        /// <summary>
        /// converts Transverse Mercator projection (easting and northing) coordinates to geodetic (latitude and longitude)
        /// coordinates, according to the current ellipsoid and Transverse Mercator projection parameters.
        /// </summary>
        /// <param name="easting">easting</param>
        /// <param name="northing">northing</param>
        /// <param name="lng">longitude</param>
        /// <param name="lat">latitude</param>
        public override void Reverse(double northing, double easting, out Latitude lat, out Longitude lng)
        {
            if (easting < Min_Easting || Max_Easting < easting)
            {
                throw new GeodeticException("Easting value is overflow.");
            }

            if (northing < Min_Northing || Max_Northing < northing)
            {
                throw new GeodeticException("Northing value is overflow.");
            }

            // Term in coordinate conversion formula - GP to Y
            double t10, t11, t12, t13, t14, t15, t16, t17;

            // True Meridional distance for latitude of origin
            double tmdo = GetMeridionalDistance(OriginLatitude.Radians);
            // True Meridional distance
            double tmd = tmdo + northing / ScaleFactor;

            // First Estimate 
            double sr = GetM(OriginLatitude.Radians);
            // Footpoint latitude
            double Bf = tmd / sr;
            for (int i = 0; i < 5; i++)
            {
                t10 = GetMeridionalDistance(Bf);
                sr = GetM(Bf);
                Bf += (tmd - t10) / sr;
            }

            // Tangent of latitude 
            double tan = Math.Tan(Bf);
            double tan2 = tan * tan;
            double tan4 = tan2 * tan2;

            // Radius of Curvature in the meridian 
            sr = GetM(Bf);
            // Radius of Curvature in the meridian 
            double sn = GetN(Bf);
            // Sine CoSine terms 
            double c = Math.Cos(Bf);

            // static readonly ant - ee *c *c 
            double es = SquaredEccentricity;
            double eta = es / (1 - es) * c * c;
            double eta2 = eta * eta;
            double eta3 = eta2 * eta;
            double eta4 = eta3 * eta;

            // Delta easting - Difference in Easting (Easting-Fe)
            double de = easting;
            double scale = ScaleFactor;

            // Latitude 
            t10 = tan / (2.0 * sr * sn * Math.Pow(scale, 2));
            t11 = tan * (5.0 + 3.0 * tan2 + eta - 4.0 * Math.Pow(eta, 2) - 9.0 * tan2 * eta)
                      / (24.0 * sr * Math.Pow(sn, 3) * Math.Pow(scale, 4));

            t12 = tan * (61.0 + 90.0 * tan2 + 46.0 * eta + 45.0 * tan4 - 252.0 * tan2 * eta - 3.0 * eta2 +
                    100.0 * eta3 - 66.0 * tan2 * eta2 - 90.0 * tan4 * eta + 88.0 * eta4 + 225.0 * tan4 * eta2
                    + 84.0 * tan2 * eta3 - 192.0 * tan2 * eta4) / (720.0 * sr * Math.Pow(sn, 5) * Math.Pow(scale, 6));
            t13 = tan * (1385.0 + 3633.0 * tan2 + 4095.0 * tan4 + 1575.0 * Math.Pow(tan, 6))
                      / (40320.0 * sr * Math.Pow(sn, 7) * Math.Pow(scale, 8));

            // Round value to be accurate to nearest meter in northing
            double latRad = Bf - Math.Pow(de, 2) * t10 + Math.Pow(de, 4) * t11 - Math.Pow(de, 6) * t12 + Math.Pow(de, 8) * t13;
            lat = Latitude.FromRadians(latRad);

            t14 = 1.0 / (sn * c * scale);
            t15 = (1.0 + 2.0 * tan2 + eta) / (6.0 * Math.Pow(sn, 3) * c * Math.Pow(scale, 3));
            t16 = (5.0 + 6.0 * eta + 28.0 * tan2 - 3.0 * eta2 + 8.0 * tan2 * eta + 24.0 * tan4
                   - 4.0 * eta3 + 4.0 * tan2 * eta2 + 24.0 * tan2 * eta3)
                     / (120.0 * Math.Pow(sn, 5) * c * Math.Pow(scale, 5));
            t17 = (61.0 + 662.0 * tan2 + 1320.0 * tan4 + 720.0 * Math.Pow(tan, 6)) / (5040.0 * Math.Pow(sn, 7)
                   * c * Math.Pow(scale, 7));

            // Difference in Longitude 
            double dlam = de * t14 - Math.Pow(de, 3) * t15 + Math.Pow(de, 5) * t16 - Math.Pow(de, 7) * t17;
            if (Math.Abs(dlam) > Max_Delta_Lon)
            {
                throw new GeodeticException("The point is too far from the center meridian.");
            }

            // Longitude round value to be accurate to nearest meter in easting
            lng = CenteralMaridian + new Angle(dlam, Angle.DataStyle.Radians);
        }
    }
}
