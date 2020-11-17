using System;
using Newtonsoft.Json;
using Geodesy.Datum.Coordinate;
using System.Collections.Generic;

namespace Geodesy.Datum.Earth.Projection
{
    /// <summary>
    /// The polar stereographic projection is a particular mapping (function) that projects a sphere onto 
    /// a plane. The projection is defined on the entire sphere, except at one point: the projection point. 
    /// Where it is defined, the mapping is smooth and bijective. It is conformal, meaning that it preserves 
    /// angles at which curves meet. It is neither isometric nor area-preserving: that is, it preserves 
    /// neither distances nor the areas of figures.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class PolarStereographic : MapProjection
    {
        /// <summary>
        /// EPSG Identifier
        /// </summary>
        public static readonly Identifier STERE = new Identifier("EPSG", "9810", "Polar Stereographic", "STERE");

        private readonly double _k0;
        /// <summary>
        /// sign of south hemispere (-1) or north hemisphere (1)
        /// </summary>
        private readonly int _sign;

        /// <summary>
        /// 
        /// </summary>
        public PolarStereographic()
            : this(new Dictionary<ProjectionParameter, double>
            {
                { ProjectionParameter.Semi_Major, Settings.Ellipsoid.a },
                { ProjectionParameter.Inverse_Flattening, Settings.Ellipsoid.ivf },
            })
        { }

        /// <summary>
        /// Create a polar stereographic projection instant.
        /// </summary>
        /// <param name="parameters">projection parameters</param>
        public PolarStereographic(Dictionary<ProjectionParameter, double> parameters)
            : base(parameters)
        {
            Identifier = STERE;
            Surface = ProjectionSurface.Azimuthal;
            Property = ProjectionProperty.Conformal;
            Orientation = ProjectionOrientation.Tangent;

            // set the optional parameters' value.
            if (OriginLatitude == null)
            {
                SetParameter(ProjectionParameter.Latitude_Of_Origin, 0.0);
            }
            if (CenteralMaridian == null)
            {
                SetParameter(ProjectionParameter.Central_Meridian, 0.0);
            }
            if (double.IsNaN(ScaleFactor))
            {
                SetParameter(ProjectionParameter.Scale_Factor, 1.0);
            }
            if (double.IsNaN(FalseEasting))
            {
                SetParameter(ProjectionParameter.False_Easting, 0.0);
            }
            if (double.IsNaN(FalseNorthing))
            {
                SetParameter(ProjectionParameter.False_Northing, 0.0);
            }

            double phi = OriginLatitude.Radians;
            _sign = Math.Sign(phi);
            double e = Math.Sqrt(SquaredEccentricity);
            if (Math.Abs(phi) != Math.PI / 2)
            {
                double esin = e * Math.Sin(phi);
                double tf = Math.Tan((Math.PI / 2 + phi) / 2) / Math.Pow((1 + esin) / (1 - esin), e / 2);
                double mf = Math.Cos(phi) / Math.Sqrt(1 - esin * esin);
                _k0 = mf * Math.Sqrt(Math.Pow(1 + e, 1 + e) * Math.Pow(1 - e, 1 - e)) / 2 / tf;
            }
            else
            {
                _k0 = ScaleFactor;
            }
        }

        /// <summary>
        /// Get the coefficients for the reverse Mercator projection associated with the ellipsoid.
        /// </summary>
        /// <returns>coefficients array</returns>
        private double[] GetInverseCoefficients()
        {
            double e2 = SquaredEccentricity;
            double e4 = e2 * e2;
            double e6 = e4 * e2;
            double e8 = e4 * e4;

            double[] inv_merc_coeff = new double[5];
            inv_merc_coeff[0] = 1.0;
            inv_merc_coeff[1] = e2 * 1 / 2 + e4 * 5 / 24 + e6 * 1 / 12 + e8 * 13 / 360;
            inv_merc_coeff[2] = e4 * 7 / 48 + e6 * 29 / 240 + e8 * 811 / 11520;
            inv_merc_coeff[3] = e6 * 7 / 120 + e8 * 81 / 1120;
            inv_merc_coeff[4] = e8 * 4279 / 161280;

            return inv_merc_coeff;
        }

        /// <summary>
        /// converts geodetic coordinates to Albers equal-area conic projection coordinates.
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lng">longitude</param>
        /// <param name="northing">northing</param>
        /// <param name="easting">easting</param>
        public override void Forward(Latitude lat, Longitude lng, out double northing, out double easting)
        {
            double phi = lat.Radians;
            double e = Math.Sqrt(SquaredEccentricity);
            double esin = e * Math.Sin(phi);

            double t;
            if (_sign < 0)
            {
                t = Math.Tan(Math.PI / 4 + phi / 2) / Math.Pow((1 + esin) / (1 - esin), e / 2);
            }
            else
            {
                t = Math.Tan(Math.PI / 4 - phi / 2) * Math.Pow((1 + esin) / (1 - esin), e / 2);
            }

            double rho = 2 * SemiMajor * _k0 * t / Math.Sqrt(Math.Pow(1 + e, 1 + e) * Math.Pow(1 - e, 1 - e));

            double lambda = lng.Radians - CenteralMaridian.Radians;
            easting = FalseEasting + rho * Math.Sin(lambda);

            double north = rho * Math.Cos(lambda);
            northing = FalseNorthing - north * _sign;
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
            double east = easting - FalseEasting;
            double north = northing - FalseNorthing;

            double e = Math.Sqrt(SquaredEccentricity);
            double rho = Math.Sqrt(east * east + north * north);
            double t = rho * Math.Sqrt(Math.Pow(1 + e, 1 + e) * Math.Pow(1 - e, 1 - e)) / 2 / SemiMajor / _k0;

            double ki = _sign * (Math.PI / 2 - 2 * Math.Atan(t));

            double phi = ki;
            double[] coeff = GetInverseCoefficients();
            for (int i = 1; i < 5; i++)
            {
                phi += coeff[i] * Math.Sin(2 * i * ki);
            }
            lat = Latitude.FromRadians(phi);

            double lambda = Math.Atan2(east, -_sign * north);
            lng = CenteralMaridian + Angle.FromRadians(lambda);
        }
    }
}
