using Geodesy.Datum.Earth;
using Geodesy.Datum.Coordinate;
using System.Collections.Generic;

namespace Geodesy.Datum.Transformation
{
    /// <summary>
    /// Helmert trsformation is also called a seven-parameter transformation and is a similarity transformation.
    /// </summary>
    public class Helmert : AffineTransform
    {
        protected TransParameters _para;

        /// <summary>
        /// Creates instance of 2D rotational and shifted transformation.
        /// </summary>
        /// <param name="theta"></param>
        /// <param name="Tx"></param>
        /// <param name="Ty"></param>
        public Helmert(Angle theta, double Tx = 0, double Ty = 0)
            : base(theta, Tx, Ty)
        {
            _para = null;
        }

        /// <summary>
        /// Creates instance of 3D shifted transformation.
        /// </summary>
        /// <param name="Tx"></param>
        /// <param name="Ty"></param>
        /// <param name="Tz"></param>
        public Helmert(double Tx, double Ty, double Tz)
        {
            _matrix = new[,] { { 0, 1, 0, Tx }, { 0, 1, 0, Ty }, { 0, 0, 1, Tz }, { 0, 0, 0, 1 } };
            _para = new TransParameters(null, null, Tx, Ty, Tz, 0, 0, 0, 0);
        }

        /// <summary>
        /// Creates instance of 3D rotational and shifted transformation.
        /// </summary>
        /// <param name="para"></param>
        public Helmert(TransParameters para)
        {
            _matrix = new[,] { { 1, -para.Rz, para.Ry, para.Tx }, { para.Rz, 1, -para.Rx, para.Ty },
                               { -para.Ry, para.Rx, 1, para.Tz }, { 0, 0, 0, 1 } };

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    _matrix[i, j] *= (1 + para.S * 1E-6);
                }

            _para = para;
        }

        /// <summary>
        /// datum transformation by Helmert formula
        /// </summary>
        /// <param name="x">source coordinate of X</param>
        /// <param name="y">source coordinate of Y</param>
        /// <param name="z">source coordinate of Z</param>
        /// <returns>The transformed x-, y- and z-ordinate</returns>
        public override (double X, double Y, double Z) Transform(double x, double y, double z)
        {
            if (_para == null) return (double.NaN, double.NaN, double.NaN);

            // 在坐标旋转中，外文文献大多以逆时针为正，而中文则以顺时针为正，所以旋转矩阵的符号存在差异。本软件遵循逆时针为正的原则!!!
            // During the coordinate rotation, most foreign papers usually adopts counterclockwise as positive, 
            // while Chinese adopts clockwise as posituve. so the signs of the rotation matrix are different. 
            // This software follows the principle of counterclockwise as positive.

            // formula is from https://en.wikipedia.org/wiki/Helmert_transformation
            double X = _para.Tx + (1 + _para.S * 1E-6) * (x - _para.Rz * y + _para.Ry * z);
            double Y = _para.Ty + (1 + _para.S * 1E-6) * (_para.Rz * x + y - _para.Rx * z);
            double Z = _para.Tz + (1 + _para.S * 1E-6) * (-_para.Ry * x + _para.Rx * y + z);

            return (X, Y, Z);
        }

        /// <summary>
        /// datum transformation by Helmert formula
        /// </summary>
        /// <param name="point">geodetic point in old datum</param>
        /// <param name="from">source earth ellipsoid</param>
        /// <param name="to">target earth ellipsoid</param>
        /// <param name="para">parameters</param>
        /// <returns>geodetic point in new datum</returns>
        public GeodeticCoord Transform(GeodeticCoord point, Ellipsoid from, Ellipsoid to)
        {
            // convert geodetic coordinate to space rectangular coordinate.
            Conversion.BLH_XYZ(from, point.Latitude, point.Longitude, point.Height, out double X, out double Y, out double Z);

            // Transform the datum
            (double x, double y, double z) = Transform(X, Y, Z);

            // convert space rectangular coordinate to geodetic coordinate.
            Conversion.XYZ_BLH(to, x, y, z, out Latitude lat, out Longitude lng, out double hgt);

            // return the coordinate.
            return new GeodeticCoord(lat, lng, hgt);
        }

        /// <summary>
        /// datum transformation by Helmert formula
        /// </summary>
        /// <param name="point">geodetic point in old datum</param>
        /// <param name="from">source earth ellipsoid</param>
        /// <param name="to">target earth ellipsoid</param>
        /// <param name="para">parameters</param>
        /// <returns>geodetic point in new datum</returns>
        public List<GeodeticCoord> Transform(List<GeodeticCoord> points, Ellipsoid from, Ellipsoid to)
        {
            List<GeodeticCoord> result = new List<GeodeticCoord>();

            foreach (GeodeticCoord pnt in points)
            {
                result.Add(Transform(pnt, from, to));
            }

            return result;
        }
    }
}
