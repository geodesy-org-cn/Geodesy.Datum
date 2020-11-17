using System;
using Newtonsoft.Json;
using Geodesy.Datum.Coordinate;
using Geodesy.Datum.Earth.Projection;

namespace Geodesy.Datum.Earth
{
    /// <summary>
    /// Arc on ellipsoid surface
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class GeoArc
    {
        #region private variables
        /// <summary>
        /// semi-major axis
        /// </summary>
        protected double _a;

        /// <summary>
        /// sqared first eccentricity
        /// </summary>
        protected double _es;

        /// <summary>
        /// sqared second eccentricity
        /// </summary>
        protected double _sse;
        #endregion

        #region constructors
        /// <summary>
        /// Create an arc with two points on ellipsoid surface
        /// </summary>
        /// <param name="start">start point</param>
        /// <param name="end">end point</param>
        public GeoArc(GeoPoint start, GeoPoint end)
        {
            // copy the ellipsoid parameters
            _a = start.Ellipsoid.a;
            _es = start.Ellipsoid.ee;
            _sse = start.Ellipsoid._ee;

            // copy the points
            Start = start;
            End = end;
        }

        /// <summary>
        /// create an arc with start point and the distance, azimuth
        /// </summary>
        /// <param name="start">start point</param>
        /// <param name="distance">distance from start pont to end point</param>
        /// <param name="azimuth">geodetic azimuth</param>
        /// <param name="ellipsoid">earth ellipsoid</param>
        public GeoArc(GeoPoint start, double distance, Angle azimuth)
        {
            // copy the ellipsoid parameters
            _a = start.Ellipsoid.a;
            _es = start.Ellipsoid.ee;
            _sse = start.Ellipsoid._ee;

            // copy the points
            Start = start;
            Length = distance;
            Azimuth = azimuth;
        }
        #endregion

        #region properties
        /// <summary>
        /// start point
        /// </summary>
        [JsonProperty]
        public GeoPoint Start { get; set; }

        /// <summary>
        /// end point
        /// </summary>
        [JsonProperty]
        public GeoPoint End { get; set; }

        /// <summary>
        /// Length from start point to end point
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// Geodetic azimuth
        /// </summary>
        public Angle Azimuth { get; set; }

        /// <summary>
        /// Inverse geodetic azimuth
        /// </summary>
        public Angle InverseAzimuth { get; set; }
        #endregion

        /// <summary>
        /// get direction correction
        /// </summary>
        /// <returns>value of direction correction</returns>
        public Angle GetDirectionCorrection()
        {
            double Bm = (Start.Latitude.Radians + End.Latitude.Radians) / 2;
            double eta2 = _sse * Math.Pow(Math.Cos(Bm), 2);
            double t = Math.Tan(Bm);

            double cosB = Math.Cos(Bm);
            double V = Math.Sqrt(1 + _sse * cosB * cosB);
            double Rm = _a * Math.Sqrt(1 + _sse) / Math.Pow(V, 2);
            double Rm2 = Rm * Rm;
            
            GaussKrueger gauss = new GaussKrueger(new Ellipsoid(_a, 1 - Math.Sqrt(1 - _es)));
            gauss.Forward(Start.Latitude, Start.Longitude, out double x1, out double y1);
            gauss.Forward(End.Latitude, End.Longitude, out double x2, out double y2);

            //此处必须用自然坐标
            double ym = (GaussKrueger.GetNaturalCoord(y1) + GaussKrueger.GetNaturalCoord(y2)) / 2;

            // P199, (6-58)
            double delta = -(x2 - x1) * (2 * y1 + y2 - ym * ym * ym / Rm2) / (6 * Rm2) - eta2 * t * (y2 - y1) * ym * ym / (Rm * Rm2);

            return Angle.FromRadians(delta);
        }
    }
}
