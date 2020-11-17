using System;
using Geodesy.Datum.Coordinate;
using System.Collections.Generic;

namespace Geodesy.Datum.Transformation
{
    public sealed partial class TransParameters
    {
        /// <summary>
        /// resolve the transformation parameters by points collection
        /// </summary>
        /// <param name="source">points in source datum</param>
        /// <param name="target">points in target datum</param>
        /// <param name="weight">weight of points</param>
        /// <param name="number">parameter number, 3 or 4 or 7</param>
        /// <returns>transform parameters</returns>
        public static TransParameters Resolve(List<(double X, double Y, double Z)> source,
                                              List<(double X, double Y, double Z)> target,
                                              double[,] weight, int number = 7)
        {
            // points count
            int count = target.Count;
            if (count != source.Count)
            {
                throw new GeodeticException("Two points collections must have same count.");
            }

            if (number != 3 && number != 4 && number != 7)
            {
                throw new GeodeticException("The number of transformation parameters can only be 3 or 4 or 7.");
            }

            if (count * 3 < number)
            {
                throw new GeodeticException("There is too less points to compute the transformation parameters.");
            }

            if (weight.GetUpperBound(0) != 3 * count - 1 || weight.GetUpperBound(1) != 3 * count - 1)
            {
                throw new GeodeticException("Weight data size error.");
            }

            // coefficient matrix A
            double[,] A = new double[count * 3, number];
            for (int i = count - 1; i >= 0; i--)
            {
                A[i * 3 + 0, 0] = 1;
                A[i * 3 + 1, 1] = 1;
                A[i * 3 + 2, 2] = 1;

                if (number == 7)
                {
                    // 此处需考虑旋转角的正负，本式以逆时针为正进行计算
                    double rou = 3600 * 180.0 / Math.PI;
                    A[i * 3 + 1, 3] = -source[i].Z / rou;
                    A[i * 3 + 2, 3] = source[i].Y / rou;
                    A[i * 3 + 2, 4] = -source[i].X / rou;

                    A[i * 3 + 0, 4] = -A[i * 3 + 1, 3];
                    A[i * 3 + 0, 5] = -A[i * 3 + 2, 3];
                    A[i * 3 + 1, 5] = -A[i * 3 + 2, 4];
                }

                if (number == 7 || number == 4)
                {
                    A[i * 3 + 0, number - 1] = source[i].X * 1E-6;
                    A[i * 3 + 1, number - 1] = source[i].Y * 1E-6;
                    A[i * 3 + 2, number - 1] = source[i].Z * 1E-6;
                }
            }

            // Observation vector L
            double[] L = new double[count * 3];
            for (int i = count - 1; i >= 0; i--)
            {
                L[i * 3 + 0] = source[i].X - target[i].X;
                L[i * 3 + 1] = source[i].Y - target[i].Y;
                L[i * 3 + 2] = source[i].Z - target[i].Z;
            }

            try
            {
                // use private 
                double[] result = SolveEquation(A, L, weight, count, number);
                return new TransParameters(null, null, result);
            }
            catch
            {
                throw new GeodeticException("Can not resolve the quation of transformation.");
            }
        }

        /// <summary>
        /// resolve the transformation parameters by points collection
        /// </summary>
        /// <param name="source">points in source datum</param>
        /// <param name="target">points in target datum</param>
        /// <returns>transform parameters</returns>
        public static TransParameters Resolve(SpaceRectangularCoord[] source, SpaceRectangularCoord[] target)
        {
            // copy the data to array.
            List<(double X, double Y, double Z)> from = new List<(double X, double Y, double Z)>();
            foreach (SpaceRectangularCoord pnt in source)
            {
                from.Add((pnt.X, pnt.Y, pnt.Z));
            }

            // copy the data to array.
            List<(double X, double Y, double Z)> to = new List<(double X, double Y, double Z)>();
            foreach (SpaceRectangularCoord pnt in target)
            {
                to.Add((pnt.X, pnt.Y, pnt.Z));
            }

            // A point is 3D， so the weight must be 
            int count = source.Length * 3;
            double[,] weight = new double[count, count];
            for (int i = count - 1; i >= 0; i--)
            {
                weight[i, i] = 1;
            }

            return Resolve(from, to, weight, 7);
        }

