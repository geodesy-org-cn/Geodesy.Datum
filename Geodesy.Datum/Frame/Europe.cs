using Geodesy.Datum.CRS;
using Geodesy.Datum.Earth;
using System.Collections.Generic;
using Geodesy.Datum.Earth.Projection;

namespace Geodesy.Datum.Frame
{
    public static class Europe
    {
        /// <summary>
        /// European Datum 1950
        /// </summary>
        public static readonly LocalDatum ED50 = new LocalDatum()
        {
            Identifier = new Identifier("EPSG", "6230", "European Datum 1950", "ED50"),
            Name = "European Datum 1950",
            ShortName = "ED50",
            AreaOfUse = "Europe - west: Andorra; Cyprus; Denmark - onshore and offshore; Faroe Islands - onshore; " +
                        "France - offshore; Germany - offshore North Sea; Gibraltar; Greece - offshore; Israel - offshore; " +
                        "Italy including San Marino and Vatican City State; Ireland offshore; Malta; Netherlands - offshore; " +
                        "North Sea; Norway including Svalbard - onshore and offshore; Portugal - mainland - offshore; " +
                        "Spain - onshore; Turkey - onshore and offshore; United Kingdom UKCS offshore east of 6°W " +
                        "including Channel Islands (Guernsey and Jersey). Egypt - Western Desert; Iraq - onshore; Jordan.",

            Ellipsoid = Ellipsoid.International1924,
            PrimeMeridian = PrimeMeridian.Greenwich,

            // Fundamental point: Potsdam (Helmert Tower).
            // Latitude: 52 deg 22 min 51.4456 sec N; Longitude: 13 deg  3 min 58.9283 sec E (of Greenwich).
            GeodeticOrigin = GeodeticOrigin.PotsdamHelmertTwr_European1950,
        };

        /// <summary>
        /// European Datum 1987
        /// </summary>
        public static readonly LocalDatum ED87 = new LocalDatum()
        {
            Name = "European Datum 1987",
            ShortName = "ED87",
            Ellipsoid = Ellipsoid.International1924,
            GeodeticOrigin = GeodeticOrigin.MunichD7835,
        };

        /// <summary>
        /// Reseau Geodesique Francais 1993
        /// </summary>
        public static readonly GeocentricDatum RGF93 = new GeocentricDatum()
        {
            Name = "Reseau Geodesique Francais 1993",
            ShortName = "RGF93",
            AreaOfUse = "France - mainland onshore north of 49°N.",

            Ellipsoid = Ellipsoid.GRS80,
            PrimeMeridian = PrimeMeridian.Greenwich,

            // https://epsg.io/3950
            Projection = new LambertConformalConic2SP(new Dictionary<ProjectionParameter, double>{
                { ProjectionParameter.Semi_Major, Ellipsoid.GRS80.a },
                { ProjectionParameter.Inverse_Flattening, Ellipsoid.GRS80.InverseFlattening },
                { ProjectionParameter.Standard_Parallel_1, 49.25 },
                { ProjectionParameter.Standard_Parallel_2, 50.75 },
                { ProjectionParameter.Latitude_Of_Origin, 50 },
                { ProjectionParameter.Central_Meridian, 3 },
                { ProjectionParameter.False_Easting, 1700000 },
                { ProjectionParameter.False_Northing, 9200000 }
            })
        };

        /// <summary>
        /// Parametry Zemli System of 1990
        /// </summary>
        public static readonly GeocentricDatum PZ90 = new GeocentricDatum()
        {
            Name = "Parametry Zemli System of 1990",
            ShortName = "PZ90",
            Ellipsoid = Ellipsoid.PZ90,
        };
    }
}
