using System;
using Geodesy.Datum.Earth;
using Geodesy.Datum.Coordinate;

namespace Geodesy.Datum.Transformation
{
    /// <summary>
    /// Conversion is the change from one coordinate system to another based on the same datum.
    /// </summary>
    public static class Conversion
    {
        /// <summary>
        /// Conversion of geodetic coordinate space rectangular coordinate.
        /// </summary>
        /// <param name="ellipsoid">ellipsoid</param>
        /// <param name="blh">geodetic coordinate</param>
        /// <returns>space rectangular coordinate</returns>
        public static SpaceRectangularCoord BLH_XYZ(Ellipsoid ellipsoid, GeodeticCoord blh)
        {
            BLH_XYZ(ellipsoid, blh.Latitude, blh.Longitude, blh.Height, out double X, out double Y, out double Z);
            return new SpaceRectangularCoord(X, Y, Z);
        }

        /// <summary>
        /// Conversion of geodetic coordinate to space rectangular coordinate.
        /// </summary>
        /// <param name="ellipsoid">ellipsoid</param>
        /// <param name="lng">longitude</param>
        /// <param name="lat">latitude</param>
        /// <param name="hgt">ellipsoid height</param>
        /// <param name="X">X component of space rectangular coordinate</param>
        /// <param name="Y">Y component of space rectangular coordinate</param>
        /// <param name="Z">Z component of space rectangular coordinate</param>
        public static void BLH_XYZ(Ellipsoid ellipsoid, Latitude lat, Longitude lng, double hgt, out double X, out double Y, out double Z)
        {
            double sinL = Math.Sin(lng.Radians);
            double cosL = Math.Cos(lng.Radians);
            double sinB = Math.Sin(lat.Radians);
            double cosB = Math.Cos(lat.Radians);

            double N = ellipsoid.a / Math.Sqrt(1 - ellipsoid.ee * sinB * sinB);

            X = (N + hgt) * cosB * cosL;
            Y = (N + hgt) * cosB * sinL;
            Z = (N * (1 - ellipsoid.ee) + hgt) * sinB;
        }

        /// <summary>
        /// Conversion of space rectangular coordinate to geodetic coordinate.
        /// </summary>
        /// <param name="ellipsoid">ellipsoid</param>
        /// <param name="xyz">space rectangular coordinate</param>
        /// <returns>geodetic coordinate</returns>
        public static GeodeticCoord XYZ_BLH(Ellipsoid ellipsoid, SpaceRectangularCoord xyz)
        {
            XYZ_BLH(ellipsoid, xyz.X, xyz.Y, xyz.Z, out Latitude lat, out Longitude lng, out double H);
            return new GeodeticCoord(lat, lng, H);
        }

        /// <summary>
        /// Conversion of space rectangular coordinate to geodetic coordinate.
        /// </summary>
        /// <param name="ellipsoid">ellipsoid</param>
        /// <param name="X">X component of space rectangular coordinate</param>
        /// <param name="Y">Y component of space rectangular coordinate</param>
        /// <param name="Z">Z component of space rectangular coordinate</param>
        /// <param name="lng">longitude</param>
        /// <param name="lat">latitude</param>
        /// <param name="hgt">ellipsoid height</param>
        public static void XYZ_BLH(Ellipsoid ellipsoid, double X, double Y, double Z, out Latitude lat, out Longitude lng, out double hgt)
        {
            double a = ellipsoid.a;
            double ee = ellipsoid.ee;

            double rL, rB, rB0;

            //求解经度
            rL = Math.Atan2(Y, X);
        
            // 迭代精度为1E-5秒
            rB = Math.Atan(Z / Math.Sqrt(X * X + Y * Y));
            rB0 = rB + 1;
            while (Math.Abs(rB - rB0) > Settings.Epsilon5)
            {
                rB0 = rB;
                double temp = a * ee * Math.Tan(rB0) / Math.Sqrt(1 + (1 - ee) * Math.Pow(Math.Tan(rB0), 2));
                rB = Math.Atan((Z + temp) / Math.Sqrt(X * X + Y * Y));
            }

            lng = Longitude.FromRadians(rL);
            lat = Latitude.FromRadians(rB);

            hgt = Math.Sqrt(X * X + Y * Y) / Math.Cos(rB) - a / Math.Sqrt(1 - ee * Math.Pow(Math.Sin(rB), 2));
        }