        /// <summary>
        /// resolve the transformation parameters by points collection
        /// </summary>
        /// <param name="source">points in source datum</param>
        /// <param name="target">points in target datum</param>
        /// <returns>transform parameters</returns>
        public static TransParameters Resolve(List<SpaceRectangularCoord> source, List<SpaceRectangularCoord> target)
        {
            // copy the data to array.
            List<(double X, double Y, double Z)> from = new List<(double X, double Y, double Z)>();
            foreach (SpaceRectangularCoord pnt in source)
            {
                from.Add((pnt.X, pnt.Y, pnt.Z));
            }

            // copy the data to array.
            List<(double X, double Y, double Z)> to = new List<(double X, double Y, double Z)>();
            foreach (SpaceRectangularCoord pnt in target)
            {
                to.Add((pnt.X, pnt.Y, pnt.Z));
            }

            // A point is 3D， so the weight must be 
            int count = source.Count * 3;
            double[,] weight = new double[count, count];
            for (int i = count - 1; i >= 0; i--)
            {
                weight[i, i] = 1;
            }

            return Resolve(from, to, weight, 7);
        }

        /// <summary>
        /// resolve the transformation parameters by points collection
        /// </summary>
        /// <param name="source">points in source datum</param>
        /// <param name="target">points in target datum</param>
        /// <param name="weight">weight of points</param>
        /// <param name="numbers">parameter number, 3 or 4 or 7</param>
        /// <returns>transform parameters</returns>
        public static TransParameters Resolve(List<SpaceRectangularCoord> source, List<SpaceRectangularCoord> target,
                                              double[,] weight, int number = 7)
        {
            // copy the data to array.
            List<(double X, double Y, double Z)> from = new List<(double X, double Y, double Z)>();
            foreach (SpaceRectangularCoord pnt in source)
            {
                from.Add((pnt.X, pnt.Y, pnt.Z));
            }

            // copy the data to array.
            List<(double X, double Y, double Z)> to = new List<(double X, double Y, double Z)>();
            foreach (SpaceRectangularCoord pnt in target)
            {
                to.Add((pnt.X, pnt.Y, pnt.Z));
            }

            return Resolve(from, to, weight, number);
        }

        /// <summary>
        /// resolve the transformation parameters by points collection
        /// </summary>
        /// <param name="source">points in source datum</param>
        /// <param name="target">points in target datum</param>
        /// <returns>transform parameters</returns>
        public static TransParameters Resolve(CartesianCoord[] source, CartesianCoord[] target)
        {
            if (source.Length > 0)
            {
                if (source[0].Dimension != 3)
                {
                    throw new GeodeticException("The source points dimension is error.");
                }
            }

            if (target.Length > 0)
            {
                if (target[0].Dimension != 3)
                {
                    throw new GeodeticException("The target points dimension is error.");
                }
            }

            // copy the data to array.
            List<(double X, double Y, double Z)> from = new List<(double X, double Y, double Z)>();
            foreach (CartesianCoord pnt in source)
            {
                double[] coord = pnt.GetCoordinate();
                from.Add((coord[0], coord[1], coord[2]));
            }

            // copy the data to array.
            List<(double X, double Y, double Z)> to = new List<(double X, double Y, double Z)>();
            foreach (CartesianCoord pnt in target)
            {
                double[] coord = pnt.GetCoordinate();
                to.Add((coord[0], coord[1], coord[2]));
            }

            // A point is 3D， so the weight must be 
            int count = source.Length * 3;
            double[,] weight = new double[count, count];
            for (int i = count - 1; i >= 0; i--)
            {
                weight[i, i] = 1;
            }

            return Resolve(from, to, weight, 7);
        }

        /// <summary>
        /// resolve the transformation parameters by points collection
        /// </summary>
        /// <param name="source">points in source datum</param>
        /// <param name="target">points in target datum</param>
        /// <returns>transform parameters</returns>
        public static TransParameters Resolve(List<CartesianCoord> source, List<CartesianCoord> target)
        {
            if (source.Count > 0)
            {
                if (source[0].Dimension != 3)
                {
                    throw new GeodeticException("The source points dimension is error.");
                }
            }

            if (target.Count > 0)
            {
                if (target[0].Dimension != 3)
                {
                    throw new GeodeticException("The target points dimension is error.");
                }
            }

            // copy the data to array.
            List<(double X, double Y, double Z)> from = new List<(double X, double Y, double Z)>();
            foreach (CartesianCoord pnt in source)
            {
                double[] coord = pnt.GetCoordinate();
                from.Add((coord[0], coord[1], coord[2]));
            }

            // copy the data to array.
            List<(double X, double Y, double Z)> to = new List<(double X, double Y, double Z)>();
            foreach (CartesianCoord pnt in target)
            {
                double[] coord = pnt.GetCoordinate();
                to.Add((coord[0], coord[1], coord[2]));
            }

            // A point is 3D， so the weight must be 
            int count = source.Count * 3;
            double[,] weight = new double[count, count];
            for (int i = count - 1; i >= 0; i--)
            {
                weight[i, i] = 1;
            }

            return Resolve(from, to, weight, 7);
        }

