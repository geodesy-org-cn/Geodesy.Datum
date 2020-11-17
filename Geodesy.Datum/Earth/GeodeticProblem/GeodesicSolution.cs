using Geodesy.Datum.Coordinate;

namespace Geodesy.Datum.Earth.GeodeticProblem
{
    /// <summary>
    /// Geodetic problem solution abstract class.
    /// </summary>
    public abstract class GeodesicSolution
    {
        /// <summary>
        /// start point of geodesic
        /// </summary>
        public GeoPoint Start { get; protected set; }

        /// <summary>
        /// end point of geodesic
        /// </summary>
        public GeoPoint End { get; protected set; }

        /// <summary>
        /// distance of geodesic
        /// </summary>
        public double Distance { get; protected set; }

        /// <summary>
        /// azimuth of geodesic
        /// </summary>
        public Angle Bearing { get; protected set; }

        /// <summary>
        /// inverse azimuth of geodesic
        /// </summary>
        public Angle InverseBearing { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public GeodesicSolution()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="distance"></param>
        /// <param name="bearing"></param>
        public GeodesicSolution(GeoPoint start, double distance, Angle bearing)
        {
            Direct(start, distance, bearing, out GeoPoint end, out Angle ivBearing);
            Start = start;
            End = end;
            Distance = distance;
            Bearing = bearing;
            InverseBearing = ivBearing;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public GeodesicSolution(GeoPoint start, GeoPoint end)
        {
            Inverse(start, end, out double distance, out Angle bearing, out Angle ivBearing);
            Start = start;
            End = end;
            Distance = distance;
            Bearing = bearing;
            InverseBearing = ivBearing;
        }

        /// <summary>
        /// Directed solution of geodetic problem from start point, distance and azimuth to end point and inverse azimuth.
        /// </summary>
        /// <param name="start">start point of geodesic</param>
        /// <param name="distance">distance of geodesic</param>
        /// <param name="bearing">azimuth of geodesic</param>
        /// <param name="end">end point of geodesic</param>
        /// <param name="ivBearing">inverse azimuth of geodesic</param>
        protected abstract void Direct(GeoPoint start, double distance, Angle bearing, out GeoPoint end, out Angle ivBearing);

        /// <summary>
        /// Inverse solution of geodetic problem from start point, end point to the distance and azimuth.
        /// </summary>
        /// <param name="start">start point of geodesic</param>
        /// <param name="end">end point of geodesic</param>
        /// <param name="distance">distance of geodesic</param>
        /// <param name="bearing">azimuth of geodesic</param>
        /// <param name="ivBearing">inverse azimuth of geodesic</param>
        protected abstract void Inverse(GeoPoint start, GeoPoint end, out double distance, out Angle bearing, out Angle ivBearing);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="distance"></param>
        /// <param name="bearing"></param>
        /// <returns></returns>
        public GeoPoint GetEndPoint(GeoPoint start, double distance, Angle bearing)
        {
            Direct(start, distance, bearing, out GeoPoint end, out _);
            return end;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="ellip"></param>
        /// <param name="sa"></param>
        /// <returns></returns>
        public GeoPoint GetEndPoint(Latitude lat, Longitude lng, Ellipsoid ellip, double distance, Angle bearing)
        {
            GeoPoint start = new GeoPoint(lat, lng, ellip);
            Direct(start, distance, bearing, out GeoPoint end, out _);
            return end;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geodesic"></param>
        /// <returns></returns>
        public GeoPoint GetEndPoint(Geodesic geodesic)
        {
            Direct(geodesic.Start, geodesic.Length, geodesic.Azimuth, out GeoPoint end, out _);
            return end;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="distance"></param>
        /// <param name="bearing"></param>
        /// <returns></returns>
        public Angle GetInverseBearing(GeoPoint start, double distance, Angle bearing)
        {
            Direct(start, distance, bearing, out _, out Angle ivb);
            return ivb;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="distance"></param>
        /// <param name="bearing"></param>
        /// <returns></returns>
        public Angle GetInverseBearing(Latitude lat, Longitude lng, Ellipsoid ellip, double distance, Angle bearing)
        {
            GeoPoint start = new GeoPoint(lat, lng, ellip);
            Direct(start, distance, bearing, out _, out Angle ivb);
            return ivb;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geodesic"></param>
        /// <returns></returns>
        public Angle GetInverseBearing(Geodesic geodesic)
        {
            Direct(geodesic.Start, geodesic.Length, geodesic.Azimuth, out _, out Angle ivb);
            return ivb;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public double GetGeodesicDistance(GeoPoint start, GeoPoint end)
        {
            Inverse(start, end, out double distance, out _, out _);
            return distance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public Angle GetGeodesicBearing(GeoPoint start, GeoPoint end)
        {
            Inverse(start, end, out _, out Angle bearing, out _);
            return bearing;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public Angle GetGeodesicInverseBearing(GeoPoint start, GeoPoint end)
        {
            Inverse(start, end, out _, out _, out Angle ivb);
            return ivb;
        }
    }
}
