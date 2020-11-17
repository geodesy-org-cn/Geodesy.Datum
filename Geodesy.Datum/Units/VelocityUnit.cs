using System;
using Newtonsoft.Json;

namespace Geodesy.Datum.Units
{
    /// <summary>
    /// Definition of velocity units.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class VelocityUnit : Unit, IEquatable<VelocityUnit>
    {
        /// <summary>
        /// Initializes a new instance of a velocity unit.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="factor"></param>
        /// <param name="abbr"></param>
        public VelocityUnit(string name, double factor, string abbr = "")
            : base(Quantity.Velocity, name, factor, abbr)
        {
            Identifier = new Identifier(typeof(VelocityUnit));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is VelocityUnit)) return false;

            return Equals((VelocityUnit)obj);
        }

        public bool Equals(VelocityUnit unit)
        {
            return Math.Abs(Factor - unit.Factor) < double.Epsilon;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly VelocityUnit MeterPerSecond = new VelocityUnit("Meter per second", 1.0, "m/s");

        /// <summary>
        /// 
        /// </summary>
        public static readonly VelocityUnit KilometerPerSecond = new VelocityUnit("Kilometer per second", 1.0E3, "km/s");

        /// <summary>
        /// 
        /// </summary>
        public static readonly VelocityUnit KilometerPerHour = new VelocityUnit("Kilometer per hour", 1 / 3.6, "km/h");

        /// <summary>
        /// 
        /// </summary>
        //public static readonly VelocityUnit RadianPerSecond = new VelocityUnit("Radian per year", 1.0, "rad/s");

        /// <summary>
        /// 1/1000/365/24/3600 = 3.170979198376459E-11
        /// </summary>
        public static readonly VelocityUnit MillimeterPerYear = new VelocityUnit("Millimeter per year", 3.170979E-11, "mm/a");
    }
}
