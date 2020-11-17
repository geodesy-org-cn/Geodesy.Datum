using Geodesy.Datum.Coordinate;

namespace Geodesy.Datum.Earth
{
    /// <summary>
    /// Geodesic object
    /// </summary>
    public class Geodesic : GeoArc
    {
        /// <summary>
        /// create a geodesic by two points
        /// </summary>
        /// <param name="start">start point</param>
        /// <param name="end">end point</param>
        public Geodesic(GeoPoint start, GeoPoint end)
            : base(start, end)
        { }

        /// <summary>
        /// create a geodesic by start point, distance and bearing
        /// </summary>
        /// <param name="start">start point</param>
        /// <param name="distance">geodesic length</param>
        /// <param name="bearing">geodesic bearing</param>
        public Geodesic(GeoPoint start, double distance, Angle bearing)
            : base(start, distance, bearing)
        { }

        /// <summary>
        /// geodesic bearing
        /// </summary>
        public Angle Bearing => Azimuth;

        /// <summary>
        /// geodesic inverse bearing
        /// </summary>
        public Angle InverseBearing => InverseAzimuth;
    }
}
