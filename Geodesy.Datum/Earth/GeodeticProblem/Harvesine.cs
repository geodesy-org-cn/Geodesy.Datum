using System;
using Geodesy.Datum.Coordinate;

namespace Geodesy.Datum.Earth.GeodeticProblem
{
    /// <summary>
    /// Harvesine's geodetic problem solution is for calculations on the basis of a spherical earth.
    /// </summary>
    /// <remarks>
    /// This class refers to https://www.movable-type.co.uk/scripts/latlong.html
    /// </remarks>
    public sealed class Harvesine : GeodesicSolution
    {
        private const double R = 6372.8e3;    // In meters

        public Harvesine()
        { }

        /// <summary>
        /// Create a class of geodetic problem with Harvesine solution
        /// </summary>
        /// <param name="start">start point of geodesic</param>
        /// <param name="distance">distance of geodesic</param>
        /// <param name="bearing">azimuth of geodesic</param>
        public Harvesine(GeoPoint start, double distance, Angle bearing)
            : base(start, distance, bearing)
        { }

        /// <summary>
        /// Create a class of geodetic problem with Harvesine solution
        /// </summary>
        /// <param name="start">start point of geodesic</param>
        /// <param name="end">end point of geodesic</param>
        public Harvesine(GeoPoint start, GeoPoint end)
            : base(start, end)
        { }

        /// <summary>
        /// Directed solution of geodetic problem from start point, distance and azimuth to end point and inverse azimuth.
        /// </summary>
        /// <param name="start">start point of geodesic</param>
        /// <param name="distance">distance of geodesic</param>
        /// <param name="bearing">azimuth of geodesic</param>
        /// <param name="end">end point of geodesic</param>
        /// <param name="ivBearing">inverse azimuth of geodesic</param>
        protected override void Direct(GeoPoint start, double distance, Angle bearing, out GeoPoint end, out Angle ivBearing)
        {
            double lat1 = start.Latitude.Radians;
            double brng = bearing.Radians;
            double dR = distance / R;

            var lat2 = Math.Asin(Math.Sin(lat1) * Math.Cos(dR) + Math.Cos(lat1) * Math.Sin(dR) * Math.Cos(brng));
            var lng2 = start.Longitude.Radians + Math.Atan2(Math.Sin(brng) * Math.Sin(dR) * Math.Cos(lat1),
                                                 Math.Cos(dR) - Math.Sin(lat1) * Math.Sin(lat2));

            end = new GeoPoint(Latitude.FromRadians(lat2), Longitude.FromRadians(lng2));
            ivBearing = GetBearing(end, start);
        }

        /// <summary>
        /// Inverse solution of geodetic problem from start point, end point to the distance and azimuth.
        /// </summary>
        /// <param name="start">start point of geodesic</param>
        /// <param name="end">end point of geodesic</param>
        /// <param name="distance">distance of geodesic</param>
        /// <param name="bearing">azimuth of geodesic</param>
        /// <param name="ivBearing">inverse azimuth of geodesic</param>
        protected override void Inverse(GeoPoint start, GeoPoint end, out double distance, out Angle bearing, out Angle ivBearing)
        {
            double lat1 = start.Latitude.Radians;
            double lat2 = end.Latitude.Radians;

            double Dlng = end.Longitude.Radians - start.Longitude.Radians;
            double sDlat = Math.Sin((lat2 - lat1) / 2);
            double sDlng = Math.Sin((Dlng) / 2);

            bearing = GetBearing(start, end);
            ivBearing = GetBearing(end, start);

            double a = sDlat * sDlat + sDlng * sDlng * Math.Cos(lat1) * Math.Cos(lat2);
            double c = 2 * Math.Asin(Math.Sqrt(a));
            distance = R * c;
        }

        /// <summary>
        /// Calculate the bearing between start point and end point.
        /// </summary>
        /// <param name="start">start point</param>
        /// <param name="end">end point</param>
        /// <returns>bearing in radians</returns>
        public Angle GetBearing(GeoPoint start, GeoPoint end)
        {
            double lat1 = start.Latitude.Radians;
            double lat2 = end.Latitude.Radians;
            double dLng = end.Longitude.Radians - start.Longitude.Radians;

            double x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLng);
            double y = Math.Sin(dLng) * Math.Cos(lat2);
            return Angle.FromRadians(Math.Atan2(y, x));
        }
    }
}
