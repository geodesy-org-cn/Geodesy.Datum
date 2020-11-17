using System;
using Newtonsoft.Json;
using Geodesy.Datum.Units;

namespace Geodesy.Datum.Coordinate
{
    /// <summary>
    /// Longitude data type. The value is from -180 to +180
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class Longitude : Angle
    {
        /// <summary>
        /// Create a longitude object
        /// </summary>
        public Longitude()
        { }

        /// <param name="ang">angle</param>
        public Longitude(Angle ang)
        {
            _degree = ang.Degrees;
        }

        /// <summary>
        /// Create a longitude object with value
        /// </summary>
        /// <param name="value">the longitude value in degree</param>
        public Longitude(double value)
        {
            _degree = value;
        }

        /// <summary>
        /// Create a longitude object with value
        /// </summary>
        /// <param name="value">longitude value</param>
        /// <param name="style">data style</param>
        public Longitude(double value, DataStyle style)
        {
            SetValue(value, style);
        }

        /// <summary>
        /// Create a latitude object with value
        /// </summary>
        /// <param name="vaue">angle vaue</param>
        /// <param name="unit">angular unit</param>
        public Longitude(double vaue, AngularUnit unit)
        {
            _degree = unit.Convert(AngularUnit.Degree, vaue);
        }

        /// <summary>
        /// Create a longitude object with value
        /// </summary>
        /// <param name="deg">degrees</param>
        /// <param name="min">minutes</param>
        /// <param name="sec">seconds</param>
        public Longitude(int deg, int min, double sec)
            : base(deg, min, sec)
        { }

        /// <summary>
        /// Create a longitude object with value
        /// </summary>
        /// <param name="deg">degrees</param>
        /// <param name="min">minutes</param>
        /// <param name="sec">seconds</param>
        /// <param name="flag">'E' or 'W'</param>
        public Longitude(int deg, int min, double sec, char flag)
        {
            if (deg < 0)
            {
                throw new GeodeticException("Degree value is overflow.");
            }

            if (min < 0 || min >= 60)
            {
                throw new GeodeticException("Minute value is overflow.");
            }

            if (sec < 0 || sec >= 60)
            {
                throw new GeodeticException("Second value is overflow.");
            }

            _degree = deg + min / 60.0 + sec / 3600.0;

            if (flag.ToString().ToUpper() == "W")
            {
                _degree *= -1;
            }
            else if (flag.ToString().ToUpper() != "E")
            {
                throw new GeodeticException("Flag of latitude is error.");
            }
        }

        #region static methods
        /// <summary>
        /// Create a longitude by radians value
        /// </summary>
        /// <param name="radians">radians</param>
        /// <returns>a new longitude</returns>
        public new static Longitude FromRadians(double radians)
        {
            return new Longitude(radians * 180 / Math.PI);
        }

        /// <summary>
        /// Create a longitude by degree value
        /// </summary>
        /// <param name="degrees">degree</param>
        /// <returns>a new longitude</returns>
        public new static Longitude FromDegrees(double degrees)
        {
            return new Longitude(degrees);
        }

        /// <summary>
        /// Create a longitude by minutes value
        /// </summary>
        /// <param name="minutes">minutes</param>
        /// <returns>a new longitude</returns>
        public new static Longitude FromMinutes(double minutes)
        {
            return new Longitude(minutes / 60);
        }

        /// <summary>
        /// Create a longitude by seconds value
        /// </summary>
        /// <param name="seconds">seconds</param>
        /// <returns>a new longitude</returns>
        public new static Longitude FromSeconds(double seconds)
        {
            return new Longitude(seconds / 3600);
        }

        /// <summary>
        /// Create a longitude by data value and it's style
        /// </summary>
        /// <param name="value">data value</param>
        /// <param name="style">data style</param>
        /// <returns>a new longitude</returns>
        public new static Longitude FromValue(double value, DataStyle style)
        {
            Angle ang = new Angle();
            ang.SetValue(value, style);
            return new Longitude(ang.Degrees);
        }

        public static readonly new Longitude Zero = new Longitude(0);
        public static readonly new Longitude NaN = new Longitude(double.NaN);
        public static readonly new Longitude MinValue = new Longitude(-180.0);
        public static readonly new Longitude MaxValue = new Longitude(180.0);
        #endregion

        #region override operators
        public static Longitude operator +(Longitude a, Angle b)
        {
            double res = a.Degrees + b.Degrees;
            return new Longitude(res);
        }

        public static Longitude operator -(Longitude a)
        {
            return new Longitude(-a.Degrees);
        }

        public static Longitude operator -(Longitude a, Angle b)
        {
            double res = a.Degrees - b.Degrees;
            return new Longitude(res);
        }

        public static Angle operator -(Longitude a, Longitude b)
        {
            double res = a.Degrees - b.Degrees;

            // if a is during (-π, 0), change it to (π, 2π)
            if (a.Radians < 0)
            {
                res += 360;
            }

            return new Angle(res);
        }
        #endregion

        /// <summary>
        /// the indicator of West ('W') or East ('E')
        /// </summary>
        public char Indicator => _degree < 0 ? 'W' : 'E';

        /// <summary>
        /// The value in degree unit
        /// </summary>
        /// <remarks>
        /// This override is to rename the Json property name only
        /// </remarks>
        [JsonProperty(PropertyName = "Longitude")]
        public override double Degrees
        {
            get => _degree;
            set => _degree = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Normalize()
        {
            base.Normalize();

            // from (0, 2π) to (-π, π)
            if (_degree > 180)
            {
                _degree -= 360;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (double.IsNaN(_degree)) return string.Empty;

            double degree = Math.Abs(_degree);

            double d = Math.Floor(degree);
            double m = Math.Floor((degree - d) * 60);
            double s = (degree - d - m / 60) * 3600;

            string str = d.ToString() + "\u00B0" + m.ToString("00") + "'" + s.ToString("00.#####") + "\"";
            str += (_degree < 0) ? "W" : "E";
            return str;
        }
    }
}
