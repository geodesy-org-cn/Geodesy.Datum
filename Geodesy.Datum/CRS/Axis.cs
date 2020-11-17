using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace Geodesy.Datum.CRS
{
    /// <summary>
    /// One of the fixed reference lines of a coordinate system. Usually, axis is a term 
    /// reserved to cartesian coordinate systems made of several perpendicular axis.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public sealed class Axis
    {
        /// <summary>
        /// The dictionary that allows to get Axes from their name.
        /// </summary>
        private static Dictionary<AxisOrientation, Dictionary<string, Axis>> _axisFromOriAndName =
            new Dictionary<AxisOrientation, Dictionary<string, Axis>>();

        /// <summary>
        /// Get the axis by name and orientation
        /// </summary>
        /// <param name="orientation">orientation type</param>
        /// <param name="name">orientation name</param>
        /// <returns>axis</returns>
        public static Axis GetAxis(AxisOrientation orientation, string name)
        {
            Dictionary<string, Axis> map = _axisFromOriAndName[orientation];

            if (map == null)
            {
                return null;
            }

            return map[name];
        }

        /// <summary>
        /// This private constructor is designed for JsonConvert.DeserializeObject().
        /// </summary>
        private Axis()
        { }
        
        /// <summary>
        /// The name of this Axis (X, Y, Z, longitude, Altitude;)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The orientation of the axis as it is defined in OGC WKT
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public AxisOrientation Orientation { get; set; }

        /// <summary>
        /// Create a new Axis.
        /// </summary>
        /// <param name="name">name of this new Axis</param>
        /// <param name="orientation">the orientation of the axis (EAST, NORTH, UP, DOWN;)</param>
        public Axis(string name, AxisOrientation orientation)
        {
            Name = name;
            Orientation = orientation;

            Dictionary<string, Axis> map = _axisFromOriAndName[orientation];
            if (map == null)
            {
                map = new Dictionary<string, Axis>();
                _axisFromOriAndName.Add(orientation, map);
            }

            if (map[Name] == null)
            {
                map.Add(Name, this);
            }
            else
            {
                map[Name] = this;
            }

            _axisFromOriAndName[orientation] = map;
        }

        /// <summary>
        /// Get the orientation name
        /// </summary>
        /// <param name="name">name</param>
        /// <returns>orientation</returns>
        public static AxisOrientation GetOrientation(string name)
        {
            switch (name.ToUpper())
            {
                case "EAST":
                    return AxisOrientation.East;

                case "NORTH":
                    return AxisOrientation.North;

                case "WEST":
                    return AxisOrientation.West;

                case "SOUTH":
                    return AxisOrientation.South;

                case "UP":
                    return AxisOrientation.Up;

                case "DOWN":
                    return AxisOrientation.Down;

                default:
                    return AxisOrientation.Other;
            }
        }

        /// <summary>
        /// Easting axis. Used for planimetric coordinate system, generally in pair with northing.
        /// </summary>
        public static readonly Axis Easting = new Axis("Easting", AxisOrientation.East);

        /// <summary>
        /// Northing axis. Used for planimetric coordinate system, generally in pair with easting.
        /// </summary>
        public static readonly Axis Northing = new Axis("Northing", AxisOrientation.North);

        /// <summary>
        /// Westing axis. Used for planimetric coordinate system, generally in pair with southing.
        /// </summary>
        public static readonly Axis Westing = new Axis("Westing", AxisOrientation.West);

        /// <summary>
        /// Southing axis. Used for planimetric coordinate system, generally in pair with westing.
        /// </summary>
        public static readonly Axis Southing = new Axis("Southing", AxisOrientation.South);

        /// <summary>
        /// x axis. Used for planimetric coordinate system, sometimes used in place of easting.
        /// </summary>
        public static readonly Axis x = new Axis("x", AxisOrientation.North);

        /// <summary>
        /// y axis. Used for planimetric coordinate system, sometimes used in place of northing.
        /// </summary>
        public static readonly Axis y = new Axis("y", AxisOrientation.East);

        /// <summary>
        /// h axis. Used for planimetric coordinate system, sometimes used in place of height.
        /// </summary>
        public static readonly Axis h = new Axis("h", AxisOrientation.Up);

        /// <summary>
        /// Altitude axis. Used for vertical/compound system.
        /// </summary>
        public static readonly Axis Altitude = new Axis("Altitude", AxisOrientation.Up);

        /// <summary>
        /// Depth axis. Used for hydrography.
        /// </summary>
        public static readonly Axis Depth = new Axis("Depth", AxisOrientation.Down);

        /// <summary>
        /// Parallel axis. Used for geographic coordinate system, generally in pair with meridian.
        /// </summary>
        public static readonly Axis Parallel = new Axis("Parallel", AxisOrientation.East);

        /// <summary>
        /// Meridian axis. Used for geographic coordinate system, generally in pair with parallel.
        /// </summary>
        public static readonly Axis Meridian = new Axis("Meridian", AxisOrientation.North);

        /// <summary>
        /// Height axis. Used for 3D Geometric coordinate system, generally with latitude and longitude axes.
        /// </summary>
        public static readonly Axis Height = new Axis("Height", AxisOrientation.Up);

        /// <summary>
        /// X axis. Used for 3D cartesian system, generally with Y and Z axes.
        /// </summary>
        public static readonly Axis X = new Axis("X", AxisOrientation.Other);

        /// <summary>
        /// Y axis. Used for 3D cartesian system, generally with X and Z axes.
        /// </summary>
        public static readonly Axis Y = new Axis("Y", AxisOrientation.Other);

        /// <summary>
        /// Z axis. Used for 3D cartesian system, generally with X and Y axes.
        /// </summary>
        public static readonly Axis Z = new Axis("Z", AxisOrientation.North);

        /// <summary>
        /// JYD 1968.0
        /// </summary>
        public static readonly Axis JYD1968Z = new Axis("JYD 1968.0", AxisOrientation.North);

        /// <summary>
        /// CIO BIH 1903.0
        /// </summary>
        public static readonly Axis CIO1903Z = new Axis("CIO BIH 1903.0", AxisOrientation.North);

        /// <summary>
        /// CIO BIH 1968.0
        /// </summary>
        public static readonly Axis CIO1968Z = new Axis("CIO BIH 1968.0", AxisOrientation.North);

        /// <summary>
        /// CIO BIH 1979.0
        /// </summary>
        public static readonly Axis CIO1979Z = new Axis("CIO BIH 1979.0", AxisOrientation.North);

        /// <summary>
        /// CIO BIH 1984.0
        /// </summary>
        public static readonly Axis CIO1984Z = new Axis("CIO BIH 1984.0", AxisOrientation.North);

        /// <summary>
        /// X direction of BIH 1968.0
        /// </summary>
        public static readonly Axis BIH1968X = new Axis("BIH 1968.0", AxisOrientation.Other);

        /// <summary>
        /// X direction of BIH 1984.0 
        /// </summary>
        public static readonly Axis BIH1984X = new Axis("BIH 1984.0", AxisOrientation.Other);
    }
}
