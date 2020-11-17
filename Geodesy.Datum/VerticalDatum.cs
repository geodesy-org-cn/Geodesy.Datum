using Geodesy.Datum.Earth;
using Geodesy.Datum.Geoid;

namespace Geodesy.Datum
{
    /// <summary>
    /// 
    /// </summary>
    public class VerticalDatum
    {
        /// <summary>
        /// origin surface of vertical height/depth value
        /// </summary>
        public enum SurfaceType
        {
            /// <summary>
            /// gravimetric height based on geoid
            /// </summary>
            Geoid,
            /// <summary>
            /// geodetic height based on ellipsoid
            /// </summary>
            Ellipsoid,
            /// <summary>
            /// tidal height/depth based on sea levels
            /// </summary>
            MeanSeaLevel,
            /// <summary>
            /// normal height from quasi-geoid
            /// </summary>
            Quasigeoid,
            /// <summary>
            /// dynamic height from local area 
            /// </summary>
            Local,
            /// <summary>
            /// height determined by atmospheric pressure
            /// </summary>
            Barometric
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="alias"></param>
        /// <param name="origin"></param>
        /// <param name="datum"></param>
        public VerticalDatum(string name, string alias, SurfaceType origin, CoordinateDatum datum)
        {
            Name = name;
            Alias = alias;
            Surface = origin;
            Ellipsoid = datum.Ellipsoid;
            Geoid = Datum.Geoid.ModelType.Unknown;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="alias"></param>
        /// <param name="origin"></param>
        public VerticalDatum(string name, string alias, SurfaceType origin)
        {
            Name = name;
            Alias = alias;
            Surface = origin;
            Ellipsoid = null;
            Geoid = Datum.Geoid.ModelType.Unknown;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="alias"></param>
        /// <param name="model"></param>
        public VerticalDatum(string name, string alias, ModelType model)
        {
            Name = name;
            Alias = alias;
            Surface = SurfaceType.Geoid;
            Geoid = model;
            Ellipsoid = null;
        }

        /// <summary>
        /// Identifier
        /// </summary>
        public Identifier Identifier { get; set; }

        /// <summary>
        /// name of vertical datum
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// alias of vertical datum
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// height origin surface
        /// </summary>
        public SurfaceType Surface { get; set; }

        /// <summary>
        /// earth ellipsoid
        /// </summary>
        public Ellipsoid Ellipsoid { get; set; }

        /// <summary>
        /// geoid model
        /// </summary>
        public ModelType Geoid { get; set; }

        /// <summary>
        /// Huanghai Mean Sea Level 1956
        /// </summary>
        public static readonly VerticalDatum Huanghai1956 = new VerticalDatum("Huanghai Mean Sea Level 1956", "Huanghai1956", SurfaceType.MeanSeaLevel);

        /// <summary>
        /// National Height Datum 1985
        /// </summary>
        public static readonly VerticalDatum China1985 = new VerticalDatum("China National Height Datum 1985", "China1985", SurfaceType.MeanSeaLevel);

        /// <summary>
        /// Theoretical Lowest Tide
        /// </summary>
        public static readonly VerticalDatum TLT = new VerticalDatum("Theoretical Lowest Tide", "TLT", SurfaceType.MeanSeaLevel);

        /// <summary>
        /// Lowest Astronomical Tide
        /// </summary>
        public static readonly VerticalDatum LAT = new VerticalDatum("Lowest Astronomical Tide", "LAT", SurfaceType.MeanSeaLevel);

        /// <summary>
        /// Mean Lower Low Water
        /// </summary>
        public static readonly VerticalDatum MLLW = new VerticalDatum("Mean Lower Low Water", "MLLW", SurfaceType.MeanSeaLevel);

        /// <summary>
        /// Lowest Low Water
        /// </summary>
        public static readonly VerticalDatum LLW = new VerticalDatum("Lowest Low Water", "LLW", SurfaceType.MeanSeaLevel);

        /// <summary>
        /// Mean Lower Low Water Springs
        /// </summary>
        public static readonly VerticalDatum MLLWS = new VerticalDatum("Mean Lower Low Water Springs", "MLLWS", SurfaceType.MeanSeaLevel);

        /// <summary>
        /// Indian Spring Low Water
        /// </summary>
        public static readonly VerticalDatum ISLW = new VerticalDatum("Indian Spring Low Water", "ISLW", SurfaceType.MeanSeaLevel);

        /// <summary>
        /// Mean Low Water
        /// </summary>
        public static readonly VerticalDatum MLW = new VerticalDatum("Mean Low Water", "MLW", SurfaceType.MeanSeaLevel);

        /// <summary>
        /// Mean Low Water Springs
        /// </summary>
        public static readonly VerticalDatum MLWS = new VerticalDatum("Mean Low Water Springs", "MLWS", SurfaceType.MeanSeaLevel);

        /// <summary>
        /// Equatorial Springs Low Water
        /// </summary>
        public static readonly VerticalDatum ESLW = new VerticalDatum("Equatorial Springs Low Water", "ESLW", SurfaceType.MeanSeaLevel);

        /// <summary>
        /// the theoretical lowest tide defined by Vladimirsky
        /// </summary>
        public static readonly VerticalDatum Vladimirsky = new VerticalDatum("Theoretical Lowest Tide defined by Vladimirsky", "Vladimirsky", SurfaceType.MeanSeaLevel);

        /// <summary>
        /// North American Vertical Datum of 1988
        /// </summary>
        public static readonly VerticalDatum NAVD88 = new VerticalDatum("North American Vertical Datum of 1988", "NAVD88", SurfaceType.MeanSeaLevel);

        /// <summary>
        /// National Geodetic Vertical Datum of 1929
        /// </summary>
        public static readonly VerticalDatum NGVD29 = new VerticalDatum("National Geodetic Vertical Datum of 1929", "NGVD29", SurfaceType.MeanSeaLevel);

        /// <summary>
        /// the Baltic System of Heights
        /// </summary>
        public static readonly VerticalDatum Kronstadt = new VerticalDatum("the Baltic System of Heights", "Kronstadt", SurfaceType.MeanSeaLevel);
    }
}
