using Newtonsoft.Json;
using Geodesy.Datum.Units;
using System.Collections.Generic;
using System.Text;

namespace Geodesy.Datum.CRS
{
    /// <summary>
    /// 2D Cartesian Coordination System
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class Cartesian2DSystem : CartesianCoordinateSystem
    {
        /// <summary>
        /// Create a 2D Cartesian coordinate system as defalut system.
        /// </summary>
        public Cartesian2DSystem()
        {
            Origin = new Origin(new double[2]);
            Dimension = 2;

            _axes = new Dictionary<string, Axis>
            {
                { "x", Axis.x },
                { "y", Axis.y }
            };

            _units = new Dictionary<string, Unit>
            {
                { "x", Settings.BaseLinearUnit },
                { "y", Settings.BaseLinearUnit }
            };
        }

        /// <summary>
        /// Create a 2D Cartesian coordinate system with specified axes.
        /// </summary>
        /// <param name="axes">axes of system</param>
        public Cartesian2DSystem(params Axis[] axes)
            : this(new Origin(new double[2]), axes)
        { }

        /// <summary>
        /// Create a 2D Cartesian coordinate system with specified axes and origin.
        /// </summary>
        /// <param name="origin">orgin point of system</param>
        /// <param name="axes">axes of system</param>
        public Cartesian2DSystem(Origin origin, params Axis[] axes)
        {
            if (axes.Length != 2)
            {
                throw new GeodeticException("Count of axes is error! It must be 2.");
            }

            Dimension = 2;
            Origin = origin;

            // set the axes
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
