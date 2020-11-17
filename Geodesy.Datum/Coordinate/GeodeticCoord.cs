using System;
using Newtonsoft.Json;
using Geodesy.Datum.Units;

namespace Geodesy.Datum.Coordinate
{
    /// <summary>
    /// Geodetic coordinate is 3D coordinate of the Earth space.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class GeodeticCoord : GeographicCoord
    {
        /// <summary>
        /// Create a null geodetic coordinate object.
        /// </summary>
        public GeodeticCoord()
        {
            Height = double.NaN;
            System = HeightSystem.Ellipsoidal;
        }

        /// <summary>
        /// Create a null geodetic coordinate.
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lng">longitude</param>
        public GeodeticCoord(Latitude lat, Longitude lng)
            : base(lat, lng)
        {
            Height = double.NaN;
            System = HeightSystem.Ellipsoidal;
        }

        /// <summary>
        /// Create a null geodetic coordinate.
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lng">longitude</param>
        /// <param name="hgt">geodetic height / ellipsoidal height</param>
        public GeodeticCoord(Latitude lat, Longitude lng, double hgt)
            : base(lat, lng)
        {
            Height = hgt;
            System = HeightSystem.Ellipsoidal;
        }

        /// <summary>
        /// Get/Set geodetic height.
        /// </summary>
        [JsonProperty(Order = 2)]
        public double Height { get; set; }

        /// <summary>
        /// Get the height system. The readonly value is Ellipsoidal
        /// </summary>
        public HeightSystem System { get; }

        /// <summary>
        /// Get/Set the unit of height.
        /// </summary>
        public LinearUnit HeightUnit { get; set; } = Settings.BaseLinearUnit;

        /// <summary>
        /// Set the coordinate value.
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lng">longitude</param>
        /// <param name="hgt">geodetic height</param>
        public void SetCoordinate(Latitude lat, Longitude lng, double hgt)
        {
            SetCoordinate(lat, lng);
            Height = hgt;
        }

        /// <summary>
        /// Set geodetic height value. 
        /// </summary>
        /// <param name="hgt"></param>
        public void SetHeight(double hgt)
        {
            Height = hgt;
        }

        /// <summary>
        /// Set geodetic height value. 
        /// </summary>
        /// <param name="hgt"></param>
        /// <param name="unit"></param>
        public void SetHeight(double hgt, LinearUnit unit)
        {
            Height = hgt;
            HeightUnit = unit;
        }

        public double GetHeight(LinearUnit unit)
        {
            return Height * HeightUnit.Factor / unit.Factor;
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
        /// Convert the object to string.
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        public new string ToString(Angle.DataStyle style)
        {
            string temp = base.ToString(style);

            if (!double.IsNaN(Height))
            {
                temp += ", H:" + Height.ToString("# ###.###");
            }

            return temp;
        }
    }
}
