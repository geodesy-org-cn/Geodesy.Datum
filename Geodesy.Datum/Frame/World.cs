using Geodesy.Datum.CRS;
using Geodesy.Datum.Time;
using Geodesy.Datum.Units;
using Geodesy.Datum.Earth;

namespace Geodesy.Datum.Frame
{
    public static class World
    {
        /// <summary>
        /// International Terrestrial Reference Frame (ITRF)
        /// </summary>
        public static readonly GeocentricDatum ITRF = ITRF2014;

        /// <summary>
        /// International Terrestrial Reference Frame (ITRF89)
        /// </summary>
        public static readonly GeocentricDatum ITRF88 = new GeocentricDatum()
        {
            ShortName = "ITRF88",
            Name = "International Terrestrial Reference Frame 1988",
            AreaOfUse = "World",
            Scope = "Geodesy",
            Remarks = "Realization of the IERS Terrestrial Reference System (ITRS) at epoch 1988.0. Replaced by ITRF89",

            Epoch = new UtcTime(1988.0),
            Ellipsoid = Ellipsoid.GRS80,
            LinearUnit = LinearUnit.Meter,
        };

        /// <summary>
        /// International Terrestrial Reference Frame (ITRF89)
        /// </summary>
        public static readonly GeocentricDatum ITRF89 = new GeocentricDatum()
        {
            ShortName = "ITRF89",
            Name = "International Terrestrial Reference Frame 1989",
            AreaOfUse = "World",
            Scope = "Geodesy",
            Remarks = "Realization of the IERS Terrestrial Reference System (ITRS) from April 1991. Replaces ITRF88. Replaced by ITRF90",

            Ellipsoid = Ellipsoid.GRS80,
            LinearUnit = LinearUnit.Meter,

            CoordinateSystem = new Cartesian3DSystem(
                                    Cartesian3DSystem.AxisYDirection.RightHand,
                                    Axis.CIO1984Z, Axis.BIH1984X, Axis.Y),
        };

        /// <summary>
        /// International Terrestrial Reference Frame (ITRF90)
        /// </summary>
        public static readonly GeocentricDatum ITRF90 = new GeocentricDatum()
        {
            ShortName = "ITRF90",
            Name = "International Terrestrial Reference Frame 1990",
            AreaOfUse = "World",
            Scope = "Geodesy",
            Remarks = "Realization of the IERS Terrestrial Reference System (ITRS) from December 1991. Replaces ITRF89. Replaced by ITRF91",

            Ellipsoid = Ellipsoid.GRS80,
            LinearUnit = LinearUnit.Meter,

            CoordinateSystem = new Cartesian3DSystem(
                                    Cartesian3DSystem.AxisYDirection.RightHand,
                                    Axis.CIO1984Z, Axis.BIH1984X, Axis.Y),
        };

        /// <summary>
        /// International Terrestrial Reference Frame (ITRF91)
        /// </summary>
        public static readonly GeocentricDatum ITRF91 = new GeocentricDatum()
        {
            ShortName = "ITRF91",
            Name = "International Terrestrial Reference Frame 1991",
            AreaOfUse = "World",
            Scope = "Geodesy",

            Ellipsoid = Ellipsoid.GRS80,
            LinearUnit = LinearUnit.Meter,

            CoordinateSystem = new Cartesian3DSystem(
                                    Cartesian3DSystem.AxisYDirection.RightHand,
                                    Axis.CIO1984Z, Axis.BIH1984X, Axis.Y),
        };

        /// <summary>
        /// International Terrestrial Reference Frame (ITRF92)
        /// </summary>
        public static readonly GeocentricDatum ITRF92 = new GeocentricDatum()
        {
            ShortName = "ITRF92",
            Name = "International Terrestrial Reference Frame 1992",
            AreaOfUse = "World",
            Scope = "Geodesy",

            Ellipsoid = Ellipsoid.GRS80,
            LinearUnit = LinearUnit.Meter,

            CoordinateSystem = new Cartesian3DSystem(
                                    Cartesian3DSystem.AxisYDirection.RightHand,
                                    Axis.CIO1984Z, Axis.BIH1984X, Axis.Y),
        };

        /// <summary>
        /// International Terrestrial Reference Frame (ITRF93)
        /// </summary>
        public static readonly GeocentricDatum ITRF93 = new GeocentricDatum()
        {
            ShortName = "ITRF93",
            Name = "International Terrestrial Reference Frame 1993",
            AreaOfUse = "World",
            Scope = "Geodesy",

            Ellipsoid = Ellipsoid.GRS80,
            LinearUnit = LinearUnit.Meter,

            CoordinateSystem = new Cartesian3DSystem(
                                    Cartesian3DSystem.AxisYDirection.RightHand,
                                    Axis.CIO1984Z, Axis.BIH1984X, Axis.Y),
        };

