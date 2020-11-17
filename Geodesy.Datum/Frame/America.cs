using Geodesy.Datum.CRS;
using Geodesy.Datum.Time;
using Geodesy.Datum.Earth;

namespace Geodesy.Datum.Frame
{
    public static class America
    {
        /// <summary>
        /// North American Datum of 1927
        /// </summary>
        public static readonly LocalDatum NAD27 = new LocalDatum()
        {
            Name = "North American Datum of 1927",
            Ellipsoid = Ellipsoid.Clarke1866,
            ShortName = "NAD27"
        };

        public static readonly LocalDatum NAD83_PACP00 = new LocalDatum()
        {
            Name = "North American Datum of 1983 (PACP00)",
            Ellipsoid = Ellipsoid.GRS80,
            AreaOfUse = "American Samoa, Marshall Islands, United States (USA) - Hawaii, " +
                       "United States minor outlying islands; onshore and offshore.",
            Remarks= "Replaces NAD83(HARN) and NAD83(FBN) in Hawaii and American Samoa. Replaced by NAD83(PA11).",
        };

        /// <summary>
        /// North American Datum of 1983
        /// </summary>
        public static readonly GeocentricDatum NAD83 = new GeocentricDatum()
        {
            Name = "North American Datum of 1983",
            Ellipsoid = Ellipsoid.GRS80,
            ShortName = "NAD83"
        };

        /// <summary>
        /// World Geodetic System of 1966
        /// </summary>
        public static readonly GeocentricDatum WGS66 = new GeocentricDatum()
        {
            Name = "World Geodetic System of 1966",
            Ellipsoid = Ellipsoid.WGS66,
            ShortName = "WGS66"
        };

        /// <summary>
        /// World Geodetic System of 1972
        /// </summary>
        public static readonly GeocentricDatum WGS72 = new GeocentricDatum()
        {
            Name = "World Geodetic System of 1972",
            Ellipsoid = Ellipsoid.WGS72,
            ShortName = "WGS72"
        };

        /// <summary>
        /// World Geodetic System of 1984 (G730)
        /// </summary>
        public static readonly GeocentricDatum WGS84_G730 = new GeocentricDatum()
        {
            Name = "World Geodetic System of 1984 (G730)",
            Ellipsoid = Ellipsoid.WGS84,
            Epoch = new UtcTime(1994, 1, 1),
            ShortName = "WGS84-G730"
        };

        /// <summary>
        /// World Geodetic System of 1984 (G873)
        /// </summary>
        public static readonly GeocentricDatum WGS84_G873 = new GeocentricDatum()
        {
            Name = "World Geodetic System of 1984 (G873)",
            Ellipsoid = Ellipsoid.WGS84,
            Epoch = new UtcTime(1997, 1, 1),
            ShortName = "WGS84-G873"
        };


        /// <summary>
        /// World Geodetic System of 1984 (G1150)
        /// </summary>
        public static readonly GeocentricDatum WGS84_G1150 = new GeocentricDatum()
        {
            Name = "World Geodetic System of 1984 (G1150)",
            Ellipsoid = Ellipsoid.WGS84,
            Epoch = new UtcTime(2001, 1, 1),
            ShortName = "WGS84"
        };

        /// <summary>
        /// World Geodetic System of 1984 (G1674)
        /// </summary>
        public static readonly GeocentricDatum WGS84_G1674 = new GeocentricDatum()
        {
            Name = "World Geodetic System of 1984 (G1674)",
            Ellipsoid = Ellipsoid.WGS84,
            Epoch = new UtcTime(2005, 1, 1),
            ShortName = "WGS84-G1674"
        };

        /// <summary>
        /// World Geodetic System of 1984 (G1762)
        /// </summary>
        public static readonly GeocentricDatum WGS84_G1762 = new GeocentricDatum()
        {
            Name = "World Geodetic System of 1984 (G1762)",
            Ellipsoid = Ellipsoid.WGS84,
            Epoch = new UtcTime(2005, 1, 1),
            ShortName = "WGS84-G1762"
        };

        /// <summary>
        /// World Geodetic System of 1984
        /// </summary>
        public static readonly GeocentricDatum WGS84 = new GeocentricDatum()
        {
            Name = "World Geodetic System of 1984",
            Ellipsoid = Ellipsoid.WGS84,
            Epoch = new UtcTime(1984, 1, 1),            
            ShortName = "WGS84"
        };
    }
}
