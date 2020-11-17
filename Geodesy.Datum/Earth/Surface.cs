using System;
using Geodesy.Datum.Coordinate;

namespace Geodesy.Datum.Earth
{
    /// <summary>
    /// reference ellipsoid
    /// </summary>
    public sealed class Surface
    {
        /// <summary>
        /// Create a ellpisoidal surface with default ellipsoid.
        /// </summary>
        public Surface()
            : this(Settings.Ellipsoid)
        { }

        /// <summary>
        /// Clone a ellipsoid surface from an earth ellipsoid 
        /// </summary>
        /// <param name="ellipsoid">earth ellipsoid</param>
        public Surface(Ellipsoid ellipsoid)
        {
            Ellipsoid = ellipsoid;
        }

        /// <summary>
        /// Create a ellipsoid surface
        /// </summary>
        /// <param name="semiMajor">semi-major axis</param>
        /// <param name="InverseFlattening">flattening</param>
        public Surface(double semiMajor, double InverseFlattening)
        {
            Ellipsoid = new Ellipsoid(semiMajor, InverseFlattening);
        }

        /// <summary>
        /// Earth ellipsoid of the 
        /// </summary>
        public Ellipsoid Ellipsoid { get; set; }

        /// <summary>
        /// Get the trapezoidal area on ellipsoid
        /// </summary>
        /// <param name="ellipsoid">ellipsoid</param>
        /// <param name="lat0">south latitude</param>
        /// <param name="lng0">west longitude</param>
        /// <param name="lat1">north latitude</param>
        /// <param name="lng1">east longitude</param>
        /// <returns>trapezoidal area</returns>
        public static double GetArea(Ellipsoid ellipsoid, Latitude lat0, Longitude lng0, Latitude lat1, Longitude lng1)
        {
            double e2 = ellipsoid.ee;
            double e4 = e2 * e2;
            double e6 = e2 * e4;
            double e8 = e4 * e4;

            //梯形图幅面积公式的系数
            double cA, cB, cC, cD, cE;
            cA = 1 + e2 / 2 + 3 * e4 / 8 + 5 * e6 / 16 + 35 * e8 / 128;
            cB = e2 / 6 + 3 * e4 / 16 + 3 * e6 / 16 + 35 * e8 / 192;
            cC = 3 * e4 / 80 + e6 / 16 + 5 * e8 / 64;
            cD = e6 / 112 + 5 * e8 / 156;
            cE = 5 * e8 / 2304;

            double Bm = (lat0 + lat1).Radians / 2;
            double dB = (lat1 - lat0).Radians / 2;
            double dL = (lng1 - lng0).Radians * 2;

            // P142 (5-47)
            return dL * ellipsoid.b * ellipsoid.b * (cA * Math.Sin(dB) * Math.Cos(Bm) - cB * Math.Sin(3 * dB) * Math.Cos(3 * Bm)
                                           + cC * Math.Sin(5 * dB) * Math.Cos(5 * Bm) - cD * Math.Sin(7 * dB) * Math.Cos(7 * Bm)
                                           + cE * Math.Sin(9 * dB) * Math.Cos(9 * Bm));
        }

        /// <summary>
        /// Get the trapezoidal area on ellipsoid
        /// </summary>
        /// <param name="lat0">south latitude</param>
        /// <param name="lng0">west longitude</param>
        /// <param name="lat1">north latitude</param>
        /// <param name="lng1">east longitude</param>
        /// <returns>trapezoidal area</returns>
        public double GetArea(Latitude lat0, Longitude lng0, Latitude lat1, Longitude lng1)
        {
            return GetArea(Ellipsoid, lat0, lng0, lat1, lng1);
        }

        /// <summary>
        /// Get the trapezoidal area on ellipsoid
        /// </summary>
        /// <param name="pnt0">west-south point</param>
        /// <param name="pnt1">east-north point</param>
        /// <returns>trapezoidal area</returns>
        public double GetArea(GeoPoint pnt0, GeoPoint pnt1)
        {
            return GetArea(pnt0.Latitude, pnt0.Longitude, pnt1.Latitude, pnt1.Longitude);
        }
    }
}
