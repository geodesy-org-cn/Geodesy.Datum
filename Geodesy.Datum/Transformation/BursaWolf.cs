using System;

namespace Geodesy.Datum.Transformation
{
    /// <summary>
    /// Datum transformation based on Bulsa-Wolf formula
    /// </summary>
    public class BursaWolf : Helmert
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="para"></param>
        public BursaWolf(TransParameters para)
            : base(para)
        {
            _para = para;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Tx"></param>
        /// <param name="Ty"></param>
        /// <param name="Tz"></param>
        /// <param name="S"></param>
        public BursaWolf(double Tx, double Ty, double Tz, double S = 0)
            : base(Tx, Ty, Tz)
        {
            for (int i = 0; i < 3; i++)
            {
                _matrix[i, i] *= 1 + S * 1E-6;
            }

            _para = new TransParameters(null, null, Tx, Ty, Tz, S, 0, 0, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public override (double X, double Y, double Z) Transform(double x, double y, double z)
        {
            if (_para == null) return (double.NaN, double.NaN, double.NaN);

            // 在坐标旋转中，外文文献大多以逆时针为正，而中文则以顺时针为正，所以旋转矩阵的符号存在差异。本软件遵循逆时针为正的原则!!!
            // During the coordinate rotation, most foreign papers usually adopts counterclockwise as positive, 
            // while Chinese adopts clockwise as posituve. so the signs of the rotation matrix are different. 
            // This software follows the principle of counterclockwise as positive.

            // formula is from <Geodesy> 此处考虑到大多数为国内用户使用布尔莎模型，所以仍然保持顺时针原则
            double X = (1 + _para.S * 1E-6) * x + (-_para.Rz * y + _para.Ry * z) + _para.Tx;
            double Y = (1 + _para.S * 1E-6) * y + (_para.Rz * x - _para.Rx * z) + _para.Ty;
            double Z = (1 + _para.S * 1E-6) * z + (-_para.Ry * x + _para.Rx * y) + _para.Tz;

            return (X, Y, Z);
        }
    }
}
