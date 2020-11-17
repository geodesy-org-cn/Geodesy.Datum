using System;
using Newtonsoft.Json;

namespace Geodesy.Datum.Coordinate
{
    /// <summary>
    /// Geographic coordinate is 2D coordinate of the Earth space.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class GeographicCoord : IGeographicCoord
    {
        /// <summary>
        /// Create a null geographic coordinate object.
        /// </summary>
        public GeographicCoord()
        { }

        /// <summary>
        /// Create a geographic coordinate object.
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lng">longitude</param>
        public GeographicCoord(Latitude lat, Longitude lng)
        {
            SetCoordinate(lat, lng);
        }

        /// <summary>
        /// latitude
        /// </summary>
        [JsonConverter(typeof(AngleJsonConverter<Latitude>))]
        [JsonProperty(Order = 0)]
        public Latitude Latitude { get; protected set; }

        /// <summary>
        /// longitude
        /// </summary>
        [JsonConverter(typeof(AngleJsonConverter<Longitude>))]
        [JsonProperty(Order = 1)]
        public Longitude Longitude { get; protected set; }

        /// <summary>
        /// the unit of angle
        /// </summary>
        public Units.AngularUnit AngularUnit { get; } = Settings.BaseAngularUnit;

        /// <summary>
        /// Set the coordinate value.
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lng">longitude</param>
        public void SetCoordinate(Latitude lat, Longitude lng)
        {
            Latitude = lat;
            Longitude = lng;

            Latitude.Normalize();
            Longitude.Normalize();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="unit"></param>
        public void SetCoordinate(double lat, double lng, Units.AngularUnit unit)
        {
            Latitude = new Latitude(lat, unit);
            Longitude = new Longitude(lng, unit);

            Latitude.Normalize();
            Longitude.Normalize();
        }


        /// <summary>
        /// Convert the object to string.
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
            if (Latitude == null)
            {
                return string.Empty;
            }
            else
            {
                return "B:" + Latitude.ToString(style) + ", L:" + Longitude.ToString(style);
            }
        }
    }
}
