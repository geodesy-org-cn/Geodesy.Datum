using System;
using Newtonsoft.Json;
using Geodesy.Datum.Coordinate;

namespace Geodesy.Datum.Earth
{
    /// <summary>
    /// Point on ellipsoid surface
    /// </summary>    
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class GeoPoint
    {
        /// <summary>
        /// semi-major axis
        /// </summary>
        private double _a;

        /// <summary>
        /// sqared first eccentricity
        /// </summary>
        private double _es;

        #region constructor
        /// <summary>
        /// Create a point
        /// </summary>
        public GeoPoint()
        {
            Ellipsoid = Settings.Ellipsoid;
            _a = Ellipsoid.a;
            _es = Ellipsoid.ee;
        }

        /// <summary>
        /// Create a point with a coordinate
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="lng">Longitude</param>
        public GeoPoint(Latitude lat, Longitude lng)
            : this()
        {
            Longitude = lng;
            Latitude = lat;
        }

        /// <summary>
        /// Create a point with a coordinate and its ellipsoid
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="lng">Longitude</param>
        /// <param name="ellipsoid">Ellipsoid</param>
        public GeoPoint(Latitude lat, Longitude lng, Ellipsoid ellipsoid)
        {
            Latitude = lat;
            Longitude = lng;
            Ellipsoid = ellipsoid;

            _a = ellipsoid.a;
            _es = ellipsoid.ee;
        }

        /// <summary>
        /// Create a point with a coordinate
        /// </summary>
        /// <param name="pnt">geodetic coordinate</param>
        public GeoPoint(GeographicCoord pnt)
            : this()
        {
            Longitude = pnt.Longitude;
            Latitude = pnt.Latitude;
        }

        /// <summary>
        /// Create a point with a coordinate and its ellipsoid
        /// </summary>
        /// <param name="pnt">geodetic coordinate</param>
        /// <param name="ellipsoid">Ellipsoid</param>
        public GeoPoint(GeographicCoord pnt, Ellipsoid ellipsoid)
        {
            Latitude = pnt.Latitude;
            Longitude = pnt.Longitude;
            Ellipsoid = ellipsoid;

            _a = ellipsoid.a;
            _es = ellipsoid.ee;
        }

        /// <summary>
        /// Create a point with a coordinate
        /// </summary>
        /// <param name="pnt">geodetic coordinate</param>
        public GeoPoint(GeodeticCoord pnt)
            : this()
        {
            Longitude = pnt.Longitude;
            Latitude = pnt.Latitude;
        }

        /// <summary>
        /// Create a point with a coordinate and its ellipsoid
        /// </summary>
        /// <param name="pnt">geodetic coordinate</param>
        /// <param name="ellipsoid">Ellipsoid</param>
        public GeoPoint(GeodeticCoord pnt, Ellipsoid ellipsoid)
        {
            Latitude = pnt.Latitude;
            Longitude = pnt.Longitude;
            Ellipsoid = ellipsoid;

            _a = ellipsoid.a;
            _es = ellipsoid.ee;
        }
        #endregion

        #region properties
        /// <summary>
        /// Latitude
        /// </summary>
        [JsonProperty]
        [JsonConverter(typeof(AngleJsonConverter<Latitude>))]
        public Latitude Latitude { get; private set; }

        /// <summary>
        /// Longitude
        /// </summary>
        [JsonProperty]
        [JsonConverter(typeof(AngleJsonConverter<Longitude>))]
        public Longitude Longitude { get; private set; }

        /// <summary>
        /// Ellipsoid
        /// </summary>
        [JsonProperty]
        public Ellipsoid Ellipsoid { get; private set; }

        /// <summary>
        /// Set the ellipsoid
        /// </summary>
        /// <param name="ellipsoid">ellipsoid</param>
        public void SetEllipsoid(Ellipsoid ellipsoid)
        {
            Ellipsoid = ellipsoid;

            _a = ellipsoid.a;
            _es = ellipsoid.ee;
        }

        /// <summary>
        /// Set the coodinate value
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lng">longitude</param>
        public void SetCoordinate(Latitude lat, Longitude lng)
        {
            Latitude = lat;
            Longitude = lng;
        }

        /// <summary>
        /// Curvature radius in prime vertical
        /// </summary>
        public double CurvatureRadiusInPrimeVertical
        {
            get
            {
                double sinB = Math.Sin(Latitude.Radians);
                return _a / Math.Sqrt(1 - _es * sinB * sinB);
            }
        }

        /// <summary>
        /// Curvature radius in meridian
        /// </summary>
        public double CurvatureRadiusInMeridian
        {
            get
            {
                double sinB = Math.Sin(Latitude.Radians);
                double W = Math.Sqrt(1 - _es * sinB * sinB);
                return _a * (1 - _es) / Math.Pow(W, 3);
            }
        }

        /// <summary>
        /// Mean curvature radius
        /// </summary>
        public double MeanCurvatureRadius
        {
            get
            {
                double cosB = Math.Cos(Latitude.Radians);
                double ses = _es / (1 - _es);
                double V = Math.Sqrt(1 + ses * cosB * cosB);
                return _a * Math.Sqrt(1 + ses) / Math.Pow(V, 2);
            }
        }
        #endregion

        /// <summary>
        /// Get the curvature radius of any azimuth
        /// </summary>
        /// <param name="azimuth">azimuth</param>
        /// <returns>curvature radius</returns>
        public double GetCurvatureRadius(Angle azimuth)
        {
            double sinB = Math.Sin(Latitude.Radians);
            double W = Math.Sqrt(1 - _es * sinB * sinB);

            double M = _a * (1 - _es) / Math.Pow(W, 3);
            double N = _a / Math.Sqrt(1 - _es * sinB * sinB);

            double cosA = Math.Cos(azimuth.Radians);
            double sinA = Math.Sin(azimuth.Radians);
            return M * N / (N * cosA * cosA + M * sinA * sinA);
        }

        /// <summary>
        /// Get the convergence angle
        /// </summary>
        /// <param name="zone">6 or 3 means the projection zone width. default is 6.</param>
        /// <returns>convergence angle</returns>
        public Angle GetConvergenceAngle(int zone = 6)
        {
            int num = (int)Math.Ceiling(Longitude.Degrees / zone);
            double dl = (Longitude.Degrees - num * zone + (zone == 6 ? 3 : 0)) * Angle.DegreeToRadian;

            double rB = Latitude.Radians;
            double tanB = Math.Tan(rB);
            double cosB2 = Math.Pow(Math.Cos(rB), 2);
            double eta2 = _es * cosB2;

            double ang = dl * Math.Sin(rB) * (1 + dl * dl * cosB2 * (1 + 3 * eta2 + 2 * eta2 * eta2) / 3
                                                + Math.Pow(dl, 4) * cosB2 * cosB2 * (2 - tanB * tanB) / 15);
            return Angle.FromRadians(ang);
        }

        /// <summary>
        /// Get the length scale in Gauss projection
        /// </summary>
        /// <param name="zone">6 or 3 means the projection zone width. default is 6.</param>
        /// <returns>length scale</returns>
        public double GetLengthScale(int zone = 6)
        {
            int num = (int)Math.Ceiling(Longitude.Degrees / zone);
            double dl = (Longitude.Degrees - num * zone + (zone == 6 ? 3 : 0)) * Angle.DegreeToRadian;

            double rB = Latitude.Radians;
            double tanB = Math.Tan(rB);
            double cosB2 = Math.Pow(Math.Cos(rB), 2);
            double eta2 = _es * cosB2;

            return 1 + dl * cosB2 * (1 + eta2) / 2 + dl * cosB2 * cosB2 * (5 - 4 * tanB * tanB) / 24;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string temp = string.Empty;

            if (Longitude != null)
            {
                temp = "B:" + Latitude.ToString() + ", L:" + Longitude.ToString();
            }

            return temp;
        }
    }
}