        /// <summary>
        /// resolve the transformation parameters by points collection
        /// </summary>
        /// <param name="source">points in source datum</param>
        /// <param name="target">points in target datum</param>
        /// <param name="weight">weight of points</param>
        /// <param name="numbers">parameter number, 3 or 4 or 7</param>
        /// <returns>transform parameters</returns>
        public static TransParameters Resolve(List<CartesianCoord> source, List<CartesianCoord> target,
                                              double[,] weight, int number = 7)
        {
            if (source.Count > 0)
            {
                if (source[0].Dimension != 3)
                {
                    throw new GeodeticException("The source points dimension is error.");
                }
            }

            if (target.Count > 0)
            {
                if (target[0].Dimension != 3)
                {
                    throw new GeodeticException("The target points dimension is error.");
                }
            }

            // copy the data to array.
            List<(double X, double Y, double Z)> from = new List<(double X, double Y, double Z)>();
            foreach (CartesianCoord pnt in source)
            {
                double[] coord = pnt.GetCoordinate();
                from.Add((coord[0], coord[1], coord[2]));
            }

            // copy the data to array.
            List<(double X, double Y, double Z)> to = new List<(double X, double Y, double Z)>();
            foreach (CartesianCoord pnt in target)
            {
                double[] coord = pnt.GetCoordinate();
                to.Add((coord[0], coord[1], coord[2]));
            }

            return Resolve(from, to, weight, number);
        }

        /// <summary>
        /// resolve the transformation parameters by points collection
        /// </summary>
        /// <param name="source">points in source datum</param>
        /// <param name="target">points in target datum</param>
        /// <returns>transform parameters</returns>
        public static TransParameters Resolve(ECEF[] source, ECEF[] target)
        {
            if (source.Length > 0)
            {
                if (source[0].Dimension != 3)
                {
                    throw new GeodeticException("The source points dimension is error.");
                }
            }

            if (target.Length > 0)
            {
                if (target[0].Dimension != 3)
                {
                    throw new GeodeticException("The target points dimension is error.");
                }
            }

            // copy the data to array.
            List<(double X, double Y, double Z)> from = new List<(double X, double Y, double Z)>();
            foreach (ECEF pnt in source)
            {
                from.Add((pnt.X, pnt.Y, pnt.Z));
            }

            // copy the data to array.
            List<(double X, double Y, double Z)> to = new List<(double X, double Y, double Z)>();
            foreach (ECEF pnt in target)
            {
                to.Add((pnt.X, pnt.Y, pnt.Z));
            }

            // A point is 3D， so the weight must be 
            int count = source.Length * 3;
            double[,] weight = new double[count, count];
            for (int i = count - 1; i >= 0; i--)
            {
                weight[i, i] = 1;
            }

            return Resolve(from, to, weight, 7);
        }

        /// <summary>
        /// resolve the transformation parameters by points collection
        /// </summary>
        /// <param name="source">points in source datum</param>
        /// <param name="target">points in target datum</param>
        /// <returns>transform parameters</returns>
        public static TransParameters Resolve(List<ECEF> source, List<ECEF> target)
        {
            if (source.Count > 0)
            {
                if (source[0].Dimension != 3)
                {
                    throw new GeodeticException("The source points dimension is error.");
                }
            }

            if (target.Count > 0)
            {
                if (target[0].Dimension != 3)
                {
                    throw new GeodeticException("The target points dimension is error.");
                }
            }

            // copy the data to array.
            List<(double X, double Y, double Z)> from = new List<(double X, double Y, double Z)>();
            foreach (ECEF pnt in source)
            {
                from.Add((pnt.X, pnt.Y, pnt.Z));
            }

            // copy the data to array.
            List<(double X, double Y, double Z)> to = new List<(double X, double Y, double Z)>();
            foreach (ECEF pnt in target)
            {
                to.Add((pnt.X, pnt.Y, pnt.Z));
            }

            // A point is 3D， so the weight must be 
            int count = source.Count * 3;
            double[,] weight = new double[count, count];
            for (int i = count - 1; i >= 0; i--)
            {
                weight[i, i] = 1;
            }

            return Resolve(from, to, weight, 7);
        }

