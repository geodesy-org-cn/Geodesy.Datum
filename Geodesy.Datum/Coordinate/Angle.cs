using System;
using Newtonsoft.Json;
using Geodesy.Datum.Units;

namespace Geodesy.Datum.Coordinate
{
    /// <summary>
    /// Angle data type
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class Angle : IComparable<Angle>, IEquatable<Angle>
    {
        /// <summary>
        /// degree value of the angle
        /// </summary>
        protected double _degree = double.NaN;

        /// <summary>
        /// Create an angle object
        /// </summary>
        public Angle()
        { }

        /// <summary>
        /// Create an angle object with value
        /// </summary>
        /// <param name="value">the angle value in degree</param>
        public Angle(double value)
        {
            _degree = value;
        }

        /// <summary>
        /// Create an angle object with value. To negative angle, (-1, 2, 3.0), (0, -1, 2.0) or (0, 0, -1.0) is correct.
        /// </summary>
        /// <param name="deg">degrees</param>
        /// <param name="min">minutes</param>
        /// <param name="sec">seconds</param>
        public Angle(int deg, int min, double sec)
        {
            if (deg == 0)
            {
                if (min == 0)
                {
                    if (Math.Abs(sec)>=60)
                    {
                        throw new GeodeticException("Minute value is overflow.");
                    }

                    _degree= sec / 3600;
                }
                else
                {
                    if (Math.Abs(min)>60)
                    {
                        throw new GeodeticException("Minute value is overflow.");
                    }

                    if (sec < 0 || sec >= 60)
                    {
                        throw new GeodeticException("Second value is overflow.");
                    }

                    _degree = Math.Sign(min) * (Math.Abs(min) / 60 + sec / 3600);
                }
            }
            else
            {
                if (min < 0 || min >= 60)
                {
                    throw new GeodeticException("Minute value is overflow.");
                }

                if (sec < 0 || sec >= 60)
                {
                    throw new GeodeticException("Second value is overflow.");
                }

                _degree = Math.Sign(deg) * (Math.Abs(deg) + min / 60.0 + sec / 3600);
            }
        }

        /// <summary>
        /// Create an angle object with value
        /// </summary>
        /// <param name="value">angle value</param>
        /// <param name="style">data style</param>
        public Angle(double value, DataStyle style)
        {
            SetValue(value, style);
        }

        /// <summary>
        /// Create an angle object with value
        /// </summary>
        /// <param name="vaue">angle vaue</param>
        /// <param name="unit">angular unit</param>
        public Angle(double vaue, AngularUnit unit)
        {
            _degree = unit.Convert(AngularUnit.Degree, vaue);
        }

        /// <summary>
        /// Degree value of the angle
        /// </summary>
        public int Degree => (int)Math.Floor(Math.Abs(_degree)) * Math.Sign(_degree);        

        /// <summary>
        /// Minute of an angle
        /// </summary>
        public int Minute
        {
            get
            {
                double ds = Math.Abs(_degree);
                double d = Math.Floor(ds);
                return (int)Math.Floor((ds - d) * 60);
            }
        }

        /// <summary>
        /// Second of an angle
        /// </summary>
        public double Second
        {
            get
            {
                double ds = Math.Abs(_degree);
                double d = Math.Floor(ds);
                double m = Math.Floor((ds - d) * 60);
                return (ds - d - m / 60) * 3600;
            }
        }

        /// <summary>
        /// The value with degree unit of an angle
        /// </summary>
        /// <remarks>
        /// Set the json property name with "angle"
        /// </remarks>
        [JsonProperty(PropertyName = "Angle")]
        public virtual double Degrees
        {
            get => _degree;
            set => _degree = value;
        }

        /// <summary>
        /// The value with minute unit of an angle
        /// </summary>
        public double Minutes
        {
            get => _degree * 60;
            set => _degree = value / 60;
        }

        /// <summary>
        /// The value with second unit of an angle
        /// </summary>
        public double Seconds
        {
            get => _degree * 3600;
            set => _degree = value / 3600;
        }

        /// <summary>
        /// The value with radian unit of an angle
        /// </summary>
        public double Radians
        {
            get => _degree * Math.PI / 180;
            set => _degree = value * 180 / Math.PI;
        }

        /// <summary>
        /// Get the angle value in specified unit
        /// </summary>
        /// <param name="unit">angular unit</param>
        /// <returns>angle value</returns>
        public double GetValue(AngularUnit unit)
        {
            return _degree * AngularUnit.Degree.Factor / unit.Factor;
        }

        /// <summary>
        /// Normalize the angle value to [0, 2π)
        /// </summary>
        public virtual void Normalize()
        {
            if (double.IsNaN(_degree)) return;

            while (_degree > 360)
            {
                _degree -= 360;
            }

            while (_degree < 0)
            {
                _degree += 360;
            }
        }

        /// <summary>
        /// Set the angle value
        /// </summary>
        /// <param name="value">angle value</param>
        /// <param name="style">angle data style</param>
        public void SetValue(double value, DataStyle style)
        {
            double d, m, s;

            // Get the sign
            int sign = Math.Sign(value);
            value = Math.Abs(value);

            switch (style)
            {
                case DataStyle.Radians:
                    _degree = value * 180 / Math.PI;
                    break;

                case DataStyle.Degrees:
                    _degree = value;
                    break;

                case DataStyle.Minutes:
                    _degree = value / 60;
                    break;

                case DataStyle.Seconds:
                    _degree = value / 3600;
                    break;

                case DataStyle.DMMSSss:
                    d = Math.Floor(value / 10000);
                    m = Math.Floor((value - d * 10000) / 100);
                    if (m >= 60) throw new GeodeticException("Minute value is overflow.");
                    s = value - d * 10000 - m * 100;
                    if (s >= 60) throw new GeodeticException("Second value is overflow.");
                    _degree = d + m / 60 + s / 3600;
                    break;

                case DataStyle.Dmmss:
                    d = Math.Floor(value);
                    m = Math.Floor((value - d) * 100);
                    if (m >= 60) throw new GeodeticException("Minute value is overflow.");
                    s = (value - d - m / 100) * 10000;
                    if (s >= 60) throw new GeodeticException("Second value is overflow.");
                    _degree = d + m / 60 + s / 3600;
                    break;

                case DataStyle.DMMmm:
                    d = Math.Floor(value / 100);
                    m = value - d * 100;
                    if (m >= 60) throw new GeodeticException("Minute value is overflow.");
                    _degree = d + m / 60;
                    break;

                case DataStyle.Dmm:
                    d = Math.Floor(value);
                    m = (value - d) * 100;
                    if (m >= 60) throw new GeodeticException("Minute value is overflow.");
                    _degree = d + m / 60;
                    break;
            }

            // add the sign
            _degree *= sign;
        }

        #region override ToString()
        /// <summary>
        /// Convert the angle to string
        /// </summary>
        /// <returns>A string with degree, minute and second</returns>
        public override string ToString()
        {
            if (double.IsNaN(_degree)) return string.Empty;

            double degree = Math.Abs(_degree);

            double d = Math.Floor(degree);
            double m = Math.Floor((degree - d) * 60);
            double s = (degree - d - m / 60) * 3600;

            string str = _degree < 0 ? "-" : "";
            str += d.ToString() + "\u00B0" + m.ToString("00") + "'" + s.ToString("00.#####") + "\"";
            return str;
        }

        /// <summary>
        /// Convert the angle to string
        /// </summary>
        /// <param name="style">the angle string format</param>
        /// <returns>a string</returns>
        public string ToString(DataStyle style)
        {
            double temp, degree;
            string str;

            switch (style)
            {
                case DataStyle.Radians:
                    str = Radians.ToString();
                    break;

                case DataStyle.Degrees:
                    str = _degree.ToString();
                    break;

                case DataStyle.Minutes:
                    str = Minutes.ToString();
                    break;

                case DataStyle.Seconds:
                    str = Seconds.ToString();
                    break;

                case DataStyle.DMMSSss:
                    str = ToDMS().ToString();
                    break;

                case DataStyle.Dmmss:
                    temp = ToDMS() / 10000;
                    str = temp.ToString();
                    break;

                case DataStyle.DMMmm:
                    degree = Math.Abs(_degree);
                    temp = Math.Floor(degree);
                    temp = temp * 100 + (degree - temp) * 60;
                    str = (temp * Math.Sign(_degree)).ToString();
                    break;

                case DataStyle.Dmm:
                    degree = Math.Abs(_degree);
                    temp = Math.Floor(degree);
                    temp = temp * 100 + (degree - temp) * 60 / 100.0;
                    str = (temp * Math.Sign(_degree)).ToString();
                    break;

                case DataStyle.DD_MM_SSssss:
                default:
                    return ToString();
            }

            return str;
        }

        /// <summary>
        /// Convert the angle to double with DDMMSS.ssss format
        /// </summary>
        /// <returns>the angle value</returns>
        public double ToDMS()
        {
            double degree = Math.Abs(_degree);

            double d = Math.Floor(degree);
            double m = Math.Floor((degree - d) * 60);
            double s = (degree - d - m / 60) * 3600;

            //处理舍入误差，10-8
            if (Math.Abs(s - 60) < Single.Epsilon)
            {
                m += 1;
                s -= 60;
            }

            //处理舍入误差，10-8
            if (Math.Abs(m - 60) < Single.Epsilon)
            {
                d += 1;
                m -= 60;
            }

            return (d * 10000 + m * 100 + s) * Math.Sign(_degree);
        }
        #endregion

        #region static methods
        /// <summary>
        /// Create an angle by radians value
        /// </summary>
        /// <param name="radians">radians</param>
        /// <returns>a new angle</returns>
        public static Angle FromRadians(double radians)
        {
            return new Angle(radians * 180 / Math.PI);
        }

        /// <summary>
        /// Create an angle by degree value
        /// </summary>
        /// <param name="degrees">degrees</param>
        /// <returns>a new angle</returns>
        public static Angle FromDegrees(double degrees)
        {
            return new Angle(degrees);
        }

        /// <summary>
        /// Create an angle by minutes value
        /// </summary>
        /// <param name="minutes">minutes</param>
        /// <returns>a new angle</returns>
        public static Angle FromMinutes(double minutes)
        {
            return new Angle(minutes / 60);
        }

        /// <summary>
        /// Create an angle by seconds value
        /// </summary>
        /// <param name="seconds">seconds</param>
        /// <returns>a new angle</returns>
        public static Angle FromSeconds(double seconds)
        {
            return new Angle(seconds / 3600);
        }

        /// <summary>
        /// Create an angle by data value and it's style
        /// </summary>
        /// <param name="value">data value</param>
        /// <param name="style">data style</param>
        /// <returns>a new angle</returns>
        public static Angle FromValue(double value, DataStyle style)
        {
            Angle ang = new Angle();
            ang.SetValue(value, style);
            return ang;
        }

        /// <summary>
        /// Get the absolute value angle
        /// </summary>
        public static Angle Abs(Angle ang)
        {
            return new Angle(Math.Abs(ang._degree));
        }

        /// <summary>
        /// Check the angle is NaN value
        /// </summary>
        public static bool IsNaN(Angle ang)
        {
            return double.IsNaN(ang._degree);
        }
        #endregion

        #region constant
        /// <summary>
        /// 
        /// </summary>
        public static readonly Angle Zero = new Angle(0);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Angle MinValue = new Angle(double.MinValue);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Angle MaxValue = new Angle(double.MaxValue);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Angle NaN = new Angle(double.NaN);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Angle Pi = new Angle(180.0);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Angle HalfPi = new Angle(90.0);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Angle TwoPi = new Angle(360.0);

        /// <summary>
        /// 
        /// </summary>
        public static readonly double SecondToRadian = Math.PI / 3600.0 / 180;

        /// <summary>
        /// 
        /// </summary>
        public static readonly double DegreeToRadian = Math.PI / 180.0;

        /// <summary>
        /// const of radian to second
        /// </summary>
        public static readonly double RadianToSecond = 180.0 * 3600 / Math.PI;

        /// <summary>
        /// const of radian to degree
        /// </summary>
        public static readonly double RadianToDegree = 180.0 / Math.PI;
        #endregion

        #region override operators
        public int CompareTo(Angle that)
        {
            return _degree < that._degree ? -1 : _degree > that._degree ? 1 : 0;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Angle ang = (Angle)obj;
            return Math.Abs(_degree - ang._degree) < double.Epsilon;
        }

        public bool Equals(Angle ang)
        {
            return Math.Abs(_degree - ang._degree) < double.Epsilon;
        }

        public static bool operator ==(Angle a, Angle b)
        {
            return Math.Abs(a._degree - b._degree) < Single.Epsilon;
        }

        public static bool operator !=(Angle a, Angle b)
        {
            return Math.Abs(a._degree - b._degree) > Single.Epsilon;
        }

        public static bool operator <(Angle a, Angle b)
        {
            return a._degree < b._degree;
        }

        public static bool operator <=(Angle a, Angle b)
        {
            return a._degree <= b._degree;
        }

        public static bool operator >(Angle a, Angle b)
        {
            return a._degree > b._degree;
        }

        public static bool operator >=(Angle a, Angle b)
        {
            return a._degree >= b._degree;
        }

        public static Angle operator +(Angle a, Angle b)
        {
            return new Angle(a._degree + b._degree);
        }

        public static Angle operator -(Angle a)
        {
            return new Angle(-a._degree);
        }

        public static Angle operator -(Angle a, Angle b)
        {
            return new Angle(a._degree - b._degree);
        }

        public static Angle operator *(Angle a, double times)
        {
            return new Angle(a._degree * times);
        }

        public static Angle operator *(double times, Angle a)
        {
            return new Angle(a._degree * times);
        }

        public static Angle operator /(double divisor, Angle a)
        {
            return new Angle(a._degree / divisor);
        }

        public static Angle operator /(Angle a, double divisor)
        {
            return new Angle(a._degree / divisor);
        }

        public override int GetHashCode()
        {
            return _degree.GetHashCode();
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public enum DataStyle
        {
            /// <summary>
            /// the value is in degree unit
            /// </summary>
            Degrees,

            /// <summary>
            /// the value is in minute unit
            /// </summary>
            Minutes,

            /// <summary>
            /// the value is in second unit
            /// </summary>
            Seconds,

            /// <summary>
            /// the value is in radian unit
            /// </summary>
            Radians,

            /// <summary>
            /// the value is composed by degree, minute and second. The decimal point locates after degree value.
            /// </summary>
            Dmmss,

            /// <summary>
            /// the value is composed by degree, minute and second. The decimal point locates at second value.
            /// </summary>
            DMMSSss,

            /// <summary>
            /// the value is composed by degree, minute. The decimal point locates after degree value.
            /// </summary>
            Dmm,

            /// <summary>
            /// the value is composed by degree, minute. The decimal point locates at minute value.
            /// </summary>
            DMMmm,

            /// <summary>
            /// the value is composed by degree, minute and second. It seperted by ° ′ ″ symbol.
            /// </summary>
            DD_MM_SSssss
        }
    }

    /// <summary>
    /// Angle type of Json converter for serialize and deserialize operation.
    /// </summary>
    /// <typeparam name="T">Type of Angle, Latitude or Longitude</typeparam>
    public class AngleJsonConverter<T> : JsonConverter
    {
        /// <summary>
        /// 
        /// </summary>
        public AngleJsonConverter()
        {
            Style = Angle.DataStyle.Degrees;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="style"></param>
        public AngleJsonConverter(Angle.DataStyle style)
        {
            Style = style;
        }

        /// <summary>
        /// 
        /// </summary>
        public Angle.DataStyle Style { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(T) == objectType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // check the property type
            if (typeof(T) != objectType) return null;

            // check the json state
            if (reader.TokenType == JsonToken.Null) return null;

            // get the property value
            object value = reader.Value;

            Angle angle = new Angle();
            if (Style == Angle.DataStyle.DD_MM_SSssss)
            {
                string[] dms = ((string)value).Split(new string[] { "\u00B0", "'", "\"" }, StringSplitOptions.RemoveEmptyEntries);
                angle = new Angle(int.Parse(dms[0]), int.Parse(dms[1]), double.Parse(dms[2]));
            }
            else
            {
                angle.SetValue((double)value, Style);
            }

            // return the value
            switch (objectType.Name)
            {
                case "Latitude":
                    return new Latitude(angle.Degrees);

                case "Longitude":
                    return new Longitude(angle.Degrees);

                case "Angle":
                default:
                    return angle;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            Angle angle = value as Angle;
            string info = angle.ToString(Style);
            if (Style == Angle.DataStyle.DD_MM_SSssss)
            {
                writer.WriteValue(info);
            }
            else
            {
                writer.WriteValue(double.Parse(info));
            }
        }
    }
}