using System;
using Newtonsoft.Json;
using Geodesy.Datum.Coordinate;
using System.Collections.Generic;

namespace Geodesy.Datum.Earth.Projection
{
    /// <summary>
    /// The Albers equal-area conic projection, or Albers projection (named after Heinrich C. Albers), 
    /// is a conic, equal area map projection that uses two standard parallels. Although scale and shape 
    /// are not preserved, distortion is minimal between the standard parallels.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class AlbersEqualAreaConic : MapProjection
    {
        /// <summary>
        /// EPSG Identifier
        /// </summary>
        public static readonly Identifier AEA = new Identifier("EPSG", "9822", "Albers Equal Area", "AEA");

        /// <summary>
        /// the default value of the first and second standard parallel.
        /// </summary>
        public const double Standard_Parallel = 0.0;

        // private auxiliary variables
        private double _C;
        private double _n;
        private double _rho0;

        #region constructors
        /// <summary>
        /// Create a Albers equal-area conic projection instant.
        /// </summary>
        public AlbersEqualAreaConic()
            :this(new Dictionary<ProjectionParameter, double> 
            {
                { ProjectionParameter.Semi_Major, Settings.Ellipsoid.a },
                { ProjectionParameter.Inverse_Flattening, Settings.Ellipsoid.ivf },
                { ProjectionParameter.Standard_Parallel_1, Standard_Parallel },
                { ProjectionParameter.Standard_Parallel_2, Standard_Parallel },
            })
        { }

        /// <summary>
        /// Create a Albers equal-area conic projection instant.
        /// </summary>
        /// <param name="parameters">projection parameters</param>
        public AlbersEqualAreaConic(Dictionary<ProjectionParameter, double> parameters)
            : base(parameters)
        {
            Identifier = AEA;
            Surface = ProjectionSurface.Conical;
            Property = ProjectionProperty.EqualArea;
            Orientation = ProjectionOrientation.Transverse;

            // check the required parameters
            CheckParameters(ProjectionParameter.Standard_Parallel_1, ProjectionParameter.Standard_Parallel_2);

            // set the optional parameters' value.
            if (CenteralMaridian == null)
            {
                SetParameter(ProjectionParameter.Central_Meridian, 0.0);
            }
            if (OriginLatitude == null)
            {
                SetParameter(ProjectionParameter.Latitude_Of_Origin, 0.0);
            }
            if (double.IsNaN(FalseEasting))
            {
                SetParameter(ProjectionParameter.False_Easting, 0.0);
            }
            if (double.IsNaN(FalseNorthing))
            {
                SetParameter(ProjectionParameter.False_Northing, 0.0);
            }

            // compute private constants
            ComputeConstants();
        }
        #endregion

        #region pravite methods
        /// <summary>
        /// An auxiliary variable
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <returns></returns>
        private double alpha(double lat)
        {
            double sin = Math.Sin(lat);
            double sin2 = sin * sin;
            double e2 = SquaredEccentricity;
            double e = Math.Sqrt(e2);

            return (1 - e2) * (sin / (1 - e2 * sin2) - Math.Log((1 - e * sin) / (1 + e * sin)) / (2 * e));
        }

        /// <summary>
        /// compute some auxiliary variables' value for projection
        /// </summary>
        private void ComputeConstants()
        {
            double lat1 = StandardParallel1.Radians;
            double lat2 = StandardParallel2.Radians;

            if (lat1 + lat2 < double.Epsilon)
            {
                throw new GeodeticException("Equal latitudes for standard parallels on opposite sides of Equator.");
            }

            double alpha1 = alpha(lat1);
            double alpha2 = alpha(lat2);

            double es = SquaredEccentricity;
            double m1 = Math.Cos(lat1) / Math.Sqrt(1 - es * Math.Pow(Math.Sin(lat1), 2));
            double m2 = Math.Cos(lat2) / Math.Sqrt(1 - es * Math.Pow(Math.Sin(lat2), 2));

            _n = (Math.Pow(m1, 2) - Math.Pow(m2, 2)) / (alpha2 - alpha1);
            _C = Math.Pow(m1, 2) + (_n * alpha1);

            double lat0 = OriginLatitude.Radians;
            _rho0 = SemiMajor * Math.Sqrt(_C - _n * alpha(lat0)) / _n;
        }
        #endregion

        /// <summary>
        /// converts geodetic coordinates to Albers equal-area conic projection coordinates.
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lng">longitude</param>
        /// <param name="northing">northing</param>
        /// <param name="easting">easting</param>
        public override void Forward(Latitude lat, Longitude lng, out double northing, out double easting)
        {
            double rho = SemiMajor * Math.Sqrt((_C - _n * alpha(lat.Radians))) / _n;
            double theta = _n * (lng - CenteralMaridian).Radians;

            easting = FalseEasting + rho * Math.Sin(theta);
            northing = FalseNorthing + _rho0 - (rho * Math.Cos(theta));
        }

        /// <summary>
        /// converts Lambert equal-area conic projection coordinates to geodetic coordinates.
        /// </summary>
        /// <param name="northing">northing</param>
        /// <param name="easting">easting</param>
        /// <param name="lat">latitude</param>
        /// <param name="lng">longitude</param>
        public override void Reverse(double northing, double easting, out Latitude lat, out Longitude lng)
        {
            double theta = Math.Atan((easting - FalseEasting) / (_rho0 - (northing - FalseNorthing)));
            double rho = Math.Sqrt(Math.Pow(easting - FalseEasting, 2) + Math.Pow(_rho0 - (northing - FalseNorthing), 2));
            double q = (_C - rho * rho * _n * _n / Math.Pow(SemiMajor, 2)) / _n;

            int counter = 0;
            double B = Math.Asin(q * 0.5);
            double Bx = double.MaxValue;


            double es = SquaredEccentricity;
            double e = Math.Sqrt(es);
            while (Math.Abs(B - Bx) > Settings.Epsilon5)
            {
                Bx = B;
                double sin = Math.Sin(Bx);
                double es2 = es * Math.Pow(sin, 2);
                B += Math.Pow(1 - es2, 2) / (2 * Math.Cos(Bx)) * (q / (1 - es) - sin / (1 - es2)
                                + 1 / (2 * e) * Math.Log((1 - e * sin) / (1 + e * sin)));
                counter++;
                if (counter > 25)
                {
                    throw new GeodeticException("Transformation failed to converge in Albers backwards transformation");
                }
            }

            lat = new Latitude(B, Angle.DataStyle.Radians);
            lng = CenteralMaridian + new Angle(theta / _n, Angle.DataStyle.Radians);
        }
    }
}
