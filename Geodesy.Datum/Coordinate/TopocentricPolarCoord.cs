using System;
using Newtonsoft.Json;
using Geodesy.Datum.Units;

namespace Geodesy.Datum.Coordinate
{
    /// <summary>
    /// Topocentric polar coordinate
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class TopocentricPolarCoord : PolarCoord
    {
        /// <summary>
        /// 
        /// </summary>
        public TopocentricPolarCoord()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="range"></param>
        /// <param name="azimuth"></param>
        /// <param name="elevation"></param>
        public TopocentricPolarCoord(double range, Angle azimuth, Angle elevation)
        {
            Range = range;
            Azimuth = azimuth;
            Elevation = elevation;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty]
        [JsonConverter(typeof(AngleJsonConverter<Angle>))]
        public Angle Elevation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="range"></param>
        /// <param name="azimuth"></param>
        /// <param name="elevation"></param>
        public void SetCoordinate(double range, Angle azimuth, Angle elevation)
        {
            Range = range;
            Azimuth = azimuth;
            Elevation = elevation;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="range"></param>
        /// <param name="linearUnit"></param>
        /// <param name="azimuth"></param>
        /// <param name="elevation"></param>
        /// <param name="angularUnit"></param>
        public void SetCoordinate(double range, LinearUnit linearUnit, double azimuth, double elevation, AngularUnit angularUnit)
        {
            Range = range;
            LinearUnit = linearUnit;

            Azimuth = new Angle(azimuth, angularUnit);
            Elevation = new Angle(elevation, angularUnit);
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

            if (!double.IsNaN(Range))
            {
                temp += "R:" + Range.ToString("# ###.###");
            }

            if (Azimuth != null)
            {
                temp += ", A:" + Azimuth.ToString(style) + ", E:" + Elevation.ToString(style);
            }

            return temp;
        }
    }
}
