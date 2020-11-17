using System;
using Newtonsoft.Json;

namespace Geodesy.Datum.Coordinate
{
    /// <summary>
    /// Astronomic geodetic coordinate
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public sealed class AstronomicCoord : GeographicCoord
    {
        /// <summary>
        /// Create a null astronomic coordinate object
        /// </summary>
        public AstronomicCoord()
        { }

        /// <summary>
        /// Create a astronomic coordinate object
        /// </summary>
        /// <param name="lat">astro-latitue</param>
        /// <param name="lng">astro-longitude</param>
        public AstronomicCoord(Latitude lat, Longitude lng)
            : base(lat, lng)
        { }

        /// <summary>
        /// Create a astronomic coordinate object
        /// </summary>
        /// <param name="lat">astro-latitue</param>
        /// <param name="lng">astro-longitude</param>
        /// <param name="azm">astro-azimuth</param>
        public AstronomicCoord(Latitude lat, Longitude lng, Angle azm)
            : base(lat, lng)
        {
            Azimuth = azm;
        }

        /// <summary>
        /// Get/Set astro-azimuth value.
        /// </summary>
        [JsonProperty(Order = 2, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(AngleJsonConverter<Angle>))]
        public Angle Azimuth { get; set; }

        /// <summary>
        /// Astronomic longitude
        /// </summary>
        [JsonIgnore]
        public Longitude λ
        {
            get => Longitude;
            private set => Longitude = value;
        }

        /// <summary>
        /// Astronomic latitude
        /// </summary>
        [JsonIgnore]
        public Latitude φ
        {
            get => Latitude;
            private set => Latitude = value;
        }

        /// <summary>
        /// Astronomic azimuth
        /// </summary>
        [JsonIgnore]
        public Angle α
        {
            get => Azimuth;
            set => Azimuth = value;
        }

        /// <summary>
        /// Set the coodinate value
        /// </summary>
        /// <param name="lat">astro-latitue</param>
        /// <param name="lng">astro-longitude</param>
        /// <param name="azm">astro-azimuth</param>
        public void SetCoorinate(Latitude lat, Longitude lng, Angle azm)
        {
            SetCoordinate(lat, lng);
            Azimuth = azm;
        }

        /// <summary>
        /// Set the coodinate value
        /// </summary>
        /// <param name="lat">astro-latitue</param>
        /// <param name="lng">astro-longitude</param>
        /// <param name="azm">astro-azimuth</param>
        /// <param name="unit">angular unit</param>
        public void SetCoorinate(double lat, double lng, double azm, Units.AngularUnit unit)
        {
            Latitude = new Latitude(lat, unit);
            Longitude = new Longitude(lng, unit);
            Azimuth = new Angle(azm, unit);
        }

        /// <summary>
        /// Convert the object to string
        /// </summary>
        /// <returns>coodinate string</returns>
        public override string ToString()
        {
            return ToString(Angle.DataStyle.DD_MM_SSssss);
        }

        /// <summary>
        /// Convert the object to string by specified format.
        /// </summary>
        /// <param name="style">angle data style</param>
        /// <returns>coodinate string</returns>
        public new string ToString(Angle.DataStyle style)
        {
            string temp = string.Empty;

            if (Longitude != null)
            {
                temp += "λ:" + Longitude.ToString(style) + ", φ:" + Latitude.ToString(style);
            }

            if (Azimuth != null)
            {
                temp += ", α:" + Azimuth.ToString(style);
            }

            return temp;
        }
    }
}