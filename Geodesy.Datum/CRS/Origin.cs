using Newtonsoft.Json;
using Geodesy.Datum.Coordinate;
using System.Collections.Generic;

namespace Geodesy.Datum.CRS
{
    /// <summary>
    /// Origin point of coordinate system.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Origin
    {
        /// <summary>
        /// Coordinate values of origin point.
        /// </summary>
        [JsonProperty(PropertyName = "Elements", NullValueHandling = NullValueHandling.Ignore)]
        private List<object> _elements;

        /// <summary>
        /// This private constructor is designed for JsonConvert.DeserializeObject().
        /// </summary>
        private Origin()
        { }

        /// <summary>
        /// Create an origin point by coordinate element list.
        /// </summary>
        /// <param name="value">coordinate element list</param>
        public Origin(params object[] value)
        {
            _elements = new List<object>();
            foreach (object o in value)
            {
                _elements.Add(o);
            }
        }

        /// <summary>
        /// Create an origin point by astronomic coordinate.
        /// </summary>
        /// <param name="coord">Astronomic coordinate</param>
        public Origin(AstronomicCoord coord)
        {
            _elements = new List<object>
            {
                coord.Latitude,
                coord.Longitude,
                coord.Azimuth
            };
        }

        /// <summary>
        /// Create an origin point by geodetic coordinate.
        /// </summary>
        /// <param name="coord">geodetic coordinate</param>
        public Origin(GeodeticCoord coord)
        {
            _elements = new List<object>
            {
                coord.Latitude,
                coord.Longitude,
                coord.Height
            };
        }

        /// <summary>
        /// Create an origin point by geographic coordinate.
        /// </summary>
        /// <param name="coord">geographic coordinate</param>
        public Origin(GeographicCoord coord)
        {
            _elements = new List<object>
            {
                coord.Latitude,
                coord.Longitude
            };
        }

        /// <summary>
        /// Create an origin point by projected coordinate.
        /// </summary>
        /// <param name="coord">projected coordinate</param>
        public Origin(ProjectedCoord coord)
        {
            _elements = new List<object>
            {
                coord.Northing,
                coord.Easting
            };
        }

        /// <summary>
        /// Create an origin point by space rectangular coordinate.
        /// </summary>
        /// <param name="coord">space rectangular coordinate</param>
        public Origin(SpaceRectangularCoord coord)
        {
            _elements = new List<object>
            {
                coord.X,
                coord.Y,
                coord.Z
            };
        }

        /// <summary>
        /// Get the demension of origin point.
        /// </summary>
        public int Dimension 
        {
            get
            {
                return _elements.Count;
            }
        }

        /// <summary>
        /// Set the origin point coordinate.
        /// </summary>
        /// <param name="value">coordinate element list</param>
        public void SetValue(params object[] value)
        {
            _elements = new List<object>();
            foreach (object o in value)
            {
                _elements.Add(o);
            }
        }

        /// <summary>
        /// Get the origin point coordinate.
        /// </summary>
        /// <returns>coordinate element array</returns>
        public object[] GetPoint()
        {
            object[] objs = new object[_elements.Count];
            _elements.CopyTo(objs);
            return objs;
        }
    }
}
