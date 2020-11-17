using System;
using Newtonsoft.Json;

namespace Geodesy.Datum.Coordinate
{
    /// <summary>
    /// A Cartesian coordinate system is a coordinate system that specifies each point uniquely in a 
    /// plane by a set of numerical coordinates, which are the signed distances to the point from two 
    /// fixed perpendicular oriented lines, measured in the same unit of length.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class CartesianCoord : ICartesianCoord, IEquatable<CartesianCoord>
    {
        /// <summary>
        /// The coordinate value.
        /// </summary>
        [JsonProperty(PropertyName = "Coordinates")]
        protected double[] _coord;

        /// <summary>
        /// Create a null object.
        /// </summary>
        public CartesianCoord()
        {
            _coord = new double[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coord"></param>
        public CartesianCoord(params double[] value)
        {
            _coord = value;
        }

        /// <summary>
        /// Get the component value.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public double this[int i]
        {
            get
            {
                return (i < 0 && i >= Dimension) ? double.NaN : _coord[i];
            }
            set
            {
                if (i >= 0 && i < Dimension) _coord[i] = value;
            }
        }

        /// <summary>
        /// Dimension of Cartesian coordinate.
        /// </summary>
        public int Dimension => _coord.Length;
        
        /// <summary>
        /// Get/Set the linear unit
        /// </summary>
        public Units.LinearUnit Unit { get; set; } = Settings.BaseLinearUnit;

        #region operations
        /// <summary>
        /// Set the coordinate value.
        /// </summary>
        /// <param name="coord"></param>
        public void SetCoordinate(double[] coord)
        {
            if (coord.Length != Dimension)
            {
                throw new GeodeticException("The coordinate length is invalid.");
            }

            _coord = coord;
        }

        /// <summary>
        /// Set the coordinate value.
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="unit"></param>
        public void SetCoordinate(double[] coord, Units.LinearUnit unit)
        {
            if (coord.Length != Dimension)
            {
                throw new GeodeticException("The coordinate length is invalid.");
            }

            _coord = coord;
            Unit = unit;
        }

        /// <summary>
        /// Get the coordinate value to a array.
        /// </summary>
        /// <returns></returns>
        public double[] GetCoordinate()
        {
            return _coord;
        }

        /// <summary>
        /// Get the coordinate value to a array.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public double[] GetCoordinate(Units.LinearUnit unit)
        {
            double[] coord = new double[Dimension];

            for (int i = Dimension - 1; i >= 0; i--)
            {
                coord[i] = _coord[i] * Unit.Factor / unit.Factor;
            }

            return coord;
        }

        /// <summary>
        /// Get the distance between this to another.
        /// </summary>
        /// <param name="other">another point</param>
        /// <returns>distance</returns>
        public double GetDistance(CartesianCoord other)
        {
            if (Dimension != other.Dimension)
            {
                throw new GeodeticException("The dimensions of two coordinates are not matched.");
            }

            double dd = 0;
            for (int i = _coord.Length - 1; i >= 0; i--)
            {
                dd += (_coord[i] - other._coord[i]) * (_coord[i] - other._coord[i]);
            }

            return Math.Sqrt(dd);
        }

        /// <summary>
        /// To make a figure larger or smaller is equivalent to multiplying the 
        /// Cartesian coordinates of every point by the same positive number m.
        /// </summary>
        /// <param name="scale"></param>
        public CartesianCoord Rescale(double scale)
        {
            if (scale <= 0)
            {
                throw new GeodeticException("the scale parameter must be a positive number.");
            }

            double[] coord = _coord;
            for (int i = Dimension - 1; i >= 0; i--)
            {
                coord[i] *= scale;
            }

            return new CartesianCoord(coord);
        }

        /// <summary>
        /// rotate the coordinate 
        /// </summary>
        /// <param name="theta">rotated angle</param>
        /// <param name="fixedAxis">1 - X axis, 2 - Y axis, 3 - Z axis</param>
        /// <returns></returns>
        public virtual CartesianCoord Rotate(Angle theta, int fixedAxis = 1)
        {
            switch (Dimension)
            {
                case 1:
                    return this;

                case 2:
                    return Rotate2D(theta);

                case 3:
                    return Rotate3D(theta, fixedAxis);

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// rotate a 2D coordinate pair
        /// </summary>
        /// <param name="theta">rotated angle</param>
        /// <returns></returns>
        public CartesianCoord Rotate2D(Angle theta)
        {
            double rad = theta.Radians;
            double x = _coord[0] * Math.Sin(rad) - _coord[1] * Math.Cos(rad);
            double y = _coord[0] * Math.Cos(rad) + _coord[1] * Math.Sin(rad);
            return new CartesianCoord(x, y);
        }

        /// <summary>
        /// rotate a 3D coordinate pair
        /// </summary>
        /// <param name="theta">rotated angle</param>
        /// <param name="fixedAxis">1 - X, 2 - Y, 3 - Z</param>
        /// <returns></returns>
        public CartesianCoord Rotate3D(Angle theta, int fixedAxis)
        {
            double rad = theta.Radians;
            double[] coord = new double[3];
            switch (fixedAxis)
            {
                case 1:
                    coord[0] = _coord[0];
                    coord[1] = _coord[1] * Math.Cos(rad) + _coord[2] * Math.Sin(rad);
                    coord[2] = -_coord[1] * Math.Sin(rad) + _coord[2] * Math.Cos(rad);
                    break;

                case 2:
                    coord[0] = _coord[0] * Math.Cos(rad) - _coord[2] * Math.Sin(rad);
                    coord[1] = _coord[1];
                    coord[2] = _coord[0] * Math.Sin(rad) + _coord[2] * Math.Cos(rad);
                    break;

                case 3:
                    coord[0] = _coord[0] * Math.Cos(rad) + _coord[1] * Math.Sin(rad);
                    coord[1] = -_coord[0] * Math.Sin(rad) + _coord[1] * Math.Cos(rad);
                    coord[2] = _coord[2];
                    break;

                default:
                    throw new GeodeticException("The reserved axis is error.");
            }

            return new CartesianCoord(coord);
        }

        /// <summary>
        /// translate/shift the coordinate origin
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public virtual CartesianCoord Shift(CartesianCoord delta)
        {
            if (Dimension != delta.Dimension)
            {
                throw new GeodeticException("The dimensions of two coordinates are not matched.");
            }

            double[] coord = new double[Dimension];
            for (int i = Dimension - 1; i >= 0; i--)
            {
                coord[i] = _coord[i] + delta._coord[i];
            }

            return new CartesianCoord(_coord);
        }

        /// <summary>
        /// translate/shift the coordinate origin
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public CartesianCoord Shift(params double[] delta)
        {
            if (Dimension != delta.Length)
            {
                throw new GeodeticException("The dimensions of two coordinates are not matched.");
            }

            double[] coord = new double[Dimension];
            for (int i = Dimension - 1; i >= 0; i--)
            {
                coord[i] = _coord[i] + delta[i];
            }

            return new CartesianCoord(_coord);
        }

        /// <summary>
        /// Reflection across a line through the origin making an angle θ with the x-axis
        /// </summary>
        /// <param name="theta">the angle with the x-axis</param>
        /// <returns></returns>
        public virtual CartesianCoord Reflect(Angle theta)
        {
            double rad = 2 * theta.Radians;
            double[] coord = new double[Dimension];

            switch (Dimension)
            {
                case 2:
                    coord[0] = _coord[0] * Math.Cos(rad) + _coord[1] * Math.Sin(rad);
                    coord[1] = _coord[0] * Math.Sin(rad) - _coord[1] * Math.Cos(rad);
                    break;

                default:
                    throw new NotImplementedException();
            }

            return new CartesianCoord(coord);
        }
        #endregion

        public bool Equals(CartesianCoord obj)
        {
            return _coord == obj._coord;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (typeof(object) != typeof(CartesianCoord)) return false;

            return _coord == ((CartesianCoord)obj)._coord;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hash = 0;
            foreach (double v in _coord)
            {
                hash *= v.GetHashCode();
            }
            return hash;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string temp = "[";

            foreach (double v in _coord)
            {
                temp += v.ToString() + ", ";
            }

            temp = temp.Substring(0, temp.Length - 2) + "]";
            return temp;
        }
    }
}
