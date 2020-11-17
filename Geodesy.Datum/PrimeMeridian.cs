using Newtonsoft.Json;
using Geodesy.Datum.Coordinate;

namespace Geodesy.Datum
{
    /// <summary>
    /// prime meridian
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public sealed class PrimeMeridian
    {
        /// <summary>
        /// Create a null object.
        /// </summary>
        public PrimeMeridian()
        { }

        /// <summary>
        /// Create a prime meridian
        /// </summary>
        /// <param name="lng">longitude</param>
        public PrimeMeridian(Longitude lng)
        {
            Longitude = lng;
        }

        /// <summary>
        /// identifier of the PrimeMeridian
        /// </summary>
        public Identifier Identifier { get; set; } = new Identifier(typeof(PrimeMeridian));

        /// <summary>
        /// longitude value of the PrimeMeridian
        /// </summary>
        [JsonConverter(typeof(AngleJsonConverter<Longitude>))]
        public Longitude Longitude { get; set; }

        /// <summary>
        /// BIH Zero Meridian (epoch 1984.0)
        /// </summary>
        public static readonly PrimeMeridian IRM = new PrimeMeridian(Longitude.Zero);

        #region data from https://github.com/orbisgis/cts/blob/master/src/main/java/org/cts/datum/PrimeMeridian.java
        /// <summary>
        /// Greenwich Meridian.
        /// </summary>
        public static readonly PrimeMeridian Greenwich = new PrimeMeridian()
        {
            Identifier = new Identifier("EPSG", "8901", "Greenwich", "Greenwich"),
            Longitude = new Longitude(0, 0, 0)
        };

        /// <summary>
        /// Lisbon Meridian.
        /// </summary>
        public static readonly PrimeMeridian Lisbon = new PrimeMeridian()
        {
            Identifier = new Identifier("EPSG", "8902", "Lisbon", "Lisbon"),
            Longitude = new Longitude(-9, 7, 54.862)
        };

        /// <summary>
        /// Paris Meridian.
        /// </summary>
        public static readonly PrimeMeridian Paris = new PrimeMeridian()
        {
            Identifier = new Identifier("EPSG", "8903", "Paris", "Paris",
                        "Value adopted by IGN (Paris) in 1936. Equivalent to 2°20'14.025\". " +
                        "Preferred by EPSG to earlier value of 2° 12' 5.022\" used by RGS London", null),
            Longitude = new Longitude(2, 20, 14.025)
        };

        /// <summary>
        /// Bogota Meridian.
        /// </summary>
        public static readonly PrimeMeridian Bogota = new PrimeMeridian()
        {
            Identifier = new Identifier("EPSG", "8904", "Bogota", "Bogota"),
            Longitude = new Longitude(-74, 4, 51.3)
        };

        /// <summary>
        /// Madrid Meridian.
        /// </summary>
        public static readonly PrimeMeridian Madrid = new PrimeMeridian()
        {
            Identifier = new Identifier("EPSG", "8905", "Madrid", "Madrid"),
            Longitude = new Longitude(-3, 41, 16.58)
        };

        /// <summary>
        /// Rome Meridian.
        /// </summary>
        public static readonly PrimeMeridian Rome = new PrimeMeridian()
        {
            Identifier = new Identifier("EPSG", "8906", "Rome", "Rome"),
            Longitude = new Longitude(12, 27, 8.4)
        };

        /// <summary>
        /// Bern Meridian.
        /// </summary>
        public static readonly PrimeMeridian Bern = new PrimeMeridian()
        {
            Identifier = new Identifier("EPSG", "8907", "Bern", "Bern",
                        "1895 value. Newer value of 7°26'22.335\" determined in 1938.", null),
            Longitude = new Longitude(7, 26, 22.5)
        };

        /// <summary>
        /// Jakarta Meridian.
        /// </summary>
        public static readonly PrimeMeridian Jakarta = new PrimeMeridian()
        {
            Identifier = new Identifier("EPSG", "8908", "Jakarta", "Jakarta"),
            Longitude = new Longitude(106, 48, 27.79)
        };

        /// <summary>
        /// Ferro Meridian.
        /// </summary>
        public static readonly PrimeMeridian Ferro = new PrimeMeridian()
        {
            Identifier = new Identifier("EPSG", "8909", "Ferro", "Ferro",
                        "Used in Austria and former Czechoslovakia.", null),
            Longitude = new Longitude(-17, 40, 0)
        };

        /// <summary>
        /// Brussels Meridian.
        /// </summary>
        public static readonly PrimeMeridian Brussels = new PrimeMeridian()
        {
            Identifier = new Identifier("EPSG", "8910", "Brussels", "Brussels"),
            Longitude = new Longitude(4, 22, 4.71)
        };

        /// <summary>
        /// Stockholm Meridian.
        /// </summary>
        public static readonly PrimeMeridian Stockholm = new PrimeMeridian()
        {
            Identifier = new Identifier("EPSG", "8911", "Stockholm"),
            Longitude = new Longitude(18, 03, 29.8)
        };

        /// <summary>
        /// Athens Meridian.
        /// </summary>
        public static readonly PrimeMeridian Athens = new PrimeMeridian()
        {
            Identifier = new Identifier("EPSG", "8912", "Athens", "Athens",
                        "Used in Greece for older mapping based on Hatt projection.", null),
            Longitude = new Longitude(23, 42, 58.815)
        };

        /// <summary>
        /// Oslo Meridian.
        /// </summary>
        public static readonly PrimeMeridian Oslo = new PrimeMeridian()
        {
            Identifier = new Identifier("EPSG", "8913", "Oslo", "Oslo",
                        "Formerly known as Kristiania or Christiania.", null),
            Longitude = new Longitude(10, 43, 22.5)
        };

        /// <summary>
        /// Paris (Royal Geographic Society) Meridian.
        /// </summary>
        public static readonly PrimeMeridian Paris_RGS = new PrimeMeridian()
        {
            Identifier = new Identifier("EPSG", "8914", "Paris (RGS)", "Paris (RGS)",
                        "Value replaced by IGN (France) in 1936 - see code 8903. Equivalent to 2.596898 grads.", null),
            Longitude = new Longitude(2, 12, 5.022)
        };
        #endregion

        #region data from https://en.wikipedia.org/wiki/Prime_meridian
        /// <summary>
        /// Bering Strait Meridian.
        /// </summary>
        public static readonly PrimeMeridian BeringStrait = new PrimeMeridian(new Longitude(-168, 30, 0));

        /// <summary>
        /// Antwerp Meridian.
        /// </summary>
        public static readonly PrimeMeridian Antwerp = new PrimeMeridian(new Longitude(4, 24, 0));

        /// <summary>
        /// Amsterdam Meridian.
        /// </summary>
        public static readonly PrimeMeridian Amsterdam = new PrimeMeridian(new Longitude(4, 53, 0));

        /// <summary>
        /// Pisa Meridian.
        /// </summary>
        public static readonly PrimeMeridian Pisa = new PrimeMeridian(new Longitude(10, 24, 0));

        /// <summary>
        /// Florence Meridian.
        /// </summary>
        public static readonly PrimeMeridian Florence = new PrimeMeridian(new Longitude(11, 15, 0));

        /// <summary>
        /// Copenhagen Meridian.
        /// </summary>
        public static readonly PrimeMeridian Copenhagen = new PrimeMeridian(new Longitude(12, 34, 32.25));

        /// <summary>
        /// Naples Meridian.
        /// </summary>
        public static readonly PrimeMeridian Naples = new PrimeMeridian(new Longitude(14, 15, 0));

        /// <summary>
        /// Krakow Meridian.
        /// </summary>
        public static readonly PrimeMeridian Krakow = new PrimeMeridian(new Longitude(19, 57, 21.43));

        /// <summary>
        /// Warsaw Meridian.
        /// </summary>
        public static readonly PrimeMeridian Warsaw = new PrimeMeridian(new Longitude(21, 0, 42));

        /// <summary>
        /// Oradea Meridian.
        /// </summary>
        public static readonly PrimeMeridian Oradea = new PrimeMeridian(new Longitude(21, 55, 16));

        /// <summary>
        /// Alexandria Meridian.
        /// </summary>
        public static readonly PrimeMeridian Alexandria = new PrimeMeridian(new Longitude(29, 53, 0));

        /// <summary>
        /// Saint Petersburg Meridian.
        /// </summary>
        public static readonly PrimeMeridian SaintPetersburg = new PrimeMeridian(new Longitude(30, 19, 42.09));

        /// <summary>
        /// Great Pyrmid of Giza Meridian.
        /// </summary>
        public static readonly PrimeMeridian GreatPyrmidOfGiza = new PrimeMeridian(new Longitude(31, 8, 3.69));

        /// <summary>
        /// Jerusalem Meridian.
        /// </summary>
        public static readonly PrimeMeridian Jerusalem = new PrimeMeridian(new Longitude(35, 13, 47.1));

        /// <summary>
        /// Mecca Meridian.
        /// </summary>
        public static readonly PrimeMeridian Mecca = new PrimeMeridian(new Longitude(39, 49, 34));

        /// <summary>
        /// Ujjain Meridian.
        /// </summary>
        public static readonly PrimeMeridian Ujjain = new PrimeMeridian(new Longitude(75, 47, 0));

        /// <summary>
        /// Kyoto Meridian.
        /// </summary>
        public static readonly PrimeMeridian Kyoto = new PrimeMeridian(new Longitude(136, 14, 0));
        #endregion
    }
}