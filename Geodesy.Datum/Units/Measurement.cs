using System;
using Newtonsoft.Json;

namespace Geodesy.Datum.Units
{
    /// <summary>
    /// Measurement is the assignment of a number to a characteristic of an object or event, 
    /// which can be compared with other objects or events.
    /// </summary>
    [JsonObject]
    public sealed class Measurement
    {
        /// <summary>
        /// Creates a new measurement.
        /// </summary>
        /// <param name="value">value of the measurement</param>
        /// <param name="unit">unit of the measurement</param>
        /// <param name="precision">precision of the measurement</param>
        public Measurement(double value, Unit unit, double precision = double.NaN)
        {
            Value = value;
            Unit = unit;
            Precision = precision;
        }

        /// <summary>
        /// The unit of the measurement
        /// </summary>
        public Unit Unit { get; set; }

        /// <summary>
        /// The value of the measurement
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// The precision of the measurement
        /// </summary>
        public double Precision { get; set; }

        /// <summary>
        /// Convert this measurement to a new value with base unit. If the base unit is not assigned, 
        /// return null.
        /// </summary>
        /// <returns>A new value with base unit</returns>
        public Measurement ToBaseUnit()
        {
            Unit unit = Unit.GetBaseUnit(Unit.Quantity);
            if (unit == null) return null;

            double factor = unit.Factor / Unit.Factor;
            return new Measurement(Value * factor, unit, Precision * factor);
        }

        /// <summary>
        ///  Convert a mearsurement with base unit to current unit. If the base unit is not assigned, 
        ///  return false.
        /// </summary>
        /// <param name="value">a mearsurement with base unit</param>
        public bool FromBaseUnit(Measurement value)
        {
            Unit unit = Unit.GetBaseUnit(Unit.Quantity);
            if (unit == null) return false;

            double factor = value.Unit.Factor / unit.Factor;
            Unit = unit;
            Value = value.Value * factor;
            Precision = value.Precision * factor;

            return true;
        }
    }
}
