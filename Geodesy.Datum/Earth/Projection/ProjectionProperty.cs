
namespace Geodesy.Datum.Earth.Projection
{
    /// <summary>
    /// Which measurement is preserved in the projection
    /// </summary>
    public enum ProjectionProperty
    {
        /// <summary>
        /// Neither equal-area nor conformal
        /// </summary>
        Aphylactic,

        /// <summary>
        /// Locally shape preserving (angle preserving)
        /// </summary>
        Conformal,

        /// <summary>
        /// Area preserving (also called Equiarea, Equivalent, Authalic)
        /// </summary>
        EqualArea,

        /// <summary>
        /// Distance preserving
        /// </summary>
        Equidistant,

        /// <summary>
        /// shortest route, a trait preserving
        /// </summary>
        Gnomonic
    }
}
