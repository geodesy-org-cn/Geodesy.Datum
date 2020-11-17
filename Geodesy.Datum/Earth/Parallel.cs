using System;
using Geodesy.Datum.Coordinate;

namespace Geodesy.Datum.Earth
{
    /// <summary>
    /// parallel arc on the ellipsoid surface
    /// </summary>
    public sealed class Parallel : GeoArc
    {
        /// <summary>
        /// equator of the earth.
        /// </summary>
        public static readonly Parallel Equator = new Parallel(Latitude.Equator);

        /// <summary>
        /// create a parallel line
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lng0">start longitude</param>
        /// <param name="lng1">end longitude</param>
        /// <param name="ellipsoid">earth ellipsoid</param>
        public Parallel(Latitude lat, Longitude lng0, Longitude lng1, Ellipsoid ellipsoid)
            : base(new GeoPoint(lat, lng0, ellipsoid), new GeoPoint(lat, lng1))
        {
            double sinB = Math.Sin(lat.Radians);
            double N = _a / Math.Sqrt(1 - _es * sinB * sinB);
            Length = N * Math.Cos(lat.Radians) * Math.Abs(lng0.Radians - lng1.Radians);

            if (lng0 < lng1)
            {
                Azimuth = Angle.HalfPi;
                InverseAzimuth = Angle.HalfPi + Angle.Pi;
            }
            else
            {
                Azimuth = Angle.HalfPi + Angle.Pi;
                InverseAzimuth = Angle.HalfPi;
            }
        }

        /// <summary>
        /// create a parallel circle
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="ellipsoid">earth ellipsoid</param>
        public Parallel(Latitude lat, Ellipsoid ellipsoid)
             : base(new GeoPoint(lat, Longitude.MinValue, ellipsoid), new GeoPoint(lat, Longitude.MaxValue))
        {
            double sinB = Math.Sin(lat.Radians);
            double N = _a / Math.Sqrt(1 - _es * sinB * sinB);
            Length = 2 * Math.PI * N * Math.Cos(lat.Radians);

            Azimuth = Angle.HalfPi;
            InverseAzimuth = Angle.HalfPi + Angle.Pi;
        }

        /// <summary>
        /// create a parallel circle in CGCS2000 ellipsoid
        /// </summary>
        /// <param name="lat">latitude</param>
        public Parallel(Latitude lat)
            : base(new GeoPoint(lat, Longitude.MinValue, Settings.Ellipsoid), new GeoPoint(lat, Longitude.MaxValue))
        {
            double sinB = Math.Sin(lat.Radians);
            double N = _a / Math.Sqrt(1 - _es * sinB * sinB);
            Length = 2 * Math.PI * N * Math.Cos(lat.Radians);

            Azimuth = Angle.HalfPi;
            InverseAzimuth = Angle.HalfPi + Angle.Pi;
        }

        /// <summary>
        /// latitude
        /// </summary>
        public Latitude Latitude => Start.Latitude;

        /// <summary>
        /// get the length of parallel arc
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lng0">start longitude</param>
        /// <param name="lng1">end longitude</param>
        /// <returns>length of parallel arc</returns>
        public double GetLength(Latitude lat, Longitude lng0, Longitude lng1)
        {
            double sinB = Math.Sin(lat.Radians);
            double N = _a / Math.Sqrt(1 - _es * sinB * sinB);
            return N * Math.Cos(lat.Radians) * Math.Abs(lng0.Radians - lng1.Radians);
        }
    }
}
