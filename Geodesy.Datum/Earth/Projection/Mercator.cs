using System;
using Newtonsoft.Json;
using Geodesy.Datum.Coordinate;
using System.Collections.Generic;

namespace Geodesy.Datum.Earth.Projection
{
    /// <summary>
    /// The Mercator projection is a cylindrical map projection presented by Flemish geographer and 
    /// cartographer Gerardus Mercator in 1569. It became the standard map projection for navigation 
    /// because of its unique property of representing any course of constant bearing as a straight segment.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class Mercator : MapProjection
    {
        /// <summary>
        /// EPSG Identifier
        /// </summary>
        public static readonly Identifier MERC = new Identifier("EPSG", "9804", "Mercator (1SP)", "MERC");

        /// <summary>
        /// 
        /// </summary>
        private readonly double _k0;

        /// <summary>
        /// 
        /// </summary>
        public Mercator()
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
        public Mercator(Dictionary<ProjectionParameter, double> parameters)
            : base(parameters)
        {
            Surface = ProjectionSurface.Cylindrical;
            Property = ProjectionProperty.Conformal;

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

            // This is a two standard parallel Mercator projection (2SP)
            if (double.IsNaN(ScaleFactor))
            {
                Identifier.Name = "Mercator_2SP";
                double rB = OriginLatitude.Radians;
                _k0 = Math.Cos(rB) / Math.Sqrt(1.0 - SquaredEccentricity * Math.Sin(rB) * Math.Sin(rB));
            }
            else //This is a one standard parallel Mercator projection (1SP)
            {
                Identifier.Name = "Mercator_1SP";
                _k0 = ScaleFactor;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="northing"></param>
        /// <param name="easting"></param>
        public override void Forward(Latitude lat, Longitude lng, out double northing, out double easting)
        {
            if (Math.Abs(lat.Degrees - 90) <= double.Epsilon)
            {
                throw new GeodeticException("Transformation cannot be computed at the poles.");
            }

            double a = SemiMajor;
            double e = Math.Sqrt(SquaredEccentricity);

            double phi = lat.Radians;
            double esinPhi = e * Math.Sin(phi);

            easting = a * _k0 * (phi - CenteralMaridian.Radians);
            northing = a * _k0 * Math.Log(Math.Tan(Math.PI * 0.25 + phi * 0.5) *
                                  Math.Pow((1 - esinPhi) / (1 + esinPhi), e * 0.5));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="northing"></param>
        /// <param name="easting"></param>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        public override void Reverse(double northing, double easting, out Latitude lat, out Longitude lng)
        {
            double a = SemiMajor;
            double es = SquaredEccentricity;

            double dX = easting; //  - _falseEasting;
            double dY = northing; // - _falseNorthing;
            double ts = Math.Exp(-dY / (a * _k0)); //t

            double chi = Math.PI / 2 - 2 * Math.Atan(ts);
            double e4 = es * es;
            double e6 = es * e4;
            double e8 = e4 * e4;

            double phi = chi + (es * 0.5 + 5 * e4 / 24 + e6 / 12 + 13 * e8 / 360) * Math.Sin(2 * chi)
                    + (7 * e4 / 48 + 29 * e6 / 240 + 811 * e8 / 11520) * Math.Sin(4 * chi) +
                    +(7 * e6 / 120 + 81 * e8 / 1120) * Math.Sin(6 * chi) +
                    +(4279 * e8 / 161280) * Math.Sin(8 * chi);

            lat = Latitude.FromRadians(phi);
            lng = CenteralMaridian + Angle.FromRadians(dX / (a * _k0));
        }
    }
}