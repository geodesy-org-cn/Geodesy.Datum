using Geodesy.Datum.Time;
using Geodesy.Datum.Earth;
using Geodesy.Datum.Units;

namespace Geodesy.Datum
{
    /// <summary>
    /// Geocentric datum with its origin at the center of mass of the earth.
    /// </summary>
    public class GeocentricDatum : CoordinateDatum
    {
        /// <summary>
        /// Create a null datum object.
        /// </summary>
        public GeocentricDatum()
        { }

        /// <summary>
        /// Create a geocentric datum
        /// </summary>
        /// <param name="name">name of datum</param>
        /// <param name="ellipsoid">earth ellipsoid</param>
        public GeocentricDatum(string name, Ellipsoid ellipsoid)
            : this(name, ellipsoid, null)
        { }

        /// <summary>
        /// Create a geocentric datum
        /// </summary>
        /// <param name="name">name of datum</param>
        /// <param name="ellipsoid">earth ellipsoid</param>
        /// <param name="alias">abbreviated alias of datum</param>
        public GeocentricDatum(string name, Ellipsoid ellipsoid, string alias)
            : this(new Identifier(typeof(GeocentricDatum)), name, ellipsoid, alias, null)
        { }

        /// <summary>
        /// Create a geocentric datum
        /// </summary>
        /// <param name="identifier">identifier of the datum</param>
        /// <param name="name">name of datum</param>
        /// <param name="ellipsoid">earth ellipsoid</param>
        /// <param name="alias">abbreviated alias of datum</param>
        /// <param name="domain">applied area of the datum</param>
        public GeocentricDatum(Identifier identifier, string name, Ellipsoid ellipsoid, string alias, string domain = "")
        {
            Identifier = identifier;
            Name = name;
            Ellipsoid = ellipsoid;
            ShortName = alias;
            AreaOfUse = domain;
        }

        /// <summary>
        /// epoch of geocentric datum
        /// </summary>
        public TimeSystem Epoch { get; set; }

        /// <summary>
        /// Length unit
        /// </summary>
        public LinearUnit LinearUnit { get; set; }

        /// <summary>
        /// Time evolution
        /// </summary>
        public object Evolution { get; set; }
    }
}
