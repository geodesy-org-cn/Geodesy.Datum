using System;
using Newtonsoft.Json;

namespace Geodesy.Datum.Units
{
    /// <summary>
    /// Definition of linear units.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class LinearUnit : Unit, IEquatable<LinearUnit>
    {
        /// <summary>
        /// Creates an instance of a linear unit
        /// </summary>
        /// <param name="name"></param>
        /// <param name="factor"></param>
        /// <param name="abbr"></param>
        public LinearUnit(string name, double factor, string abbr = "")
            : base(Quantity.Length, name, factor, abbr)
        {
            Identifier = new Identifier(typeof(LinearUnit));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is LinearUnit)) return false;
            return Equals((LinearUnit)obj);
        }

        public bool Equals(LinearUnit unit)
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
        public static readonly LinearUnit Meter = new LinearUnit("Meter", 1.0, "m")
        {
            Identifier = new Identifier("EPSG", "9001", "meter", "m"),
        };

        /// <summary>
        /// 
        /// </summary>
        public static readonly LinearUnit Foot = new LinearUnit("International Foot", 0.3048, "ft")
        {
            Identifier = new Identifier("EPSG", "9002", "foot", "ft"),
        };

        /// <summary>
        /// 
        /// </summary>
        public static readonly LinearUnit Yard = new LinearUnit("International Yard", 0.9144, "yd")
        {
            Identifier = new Identifier("EPSG", "9096", "yard", "yd"),
        };

        /// <summary>
        /// 
        /// </summary>
        public static readonly LinearUnit UsFoot = new LinearUnit("U.S. Surveyor’s Foot", 0.3048006096, "us-ft")
        {
            Identifier = new Identifier("EPSG", "9003", "foot_us", "us-ft"),
        };

        /// <summary>
        /// 
        /// </summary>
        public static readonly LinearUnit UsInch = new LinearUnit("U.S. Surveyor’s Inch", 0.0254000508, "us-in");

        /// <summary>
        /// 
        /// </summary>
        public static readonly LinearUnit UsRod = new LinearUnit("U.S. Surveyor’s Rod", 5.0292100584, "us-rd");

        /// <summary>
        /// 
        /// </summary>
        public static readonly LinearUnit UsChain = new LinearUnit("U.S. Surveyor’s Chain", 20.116840234, "us-ch");

        /// <summary>
        /// 
        /// </summary>
        public static readonly LinearUnit UsMile = new LinearUnit("U.S. Surveyor’s Statute Mile", 1609.3472187, "us-mi");

        /// <summary>
        /// 
        /// </summary>
        public static readonly LinearUnit Decimeter = new LinearUnit("Decimeter", 1E-1, "dm");

        /// <summary>
        /// 
        /// </summary>
        public static readonly LinearUnit Centimeter = new LinearUnit("Centimeter", 1E-2, "cm");

        /// <summary>
        /// 
        /// </summary>
        public static readonly LinearUnit Millimeter = new LinearUnit("Millimeter", 1E-3, "mm");

        /// <summary>
        /// 
        /// </summary>
        public static readonly LinearUnit Micrometer = new LinearUnit("Centimeter", 1E-6, "μm");

        /// <summary>
        /// 
        /// </summary>
        public static readonly LinearUnit Nanometer = new LinearUnit("Nanometer", 1E-9, "nm");

        /// <summary>
        /// 
        /// </summary>
        public static readonly LinearUnit Picometer = new LinearUnit("Picometer", 1E-12, "pm");

        /// <summary>
        /// 
        /// </summary>
        public static readonly LinearUnit Decameter = new LinearUnit("Decameter", 1E1, "dam");

        /// <summary>
        /// 
        /// </summary>
        public static readonly LinearUnit Hectometer = new LinearUnit("Hectometer", 1E2, "hm");

        /// <summary>
        /// 
        /// </summary>
        public static readonly LinearUnit Kilometer = new LinearUnit("Kilometer", 1E3, "km");

        /// <summary>
        /// 
        /// </summary>
        public static readonly LinearUnit Megameter = new LinearUnit("Megameter", 1E6, "Mm");

        /// <summary>
        /// 
        /// </summary>
        public static readonly LinearUnit LightYear = new LinearUnit("LightYear", 9.4607304725808E15, "ly");

        /// <summary>
        /// 
        /// </summary>
        public static readonly LinearUnit Mile = new LinearUnit("International Statute Mile", 1609.344, "mi");

        /// <summary>
        /// 
        /// </summary>
        public static readonly LinearUnit NauticalMile = new LinearUnit("International Nautical Mile", 1852, "kmi");

        /// <summary>
        /// 
        /// </summary>
        public static readonly LinearUnit Inch = new LinearUnit("International Inch", 0.0254, "in");

        /// <summary>
        /// 
        /// </summary>
        public static readonly LinearUnit Fathom = new LinearUnit("International Fathom", 1.8288, "fath");

        /// <summary>
        /// 
        /// </summary>
        public static readonly LinearUnit Chain = new LinearUnit("International Chain", 20.1168, "ch");

        /// <summary>
        /// 
        /// </summary>
        public static readonly LinearUnit Link = new LinearUnit("International Link", 0.2011684023368, "li");

        /// <summary>
        /// 
        /// </summary>
        public static readonly LinearUnit Rod = new LinearUnit("Rod", 5.0292, "rd");

        /// <summary>
        /// 
        /// </summary>
        public static readonly LinearUnit Pole = new LinearUnit("Pole ", 5.0292);

        /// <summary>
        /// 
        /// </summary>
        public static readonly LinearUnit Perch = new LinearUnit("Perch", 5.0292);

        /// <summary>
        /// 
        /// </summary>
        public static readonly LinearUnit League = new LinearUnit("League", 4800, " league");

        /// <summary>
        /// 
        /// </summary>
        public static readonly LinearUnit Furlong = new LinearUnit("Furlong", 201.17, "fur");
    }
}
