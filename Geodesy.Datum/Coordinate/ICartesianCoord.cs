
namespace Geodesy.Datum.Coordinate
{
    public interface ICartesianCoord
    {
        /// <summary>
        /// 
        /// </summary>
        int Dimension { get; }

        /// <summary>
        /// 
        /// </summary>
        Units.LinearUnit Unit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        double[] GetCoordinate();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        double[] GetCoordinate(Units.LinearUnit unit); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coord"></param>
        void SetCoordinate(double[] coord);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="unit"></param>
        void SetCoordinate(double[] coord, Units.LinearUnit unit);

        /// <summary>
        /// 
        /// </summary>
        double GetDistance(CartesianCoord coord);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scale"></param>
        CartesianCoord Rescale(double scale);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="theta"></param>
        /// <param name="fixedAxis"></param>
        /// <returns></returns>
        CartesianCoord Rotate(Angle theta, int fixedAxis);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        CartesianCoord Shift(CartesianCoord delta);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        CartesianCoord Shift(params double[] delta);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="theta"></param>
        /// <returns></returns>
        CartesianCoord Reflect(Angle theta);
    }
}
