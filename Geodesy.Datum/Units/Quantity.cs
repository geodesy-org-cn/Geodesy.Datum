using System;
using Newtonsoft.Json;

namespace Geodesy.Datum.Units
{
    /// <summary>
    /// A factory to easily create Quantities from simple Strings.
    /// </summary>
    /// <remarks>
    /// Quantity is a property that can exist as a multitude or magnitude, which illustrate discontinuity and 
    /// continuity. Quantities can be compared in terms of "more", "less", or "equal", or by assigning a 
    /// numerical value in terms of a unit of measurement. Mass, time, distance, heat, and angular separation 
    /// are among the familiar examples of quantitative properties.
    /// </remarks>
    [JsonObject]
    public sealed class Quantity : IEquatable<Quantity>
    {
        /// <summary>
        /// Creates a new Quantity from a String.
        /// </summary>
        /// <param name="name">name of the Quantity</param>
        public Quantity(string name)
        {
            Name = name;
        }

        /// <summary>
        /// name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(Quantity o)
        {
            if (!(o.GetType().ToString() == "Quantity"))
            {
                return false;
            }

            return ToString().Equals(o.ToString(), StringComparison.CurrentCultureIgnoreCase);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        /// <summary>
        /// Length
        /// </summary>
        public static readonly Quantity Length = new Quantity("Length");

        /// <summary>
        /// Angle
        /// </summary>
        public static readonly Quantity Angle = new Quantity("Angle");

        /// <summary>
        /// Time
        /// </summary>
        public static readonly Quantity Time = new Quantity("Time");

        /// <summary>
        /// Mass
        /// </summary>
        public static readonly Quantity Mass = new Quantity("Mass");

        /// <summary>
        /// Mass
        /// </summary>
        public static readonly Quantity Area = new Quantity("Area");

        /// <summary>
        /// Mass
        /// </summary>
        public static readonly Quantity Velocity = new Quantity("Velocity");

        /// <summary>
        /// Dimensionless
        /// </summary>
        public static readonly Quantity Dimensionless = new Quantity("Dimensionless");
    }

    public class QuantityJsonConvertor : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // check the property type
            if (typeof(Quantity) != objectType) return null;

            // check the json state
            if (reader.TokenType == JsonToken.Null) return null;

            // get the property value
            object value = reader.Value;

            return new Quantity((string)value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteValue(((Quantity)value).Name);
        }
    }
}