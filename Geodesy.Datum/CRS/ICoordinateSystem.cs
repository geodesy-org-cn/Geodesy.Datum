using Geodesy.Datum.Units;

namespace Geodesy.Datum.CRS
{
    /// <summary>
    /// Interface of coordinate system
    /// </summary>
    public interface ICoordinateSystem
    {
        /// <summary>
        /// Zero point of coordinate sytem
        /// </summary>
        Origin Origin { get; set; }

        /// <summary>
        /// Dimension of the coordinate system
        /// </summary>
        int Dimension { get; }

        /// <summary>
        /// Get axis
        /// </summary>
        /// <param name="name">name of axis</param>
        /// <returns>axis</returns>
        Axis GetAxis(string name);

        /// <summary>
        /// Set the axis
        /// </summary>
        /// <param name="axis">axis</param>
        /// <returns></returns>
        bool SetAxis(Axis axis);

        /// <summary>
        /// Get the unit of axis
        /// </summary>
        /// <param name="axis">axis</param>
        /// <returns>quantity</returns>
        Unit GetUnit(Axis axis);

        /// <summary>
        /// Set the unit of axis
        /// </summary>
        /// <param name="axis">axis</param>
        /// <param name="unit">unit</param>
        /// <returns></returns>
        bool SetUnit(Axis axis, Unit unit);
    }
}
