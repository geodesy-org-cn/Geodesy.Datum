using System;
using Newtonsoft.Json;
using Geodesy.Datum.Coordinate;
using System.Collections.Generic;

namespace Geodesy.Datum.Earth.Projection
{
    /// <summary>
    /// The Lambert Conformal Conic projection is a standard projection for presenting maps of land areas 
	/// whose East-West extent is large compared with their North-South extent. This projection is "conformal" 
	/// in the sense that lines of latitude and longitude, which are perpendicular to one another 
	/// on the earth's surface, are also perpendicular to one another in the projected domain.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class LambertConformalConic2SP : MapProjection
    {
        /// <summary>
        /// EPSG Identifier
        /// </summary>
        public static readonly Identifier LCC2SP = new Identifier("EPSG", "9801", "Lambert Conic Conformal (2SP)", "Lambert tangent");

        // private auxiliary variables
        private double _n;
        private double _rho;
        private double _F;

        #region constructors
        /// <summary>
        /// 
        /// </summary>
        public LambertConformalConic2SP()
            : this(new Dictionary<ProjectionParameter, double>
            {
                { ProjectionParameter.Semi_Major, Settings.Ellipsoid.a },
                { ProjectionParameter.Inverse_Flattening, Settings.Ellipsoid.ivf },
            })
        { }

        /// <summary>
        /// Create a Lambert conformal conic projection instant.
        /// </summary>
        /// <param name="parameters">projection parameters</param>
        public LambertConformalConic2SP(Dictionary<ProjectionParameter, double> parameters)
            : base(parameters)
        {
            Identifier = LCC2SP;
            Surface = ProjectionSurface.Conical;
            Property = ProjectionProperty.Conformal;
            Orientation = ProjectionOrientation.Oblique;

            // set the optional parameters' value.
            if (OriginLatitude == null)
            {
                SetParameter(ProjectionParameter.Latitude_Of_Origin, 0.0);
            }
            if (CenteralMaridian == null)
            {
                SetParameter(ProjectionParameter.Central_Meridian, 0.0);
            }
            if (StandardParallel1 == null)
            {
                SetParameter(ProjectionParameter.Standard_Parallel_1, 0.0);
            }
            if (StandardParallel2 == null)
            {
                SetParameter(ProjectionParameter.Standard_Parallel_2, 0.0);
            }
            if (double.IsNaN(FalseEasting))
            {
                SetParameter(ProjectionParameter.False_Easting, 0.0);
            }
            if (double.IsNaN(FalseNorthing))
            {
                SetParameter(ProjectionParameter.False_Northing, 0.0);
            }

            ComputeConstants();
        }
        #endregion

        #region pravite methods
        /// <summary>
        /// compute some auxiliary variables' value for projection
        /// </summary>
        private void ComputeConstants()
        {
            double lat0 = OriginLatitude.Radians;
            double lat1 = StandardParallel1.Radians;
            double lat2 = StandardParallel2.Radians;

            // Standard Parallels cannot be equal and on opposite sides of the equator
            if (Math.Abs(lat1 + lat2) < Settings.Epsilon3)
            {
                throw new GeodeticException("Equal latitudes for St. Parallels on opposite sides of equator.");
            }

            double t0 = Get_t(lat0);
            double m1 = Get_m(lat1);
            double t1 = Get_t(lat1);
            double m2 = Get_m(lat2);
            double t2 = Get_t(lat2);

            if (Math.Abs(lat1 - lat2) > Settings.Epsilon3)
            {
                _n = Math.Log(m1 / m2) / Math.Log(t1 / t2);
            }
            else
            {
                _n = Math.Sin(lat1);
            }

            _F = m1 / (_n * Math.Pow(t1, _n));
            _rho = SemiMajor * _F * Math.Pow(t0, _n);
        }

        /// <summary>
        /// compute the static readonly ant t
        /// </summary>
        /// <param name="phi">latitude</param>
        /// <returns>static readonly ant t</returns>
        private double Get_t(double phi)
        {
            double e = Math.Sqrt(SquaredEccentricity);
            double temp = e * Math.Sin(phi);
            temp = Math.Pow((1.0 - temp) / (1.0 + temp), e / 2);
            return Math.Tan(0.25 * Math.PI - 0.5 * phi) / temp;
        }

        /// <summary>
        /// compute the static readonly ant m
        /// </summary>
        /// <param name="phi">latitude</param>
        /// <returns>static readonly ant m</returns>
        private double Get_m(double phi)
        {
            double con = Math.Sqrt(SquaredEccentricity) * Math.Sin(phi);
            return Math.Cos(phi) / Math.Sqrt(1.0 - con * con);
        }

        /// <summary>
        /// compute the latitude angle, phi2, for the reverse of the Lambert Conformal Conic
        /// and Polar Stereographic projections.
        /// </summary>
        /// <param name="t">Constant value t</param>
        /// <param name="flag">Error flag number</param>
        private double phi2z(double t, out long flag)
        {
            flag = 0;

            double e = Math.Sqrt(SquaredEccentricity);
            double chi = Math.PI / 2 - 2 * Math.Atan(t);

            for (int i = 0; i <= 15; i++)
            {
                double eSin = e * Math.Sin(chi);
                double dphi = Math.PI / 2 - 2 * Math.Atan(t * Math.Pow(((1.0 - eSin) / (1.0 + eSin)), e / 2)) - chi;

                chi += dphi;
                if (Math.Abs(dphi) <= 1E-10) return chi;
            }

            throw new GeodeticException("Convergence error - phi2z-conv");
        }
        #endregion

        #region direct solution and reverse solution
        /// <summary>
        /// converts geodetic coordinates to Lambert conformal conic projection coordinates.
        /// </summary>
        /// <param name="lng">longitude</param>
        /// <param name="lat">latitude</param>
        /// <param name="easting">easting</param>
        /// <param name="northing">northing</param>
        public override void Forward(Latitude lat, Longitude lng, out double northing, out double easting)
        {
            double rho;

            double rB = lat.Radians;
            double rL = lng.Radians;

            double temp = Math.Abs(Math.Abs(rB) - Math.PI / 2);
            if (temp > 1E-10)
            {
                double t = Get_t(rB);
                rho = SemiMajor * _F * Math.Pow(t, _n);
            }
            else
            {
                temp = rB * _n;
                if (temp <= 0) throw new GeodeticException("");
                rho = 0;
            }

            Longitude L = Longitude.FromRadians(rL - CenteralMaridian.Radians);
            L.Normalize();
            double gamma = _n * L.Radians;

            easting = rho * Math.Sin(gamma) + FalseEasting;
            northing = _rho - rho * Math.Cos(gamma) + FalseNorthing;
        }

        /// <summary>
        /// converts Lambert conformal conic projection coordinates to geodetic coordinates.
        /// </summary>
        /// <param name="easting">easting</param>
        /// <param name="northing">northing</param>
        /// <param name="lng">longitude</param>
        /// <param name="lat">latitude</param>
        public override void Reverse(double northing, double easting, out Latitude lat, out Longitude lng)
        {
            double rho;
            double temp;

            double dX = easting - FalseEasting;
            double dY = _rho - northing + FalseNorthing;
            if (_n > 0)
            {
                rho = Math.Sqrt(dX * dX + dY * dY);
                temp = 1.0;
            }
            else
            {
                rho = -Math.Sqrt(dX * dX + dY * dY);
                temp = -1.0;
            }

            double gamma = 0.0;
            if (rho != 0)
            {
                gamma = Math.Atan2((temp * dX), (temp * dY));
            }

            if ((rho != 0) || (_n > 0.0))
            {
                double t = Math.Pow((rho / (SemiMajor * _F)), 0.1 * _n);
                lat = Latitude.FromRadians(phi2z(t, out long flag));
                if (flag != 0) throw new ArgumentException();
            }
            else
            {
                lat = new Latitude(-90.0);
            }

            lng = Longitude.FromRadians(gamma / _n + CenteralMaridian.Radians);
            lng.Normalize();
        }
        #endregion
    }
}