        /// <summary>
        /// International Terrestrial Reference Frame (ITRF94)
        /// </summary>
        public static readonly GeocentricDatum ITRF94 = new GeocentricDatum()
        {
            ShortName = "ITRF94",
            Name = "International Terrestrial Reference Frame 1994",
            AreaOfUse = "World",
            Scope = "Geodesy",

            Ellipsoid = Ellipsoid.GRS80,
            LinearUnit = LinearUnit.Meter,

            CoordinateSystem = new Cartesian3DSystem(
                                    Cartesian3DSystem.AxisYDirection.RightHand,
                                    Axis.CIO1984Z, Axis.BIH1984X, Axis.Y),
        };

        /// <summary>
        /// International Terrestrial Reference Frame (ITRF96)
        /// </summary>
        public static readonly GeocentricDatum ITRF96 = new GeocentricDatum()
        {
            ShortName = "ITRF96",
            Name = "International Terrestrial Reference Frame 1996",
            AreaOfUse = "World",
            Scope = "Geodesy",

            Ellipsoid = Ellipsoid.GRS80,
            LinearUnit = LinearUnit.Meter,

            CoordinateSystem = new Cartesian3DSystem(
                                    Cartesian3DSystem.AxisYDirection.RightHand,
                                    Axis.CIO1984Z, Axis.BIH1984X, Axis.Y),
        };

        /// <summary>
        /// International Terrestrial Reference Frame (ITRF97)
        /// </summary>
        public static readonly GeocentricDatum ITRF97 = new GeocentricDatum()
        {
            ShortName = "ITRF97",
            Name = "International Terrestrial Reference Frame 1997",
            AreaOfUse = "World",
            Scope = "Geodesy",

            Ellipsoid = Ellipsoid.GRS80,
            LinearUnit = LinearUnit.Meter,

            CoordinateSystem = new Cartesian3DSystem(
                                    Cartesian3DSystem.AxisYDirection.RightHand,
                                    Axis.CIO1984Z, Axis.BIH1984X, Axis.Y),
        };

        /// <summary>
        /// International Terrestrial Reference Frame (ITRF2000)
        /// </summary>
        public static readonly GeocentricDatum ITRF2000 = new GeocentricDatum()
        {
            ShortName = "ITRF2000",
            Name = "International Terrestrial Reference Frame 2000",
            AreaOfUse = "World",
            Scope = "Geodesy",

            Ellipsoid = Ellipsoid.GRS80,
            LinearUnit = LinearUnit.Meter,

            CoordinateSystem = new Cartesian3DSystem(
                                    Cartesian3DSystem.AxisYDirection.RightHand,
                                    Axis.CIO1984Z, Axis.BIH1984X, Axis.Y),
        };

        /// <summary>
        /// International Terrestrial Reference Frame (ITRF2005)
        /// </summary>
        public static readonly GeocentricDatum ITRF2005 = new GeocentricDatum()
        {
            ShortName = "ITRF2005",
            Name = "International Terrestrial Reference Frame 2005",
            AreaOfUse = "World",
            Scope = "Geodesy",

            Ellipsoid = Ellipsoid.GRS80,
            LinearUnit = LinearUnit.Meter,

            CoordinateSystem = new Cartesian3DSystem(
                                    Cartesian3DSystem.AxisYDirection.RightHand,
                                    Axis.CIO1984Z, Axis.BIH1984X, Axis.Y),
        };

        /// <summary>
        /// International Terrestrial Reference Frame (ITRF2008)
        /// </summary>
        public static readonly GeocentricDatum ITRF2008 = new GeocentricDatum()
        {
            ShortName = "ITRF2008",
            Name = "International Terrestrial Reference Frame 2008",
            AreaOfUse = "World",
            Scope = "Geodesy",
            Remarks = "Realization of the IERS Terrestrial Reference System(ITRS) from 2012. Replaces ITRF2005",

            Epoch = new UtcTime(2005.0),
            Ellipsoid = Ellipsoid.GRS80,
            LinearUnit = LinearUnit.Meter,

            CoordinateSystem = new Cartesian3DSystem(
                                    Cartesian3DSystem.AxisYDirection.RightHand,
                                    Axis.CIO1984Z, Axis.BIH1984X, Axis.Y),
        };

        /// <summary>
        /// International Terrestrial Reference Frame (ITRF2014)
        /// </summary>
        public static readonly GeocentricDatum ITRF2014 = new GeocentricDatum()
        {
            ShortName = "ITRF2014",
            Name = "International Terrestrial Reference Frame 2014",
            AreaOfUse = "World",
            Scope = "Geodesy",
            Remarks = "Realization of the IERS Terrestrial Reference System (ITRS). Replaces ITRF2008 from January 2016",

            Epoch = new UtcTime(2010.0),
            Ellipsoid = Ellipsoid.GRS80,
            LinearUnit = LinearUnit.Meter,

            CoordinateSystem = new Cartesian3DSystem(
                                    Cartesian3DSystem.AxisYDirection.RightHand,
                                    Axis.CIO1984Z, Axis.BIH1984X, Axis.Y),
        };
    }
}
