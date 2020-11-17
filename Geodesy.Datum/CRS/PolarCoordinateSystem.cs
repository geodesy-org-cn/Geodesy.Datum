using Newtonsoft.Json;
using Geodesy.Datum.Units;
using System.Collections.Generic;

namespace Geodesy.Datum.CRS
{
    /// <summary>
    /// Polar coordinate system
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class PolarCoordinateSystem : CoordinateSystem
    {
        /// <summary>
        /// Create a polar coordinate system
        /// </summary>
        public PolarCoordinateSystem()
        {
            Dimension = 2;
            Origin = new Origin(new double[2]);

            _axes = new Dictionary<string, Axis>
            {
                { "Polar Axis", Axis.Meridian }
            };

            _units = new Dictionary<string, Unit>
            {
                { "Polar Axis", AngularUnit.Degree },
                { "Polar Radius", Settings.BaseLinearUnit }
            };
        }

        /// <summary>
        /// Create a polar coordinate system
        /// </summary>
        /// <param name="pole">polar pole</param>
        /// <param name="axis">polar axis</param>
        public PolarCoordinateSystem(Origin pole, Axis axis)
        {
            // polar point
            Origin = pole;
            Dimension = 2;

            // polar axis
            _axes = new Dictionary<string, Axis>
            {
                { axis.Name, axis }
            };

            _units = new Dictionary<string, Unit>
            {
                { axis.Name, AngularUnit.Degree },
                { "Polar Radius", Settings.BaseLinearUnit }
            };
        }
    }
}
