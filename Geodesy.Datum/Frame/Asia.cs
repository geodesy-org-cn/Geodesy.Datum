using Geodesy.Datum.CRS;
using Geodesy.Datum.Time;
using Geodesy.Datum.Earth;
using Geodesy.Datum.Units;
using Geodesy.Datum.Coordinate;
using Geodesy.Datum.Earth.Projection;

namespace Geodesy.Datum.Frame
{
    public static class Asia
    {
        /// <summary>
        /// Beijing Coordinate System 1954
        /// </summary>
        public static readonly LocalDatum BJS54 = new LocalDatum()
        {
            ShortName = "BJS54",
            Name = "Beijing Coordinate System 1954",
            AreaOfUse = "China - onshore and offshore",

            Ellipsoid = Ellipsoid.Krassovsky,
            NormalEllipsoid = null,

            GeodeticOrigin = new GeodeticOrigin(new Latitude(59, 46, 18.55),
                                                new Longitude(30, 19, 42.09),
                                                "Pulkovo 1942")
            {
                Location = "Pulkovo Geodetic Observation of the former Soviet Union",
                Orientation = EllipsoidOrientation.MutiplePoints
            },

            Projection = new GaussKrueger(Ellipsoid.Krassovsky),
        };

        /// <summary>
        /// China’s National Geodetic Coordinate System 1980
        /// </summary>
        public static readonly LocalDatum XAS80 = new LocalDatum()
        {
            ShortName = "XAS80",
            Name = "China’s National Geodetic Coordinate System 1980",
            AreaOfUse = "China - onshore and offshore",

            Ellipsoid = Ellipsoid.GRS75,
            NormalEllipsoid = new NormalEllipsoid(Ellipsoid.GRS75),

            GeodeticOrigin = new GeodeticOrigin(new Latitude(34, 32, 27),
                                                new Longitude(108, 55, 25),
                                                "Xi'An 1980")
            {
                Location = "Shijisi Village, Yongle Township, Jingyang County, Shaanxi Province, China",
                Orientation = EllipsoidOrientation.MutiplePoints,
                VerticalDeviation = new VerticalDeviation(-1.9, -1.6),
                ElevationAnomaly = -14
            },

            CoordinateSystem = new Cartesian3DSystem(
                                    Cartesian3DSystem.AxisYDirection.RightHand,
                                    Axis.JYD1968Z, Axis.X, Axis.Y),

            Projection = new GaussKrueger(Ellipsoid.GRS75),
        };

        /// <summary>
        /// Beijing Coordinate System 1954 (New)
        /// </summary>
        public static readonly LocalDatum NBJ54 = new LocalDatum()
        {
            ShortName = "NBJ54",
            Name = "Beijing Coordinate System 1954 (New)",
            AreaOfUse = "China - onshore and offshore",

            Ellipsoid = Ellipsoid.Krassovsky,

            GeodeticOrigin = XAS80.GeodeticOrigin,
            CoordinateSystem = XAS80.CoordinateSystem,

            Projection = new GaussKrueger(Ellipsoid.Krassovsky),
        };

        /// Geocentric Coordinate System 1978 (DX-1)
        /// </summary>
        public static readonly GeocentricDatum DX1 = new GeocentricDatum()
        {
            ShortName = "DX-1",
            Name = "Geocentric Coordinate System 1978"
        };

        /// <summary>
        /// Geocentric Coordinate System 1988 (DX-2)
        /// </summary>
        public static readonly GeocentricDatum DX2 = new GeocentricDatum()
        {
            ShortName = "DX-2",
            Name = "Geocentric Coordinate System 1988",

            CoordinateSystem = new Cartesian3DSystem(
                                    Cartesian3DSystem.AxisYDirection.RightHand,
                                    Axis.CIO1968Z, Axis.BIH1968X, Axis.Y)
        };

        /// <summary>
        /// China Geodetic Coordinate System 2000
        /// </summary>
        public static readonly GeocentricDatum CGCS2000 = new GeocentricDatum()
        {
            ShortName = "CGCS2000",
            Name = "China Geodetic Coordinate System 2000",

            Epoch = new UtcTime(2000.0),
            Ellipsoid = Ellipsoid.CGCS2000,
            LinearUnit = LinearUnit.Meter,

            CoordinateSystem = new Cartesian3DSystem(
                                    Cartesian3DSystem.AxisYDirection.RightHand,
                                    Axis.CIO1984Z, Axis.BIH1984X, Axis.Y),

            Projection = new GaussKrueger(Ellipsoid.CGCS2000),
        };