        /// <summary>
        /// Conversion of topocentric rectangular coordinate to space rectangular coordinate
        /// </summary>
        /// <param name="ellipsoid">ellipsoid</param>
        /// <param name="neu">topocentric rectangular coordinate</param>
        /// <param name="site">geodetic coordinate of station</param>
        /// <returns>space rectangular coordinate</returns>
        public static SpaceRectangularCoord NEU_XYZ(Ellipsoid ellipsoid, TopocentricRectCoord neu, GeodeticCoord site)
        {
            NEU_XYZ(ellipsoid, site.Latitude, site.Longitude, site.Height, neu.Northing, neu.Easting, neu.Upping, out double X, out double Y, out double Z);
            return new SpaceRectangularCoord(X, Y, Z);
        }

        /// <summary>
        /// Conversion of geodetic coordinate to space rectangular coordinate.
        /// </summary>
        /// <remarks>
        /// 站心坐标系X轴指向北方向，Z轴指向天方向，Y轴构成右手坐标系，即东方向
        /// </remarks>
        /// <param name="ellipsoid">ellipsoid</param>
        /// <param name="lng">longitude of station</param>
        /// <param name="lat">latitude of station</param>
        /// <param name="hgt">ellipsoid height of station</param>
        /// <param name="E">east component of topocentric rectangular coordinate</param>
        /// <param name="N">north component of topocentric rectangular coordinate</param>
        /// <param name="U">Upper component of topocentric rectangular coordinate</param>
        /// <param name="X">X component of space rectangular coordinate</param>
        /// <param name="Y">Y component of space rectangular coordinate</param>
        /// <param name="Z">Z component of space rectangular coordinate</param>
        public static void NEU_XYZ(Ellipsoid ellipsoid, Latitude lat, Longitude lng, double hgt, double N, double E, double U, out double X, out double Y, out double Z)
        {
            BLH_XYZ(ellipsoid, lat, lng, hgt, out X, out Y, out Z);

            double sinB = Math.Sin(lat.Radians);
            double cosB = Math.Cos(lat.Radians);
            double sinL = Math.Sin(lng.Radians);
            double cosL = Math.Cos(lng.Radians);

            X = X - sinB * cosL * N - sinL * E + cosB * cosL * U;
            Y = Y - sinB * sinL * N + cosL * E + cosB * sinL * U;
            Z = Z + cosB * N + sinB * U;
        }

        /// <summary>
        /// Conversion of space rectangular coordinate to topocentric rectangular coordinate.
        /// </summary>
        /// <param name="ellipsoid">ellipsoid</param>
        /// <param name="XYZ">space rectangular coordinate</param>
        /// <param name="site">geodetic coordinate of station</param>
        /// <returns>topocentric rectangular coordinate</returns>
        public static TopocentricRectCoord XYZ_NEU(Ellipsoid ellipsoid, SpaceRectangularCoord XYZ, GeodeticCoord site)
        {
            XYZ_NEU(ellipsoid, site.Latitude, site.Longitude, site.Height, XYZ.X, XYZ.Y, XYZ.Z, out double N, out double E, out double U);
            return new TopocentricRectCoord(N, E, U);
        }

        /// <summary>
        /// Conversion of space rectangular coordinate to topocentric rectangular coordinate.
        /// </summary>
        /// <remarks>
        /// 站心直角坐标系X轴指向北方向，Z轴指向天方向，Y轴构成右手坐标系，即东方向
        /// </remarks>
        /// <param name="ellipsoid">ellipsoid</param>
        /// <param name="lat">latitude</param>
        /// <param name="lng">longitude</param>
        /// <param name="height">ellipsoid height</param>
        /// <param name="X">X of space rectangular coordinate</param>
        /// <param name="Y">Y of space rectangular coordinate</param>
        /// <param name="Z">Z of space rectangular coordinate</param>
        /// <param name="E">easting</param>
        /// <param name="N">northing</param>
        /// <param name="U">upping</param>
        public static void XYZ_NEU(Ellipsoid ellipsoid, Latitude lat, Longitude lng, double height, double X, double Y, double Z, out double N, out double E, out double U)
        {
            BLH_XYZ(ellipsoid, lat, lng, height, out double X0, out double Y0, out double Z0);

            double sinB = Math.Sin(lat.Radians);
            double cosB = Math.Cos(lat.Radians);
            double sinL = Math.Sin(lng.Radians);
            double cosL = Math.Cos(lng.Radians);

            N = -sinB * cosL * (X - X0) - sinB * sinL * (Y - Y0) + cosB * (Z - Z0);
            E = -sinL * (X - X0) + cosL * (Y - Y0);
            U = cosB * cosL * (X - X0) + cosB * sinL * (Y - Y0) + sinB * (Z - Z0);
        }

