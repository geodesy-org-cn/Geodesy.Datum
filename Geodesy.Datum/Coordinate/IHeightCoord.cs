
namespace Geodesy.Datum.Coordinate
{
    public interface IHeightCoord : ICartesianCoord
    {
        /// <summary>
        /// 
        /// </summary>
        double Height { get; set; }

        /// <summary>
        /// 
        /// </summary>
        HeightSystem System { get;  }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hgt"></param>
        void SetHeight(double hgt);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hgt"></param>
        /// <param name="unit"></param>
        void SetHeight(double hgt, Units.Unit unit);
    }
}
