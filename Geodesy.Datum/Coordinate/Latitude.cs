using System;
using Newtonsoft.Json;
using Geodesy.Datum.Units;

namespace Geodesy.Datum.Coordinate
{
    /// <summary>
    /// Latitude data type. The value is from -90 to +90
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]

    public sealed class Latitude : Angle
    {
        /// <summary>
        /// Create a latitude object
        /// </summary>
        public Latitude()
        { }

        /// <summary>
        /// Create a latitude object with an angle
        /// </summary>
        /// <param name="ang">an angle</param>
        public Latitude(Angle ang)
        {
            _degree = ang.Degrees;
        }

        /// <summary>
        /// Create a latitude object with value
        /// </summary>
        /// <param name="value">the latitude value in degree</param>
        public Latitude(double value)
        {
            _degree = value;
        }

        /// <summary>
        /// Create a latitude object with value
        /// </summary>
        /// <param name="value">latitude value</param>
        /// <param name="style">data style</param>
        public Latitude(double value, DataStyle style)
        {
            SetValue(value, style);
        }

        /// <summary>
        /// Create a latitude object with value
        /// </summary>
        /// <param name="vaue">angle vaue</param>
        /// <param name="unit">angular unit</param>
        public Latitude(double vaue, AngularUnit unit)
        {
            _degree = unit.Convert(AngularUnit.Degree, vaue);
        }

        /// <summary>
        /// Create a longitude object with value
        /// </summary>
        /// <param name="deg">degrees</param>
        /// <param name="min">minutes</param>
        /// <param name="sec">seconds</param>
        public Latitude(int deg, int min, double sec)
            : base(deg, min, sec)
        { }

        /// <summary>
        /// Create a latitude object with value
        /// </summary>
        /// <param name="deg">degrees</param>
        /// <param name="min">minutes</param>
        /// <param name="sec">seconds</param>
        /// <param name="flag">'N' or 'S'</param>
        public Latitude(int deg, int min, double sec, char flag)
        {
            if (deg<0)
            {
                throw new GeodeticException("Degree value is overflow.");
            }

            if (min<0 || min >=60)
            {
                throw new GeodeticException("Minute value is overflow.");
            }

            if (sec < 0 || sec >= 60)
            {
                throw new GeodeticException("Second value is overflow.");
            }

            _degree = deg + min / 60.0 + sec / 3600.0;

            if (flag.ToString().ToUpper() == "S")
            {
                _degree *= -1;
            }
            else if (flag.ToString().ToUpper() != "N")
            {
                throw new GeodeticException("Flag of latitude is error.");
            }
        }

        #region static methods
        /// <summary>
        /// Create an latitude by radians value
        /// </summary>
        /// <param name="radians">latitude in radian unit</param>
        /// <returns>a new latitude</returns>
        public new static Latitude FromRadians(double radians)
        {
            return new Latitude(radians * 180 / Math.PI);
        }

        /// <summary>
        /// Create an latitude by radians value
        /// </summary>
        /// <param name="degrees">latitude in degree unit</param>
        /// <returns>a new latitude</returns>
        public new static Latitude FromDegrees(double degrees)
        {
            return new Latitude(degrees);
        }

        /// <summary>
        /// Create an latitude by radians value
        /// </summary>
        /// <param name="minutes">latitude in minute unit</param>
        /// <returns>a new latitude</returns>
        public new static Latitude FromMinutes(double minutes)
        {
            return new Latitude(minutes / 60);
        }

        /// <summary>
        /// Create an latitude by radians value
        /// </summary>
        /// <param name="seconds">latitude in second unit</param>
        /// <returns>a new latitude</returns>
        public new static Latitude FromSeconds(double seconds)
        {
            return new Latitude(seconds / 3600);
        }

        /// <summary>
        /// Create an latitude by data value and it's style
        /// </summary>
        /// <param name="value">data value</param>
        /// <param name="style">data style</param>
        /// <returns>a new latitude</returns>
        public new static Latitude FromValue(double value, DataStyle style)
        {
            Angle ang = new Angle();
            ang.SetValue(value, style);
            return new Latitude(ang.Degrees);
        }
        #endregion

        #region constants
        /// <summary>
        /// equator
        /// </summary>
        public static readonly Latitude Equator = new Latitude(0);

        /// <summary>
        /// north pole
        /// </summary>
        public static readonly Latitude NorthPole = new Latitude(90.0);

        /// <summary>
        /// south pole
        /// </summary>
        public static readonly Latitude SouthPole = new Latitude(-90.0);

        /// <summary>
        /// 
        /// </summary>
        public static readonly new Latitude NaN = new Latitude(double.NaN);

        /// <summary>
        /// 
        /// </summary>
        public static readonly new Latitude MinValue = new Latitude(-90.0);

        /// <summary>
        /// 
        /// </summary>
        public static readonly new Latitude MaxValue = new Latitude(90.0);
        #endregion

        #region override operators
        public static Latitude operator +(Latitude a, Angle b)
        {
            double res = a.Degrees + b.Degrees;
            return new Latitude(res);
        }

        public static Latitude operator -(Latitude a)
        {
            return new Latitude(-a.Degrees);
        }

        public static Latitude operator -(Latitude a, Angle b)
        {
            double res = a.Degrees - b.Degrees;
            return new Latitude(res);
        }

        public static Angle operator -(Latitude a, Latitude b)
        {
            double res = a.Degrees - b.Degrees;

            // if a is during (-π, 0), change it to (π, 2π)
            if (a.Degrees < 0)
            {
                res += 360;
            }

            // this is an absolute angle
            return new Angle(res);
        }
        #endregion

        /// <summary>
        /// The indicator of North ('N') or South ('S').
        /// </summary>
        public char Indicator => _degree < 0 ? 'N' : 'S';

        /// <summary>
        /// The value in degree unit
        /// </summary>
        /// <remarks>
        /// This override is to rename the Json property name only
        /// </remarks>
        [JsonProperty(PropertyName = "Latitude")]
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

            // during (π/2, π3/2) is invalid
            if (_degree > 90 && _degree < 270)
            {
                throw new GeodeticException("Latitude value is overflow.");
            }

            // from (0, 2π) to (-π/2, π/2)
            if (_degree > 270)
            {
                _degree -= 360;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            Normalize();

            if (_degree < -90 || _degree > 90)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// \u00B0
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
            str += _degree < 0 ? "S" : "N";
            return str;
        }
    }
}
