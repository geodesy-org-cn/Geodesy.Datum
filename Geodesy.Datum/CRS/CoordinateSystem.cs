using Newtonsoft.Json;
using Geodesy.Datum.Units;
using System.Collections.Generic;

namespace Geodesy.Datum.CRS
{
    /// <summary>
    /// An abstract class of coordinate system
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class CoordinateSystem : ICoordinateSystem
    {
        /// <summary>
        /// The axes of coordinate system
        /// </summary>
        [JsonProperty(PropertyName = "Axes", NullValueHandling = NullValueHandling.Ignore)]
        protected Dictionary<string, Axis> _axes;

        /// <summary>
        /// The units of each axis.
        /// </summary>
        [JsonProperty(PropertyName = "Units", NullValueHandling = NullValueHandling.Ignore)]
        protected Dictionary<string, Unit> _units;

        /// <summary>
        /// Identifier
        /// </summary>
        [JsonProperty(Order =0)]
        public Identifier Identifier { get; set; }

        /// <summary>
        /// Origin of the coordinate system
        /// </summary>
        [JsonProperty]
        public Origin Origin { get; set; }

        /// <summary>
        /// Dimension of the coordinate system
        /// </summary>
        public int Dimension { get; protected set; }

        /// <summary>
        /// This private constructor is designed for JsonConvert.DeserializeObject().
        /// </summary>
        protected CoordinateSystem()
        { }

        /// <summary>
        /// Get the axis value
        /// </summary>
        /// <param name="name">axis name</param>
        /// <returns>axis</returns>
        public Axis GetAxis(string name)
        {
            return (_axes.ContainsKey(name)) ? _axes[name] : null;
        }

        /// <summary>
        /// Set the axis value
        /// </summary>
        /// <param name="axis">axis</param>
        /// <returns></returns>
        public bool SetAxis(Axis axis)
        {
            if (_axes.ContainsKey(axis.Name))
            {
                _axes[axis.Name] = axis;
                return true;
            }
            else
            {
                if (_axes.Count < Dimension)
                {
                    _axes.Add(axis.Name, axis);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Get the unit of axis.
        /// </summary>
        /// <param name="axis">axis</param>
        /// <returns>unit</returns>
        public Unit GetUnit(Axis axis)
        {
            return _units.ContainsKey(axis.Name) ? _units[axis.Name] : null;
        }

        /// <summary>
        /// Set the unit of axis
        /// </summary>
        /// <param name="axis">axis</param>
        /// <param name="unit">unit</param>
        /// <returns></returns>
        public bool SetUnit(Axis axis, Unit unit)
        {
            if (_units.ContainsKey(axis.Name))
            {
                _units[axis.Name] = unit;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