        /// <summary>
        /// BeiDou coordinate system
        /// </summary>
        public static readonly GeocentricDatum BDCS = new GeocentricDatum()
        {
            ShortName = "BDCS",
            Name = "BeiDou Coordinate System",

            Epoch = new UtcTime(2000.0),
            Ellipsoid = Ellipsoid.CGCS2000,
            LinearUnit = LinearUnit.Meter,

            CoordinateSystem = new Cartesian3DSystem(
                                    Cartesian3DSystem.AxisYDirection.RightHand,
                                    Axis.CIO1984Z, Axis.BIH1984X, Axis.Y),
        };



        /// <summary>
        /// Macao Geodetic Datum 2008
        /// </summary>
        public static readonly GeocentricDatum Macao2008 = new GeocentricDatum()
        {
            ShortName = "Macao2008",
            Name = "Macao Geodetic Datum 2008",
            AreaOfUse = "China - Macao - onshore and offshore",

            Epoch = new UtcTime(2008.38),
            Ellipsoid = Ellipsoid.GRS80,
            LinearUnit = LinearUnit.Meter,

            CoordinateSystem = new Cartesian3DSystem(
                                    Cartesian3DSystem.AxisYDirection.RightHand,
                                    Axis.CIO1984Z, Axis.BIH1984X, Axis.Y),
        };

        /// <summary>
        /// Macao 1920
        /// </summary>
        public static readonly LocalDatum Macao1920 = new LocalDatum()
        {
            Name = "Macao 1920",
            Ellipsoid = Ellipsoid.International1924,

            GeodeticOrigin = new GeodeticOrigin(new Latitude(22, 11, 03.139), 
                                                new Longitude(113, 31, 43.625))
            {
                Name = "Monte da Barra",
                Description = "Avenida Conselheiro Borja base A, later transferred to Monte da Barra",
            },

            Remarks = "Replaces Macao 1907. In 1955 an adjustment made in 1940 was assessed to have " + 
                      "unresolvable conflicts and the 1920 adjustment was reinstated.",
        };

        /// <summary>
        /// Japanese Geodetic Datum 2011
        /// </summary>
        public static readonly GeocentricDatum JGD2011 = new GeocentricDatum()
        {
            ShortName = "JDG2011",
            Name = "Japanese Geodetic Datum 2011",

            Epoch = new UtcTime(2011, 10, 21),
            Ellipsoid = Ellipsoid.GRS80,
            LinearUnit = LinearUnit.Meter,

            CoordinateSystem = new Cartesian3DSystem(
                                    Cartesian3DSystem.AxisYDirection.RightHand,
                                    Axis.CIO1984Z, Axis.BIH1984X, Axis.Y),
        };

        /// <summary>
        /// Japanese Geodetic Datum 2000
        /// </summary>
        public static readonly GeocentricDatum JGD2000 = new GeocentricDatum()
        {
            ShortName = "JGD2000",
            Name = "Japanese Geodetic Datum 2000",

            Epoch = new UtcTime(1997, 1, 1),
            Ellipsoid = Ellipsoid.GRS80,
            LinearUnit = LinearUnit.Meter,


            CoordinateSystem = new Cartesian3DSystem(
                                    Cartesian3DSystem.AxisYDirection.RightHand,
                                    Axis.CIO1984Z, Axis.BIH1984X, Axis.Y),
        };

        /// <summary>
        /// Geocentric datum of Korea
        /// </summary>
        public static readonly GeocentricDatum Korea2000 = new GeocentricDatum()
        {
            ShortName = "Korea2000",
            Name = "Geocentric datum of Korea",

            Epoch = new UtcTime(2002, 1, 1),
            Ellipsoid = Ellipsoid.GRS80,
            LinearUnit = LinearUnit.Meter,

            CoordinateSystem = new Cartesian3DSystem(
                                    Cartesian3DSystem.AxisYDirection.RightHand,
                                    Axis.CIO1984Z, Axis.BIH1984X, Axis.Y),
        };

        /// <summary>
        /// Korean Datum 1995
        /// </summary>
        public static readonly GeocentricDatum Korean1995 = new GeocentricDatum()
        {
            ShortName = "Korean1995",
            Name = "Korean Datum 1995",

            Epoch = new UtcTime(1995, 1, 1),
            Ellipsoid = Ellipsoid.WGS84,
            LinearUnit = LinearUnit.Meter,

            CoordinateSystem = new Cartesian3DSystem(
                                    Cartesian3DSystem.AxisYDirection.RightHand,
                                    Axis.CIO1984Z, Axis.BIH1984X, Axis.Y),
        };
    }
}
