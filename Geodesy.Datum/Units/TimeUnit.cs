using System;
using Newtonsoft.Json;

namespace Geodesy.Datum.Units
{
    /// <summary>
    /// Definition of time units.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class TimeUnit : Unit, IEquatable<TimeUnit>
    {
        /// <summary>
        /// Creates an instance of a time unit
        /// </summary>
        /// <param name="name"></param>
        /// <param name="factor"></param>
        /// <param name="abbr"></param>
        public TimeUnit(string name, double factor, string abbr = "")
            : base(Quantity.Time, name, factor, abbr)
        {
            Identifier = new Identifier(typeof(TimeUnit));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TimeUnit)) return false;
            return Equals((TimeUnit)obj);
        }

        public bool Equals(TimeUnit unit)
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
        public static readonly TimeUnit Second = new TimeUnit("Second", 1, "s");

        /// <summary>
        /// 
        /// </summary>
        public static readonly TimeUnit Minute = new TimeUnit("Minute", 60, "m");

        /// <summary>
        /// 
        /// </summary>
        public static readonly TimeUnit Hour = new TimeUnit("Hour", 1, "h");

        /// <summary>
        /// 
        /// </summary>
        public static readonly TimeUnit Week = new TimeUnit("Week", 7 * 24 * 3600);
    }
}
