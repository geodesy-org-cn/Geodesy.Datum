using System;
using Newtonsoft.Json;

namespace Geodesy.Datum.Coordinate
{
    /// <summary>
    /// Geodetic polar coordinate is the geodesic on ellipsoid surface.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class GeodeticPolarCoord : PolarCoord
    {
        /// <summary>
        /// Create a null geodetic polar coordinate object.
        /// </summary>
        public GeodeticPolarCoord()
        { }

        /// <summary>
        /// Create a geodetic polar coordinate object.
        /// </summary>
        /// <param name="range"></param>
        /// <param name="azimuth"></param>
        public GeodeticPolarCoord(double range, Angle azimuth)
            : base(range, azimuth)
        { }
        
        /*
        /// <summary>
        /// Create a null geodetic polar coordinate object.
        /// </summary>
        public GeodeticPolarCoord()
        {
            Origin = null;
        }

        /// <summary>
        /// Create a geodetic polar coordinate object.
        /// </summary>
        /// <param name="origin">polar pole</param>
        /// <param name="distance">length of geodesics</param>
        /// <param name="azimuth">azimuth of geodesics</param>
        public GeodeticPolarCoord(GeographicCoord origin, double distance, Angle azimuth)
            : base(distance, azimuth)
        {
            Origin = origin;
        }

        /// <summary>
        /// Create a geodetic polar coordinate object.
        /// </summary>
        /// <param name="origin">polar pole</param>
        /// <param name="distance">length of geodesics</param>
        /// <param name="azimuth">azimuth of geodesics</param>
        public GeodeticPolarCoord(GeodeticCoord origin, double distance, Angle azimuth)
            : base(distance, azimuth)
        {
            Origin = new GeographicCoord(origin.Latitude, origin.Longitude);
        }

        /// <summary>
        /// Create a geodetic polar coordinate object.
        /// </summary>
        /// <param name="lat">latitude of polar pole</param>
        /// <param name="lng">longitude of polar pole</param>
        /// <param name="distance">length of geodesics</param>
        /// <param name="azimuth">azimuth of geodesics</param>
        public GeodeticPolarCoord(Latitude lat, Longitude lng, double distance, Angle azimuth)
             : base(distance, azimuth)
        {
            Origin = new GeographicCoord(lat, lng);
        }

        /// <summary>
        /// Get/Set polar pole
        /// </summary>
        [JsonProperty(Order =2)]
        public GeographicCoord Origin { get; set; }

        /// <summary>
        /// Set polar pole value
        /// </summary>
        /// <param name="lat">latitude of polar pole</param>
        /// <param name="lng">longitude of polar pole</param>
        public void SetOrigin(Latitude lat, Longitude lng)
        {
            Origin = new GeographicCoord(lat, lng);
        }

        /// <summary>
        /// Set polar pole value
        /// </summary>
        /// <param name="latlng">geodetic coordinate value of origin</param>
        public void SetOrigin(GeodeticCoord latlng)
        {
            Origin = new GeographicCoord(latlng.Latitude, latlng.Longitude);
        }

        /// <summary>
        /// Set polar pole value
        /// </summary>
        /// <param name="latlng">geographic coordinate value of origin</param>
        public void SetOrigin(GeographicCoord latlng)
        {
            Origin = latlng;
        }

        /// <summary>
        /// Set geodetic polar coordinate value.
        /// </summary>
        /// <param name="lat">latitude of polar pole</param>
        /// <param name="lng">longitude of polar pole</param>
        /// <param name="distance">length of geodesics</param>
        /// <param name="azimuth">azimuth of geodesics</param>
        public void SetValue(Latitude lat, Longitude lng, double distance, Angle azimuth)
        {
            base.SetValue(distance, azimuth);
            Origin = new GeographicCoord(lat, lng);
        }

        /// <summary>
        /// Set geodetic polar coordinate value.
        /// </summary>
        /// <param name="latlng">polar pole</param>
        /// <param name="distance">length of geodesics</param>
        /// <param name="azimuth">azimuth of geodesics</param>
        public void SetValue(GeodeticCoord latlng, double distance, Angle azimuth)
        {
            base.SetValue(distance, azimuth);
            Origin = new GeographicCoord(latlng.Latitude, latlng.Longitude);
        }

        /// <summary>
        /// Set geodetic polar coordinate value.
        /// </summary>
        /// <param name="latlng">polar pole</param>
        /// <param name="distance">length of geodesics</param>
        /// <param name="azimuth">azimuth of geodesics</param>
        public void SetValue(GeographicCoord latlng, double distance, Angle azimuth)
        {
            base.SetValue(distance, azimuth);
            Origin = latlng;
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

            if (Origin != null)
            {
                if (temp.Length > 0) temp += ", ";
                temp += "B:" + Origin.Latitude.ToString(style) + ", L:" + Origin.Longitude.ToString(style);
            }

            return temp;
        }
        */
    }
}
