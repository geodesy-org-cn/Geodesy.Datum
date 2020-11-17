using System;
using Newtonsoft.Json;
using Geodesy.Datum.Units;

namespace Geodesy.Datum.Coordinate
{
    /// <summary>
    /// Vertical height
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class HeightCoord : CartesianCoord
    {
        /// <summary>
        /// 
        /// </summary>
        public HeightCoord()
            : this(double.NaN)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="height"></param>
        public HeightCoord(double height)
        {
            _coord = new double[1];
            _coord[0] = height;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="height"></param>
        /// <param name="unit"></param>
        public HeightCoord(double height, LinearUnit unit)
        {
            _coord[0] = height;
            Unit = unit;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty]
        public double Height
        {
            get => _coord[0];
            set => _coord[0] = value;
        }

        /// <summary>
        /// Height System
        /// </summary>
        public HeightSystem System { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hgt"></param>
        public void SetHeight(double hgt)
        {
            _coord[0] = hgt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hgt"></param>
        /// <param name="unit"></param>
        public void SetValue(double hgt, LinearUnit unit)
        {
            _coord[0] = hgt;
            Unit = unit;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public double GetValue(LinearUnit unit)
        {
            return _coord[0] * Unit.Factor / unit.Factor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _coord[0].ToString();
        }
    }
}
