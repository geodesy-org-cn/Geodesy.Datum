using System;
using Newtonsoft.Json;
using Geodesy.Datum.Coordinate;

namespace Geodesy.Datum
{
    /// <summary>
    /// Deviation of the vertical data type
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class VerticalDeviation
    {
        /// <summary>
        /// Create a deviation of the vertical
        /// </summary>
        /// <param name="ns">north-south component by second unit</param>
        /// <param name="ew">east-west component by second unit</param>
        public VerticalDeviation(double ns, double ew)
        {
            ξ = ns;
            η = ew;
        }

        /// <summary>
        /// Create a deviation of the vertical
        /// </summary>
        /// <param name="ns">north-south component by angle unit</param>
        /// <param name="ew">east-west component by angle unit</param>
        public VerticalDeviation(Angle ns, Angle ew)
        {
            ξ = ns.Seconds;
            η = ew.Seconds;
        }

        /// <summary>
        /// the north-south component, unit second
        /// </summary>
        [JsonProperty]
        public double ξ { get; set; }

        /// <summary>
        /// the east-west component, unit second
        /// </summary>
        [JsonProperty]
        public double η { get; set; }

        /// <summary>
        /// the north-south component
        /// </summary>
        public Angle NorthSouthValue
        {
            get => Angle.FromSeconds(ξ);
            set => ξ = value.Seconds;
        }

        /// <summary>
        /// the east-west component
        /// </summary>
        public Angle EastWestValue
        {
            get => Angle.FromSeconds(η);
            set => η = value.Seconds;
        }
    }
}
