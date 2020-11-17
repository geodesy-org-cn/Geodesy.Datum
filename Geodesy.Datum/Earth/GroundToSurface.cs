using System;
using Geodesy.Datum.Coordinate;

namespace Geodesy.Datum.Earth
{
    /// <summary>
    /// 地面观测值归算至椭球面的改正
    /// </summary>
    public static class GroundToSurface
    {
        /// <summary>
        /// 获得垂涎偏差改正值
        /// </summary>
        /// <param name="ξ">垂涎偏差子午分量，以秒为单位</param>
        /// <param name="η">垂涎偏差卯酉分量，以秒为单位</param>
        /// <param name="azimuth">大地方位角</param>
        /// <param name="vertical">垂直角</param>
        /// <returns>垂涎偏差改正</returns>
        public static Angle GetCorrectionOfVerticalDeflection(double ξ, double η, Angle azimuth, Angle vertical)
        {
            ξ = ξ / 3600 * Math.PI / 180;
            η = η / 3600 * Math.PI / 180;

            // P151 (5-55)
            double delta = -(ξ * Math.Sin(azimuth.Radians) - η * Math.Cos(azimuth.Radians)) * Math.Tan(vertical.Radians);
            return Angle.FromRadians(delta);
        }

        /// <summary>
        /// 获得垂涎偏差改正值
        /// </summary>
        /// <param name="vd"></param>
        /// <param name="azimuth"></param>
        /// <param name="vertical"></param>
        /// <returns></returns>
        public static Angle GetCorrectionOfVerticalDeflection(VerticalDeviation vd, Angle azimuth, Angle vertical)
        {
            return GetCorrectionOfVerticalDeflection(vd.ξ, vd.η, azimuth, vertical);
        }

        /// <summary>
        /// 获得标高差改正值
        /// </summary>
        /// <param name="ellipsoid">参考椭球</param>
        /// <param name="lat2">目标点纬度</param>
        /// <param name="elevation2">目标点高程</param>
        /// <param name="azimuth1">测站到目标点的大地方位角</param>
        /// <returns>标高差改正值</returns>
        public static Angle GetCorrectionOfElevationDifference(Ellipsoid ellipsoid, Latitude lat2, double elevation2, Angle azimuth1)
        {
            double cosB = Math.Cos(lat2.Radians);
            double sinB = Math.Sin(lat2.Radians);

            double W = Math.Sqrt(1 - ellipsoid.ee * sinB * sinB);
            double M = ellipsoid.a * (1 - ellipsoid.ee) / Math.Pow(W, 3);

            // P152 (5-56)
            double delta = ellipsoid.ee * elevation2 * cosB * cosB * Math.Sin(2 * azimuth1.Radians) / 2 / M;
            return Angle.FromRadians(delta);
        }

        /// <summary>
        /// 获得截面差改正值
        /// </summary>
        /// <param name="ellipsoid">参考椭球</param>
        /// <param name="distance">大地线长</param>
        /// <param name="lat1">测站纬度</param>
        /// <param name="lat2">目标点纬度</param>
        /// <param name="azimuth1">测站到目标点的大地方位角</param>
        /// <returns>截面差改正</returns>
        public static Angle GetCorrectionOfGeodesicNormalSection(Ellipsoid ellipsoid, double distance, Latitude lat1, Latitude lat2, Angle azimuth1)
        {
            double cosB = Math.Cos(lat1.Radians);
            double sinB = Math.Sin(lat1.Radians);
            double N = ellipsoid.a / Math.Sqrt(1 - ellipsoid.ee * sinB * sinB);

            // P152 (5-57)
            double delta = -ellipsoid.ee * distance * distance * cosB * cosB * Math.Sin(azimuth1.Radians * 2) / 12 / N / N;
            return Angle.FromRadians(delta);
        }

        /// <summary>
        /// 观测天顶距归算至椭球面
        /// </summary>
        /// <param name="ξ">垂线偏差之子午分量，以秒为单位</param>
        /// <param name="η">垂线偏差之卯酉分量，以秒为单位</param>
        /// <param name="zenith">观测天顶距</param>
        /// <param name="azimuth">大地方位角</param>
        /// <returns>椭球面上的天顶距</returns>
        public static Angle ZenithToSurface(double ξ, double η, Angle zenith, Angle azimuth)
        {
            ξ = ξ / 3600 * Math.PI / 180;
            η = η / 3600 * Math.PI / 180;

            // P155 (5-60)
            return zenith + Angle.FromRadians(ξ * Math.Cos(azimuth.Radians) + η * Math.Sin(azimuth.Radians));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vd"></param>
        /// <param name="zenith"></param>
        /// <param name="azimuth"></param>
        /// <returns></returns>
        public static Angle ZenithToSurface(VerticalDeviation vd, Angle zenith, Angle azimuth)
        {
            return zenith + vd.NorthSouthValue * Math.Cos(azimuth.Radians) + vd.EastWestValue * Math.Sin(azimuth.Radians);
        }

        /// <summary>
        /// 地面观测距离归算成大地线长
        /// </summary>
        /// <param name="ellipsoid">参考椭球</param>
        /// <param name="dist">地面距离</param>
        /// <param name="h0">测站点高程</param>
        /// <param name="h1">目标点高程</param>
        /// <param name="lat0">测站点纬度</param>
        /// <param name="azimuth">大地方位角</param>
        /// <returns>大地线长</returns>
        public static double DistanceToSurface(Ellipsoid ellipsoid, double dist, double h0, double h1, Latitude lat0, Angle azimuth)
        {
            double cosB = Math.Cos(lat0.Radians);
            double sinB = Math.Sin(lat0.Radians);
            double cosA = Math.Cos(azimuth.Radians);

            double D = Math.Sqrt(dist * dist - (h1 - h0) * (h1 - h0));
            double Hm = (h1 + h0) / 2;
            double N = ellipsoid.a / Math.Sqrt(1 - ellipsoid.ee * sinB * sinB);
            double Ra = N / Math.Sqrt(1 + ellipsoid.ee * cosB * cosB * cosA * cosA);

            // P156 (5-64)
            return D * Ra / (Ra + Hm) + Math.Pow(dist, 3) / Ra / Ra / 24
                                      + 1.25E-16 * Hm * dist * dist * Math.Sin(lat0.Radians * 2) * cosA;
        }

        /// <summary>
        /// 将天文方位角转换成大地方位角
        /// </summary>
        /// <param name="astroLng">天文经度</param>
        /// <param name="astroLat">天文纬度</param>
         /// <param name="azimuth">天文方位角</param>
       /// <param name="geoLon">大地经度</param>
        /// <returns>大地方位角</returns>
        public static Angle GetAzimuthFromAstronomic(Latitude astroLat, Longitude astroLng, Angle azimuth, Longitude geoLon)
        {
            //P158 (5-67)
            return azimuth - (astroLng - geoLon) * Math.Sin(astroLat.Radians);
        }

        /// <summary>
        /// 将天文方位角转换成大地方位角
        /// </summary>
        /// <param name="azimuth">天文方位角</param>
        /// <param name="astroLat">天文纬度</param>
        /// <param name="η">垂线偏差之卯酉分量，以秒为单位</param>
        /// <returns>大地方位角</returns>
        public static Angle GetAzimuthFromAstronomic(Angle azimuth, Latitude astroLat, double η)
        {
            //P158 (5-68)
            return azimuth - Angle.FromSeconds(η) * Math.Tan(astroLat.Radians);
        }
    }
}
