using System;
using Newtonsoft.Json;

namespace Geodesy.Datum.Units
{
    /// <summary>
    /// Definition of mass units.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class MassUnit : Unit, IEquatable<MassUnit>
    {
        /// <summary>
        /// Initializes a new instance of a mass unit.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="factor"></param>
        /// <param name="abbr"></param>
        public MassUnit(string name, double factor, string abbr = "")
            : base(Quantity.Mass, name, factor, abbr)
        {
            Identifier = new Identifier(typeof(MassUnit));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MassUnit)) return false;
            return Equals((MassUnit)obj);
        }

        public bool Equals(MassUnit unit)
        {
            return Math.Abs(Factor - unit.Factor) < double.Epsilon;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// The standard International System of Units (SI) unit of mass is the kilogram.
        /// </summary>
        public static readonly MassUnit Kilogram = new MassUnit("Kilogram", 1.0, "kg");

        /// <summary>
        /// 
        /// </summary>
        public static readonly MassUnit Gram = new MassUnit("Gram", 1.0E-3, "g");

        /// <summary>
        /// the pound (lb) is a unit of both mass and force, used mainly in the United States.
        /// </summary>
        public static readonly MassUnit Pound = new MassUnit("Pound", 0.45359237, "lb");

        /// <summary>
        /// The tonne is a non-SI unit of mass equal to 1,000 kilograms.
        /// </summary>
        public static readonly MassUnit Tonne = new MassUnit("Tonne", 1.0E3, "t");

        /// <summary>
        /// The slug is a derived unit of mass in a weight-based system of measures, most notably within the 
        /// British Imperial measurement system and in the United States customary measures system.
        /// </summary>
        public static readonly MassUnit Slug = new MassUnit("Slug", 14.59390, "slug");

        /// <summary>
        /// The Planck mass is the maximum mass of point particles. It is used in particle physics.
        /// </summary>
        public static readonly MassUnit PlanckMass = new MassUnit("Planck mass", 2.18E-8, "mP");

        /// <summary>
        /// The solar mass is defined as the mass of the Sun. It is primarily used in astronomy to compare large masses such as stars or galaxies.
        /// </summary>
        public static readonly MassUnit SolarMass = new MassUnit("solar mass", 1.99E30, "M☉");

        /// <summary>
        /// The electronvolt (eV) is a unit of energy, but because of the mass–energy equivalence it 
        /// can easily be converted to a unit of mass, and is often used like one.
        /// </summary>
        public static readonly MassUnit Electronvolt = new MassUnit("Electronvolt", 1.78266192E-36, "eV/C2");

        /// <summary>
        /// The dalton or unified atomic mass unit is a unit of mass widely used in physics and chemistry.
        /// </summary>
        public static readonly MassUnit Dalton = new MassUnit("Dalton", 1.66053906660E-27, "Da");
    }
}
