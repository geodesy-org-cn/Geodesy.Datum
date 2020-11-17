
namespace Geodesy.Datum.Earth.Projection
{
    /// <summary>
    /// Projection classification based on the surface type.
    /// </summary>
    public enum ProjectionSurface
    {
        /// <summary>
        /// Azimuthal
        /// </summary>
        Azimuthal, // or Stereographic

        /// <summary>
        /// Conical
        /// </summary>
        Conical,

        /// <summary>
        /// Cylindrical
        /// </summary>
        Cylindrical,

        /// <summary>
        /// Hybrid
        /// </summary>
        Hybrid,

        /// <summary>
        /// Miscellaneous
        /// </summary>
        Miscellaneous,

        /// <summary>
        /// Polyconical
        /// </summary>
        Polyconical,

        /// <summary>
        /// Pseudo azinuthal
        /// </summary>
        PseudoAzinuthal,

        /// <summary>
        /// Pseudo conical
        /// </summary>
        PseudoConical,

        /// <summary>
        /// Pseudo cylindrical
        /// </summary>
        PseudoCylindrical,

        /// <summary>
        /// Retro azimuthal
        /// </summary>
        RetroAzimuthal
    }
}
