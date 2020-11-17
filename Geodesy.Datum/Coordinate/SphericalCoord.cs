using System;
using Newtonsoft.Json;
using Geodesy.Datum.Units;

namespace Geodesy.Datum.Coordinate
{
    /// <summary>
    /// Spherical coordinate system is a coordinate system for three-dimensional space where 
    /// the position of a point.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class SphericalCoord
    {
        /// <summary>
        /// Create a null spherical coordinate object.
        /// </summary>
        public SphericalCoord()
        {
            Radius = double.NaN;
        }

        /// <summary>
        /// Create a spherical coordinate object.
        /// </summary>
        /// <param name="radius">radial distance</param>
        /// <param name="polar">polar angle</param>
        /// <param name="azimuth">azimuth angle</param>
        public SphericalCoord(double radius, Angle polar, Angle azimuth)
        {
            Radius = radius;
            if (Radius < 0)
            {
                throw new GeodeticException("Error radial value");
            }

            Polar = polar;
            Polar.Normalize();
            if (Polar > Angle.Pi)
            {
                throw new GeodeticException("Error polar angle");
            }

            Azimuth = azimuth;
            Azimuth.Normalize();
        }

        /// <summary>
        /// get/set radial distance
        /// </summary>
        [JsonProperty]
        public double Radius { get; set; }

        /// <summary>
        /// get/set polar angle
        /// </summary>
        [JsonProperty]
        [JsonConverter(typeof(AngleJsonConverter<Angle>))]
        public Angle Polar { get; set; }

        /// <summary>
        /// get/set azimuth angle
        /// </summary>
        [JsonProperty]
        [JsonConverter(typeof(AngleJsonConverter<Angle>))]
        public Angle Azimuth { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public AngularUnit AngularUnit { get; } = Settings.BaseAngularUnit;

        /// <summary>
        /// 
        /// </summary>
        public LinearUnit LinearUnit { get; set; } = Settings.BaseLinearUnit;

        /// <summary>
        /// get/set radial distance
        /// </summary>
        public double ρ
        {
            get => Radius;
            set => Radius = value;
        }

        /// <summary>
        /// get/set polar angle
        /// </summary>
        public Angle θ
        {
            get => Polar;
            set => Polar = value;
        }

        /// <summary>
        /// get/set azimuth angle
        /// </summary>
        public Angle λ
        {
            get => Azimuth;
            set => Azimuth = value;
        }

        /// <summary>
        /// Set the spherical coordinate value.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="polar"></param>
        /// <param name="azimuth"></param>
        public void SetCoordinate(double radius, Angle polar, Angle azimuth)
        {
            Radius = radius;
            Polar = polar;
            Azimuth = azimuth;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="linearUnit"></param>
        /// <param name="polar"></param>
        /// <param name="azimuth"></param>
        /// <param name="angularUnit"></param>
        public void SetCoordinate(double radius, LinearUnit linearUnit, double polar, double azimuth, AngularUnit angularUnit)
        {
            Radius = radius;
            LinearUnit = linearUnit;

            Polar = new Angle(polar, angularUnit);
            Azimuth = new Angle(azimuth, angularUnit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString(Angle.DataStyle.DD_MM_SSssss);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        public string ToString(Angle.DataStyle style)
        {
            string temp = string.Empty;

            if (!double.IsNaN(Radius))
            {
                temp += "ρ:" + Radius.ToString("# ###.###");
            }

            if (Polar != null)
            {
                temp += ", θ:" + Polar.ToString(style) + ", λ:" + Azimuth.ToString(style);
            }

            return temp;
        }
    }
}