using Newtonsoft.Json;
using Geodesy.Datum.Units;
using System.Collections.Generic;

namespace Geodesy.Datum.CRS
{
    /// <summary>
    /// 3D Cartesian Coordination System
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class Cartesian3DSystem : CartesianCoordinateSystem
    {
        /// <summary>
        /// Axis Y relationship to axis X and axis Z
        /// </summary>
        public enum AxisYDirection
        {
            /// <summary>
            /// Axis Y is right hand to Axis X and axis Z
            /// </summary>
            RightHand,

            /// <summary>
            /// Axis Y is left hand to Axis X and axis Z
            /// </summary>
            LeftHand
        }

        /// <summary>
        /// Create a 3D Cartesian coordinate system.
        /// </summary>
        public Cartesian3DSystem()
            : this(AxisYDirection.RightHand)
        { }

        /// <summary>
        /// Create a 3D Cartesian coordinate system.
        /// </summary>
        /// <param name="direction">Left hand or right hand</param>
        public Cartesian3DSystem(AxisYDirection direction)
        {
            Dimension = 3;
            Origin = new Origin(new double[3]);
            YDirection = direction;

            _axes = new Dictionary<string, Axis>
            {
                { "X", Axis.X },
                { "Z", Axis.Z }
            };

            if (direction == AxisYDirection.LeftHand)
            {
                _axes.Add("Y", new Axis("Y", AxisOrientation.West));
            }
            else
            {
                _axes.Add("Y", Axis.Y);
            }

            _units = new Dictionary<string, Unit>
            {
                { "X", Settings.BaseLinearUnit },
                { "Y", Settings.BaseLinearUnit },
                { "Z", Settings.BaseLinearUnit }
            };
        }

        /// <summary>
        /// Create a 3D Cartesian coordinate system with specified axes.
        /// </summary>
        /// <param name="direction">Axis Y direction</param>
        /// <param name="axes">specified axes</param>
        public Cartesian3DSystem(AxisYDirection direction, params Axis[] axes)
            : this(new Origin(new double[3]), direction, axes)
        { }

        /// <summary>
        /// Create a 3D Cartesian coordinate system with specified axes and origin.
        /// </summary>
        /// <param name="origin">origin of the coordinate system</param>
        /// <param name="direction">Axis Y direction</param>
        /// <param name="axes">specified axes</param>
        public Cartesian3DSystem(Origin origin, AxisYDirection direction, params Axis[] axes)
        {
            if (axes.Length != 3)
            {
                throw new GeodeticException("Count of axis is error! It must be 3.");
            }

            Dimension = 3;
            Origin = origin;
            YDirection = direction;

            _axes = new Dictionary<string, Axis>
            {
                { "X", Axis.X },
                { "Z", Axis.Z }
            };

            if (direction == AxisYDirection.LeftHand)
            {
                _axes.Add("Y", new Axis("Y", AxisOrientation.West));
            }
            else
            {
                _axes.Add("Y", Axis.Y);
            }

            _units = new Dictionary<string, Unit>
            {
                { "X", Settings.BaseLinearUnit },
                { "Y", Settings.BaseLinearUnit },
                { "Z", Settings.BaseLinearUnit }
            };
        }

        /// <summary>
        /// Left hand or right hand
        /// </summary>
        public AxisYDirection YDirection { get; set; } = AxisYDirection.RightHand;
    }
}
