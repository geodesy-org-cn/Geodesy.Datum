using System;
using Newtonsoft.Json;
using Geodesy.Datum.Coordinate;
using System.Collections.Generic;

namespace Geodesy.Datum.Earth.Projection
{
    /// <summary>
    /// The Cassini projection (also sometimes known as the Cassini–Soldner projection or Soldner projection) 
    /// is a map projection described by César-François Cassini de Thury in 1745.[2] It is the transverse 
    /// aspect of the equirectangular projection, in that the globe is first rotated so the central meridian 
    /// becomes the "equator", and then the normal equirectangular projection is applied.
    /// </summary>
    /// <remarks>
    /// https://en.wikipedia.org/wiki/Cassini_projection.
    /// </remarks>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class CassiniSoldner : MapProjection
    {
        /// <summary>
        /// EPSG Identifier
        /// </summary>
        public static readonly Identifier CASS = new Identifier("EPSG", "9806", "Cassini-Soldner", "CASS");

        /// <summary>
        /// distance of center meridian
        /// </summary>
        private readonly double _M0;

        /// <summary>
        /// 
        /// </summary>
        public CassiniSoldner()
            : this(new Dictionary<ProjectionParameter, double>
            {
                { ProjectionParameter.Semi_Major, Settings.Ellipsoid.a },
                { ProjectionParameter.Inverse_Flattening, Settings.Ellipsoid.ivf },
            })
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        public CassiniSoldner(Dictionary<ProjectionParameter, double> parameters)
            : base(parameters)
        {
            Identifier = CASS;
            Surface = ProjectionSurface.Cylindrical;
            Property = ProjectionProperty.Conformal;
            Orientation = ProjectionOrientation.Transverse;

            // set the optional parameters' value.
            if (OriginLatitude == null)
            {
                SetParameter(ProjectionParameter.Latitude_Of_Origin, 0.0);
            }
            if (CenteralMaridian == null)
            {
                SetParameter(ProjectionParameter.Central_Meridian, 0.0);
            }
            if (double.IsNaN(FalseEasting))
            {
                SetParameter(ProjectionParameter.False_Easting, 0.0);
            }
            if (double.IsNaN(FalseNorthing))
            {
                SetParameter(ProjectionParameter.False_Northing, 0.0);
            }

            _M0 = GetMeridionalDistance(OriginLatitude.Radians);
        }

        /// <summary>
        /// converts geodetic coordinates to Cassini Soldner projection coordinates.
        /// </summary>
        /// <param name="lat">latitude (phi)</param>
        /// <param name="lng">longitude (lamda)</param>
        /// <param name="northing">northing</param>
        /// <param name="easting">easting</param>
        public override void Forward(Latitude lat, Longitude lng, out double northing, out double easting)
        {
            double lamda = (lng - CenteralMaridian).Radians;
            double phi = lat.Radians;

            double sinPhi = Math.Sin(phi);
            double cosPhi = Math.Cos(phi);
            double tanPhi = Math.Tan(phi);

            double a = SemiMajor;
            double es = SquaredEccentricity;

            // If the Earth is sphere
            if (es == 0)
            {
                easting = a * Math.Asin(cosPhi * Math.Sin(lamda)) + FalseEasting;
                northing = a * Math.Atan2(tanPhi, Math.Cos(lamda)) - _M0 + FalseNorthing;
                return;
            }

            double N = 1 / Math.Sqrt(1 - es * sinPhi * sinPhi);
            double T = tanPhi * tanPhi;
            double A = lamda * cosPhi;
            double AA = A * A;
            double C = es * cosPhi * cosPhi / (1 - es);

            easting = N * A * (1.0 - AA * T / 6.0 - (8.0 - T + 8.0 * C) * AA * AA * T / 120.0);
            easting = easting * a + FalseEasting;

            northing = GetMeridionalDistance(phi) - _M0;
            northing += a * N * tanPhi * AA * (0.5 + (5.0 - T + 6.0 * C) * AA / 24.0);
            northing += FalseNorthing;
        }

        /// <summary>
        /// converts Cassini Soldner projection coordinates to geodetic coordinates.
        /// </summary>
        /// <param name="northing">northing</param>
        /// <param name="easting">easting</param>
        /// <param name="lat">latitude</param>
        /// <param name="lng">longitude</param>
        public override void Reverse(double northing, double easting, out Latitude lat, out Longitude lng)
        {
            double a = SemiMajor;
            double es = SquaredEccentricity;

            northing -= FalseNorthing - _M0;
            easting = (easting - FalseEasting) / a;

            double phi, lambda;

            // If the Earth is sphere
            if (es == 0)
            {
                northing /= a;
                phi = Math.Asin(Math.Sin(northing) * Math.Cos(easting));
                lambda = Math.Atan2(Math.Tan(easting), Math.Cos(northing));
            }
            else
            {
                double phi0 = GetMeridionalLatitude(northing);
                if (Math.Abs(Math.Abs(phi0) - Math.PI / 2) < Single.Epsilon)
                {
                    lat = Latitude.FromRadians(phi0);
                    lng = new Longitude(0);
                    return;
                }

                double sinPhi = Math.Sin(phi0);
                double cosPhi = Math.Cos(phi0);
                double tanPhi = Math.Tan(phi0);

                double T = tanPhi * tanPhi;
                double N = 1 / Math.Sqrt(1 - es * sinPhi * sinPhi);
                double D = easting / N;
                double DD = D * D;
                double R = (1 - es) * Math.Pow(N, 3);

                phi = phi0 - N * tanPhi * DD * (0.5 - (1 + 3 * T) * DD / 24.0) / R;
                lambda = D * (1 - T * DD / 3.0 + (1 + 3 * T) * T * DD * DD / 15.0) / cosPhi;
            }

            lat = Latitude.FromRadians(phi);
            lng = CenteralMaridian + Angle.FromRadians(lambda);
        }
    }
}
