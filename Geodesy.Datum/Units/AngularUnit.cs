using System;
using Newtonsoft.Json;

namespace Geodesy.Datum.Units
{
    /// <summary>
    /// Definition of angular units.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class AngularUnit : Unit, IEquatable<AngularUnit>
    {
        /// <summary>
        /// Initializes a new instance of a angular unit.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="factor"></param>
        /// <param name="abbr"></param>
        public AngularUnit(string name, double factor, string abbr = "")
            : base(Quantity.Angle, name, factor, abbr)
        {
            Identifier = new Identifier(typeof(AngularUnit));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is AngularUnit)) return false;
            return Equals((AngularUnit)obj);
        }

        public bool Equals(AngularUnit unit)
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
        public static readonly AngularUnit Radian = new AngularUnit("Radian", 1.0, "rad")
        {
            Identifier = new Identifier("EPSG", "9101", "radian", "rad"),
        };

        /// <summary>
        /// 
        /// </summary>
        public static readonly AngularUnit Degree = new AngularUnit("Degree", Math.PI / 180, "\u00B0")
        {
            Identifier = new Identifier("EPSG", "9102", "degree", "\u00B0"),
        };

        /// <summary>
        /// 
        /// </summary>
        public static readonly AngularUnit Minute = new AngularUnit("Minute", Math.PI / 10800, "'")
        {
            Identifier = new Identifier("EPSG", "9103", "minute", "'"),
        };

        /// <summary>
        /// 
        /// </summary>
        public static readonly AngularUnit Second = new AngularUnit("Second", Math.PI / 648000, "\"")
        {
            Identifier = new Identifier("EPSG", "9104", "second", "\""),
        };

        /// <summary>
        /// 
        /// </summary>
        public static readonly AngularUnit Grad = new AngularUnit("Grad", Math.PI / 200, "gr")
        {
            Identifier = new Identifier("EPSG", "9105", "grad", "gr"),
        };

        /// <summary>
        /// 
        /// </summary>
        public static readonly AngularUnit Gon = new AngularUnit("Gon", Math.PI / 200, "g")
        {
            Identifier = new Identifier("EPSG", "9106", "gon", "g"),
        };
    }
}
