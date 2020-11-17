using Newtonsoft.Json;
using Geodesy.Datum.Units;
using System.Collections.Generic;

namespace Geodesy.Datum.CRS
{
    /// <summary>
    /// Spherical coordinate system
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class SphericalCoordinateSystem : CoordinateSystem
    {
        /// <summary>
        /// Create a spherical coordinate system
        /// </summary>
        public SphericalCoordinateSystem()
        {
            Dimension = 3;
            Origin = new Origin(new double[3]);

            _axes = new Dictionary<string, Axis>
            {
                { "X", Axis.X },
                { "Z", Axis.Z }
            };

            _units = new Dictionary<string, Unit>
            {
                { "X", AngularUnit.Degree },
                { "Z", AngularUnit.Degree },
                { "Polar Radius", Settings.BaseLinearUnit }
            };
        }

        /// <summary>
        /// Create a spherical coordinate system
        /// </summary>
        /// <param name="origin">origin point</param>
        /// <param name="axes">axes</param>
        public SphericalCoordinateSystem(Origin origin, params Axis[] axes)
        {
            if (axes.Length != 2)
            {
                throw new GeodeticException("Count of axes is error! It must be 2.");
            }

            Dimension = 3;
            Origin = origin;

            _axes = new Dictionary<string, Axis>
            {
                { axes[0].Name, axes[0] },
                { axes[1].Name, axes[1] }
            };

            _units = new Dictionary<string, Unit>
            {
                { axes[0].Name, AngularUnit.Degree },
                { axes[1].Name, AngularUnit.Degree },
                { "Polar Radius", Settings.BaseLinearUnit }
            };
        }
    }
}
