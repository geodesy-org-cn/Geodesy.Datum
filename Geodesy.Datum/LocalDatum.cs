using Geodesy.Datum.Earth;

namespace Geodesy.Datum
{
    /// <summary>
    /// Local coodinate datum (also called non-geocentric datum) with its origin at the geometric
    /// center of the reference ellipsoid based on classical surveying techniques.
    /// </summary>
    public class LocalDatum : CoordinateDatum
    {
        /// <summary>
        /// Create a null datum object.
        /// </summary>
        public LocalDatum()
        { }

        /// <summary>
        /// create a local datum
        /// </summary>
        /// <param name="name">name of datum</param>
        /// <param name="ellipsoid">reference ellipsoid</param>
        /// <param name="origin">geodetic origin</param>
        /// <param name="alias">alias of the datum</param>
        public LocalDatum(string name, Ellipsoid ellipsoid, string alias = "")
            : this(new Identifier(typeof(LocalDatum)), name, ellipsoid, alias, null)
        { }

        /// <summary>
        /// create a local datum
        /// </summary>
        /// <param name="identifier">identifier of the datum</param>
        /// <param name="name">name of datum</param>
        /// <param name="ellipsoid">reference ellipsoid</param>
        /// <param name="origin">geodetic origin</param>
        /// <param name="alias">alias of the datum</param>
        /// <param name="domain">applied area of the datum</param>
        public LocalDatum(Identifier identifier, string name, Ellipsoid ellipsoid, string alias, string domain)
        {
            Identifier = identifier;
            Name = name;
            Ellipsoid = ellipsoid;
            ShortName = alias;
            AreaOfUse = domain;
        }

        /// <summary>
        /// geodetic origin
        /// </summary>
        public GeodeticOrigin GeodeticOrigin { get; set; }

        /// <summary>
        /// Height system
        /// </summary>
        public HeightSystem HeightSystem { get; set; }

        /// <summary>
        /// normal ellipsoid
        /// </summary>
        public NormalEllipsoid NormalEllipsoid { get; set; }
    }
}