        /// <summary>
        /// resolve the transformation parameters by points collection
        /// </summary>
        /// <param name="source">points in source datum</param>
        /// <param name="target">points in target datum</param>
        /// <param name="weight">weight of points</param>
        /// <param name="numbers">parameter number, 3 or 4 or 7</param>
        /// <returns>transform parameters</returns>
        public static TransParameters Resolve(List<ECEF> source, List<ECEF> target,
                                              double[,] weight, int number = 7)
        {
            if (source.Count > 0)
            {
                if (source[0].Dimension != 3)
                {
                    throw new GeodeticException("The source points dimension is error.");
                }
            }

            if (target.Count > 0)
            {
                if (target[0].Dimension != 3)
                {
                    throw new GeodeticException("The target points dimension is error.");
                }
            }

            // copy the data to array.
            List<(double X, double Y, double Z)> from = new List<(double X, double Y, double Z)>();
            foreach (ECEF pnt in source)
            {
                from.Add((pnt.X, pnt.Y, pnt.Z));
            }

            // copy the data to array.
            List<(double X, double Y, double Z)> to = new List<(double X, double Y, double Z)>();
            foreach (ECEF pnt in target)
            {
                to.Add((pnt.X, pnt.Y, pnt.Z));
            }

            return Resolve(from, to, weight, number);
        }


        #region Solve the equation
        private static double[] SolveEquation(double[,] A, double[] L, double[,] P, int pntCount, int count)
        {
            // Normal equation
            double[,] B = new double[count, count + 1];
            for (int i = 0; i < count; i++)
            {
                // ATPA
                for (int k = 0; k < count; k++)
                {
                    B[i, k] = 0;
                    for (int j = 0; j < pntCount * 3; j++)
                    {
                        B[i, k] += A[j, i] * A[j, k] * P[j, j];
                    }
                }

                // AtPL
                B[i, count] = 0;
                for (int j = 0; j < pntCount * 3; j++)
                {
                    B[i, count] -= A[j, i] * L[j] * P[j, j];
                }
            }

            double[,] QQ = new double[count, count];
            for (int i = 0; i < count; i++)
            {
                QQ[i, i] = 1;
            }

            // Inverse N
            for (int i = 0; i < count; i++)
            {
                if (i != count - 1)
                {
                    double max = Math.Abs(B[i, i]);
                    int mark = i;
                    for (int j = i + 1; j < count; j++)
                    {
                        if (max < Math.Abs(B[j, i]))
                        {
                            max = Math.Abs(B[j, i]);
                            mark = j;
                        }
                    }

                    if (i != mark)
                    {
                        for (int j = 0; j <= count; j++)
                        {
                            double temp = B[i, j];
                            B[i, j] = B[mark, j];
                            B[mark, j] = temp;
                        }

                        for (int j = 0; j < count; j++)
                        {
                            double temp = QQ[i, j];
                            QQ[i, j] = QQ[mark, j];
                            QQ[mark, j] = temp;
                        }
                    }
                }

                for (int j = i + 1; j <= count; j++)
                {
                    B[i, j] /= B[i, i];
                }

                for (int j = 0; j < count; j++)
                {
                    QQ[i, j] /= B[i, i];
                }

                for (int j = 0; j < count; j++)
                {
                    if (B[i, j] != 0 && i != j)
                    {
                        for (int k = i + 1; k <= count; k++)
                        {
                            B[j, k] -= B[j, i] * B[i, k];
                        }

                        for (int k = 0; k < count; k++)
                        {
                            QQ[j, k] -= B[j, i] * QQ[i, k];
                        }
                    }
                }
            }

            //double M = 0;
            //for (int i = 0; i < pntCount * 3; i++)
            //{
            //    for (int j = 0; j < count; j++)
            //    {
            //        L[i] += A[i, j] * B[j, count];
            //    }
            //    M += L[i] * L[i] * P[i];
            //}

            //for (int i = 0; i < count; i++)
            //    for (int j = 0; j < count; j++)
            //        QQ[i, j] *= M / (pntCount * 3 - count);

            double[] result = new double[count];
            for (int i = 0; i < count; i++)
                result[i] = B[i, count];

            return result;
        }
        #endregion
    }
}
