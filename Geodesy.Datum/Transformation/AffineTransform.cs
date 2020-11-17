using System;
using Geodesy.Datum.Coordinate;
using System.Collections.Generic;

namespace Geodesy.Datum.Transformation
{
    /// <summary>
    /// The affine transformation applies translation and scaling/rotation terms on the (x,y,z) 
    /// coordinates, and translation and scaling on the temporal cordinate.
    /// </summary>
    /// <remarks>
    /// 在坐标旋转中，外文文献大多以逆时针为正，而中文则以顺时针为正，所以旋转矩阵的符号存在差异。本软件遵循逆时针为正的原则!!!
    /// During the coordinate rotation, most foreign papers usually adopts counterclockwise as positive, 
    /// while Chinese adopts clockwise as posituve. so the signs of the rotation matrix are different. 
    /// This software follows the principle of counterclockwise as positive.
    /// </remarks>
    public class AffineTransform
    {
        /// <summary>
        /// Represents transform matrix of this affine transformation from input points to output ones 
        /// using dimensionality defined within the affine transform.
        /// </summary>
        protected double[,] _matrix;

        #region constructors
        /// <summary>
        /// Creates instance of scaling transformation.
        /// </summary>
        /// <param name="scale"></param>
        public AffineTransform(params double[] scale)
        {
            int len = scale.Length;
            _matrix = new double[len + 1, len + 1];

            for (int i = len - 1; i >= 0; i--)
            {
                _matrix[i, i] = scale[i];
            }

            _matrix[len, len] = 1;
        }

        /// <summary>
        /// Creates instance of 2D rotational and translated transformation.
        /// </summary>
        /// <param name="theta">roteted angle，counterclockwise as positive</param>
        /// <param name="Tx">translate value of x</param>
        /// <param name="Ty">translate value of y</param>
        public AffineTransform(Angle theta, double Tx = 0, double Ty = 0)
        {
            double sin = Math.Sin(theta.Radians);
            double cos = Math.Cos(theta.Radians);
            _matrix = new[,] { { cos, -sin, Tx }, { sin, cos, Ty }, { 0, 0, 1 } };
        }

