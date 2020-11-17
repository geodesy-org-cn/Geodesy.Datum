using System;
using Geodesy.Datum.Coordinate;

namespace Geodesy.Datum.Earth
{
    /// <summary>
    /// meridian arc on the ellipsoid surface
    /// </summary>
    public class Meridian : GeoArc
    {
        /// <summary>
        /// Create a meridian line
        /// </summary>
        /// <param name="lng">longitude</param>
        /// <param name="lat0">start latitude</param>
        /// <param name="lat1">end latitude</param>
        /// <param name="ellipsoid">earth ellipsoid</param>
        public Meridian(Longitude lng, Latitude lat0, Latitude lat1, Ellipsoid ellipsoid)
            : base(new GeoPoint(lat0, lng, ellipsoid), new GeoPoint(lat1, lng))
        {
            Length = Math.Abs(GetLength(lat1) - GetLength(lat0));
            if (lat1 > lat0)
            {
                Azimuth = Angle.Zero;
                InverseAzimuth = Angle.Pi;
            }
            else
            {
                Azimuth = Angle.Pi;
                InverseAzimuth = Angle.Zero;
            }
        }

        /// <summary>
        /// Create a meridian with latitude from 0 to 90
        /// </summary>
        /// <param name="lng">longitude</param>
        /// <param name="ellipsoid">earth ellipsoid</param>
        public Meridian(Longitude lng, Ellipsoid ellipsoid)
            : base(new GeoPoint(Latitude.SouthPole, lng, ellipsoid), new GeoPoint(Latitude.NorthPole, lng))
        {
            Length = GetLength(Latitude.NorthPole) * 2;
            Azimuth = Angle.Zero;
            InverseAzimuth = Angle.Pi;
        }

        /// <summary>
        /// Create a meridian with latitude from 0 to 90.
        /// </summary>
        /// <param name="lng">longitude</param>
        public Meridian(Longitude lng)
            : base(new GeoPoint(Latitude.SouthPole, lng, Settings.Ellipsoid), new GeoPoint(Latitude.NorthPole, lng))
        {
            Length = GetLength(Latitude.NorthPole) * 2;
            Azimuth = Angle.Zero;
            InverseAzimuth = Angle.Pi;
        }

        /// <summary>
        /// longitude
        /// </summary>
        public Longitude Longitude => Start.Longitude;

        /// <summary>
        /// get the arc length of meridian from equator on the ellpsoid
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <returns>arc length</returns>
        public double GetLength(Latitude lat)
        {
            double e2 = _es;
            double e4 = e2 * e2;
            double e6 = e2 * e4;
            double e8 = e4 * e4;
            double e10 = e4 * e6;
            double rB = lat.Radians;

            //子午线弧长公式的系数
            double cA, cB, cC, cD, cE, cF;
            cA = 1 + 3 * e2 / 4 + 45 * e4 / 64 + 175 * e6 / 256 + 11025 * e8 / 16384 + 43659 * e10 / 65536;
            cB = 3 * e2 / 4 + 15 * e4 / 16 + 525 * e6 / 512 + 2205 * e8 / 2048 + 72765 * e10 / 65536;
            cC = 15 * e4 / 64 + 105 * e6 / 256 + 2205 * e8 / 4096 + 10395 * e10 / 16384;
            cD = 35 * e6 / 512 + 315 * e8 / 2048 + 31185 * e10 / 131072;
            cE = 315 * e8 / 16384 + 3465 * e10 / 65536;
            cF = 693 * e10 / 131072;

            //子午线弧长
            return _a * (1 - _es) * (cA * rB - cB * Math.Sin(2 * rB) / 2 + cC * Math.Sin(4 * rB) / 4
                   - cD * Math.Sin(6 * rB) / 6 + cE * Math.Sin(8 * rB) / 8 - cF * Math.Sin(10 * rB) / 10);
        }

        /// <summary>
        /// get the latitude from the arc length of meridian
        /// </summary>
        /// <param name="length">arc length</param>
        /// <returns>latitude</returns>
        public Latitude GetLatitude(double length)
        {
            double e2 = _es;
            double e4 = e2 * e2;
            double e6 = e2 * e4;
            double e8 = e4 * e4;
            double e10 = e4 * e6;

            //子午线弧长公式的系数
            double cA, cB, cC, cD, cE, cF;
            cA = 1 + 3 * e2 / 4 + 45 * e4 / 64 + 175 * e6 / 256 + 11025 * e8 / 16384 + 43659 * e10 / 65536;
            cB = 3 * e2 / 4 + 15 * e4 / 16 + 525 * e6 / 512 + 2205 * e8 / 2048 + 72765 * e10 / 65536;
            cC = 15 * e4 / 64 + 105 * e6 / 256 + 2205 * e8 / 4096 + 10395 * e10 / 16384;
            cD = 35 * e6 / 512 + 315 * e8 / 2048 + 31185 * e10 / 131072;
            cE = 315 * e8 / 16384 + 3465 * e10 / 65536;
            cF = 693 * e10 / 131072;

            double B = length / _a / (1 - _es) / cA;
            double B0 = B + 0.1;
            while (Math.Abs(B - B0) > Settings.Epsilon5)
            {
                B0 = B;
                B = (length / _a / (1 - _es) + cB * Math.Sin(2 * B0) / 2 - cC * Math.Sin(4 * B0) / 4
                     + cD * Math.Sin(6 * B0) / 6 - cE * Math.Sin(8 * B0) / 8 + cF * Math.Sin(10 * B0) / 10) / cA;
            }

            return new Latitude(B, Angle.DataStyle.Radians);
        }
    }
}
