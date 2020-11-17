using System;
using Newtonsoft.Json;
using Geodesy.Datum.Coordinate;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;

namespace Geodesy.Datum.Earth.Projection
{
    /// <summary>
    /// abstract class of geodetic map projection
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class MapProjection : IMapProjection
    {
        /// <summary>
        /// projection parameters
        /// </summary>
        [JsonProperty(PropertyName = "Parameters")]
        //[JsonConverter(typeof(ParameterJsonConvertor))]
        private readonly Dictionary<ProjectionParameter, double> _parameters = new Dictionary<ProjectionParameter, double>();

        #region constructor
        /// <summary>
        /// 
        /// </summary>
        public MapProjection()
            : this(new Dictionary<ProjectionParameter, double>
            {
                { ProjectionParameter.Semi_Major, Settings.Ellipsoid.a },
                { ProjectionParameter.Inverse_Flattening,Settings.Ellipsoid.ivf },
            })
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        public MapProjection(Dictionary<ProjectionParameter, double> parameters)
        {
            _parameters = parameters;
            CheckParameters(ProjectionParameter.Semi_Major, ProjectionParameter.Inverse_Flattening);
        }
        #endregion

        /// <summary>
        /// Identifier of projection
        /// </summary>
        [JsonProperty]
        public Identifier Identifier { get; set; }

        /// <summary>
        /// projection surface
        /// </summary>
        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public ProjectionSurface Surface { get; protected set; }

        /// <summary>
        /// Preservation of projection
        /// </summary>
        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public ProjectionProperty Property { get; protected set; }

        /// <summary>
        /// projection orientation
        /// </summary>
        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public ProjectionOrientation Orientation { get; protected set; }

        #region projection parameters
        /// <summary>
        /// get parameter count
        /// </summary>
        /// <returns></returns>
        public int GetParameterCount()
        {
            return _parameters.Count;
        }

        /// <summary>
        /// get parameter by standard key
        /// </summary>
        /// <param name="para">standard key</param>
        /// <returns>parameter</returns>
        public double GetParameter(ProjectionParameter para)
        {
            return _parameters.ContainsKey(para) ? _parameters[para] : double.NaN;
        }

        /// <summary>
        /// set the parameter value
        /// </summary>
        /// <param name="key">parameter key</param>
        /// <param name="value">parameter value</param>
        public void SetParameter(ProjectionParameter para, double value)
        {
            if (_parameters.ContainsKey(para))
            {
                _parameters[para] = value;
            }
            else
            {
                _parameters.Add(para, value);
            }
        }

        /// <summary>
        /// semi-major of ellipsoid
        /// </summary>
        public double SemiMajor => GetParameter(ProjectionParameter.Semi_Major);

        /// <summary>
        /// inverse flattening of ellipsoid. If IverseFlattening is nagetive, it means 
        /// the earth is approximated a sphere.
        /// </summary>
        public double InverFlattening => GetParameter(ProjectionParameter.Inverse_Flattening);

        /// <summary>
        /// the squared first eccentricity
        /// </summary>
        protected double SquaredEccentricity
        {
            get
            {
                double ivf = InverFlattening;
                // If ivf <= 0, it means the earth is sphere.
                return (ivf <= 0) ? 0 : (2 * ivf - 1) / ivf / ivf;
            }
        }

        /// <summary>
        /// central meridian of projection
        /// </summary>
        public Longitude CenteralMaridian
        {
            get
            {
                double value = GetParameter(ProjectionParameter.Central_Meridian);
                return double.IsNaN(value) ? null : new Longitude(value);
            }
        }

        /// <summary>
        /// the reference latitude of projection
        /// </summary>
        public Latitude OriginLatitude
        {
            get
            {
                double value = GetParameter(ProjectionParameter.Latitude_Of_Origin);
                if (double.IsNaN(value))
                {
                    //Allow for altenative name
                    value = GetParameter(ProjectionParameter.True_Scale_Latitude);
                    return double.IsNaN(value) ? null : new Latitude(value);
                }
                else
                {
                    return new Latitude(value);
                }
            }
        }

        /// <summary>
        /// the first standard parallel of secant conformal conic projections
        /// </summary>
        public Latitude StandardParallel1
        {
            get
            {
                double value = GetParameter(ProjectionParameter.Standard_Parallel_1);
                return double.IsNaN(value) ? null : new Latitude(value);
            }
        }

        /// <summary>
        /// the second standard parallel of secant conformal conic projections
        /// </summary>
        public Latitude StandardParallel2
        {
            get
            {
                double value = GetParameter(ProjectionParameter.Standard_Parallel_2);
                return double.IsNaN(value) ? null : new Latitude(value);
            }
        }

        /// <summary>
        /// false easting of projection
        /// </summary>
        public double FalseEasting => GetParameter(ProjectionParameter.False_Easting);

        /// <summary>
        /// false northing of projection
        /// </summary>
        public double FalseNorthing => GetParameter(ProjectionParameter.False_Northing);

        /// <summary>
        /// scale factor
        /// </summary>
        public double ScaleFactor => GetParameter(ProjectionParameter.Scale_Factor);

        /// <summary>
        /// width of zone
        /// </summary>
        public double WidthOfZone => GetParameter(ProjectionParameter.Width_Of_Zone);

        /// <summary>
        /// azimuth
        /// </summary>
        public Angle Azimuth
        {
            get
            {
                double value = GetParameter(ProjectionParameter.Azimuth);
                return double.IsNaN(value) ? null : new Angle(value);
            }
        }

        /// <summary>
        /// rectified grid angle
        /// </summary>
        public double RectifiedGridAngle => GetParameter(ProjectionParameter.Rectified_Grid_Angle);
        #endregion

        #region protected methods
        /// <summary>
        /// Get the coefficients of meridian length computing.
        /// </summary>
        /// <returns></returns>
        private double[] GetCoefficients()
        {
            double[] coeff = new double[6];

            double e2 = SquaredEccentricity;
            double e4 = e2 * e2;
            double e6 = e2 * e4;
            double e8 = e4 * e4;
            double e10 = e4 * e6;

            coeff[0] = 1 + 3 * e2 / 4 + 45 * e4 / 64 + 175 * e6 / 256 + 11025 * e8 / 16384 + 43659 * e10 / 65536;
            coeff[1] = 3 * e2 / 4 + 15 * e4 / 16 + 525 * e6 / 512 + 2205 * e8 / 2048 + 72765 * e10 / 65536;
            coeff[2] = 15 * e4 / 64 + 105 * e6 / 256 + 2205 * e8 / 4096 + 10395 * e10 / 16384;
            coeff[3] = 35 * e6 / 512 + 315 * e8 / 2048 + 31185 * e10 / 131072;
            coeff[4] = 315 * e8 / 16384 + 3465 * e10 / 65536;
            coeff[5] = 693 * e10 / 131072;

            return coeff;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected bool CheckParameters(params ProjectionParameter[] parameters)
        {
            foreach (ProjectionParameter para in parameters)
            {
                if (double.IsNaN(GetParameter(para)))
                {
                    throw new GeodeticException("Missing projection parameter '" + para.ToString() + "'.");
                }
            }

            return true;
        }

        /// <summary>
        /// Get the meridional distance
        /// </summary>
        /// <param name="lat">latitude in radians</param>
        /// <returns>meridian distance</returns>
        protected double GetMeridionalDistance(double lat)
        {
            double[] coeff = GetCoefficients();
            return SemiMajor * (1 - SquaredEccentricity) * (coeff[0] * lat -
                                           coeff[1] * Math.Sin(2 * lat) / 2 +
                                           coeff[2] * Math.Sin(4 * lat) / 4 -
                                           coeff[3] * Math.Sin(6 * lat) / 6 +
                                           coeff[4] * Math.Sin(8 * lat) / 8 -
                                           coeff[5] * Math.Sin(10 * lat) / 10);
        }

        /// <summary>
        /// Iterated for solve latitude from meridional distance
        /// </summary>
        /// <param name="distance">meridian distance</param>
        /// <returns>latitude in radians</returns>
        protected double GetMeridionalLatitude(double distance)
        {
            double[] coeff = GetCoefficients();
            double rB = distance / SemiMajor / (1 - SquaredEccentricity) / coeff[0];
            double rB0 = rB + 0.1;
            while (Math.Abs(rB - rB0) > Settings.Epsilon5)
            {
                rB0 = rB;
                rB = (distance / SemiMajor / (1 - SquaredEccentricity) + coeff[1] * Math.Sin(2 * rB0) / 2 -
                                                        coeff[2] * Math.Sin(4 * rB0) / 4 +
                                                        coeff[3] * Math.Sin(6 * rB0) / 6 -
                                                        coeff[4] * Math.Sin(8 * rB0) / 8 +
                                                        coeff[5] * Math.Sin(10 * rB0) / 10) / coeff[0];
            }

            return rB;
        }

        /// <summary>
        /// Get the geocentric latitude from geodetic latitude. The geocentric latitude 
        /// (also called planetocentric latitude in the context of non-Earth bodies) 
        /// is the angle between the equatorial plane and a line joining the body centre 
        /// to the considered point.
        /// </summary>
        /// <param name="lat">geodetic latitude</param>
        /// <returns>geocentric latitude</returns>
        protected double GetGeocentricLatitude(double lat)
        {
            // It is called Reduced Latitude in <geodesy>.
            return Math.Atan((1 - SquaredEccentricity) * Math.Tan(lat));
        }

        /// <summary>
        /// Calculate the parameter M
        /// </summary>
        /// <param name="lat">latitude in radian</param>
        /// <returns>M</returns>
        protected double GetM(double lat)
        {
            double es = SquaredEccentricity;
            double sqe2 = es / (1 - es);
            double cosB = Math.Cos(lat);
            double V2 = 1 + sqe2 * cosB * cosB;
            return SemiMajor * Math.Sqrt(1 + sqe2) / Math.Pow(V2, 1.5);
        }

        /// <summary>
        /// Calculate the parameter N
        /// </summary>
        /// <param name="lat">latitude in radian</param>
        /// <returns>N</returns>
        protected double GetN(double lat)
        {
            double sinB = Math.Sin(lat);
            return SemiMajor / Math.Sqrt(1 - SquaredEccentricity * sinB * sinB);
        }
        #endregion

        #region direct and reverse abstract solution
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lng"></param>
        /// <param name="lat"></param>
        /// <param name="easting"></param>
        /// <param name="northing"></param>
        public abstract void Forward(Latitude lat, Longitude lng, out double northing, out double easting);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="easting"></param>
        /// <param name="northing"></param>
        /// <param name="lng"></param>
        /// <param name="lat"></param>
        public abstract void Reverse(double northing, double easting, out Latitude lat, out Longitude lng);
        #endregion

        #region public methods
        /// <summary>
        /// projection direct solution from geodetic coordinate to projected coordinate.
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lng">longitude</param>
        /// <returns>projected coordinate</returns>
        public (double northing, double easting) Forward(Latitude lat, Longitude lng)
        {
            Forward(lat, lng, out double north, out double east);
            return (north, east);
        }

        /// <summary>
        /// projection direct solution from geodetic coordinate to projected coordinate.
        /// </summary>
        /// <param name="point">geodetic point</param>
        /// <returns>projected point</returns>
        public ProjectedCoord Forward(GeodeticCoord point)
        {
            Forward(point.Latitude, point.Longitude, out double northing, out double easting);
            return new ProjectedCoord(northing, easting);
        }

        /// <summary>
        /// projection direct solution from geodetic coordinate to projected coordinate.
        /// </summary>
        /// <param name="points">geodetic points collection</param>
        /// <returns>projected point collection</returns>
        public List<ProjectedCoord> Forward(List<GeodeticCoord> points)
        {
            List<ProjectedCoord> pts = new List<ProjectedCoord>();

            foreach (GeodeticCoord pnt in points)
            {
                Forward(pnt.Latitude, pnt.Longitude, out double northing, out double easting);
                pts.Add(new ProjectedCoord(northing, easting));
            }

            return pts;
        }

        /// <summary>
        /// projection direct solution from geographic coordinate to projected coordinate.
        /// </summary>
        /// <param name="point">geographic point</param>
        /// <returns>projected point</returns>
        public ProjectedCoord Forward(GeographicCoord point)
        {
            Forward(point.Latitude, point.Longitude, out double northing, out double easting);
            return new ProjectedCoord(northing, easting);
        }

        /// <summary>
        /// projection direct solution from geodetic coordinate to projected coordinate.
        /// </summary>
        /// <param name="points">geographic points collection</param>
        /// <returns>projected point collection</returns>
        public List<ProjectedCoord> Forward(List<GeographicCoord> points)
        {
            List<ProjectedCoord> pts = new List<ProjectedCoord>();

            foreach (GeographicCoord pnt in points)
            {
                Forward(pnt.Latitude, pnt.Longitude, out double northing, out double easting);
                pts.Add(new ProjectedCoord(northing, easting));
            }

            return pts;
        }

        /// <summary>
        /// projection reverse solution from projected coordinate to geographic coordinate.
        /// </summary>
        /// <param name="northing">projected northing</param>
        /// <param name="easting">projected easting</param>
        /// <returns>geographic coordinate</returns>
        public (Latitude lat, Longitude lng) Reverse(double northing, double easting)
        {
            Reverse(northing, easting, out Latitude B, out Longitude L);
            return (B, L);
        }

        /// <summary>
        /// projection reverse solution from projected coordinate to geographic coordinate.
        /// </summary>
        /// <param name="point">projected point</param>
        /// <returns>geodetic point</returns>
        public GeographicCoord Reverse(ProjectedCoord point)
        {
            Reverse(point.Northing, point.Easting, out Latitude lat, out Longitude lng);
            return new GeographicCoord(lat, lng);
        }

        /// <summary>
        /// projection reverse solution from projected coordinate to geodetic coordinate.
        /// </summary>
        /// <param name="points">projected point collection</param>
        /// <returns>geodetic point collection</returns>
        public List<GeographicCoord> Reverse(List<ProjectedCoord> points)
        {
            List<GeographicCoord> pts = new List<GeographicCoord>();

            foreach (ProjectedCoord pnt in points)
            {
                Reverse(pnt.Northing, pnt.Easting, out Latitude lat, out Longitude lng);
                pts.Add(new GeographicCoord(lat, lng));
            }

            return pts;
        }
        #endregion
    }
}