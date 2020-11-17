using Newtonsoft.Json;
using Geodesy.Datum.Units;
using System.Collections.Generic;

namespace Geodesy.Datum.CRS
{
    /// <summary>
    /// Cartesian coordinate system
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class CartesianCoordinateSystem : CoordinateSystem
    {
        /// <summary>
        /// Create a null object
        /// </summary>
        public CartesianCoordinateSystem()
        {
            Dimension = 0;
        }

        /// <summary>
        /// Create a Cartesian coordinate system with specified axes.
        /// </summary>
        /// <param name="axes">axes of system</param>
        public CartesianCoordinateSystem(params Axis[] axes)
            : this(new Origin(new double[axes.Length]), axes)
        { }

        /// <summary>
        /// Create a Cartesian coordinate system with specified axes and origin.
        /// </summary>
        /// <param name="origin">orgin point of system</param>
        /// <param name="axes">axes of system</param>
        public CartesianCoordinateSystem(Origin origin, params Axis[] axes)
        {
            Origin = origin;
            Dimension = axes.Length;

            // set the axes and quantities
            _axes = new Dictionary<string, Axis>();
            _units = new Dictionary<string, Unit>();
            foreach (Axis axis in axes)
            {
                _axes.Add(axis.Name, axis);
                _units.Add(axis.Name, Settings.BaseLinearUnit);
            }
        }
    }
}