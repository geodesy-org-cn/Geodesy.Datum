using System;
using Newtonsoft.Json;
using Geodesy.Datum.Units;

namespace Geodesy.Datum.Coordinate
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class PolarCoord : IPolarCoord
    {
        /// <summary>
        /// 
        /// </summary>
        public PolarCoord()
        {
            Range = double.NaN;
            Azimuth = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="range"></param>
        /// <param name="azimuth"></param>
        public PolarCoord(double range, Angle azimuth)
        {
            Range = range;
            Azimuth = azimuth;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(Order =0)]
        public double Range { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(Order =1)]
        [JsonConverter(typeof(AngleJsonConverter<Angle>))]
        public Angle Azimuth { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public LinearUnit LinearUnit { get; set; } = Settings.BaseLinearUnit;

        /// <summary>
        /// 
        /// </summary>
        public AngularUnit AngularUnit { get; } = Settings.BaseAngularUnit;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="range"></param>
        /// <param name="azimuth"></param>
        public void SetCoordinate(double range, Angle azimuth)
        {
            Range = range;
            Azimuth = azimuth;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="range"></param>
        /// <param name="lengthUnit"></param>
        /// <param name="azimuth"></param>
        /// <param name="angleUnit"></param>
        public void SetCoordinate(double range, LinearUnit lengthUnit, double azimuth, AngularUnit angleUnit)
        {
            Range = range;
            LinearUnit = lengthUnit;
            Azimuth = new Angle(azimuth, angleUnit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Azimuth == null) return string.Empty;

            return "S:" + Range.ToString("0.####") + ", A:" + Azimuth.ToString(Angle.DataStyle.DD_MM_SSssss);
        }
    }
}