        /// <summary>
        /// Creates instance of 3D rotational and translated transformation.
        /// </summary>
        /// <param name="rotation">roteted angle of transformation，counterclockwise as positive</param>
        /// <param name="fixAxis">which axis is fixed, 1 - X, 2 - Y, 3 - Z.</param>
        public AffineTransform(Angle rotation, int fixAxis)
        {
            double sin = Math.Sin(rotation.Radians);
            double cos = Math.Cos(rotation.Radians);

            switch (fixAxis)
            {
                case 1:
                    _matrix = new[,] { { 1, 0, 0, 0 }, { 0, cos, -sin, 0 }, { 0, sin, cos, 0 }, { 0, 0, 0, 1 } };
                    break;

                case 2:
                    _matrix = new[,] { { cos, 0, sin, 0 }, { 0, 1, 0, 0 }, { -sin, 0, cos, 0 }, { 0, 0, 0, 1 } };
                    break;

                case 3:
                    _matrix = new[,] { { cos, -sin, 0, 0 }, { sin, cos, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } };
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Creates instance of 3D rotational and translated transformation.
        /// </summary>
        /// <param name="Rx">rotation angle when X fixed，counterclockwise as positive</param>
        /// <param name="Ry">rotation angle when Y fixed，counterclockwise as positive</param>
        /// <param name="Rz">rotation angle when Z fixed，counterclockwise as positive</param>
        /// <param name="Tx">translate value of X</param>
        /// <param name="Ty">translate value of Y</param>
        /// <param name="Tz">translate value of Z</param>
        public AffineTransform(Angle Rx, Angle Ry, Angle Rz, double Tx = 0, double Ty = 0, double Tz = 0)
        {
            _matrix = new double[4, 4];

            double sx = Math.Sin(Rx.Radians);
            double cx = Math.Cos(Rx.Radians);
            double sy = Math.Sin(Ry.Radians);
            double cy = Math.Cos(Ry.Radians);
            double sz = Math.Sin(Rz.Radians);
            double cz = Math.Cos(Rz.Radians);

            _matrix[0, 0] = cy * cz;
            _matrix[0, 1] = -cx * sz + sx * sy * cz;
            _matrix[0, 2] = sx * sz + cx * sy * cz;
            _matrix[1, 0] = cy * sz;
            _matrix[1, 1] = cx * cz + sx * sy * sz;
            _matrix[1, 2] = -sx * cz + cx * sy * sz;
            _matrix[2, 0] = -sy;
            _matrix[2, 1] = sx * cy;
            _matrix[2, 2] = cx * cy;

            _matrix[0, 3] = Tx;
            _matrix[1, 3] = Ty;
            _matrix[2, 3] = Tz;

            _matrix[3, 3] = 1;
        }

        /// <summary>
        /// Creates instance of affine transform using the specified matrix. 
        /// </summary>
        /// <remarks>
        /// If the transform's input dimension is M, and output dimension is N, then the matrix will 
        /// have size [N+1][M+1]. The +1 in the matrix dimensions allows the matrix to do a shift, as 
        /// well as a rotation. The [M][j] element of the matrix will be the j'th ordinate of the moved 
        /// origin. The [i][N] element of the matrix will be 0 for i less than M, and 1 for i equals M.
        /// </remarks>
        /// <param name="matrix">Matrix used to create afiine transform</param>
        public AffineTransform(double[,] matrix)
        {
            //check validity
            if (matrix == null)
            {
                throw new GeodeticException("matrix is null");
            }
            if (matrix.GetLength(0) <= 1)
            {
                throw new GeodeticException("Transformation matrix must have at least 2 rows.");
            }
            if (matrix.GetLength(1) <= 1)
            {
                throw new GeodeticException("Transformation matrix must have at least 2 columns.");
            }

            //use specified matrix
            _matrix = matrix;
        }
        #endregion constructors

        /// <summary>
        /// Gets the dimension of input points.
        /// </summary>
        public int DimSource => _matrix.GetUpperBound(1);

        /// <summary>
        /// Gets the dimension of output points.
        /// </summary>
        public int DimTarget => _matrix.GetUpperBound(0);

        #region transform methods
        /// <summary>
        /// Returns this affine transform as an affine transform matrix.
        /// </summary>
        /// <returns></returns>
        public double[,] GetMatrix()
        {
            return (double[,])_matrix.Clone();
        }

        /// <summary>
        /// Transforms a coordinate point.
        /// </summary>
        /// <param name="source">source coordinate</param>
        /// <returns>converted coordinate</returns>
        public double[] Transform(params double[] source)
        {
            if (source.Length > DimSource) return null;

            // if the target length is less than transform array columns.
            if (source.Length < DimSource) Array.Resize(ref source, DimSource);

            //use transformation matrix to create output points that has dimTarget dimensionality
            double[] target = new double[DimTarget];

            //count each target dimension using the apropriate row
            for (int row = DimTarget - 1; row >= 0; row--)
            {
                //start with the last value which is in fact multiplied by 1
                double dimVal = _matrix[row, DimSource];
                for (int col = DimSource - 1; col >= 0; col--)
                {
                    dimVal += _matrix[row, col] * source[col];
                }
                target[row] = dimVal;
            }

            return target;
        }
        #endregion

        #region 
        /// <summary>
        /// Transforms a single 3-dimensional point
        /// </summary>
        /// <param name="x">The ordinate value on the first axis, either x or longitude.</param>
        /// <param name="y">The ordinate value on the second axis, either y or latitude.</param>
        /// <param name="z">The ordinate value on the third axis, either z, height or altitude</param>
        /// <returns>The transformed x-, y- and z-ordinate values</returns>
        public virtual (double X, double Y, double Z) Transform(double x, double y, double z)
        {
            if (DimSource != 3)
            {
                throw new GeodeticException("The transform matrix dimension is not matched.");
            }

            double[] source = new double[] { x, y, z, 1 };
            double[] target = new double[DimTarget];

            //count each target dimension using the apropriate row
            for (int row = DimTarget - 1; row >= 0; row--)
            {
                //start with the last value which is in fact multiplied by 1
                double dimVal = _matrix[row, DimSource];
                for (int col = DimSource - 1; col >= 0; col--)
                {
                    dimVal += _matrix[row, col] * source[col];
                }
                target[row] = dimVal;
            }

            return (target[0], target[1], target[2]);
        }

        /// <summary>
        /// Transforms a single 3-dimensional point in-place
        /// </summary>
        /// <param name="x">The ordinate value on the first axis, either x or longitude.</param>
        /// <param name="y">The ordinate value on the second axis, either y or latitude.</param>
        /// <param name="z">The ordinate value on the third axis, either z, height or altitude</param>
        public void Transform(ref double x, ref double y, ref double z)
        {
            (x, y, z) = Transform(x, y, z);
        }

        /// <summary>
        /// Transforms a single 2-dimensional point
        /// </summary>
        /// <param name="x">The ordinate value on the first axis, either x or longitude.</param>
        /// <param name="y">The ordinate value on the second axis, either y or latitude.</param>
        /// <returns>The transformed x- and y-ordinate values</returns>
        public (double x, double y) Transform(double x, double y)
        {
            double z = 0;
            (x, y, _) = Transform(x, y, z);
            return (x, y);
        }

        /// <summary>
        /// Transforms a single 2-dimensional point in-place
        /// </summary>
        /// <param name="x">The ordinate value on the first axis, either x or longitude.</param>
        /// <param name="y">The ordinate value on the second axis, either y or latitude.</param>
        public void Transform(ref double x, ref double y)
        {
            double z = 0;
            (x, y, _) = Transform(x, y, z);
        }

        /// <summary>
        /// Transforms a Cartesian coordinate point array.
        /// </summary>
        /// <param name="points">source points array</param>
        /// <returns></returns>
        public List<CartesianCoord> Transform(List<CartesianCoord> points)
        {
            List<CartesianCoord> targets = new List<CartesianCoord>();

            foreach (CartesianCoord pnt in points)
            {
                double[] xyz = pnt.GetCoordinate();
                (double X, double Y, double Z) = Transform(xyz[0], xyz[1], xyz[2]);
                targets.Add(new CartesianCoord(X, Y, Z));
            }

            return targets;
        }

        /// <summary>
        /// Transforms a Cartesian coordinate point.
        /// </summary>
        /// <param name="point">source coordinate</param>
        /// <returns>converted coordinate</returns>
        public CartesianCoord Transform(CartesianCoord point)
        {
            if (point.Dimension > 3) return null;

            double[] xyz = point.GetCoordinate();
            (double X, double Y, double Z) = Transform(xyz[0], xyz[1], xyz[2]);
            return new CartesianCoord(X, Y, Z);
        }

        /// <summary>
        /// Transforms a space rectangular coordinate point.
        /// </summary>
        /// <param name="point">source coordinate</param>
        public void Transform(ref CartesianCoord point)
        {
            if (point.Dimension > 3) return;

            double[] xyz = point.GetCoordinate();
            (double X, double Y, double Z) = Transform(xyz[0], xyz[1], xyz[2]);
            point.SetCoordinate(new double[] { X, Y, Z });
        }

        /// <summary>
        /// Transforms a space rectangular coordinate point array.
        /// </summary>
        /// <param name="points">source points array</param>
        /// <returns></returns>
        public List<SpaceRectangularCoord> Transform(List<SpaceRectangularCoord> points)
        {
            List<SpaceRectangularCoord> targets = new List<SpaceRectangularCoord>();

            foreach (SpaceRectangularCoord pnt in points)
            {
                (double X, double Y, double Z) = Transform(pnt.X, pnt.Y, pnt.Z);
                targets.Add(new SpaceRectangularCoord(X, Y, Z));
            }

            return targets;
        }

        /// <summary>
        /// Transforms a space rectangular coordinate point.
        /// </summary>
        /// <param name="point">source coordinate</param>
        /// <returns>converted coordinate</returns>
        public SpaceRectangularCoord Transform(SpaceRectangularCoord point)
        {
            if (point.Dimension > 3) return null;

            (double X, double Y, double Z) = Transform(point.X, point.Y, point.Z);
            return new SpaceRectangularCoord(X, Y, Z);
        }

        /// <summary>
        /// Transforms a space rectangular coordinate point.
        /// </summary>
        /// <param name="point">source coordinate</param>
        public void Transform(ref SpaceRectangularCoord point)
        {
            if (point.Dimension > 3) return;

            (double X, double Y, double Z) = Transform(point.X, point.Y, point.Z);
            point.SetCoordinate(X, Y, Z);
        }

        /// <summary>
        /// Transforms a earth-centered, earth-fixed coordinate point array.
        /// </summary>
        /// <param name="points">source points array</param>
        /// <returns></returns>
        public List<ECEF> Transform(List<ECEF> points)
        {
            List<ECEF> targets = new List<ECEF>();

            foreach (ECEF pnt in points)
            {
                (double X, double Y, double Z) = Transform(pnt.X, pnt.Y, pnt.Z);
                targets.Add(new ECEF(X, Y, Z));
            }

            return targets;
        }

        /// <summary>
        /// Transforms a earth-centered, earth-fixed coordinate point.
        /// </summary>
        /// <param name="point">source coordinate</param>
        /// <returns>converted coordinate</returns>
        public ECEF Transform(ECEF point)
        {
            if (point.Dimension > 3) return null;

            (double X, double Y, double Z) = Transform(point.X, point.Y, point.Z);
            return new ECEF(X, Y, Z);
        }

        /// <summary>
        /// Transforms a earth-centered, earth-fixed coordinate point.
        /// </summary>
        /// <param name="point">source coordinate</param>
        public void Transform(ref ECEF point)
        {
            if (point.Dimension > 3) return;

            (double X, double Y, double Z) = Transform(point.X, point.Y, point.Z);
            point.SetCoordinate(X, Y, Z);
        }
        #endregion public methods
    }
}