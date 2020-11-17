using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Geodesy.Datum.Units
{
    /// <summary>
    /// Unit is a standardised quantity of a physical property, used as a factor to express occurring 
    /// quantities of that property.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public abstract class Unit : IEquatable<Unit>
    {
        /// <summary>
        /// base units dictionary
        /// </summary>
        private static Dictionary<Quantity, Unit> _baseUnits = new Dictionary<Quantity, Unit>
        {
            { Quantity.Length, Settings.BaseLinearUnit },
            { Quantity.Angle, Settings.BaseAngularUnit },
            { Quantity.Time, Settings.BaseTimeUnit },
            { Quantity.Mass, Settings.BaseMassUnit },
            { Quantity.Velocity, Settings.BaseVelocityUnit },
        };

        /// <summary>
        /// Set the base unit for this quantity.
        /// </summary>
        /// <param name="unit">base Unit</param>
        public static void SetBaseUnit(Unit unit)
        {
            if (unit == null) return;

            if (_baseUnits.ContainsKey(unit.Quantity))
            {
                _baseUnits[unit.Quantity] = unit;
            }
            else
            {
                _baseUnits.Add(unit.Quantity, unit);
            }
        }

        /// <summary>
        /// Get the base unit for this quantity.
        /// </summary>
        /// <param name="quantity">the quantity of the Unit to be found</param>
        /// <returns>the base unit</returns>
        public static Unit GetBaseUnit(Quantity quantity)
        {
            if (quantity == null) return null;

            if (_baseUnits.ContainsKey(quantity))
            {
                return _baseUnits[quantity];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a new Unit for the Quantity.
        /// </summary>
        /// <param name="quantity">the quantity measured by this unit</param>
        /// <param name="name">the name of this unit</param>
        /// <param name="factor">the scale factor of this unit compared to the base unit</param>
        /// <param name="abbr">abbreviated name</param>
        public Unit(Quantity quantity, string name, double factor, string abbr = "")
        {
            Quantity = quantity;
            Name = name;
            Factor = factor;
            Abbreviation = abbr;
        }

        /// <summary>
        /// The quantity measured by this unit
        /// </summary>
        [JsonConverter(typeof(QuantityJsonConvertor))]
        public Quantity Quantity { get; set; }

        /// <summary>
        /// The name of this unit
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The scale factor of this unit compared to the base unit
        /// </summary>
        public double Factor { get; set; }

        /// <summary>
        /// Abbreviated name of this unit
        /// </summary>
        public string Abbreviation { get; set; }

        /// <summary>
        /// The quantity measured by this unit
        /// </summary>
        [JsonIgnore]
        public Identifier Identifier { get; set; } = new Identifier(typeof(Unit));

        /// <summary>
        /// Convert the value to a new unit
        /// </summary>
        /// <param name="unit">new unit</param>
        /// <param name="value">the value with current unit</param>
        /// <returns>the value with new unit</returns>
        public double Convert(Unit unit, double value)
        {
            return value * Factor / unit.Factor;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Unit)) return false;
            return Equals((Unit)obj);
        }

        public bool Equals(Unit unit)
        {
            return (Quantity == unit.Quantity) && (Math.Abs(Factor - unit.Factor) < double.Epsilon);
        }

        public override int GetHashCode()
        {
            return Quantity.Name.GetHashCode() + Factor.GetHashCode();
        }
    }
}