using System;
using Newtonsoft.Json;
using Geodesy.Datum.Units;

namespace Geodesy.Datum.Coordinate
{
    /// <summary>
    /// Space rectangular coordinate 
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class SpaceRectangularCoord : CartesianCoord
    {
        /// <summary>
        /// Create a null space rectangular coordinate object.
        /// </summary>
        public SpaceRectangularCoord()
            : base(double.NaN, double.NaN, double.NaN)
        { }

        /// <summary>
        /// Create a space rectangular coordinate object.
        /// </summary>
        /// <param name="X">X component</param>
        /// <param name="Y">Y component</param>
        /// <param name="Z">Z component</param>
        public SpaceRectangularCoord(double X, double Y, double Z)
            : base(X, Y, Z)
        { }

        /// <summary>
        /// Create a space rectangular coordinate object.
        /// </summary>
        /// <param name="xyz">coordinate components array</param>
        public SpaceRectangularCoord(double[] xyz)
        {
            if (xyz.Length != 3)
            {
                throw new GeodeticException("The coordinate length is error.");
            }
            _coord = xyz;
        }

        /// <summary>
        /// Get component X
        /// </summary>
        [JsonProperty(Order = 0)]
        public double X
        {
            get => _coord[0];
            private set => _coord[0] = value;
        }

        /// <summary>
        /// Get component Y
        /// </summary>
        [JsonProperty(Order = 1)]
        public double Y
        {
            get => _coord[1];
            private set => _coord[1] = value;
        }

        /// <summary>
        /// Get component Z
        /// </summary>
        [JsonProperty(Order = 2)]
        public double Z
        {
            get => _coord[2];
            private set => _coord[2] = value;
        }

        /// <summary>
        /// Set coordinate value
        /// </summary>
        /// <param name="x">component X</param>
        /// <param name="y">component Y</param>
        /// <param name="z">component Z</param>
        public void SetCoordinate(double x, double y, double z)
        {
            _coord = new double[3];
            _coord[0] = x;
            _coord[1] = y;
            _coord[2] = z;
        }

        /// <summary>
        /// Set coordinate value
        /// </summary>
        /// <param name="x">component X</param>
        /// <param name="y">component Y</param>
        /// <param name="z">component Z</param>
        public void SetCoordinate(double x, double y, double z, LinearUnit unit)
        {
            _coord = new double[3];
            _coord[0] = x;
            _coord[1] = y;
            _coord[2] = z;
            Unit = unit;
        }

        /// <summary>
        /// Rotate the coordinate
        /// </summary>
        /// <param name="theta">rotated angle</param>
        /// <param name="fixedAxis">1 - X axis, 2 - Y axis, 3 - Z axis</param>
        /// <returns></returns>
        public new SpaceRectangularCoord Rotate(Angle theta, int fixedAxis)
        {
            return new SpaceRectangularCoord(Rotate3D(theta, fixedAxis).GetCoordinate());
        }

        /// <summary>
        /// translate/shift the coordinate origin
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public SpaceRectangularCoord Shift(SpaceRectangularCoord delta)
        {
            double[] coord = new double[3];
            coord[0] = _coord[0] + delta.X;
            coord[1] = _coord[1] + delta.Y;
            coord[2] = _coord[2] + delta.Z;
            return new SpaceRectangularCoord(coord);
        }

        /// <summary>
        /// Convert the coordinate object to string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (!double.IsNaN(X))
            {
                return "X:" + X.ToString("# ### ###.###") + ", Y:" + Y.ToString("# ### ###.###") + ", Z:" + Z.ToString("# ### ###.###");
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
