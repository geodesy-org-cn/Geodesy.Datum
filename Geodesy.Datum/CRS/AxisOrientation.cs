
namespace Geodesy.Datum.CRS
{
    /// <summary>
    /// Axis different directions.
    /// </summary>
    public enum AxisOrientation
    {
        /// <summary>
        /// Unknown or unspecified axis orientation
        /// </summary>
        Other,

        /// <summary>
        /// Increasing ordinates values go  east
        /// </summary>
        East,

        /// <summary>
        /// Increasing ordinates values go west
        /// </summary>
        West,

        /// <summary>
        /// Increasing ordinates values go north
        /// </summary>
        North,

        /// <summary>
        /// Increasing ordinates values go south
        /// </summary>
        South,

        /// <summary>
        /// Increasing ordinates values go up
        /// </summary>
        Up,

        /// <summary>
        /// Increasing ordinates values go down
        /// </summary>
        Down
    }
}
