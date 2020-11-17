
namespace Geodesy.Datum.Coordinate
{
    public interface IPolarCoord
    {
        /// <summary>
        /// 
        /// </summary>
        Angle Azimuth { get; set; }

        /// <summary>
        /// 
        /// </summary>
        double Range { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Units.AngularUnit AngularUnit { get; }

        /// <summary>
        /// 
        /// </summary>
        Units.LinearUnit LinearUnit { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="azimuth"></param>
        void SetCoordinate(double distance, Angle azimuth);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="length"></param>
        /// <param name="azimuth"></param>
        /// <param name="angle"></param>
        void SetCoordinate(double distance, Units.LinearUnit length, double azimuth, Units.AngularUnit angle);
    }
}