        /// <summary>
        /// Conversion of topocentric rectangular coordinate to topocenteric polar coordinate.
        /// </summary>
        /// <param name="site">topocentric rectangular coordinate</param>
        /// <returns>topocenteric polar coordinate</returns>
        public static TopocentricPolarCoord NEU_RAE(TopocentricRectCoord site)
        {
            NEU_RAE(site.Northing, site.Easting, site.Upping, out double range, out Angle azimuth, out Angle elevation);
            return new TopocentricPolarCoord(range, azimuth, elevation);
        }

        /// <summary>
        /// Conversion of topocentric rectangular coordinate to topocenteric polar coordinate.
        /// </summary>
        /// <param name="northing">northing of topocentric rectangular coordinate</param>
        /// <param name="easting">easting of topocentric rectangular coordinate</param>
        /// <param name="upping">upping of topocentric rectangular coordinate</param>
        /// <param name="range">range of topocenteric polar coordinate</param>
        /// <param name="azimuth">azimuth of topocenteric polar coordinate</param>
        /// <param name="elevation">elevation of topocenteric polar coordinate</param>
        public static void NEU_RAE(double northing, double easting, double upping, out double range, out Angle azimuth, out Angle elevation)
        {
            range = Math.Sqrt(northing * northing + easting * easting + upping * upping);

            double angle = Math.Atan2(easting, northing);
            if (easting > 0)
            {
                angle = 0.5 * Math.PI - angle;
            }
            else
            {
                angle = 1.5 * Math.PI - angle;
            }
            azimuth = Angle.FromRadians(angle);

            elevation = Angle.FromRadians(Math.Atan2(upping, range));
        }

        /// <summary>
        /// Conversion of topocenteric polar coordinate to topocentric rectangular coordinate.
        /// </summary>
        /// <param name="target">topocenteric polar coordinate</param>
        /// <returns>topocentric rectangular coordinate</returns>
        public static TopocentricRectCoord RAE_NEU(TopocentricPolarCoord target)
        {
            RAE_NEU(target.Range, target.Azimuth, target.Elevation, out double N, out double E, out double U);
            return new TopocentricRectCoord(N, E, U);
        }

        /// <summary>
        /// Conversion of topocenteric polar coordinate to topocentric rectangular coordinate.
        /// </summary>
        /// <param name="range">range of topocenteric polar coordinate</param>
        /// <param name="azimuth">azimuth of topocenteric polar coordinate</param>
        /// <param name="elevation">elevation of topocenteric polar coordinate</param>
        /// <param name="northing">northing of topocentric rectangular coordinate</param>
        /// <param name="easting">easting of topocentric rectangular coordinate</param>
        /// <param name="upping">upping of topocentric rectangular coordinate</param>
        public static void RAE_NEU(double range, Angle azimuth, Angle elevation, out double northing, out double easting, out double upping)
        {
            upping = range * Math.Sin(elevation.Radians);

            double angle = azimuth.Radians;
            if (angle < Math.PI)
            {
                angle = 0.5 * Math.PI - angle;
            }
            else
            {
                angle = 1.5 * Math.PI - angle;
            }

            easting = range * Math.Cos(elevation.Radians) * Math.Cos(angle);
            northing = range * Math.Cos(elevation.Radians) * Math.Sin(angle);

            if (azimuth < Angle.Pi) easting *= -1;
            if (azimuth > Angle.Pi) northing *= -1;
        }
    }
}
