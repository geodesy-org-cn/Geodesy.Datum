using System;
using System.Reflection;
using System.Collections.Generic;

namespace Geodesy.Datum.Time
{
    /// <summary>
    /// 时间系统
    /// </summary>
    public abstract class TimeSystem : IComparable<TimeSystem>
    {
        /// <summary>
        /// 一周所包含的秒数
        /// </summary>
        public static readonly double SecondInWeek = 3600 * 24 * 7;

        /// <summary>
        /// 一天所包含的秒数
        /// </summary>
        public static readonly double SecondInDay = 3600 * 24;

        #region 闰秒（跳秒）数据
        /// <summary>
        /// leap data struct
        /// </summary>
        private struct LeapData
        {
            /// <summary>
            /// epoch year
            /// </summary>
            public int Year;
            /// <summary>
            /// epoch month
            /// </summary>
            public int Month;
            /// <summary>
            /// delta t(sec)
            /// </summary>
            public double Delta;
            /// <summary>
            /// rate (sec/day)
            /// </summary>
            public double Rate;
            /// <summary>
            /// leaps
            /// </summary>
            public double Leaps;

            /// <summary>
            /// initalize preleap data for [1972, now)
            /// </summary>
            /// <param name="year"></param>
            /// <param name="month"></param>
            /// <param name="delta"></param>
            /// <param name="rate"></param>
            public LeapData(int year, int month, double delta, double rate)
            {
                Year = year;
                Month = month;
                Delta = delta;
                Rate = rate;
                Leaps = -1;
            }

            /// <summary>
            /// initial leap data for [1960, 1972)
            /// </summary>
            /// <param name="year"></param>
            /// <param name="month"></param>
            /// <param name="leap"></param>
            public LeapData(int year, int month, double leap)
            {
                Year = year;
                Month = month;
                Delta = 0;
                Rate = 0;
                Leaps = leap;
            }
        }

        /// <summary>
        /// epoch year, epoch month(1-12), delta t(sec), rate (sec/day) for [1960,1972).
        /// </summary>
        private static readonly List<LeapData> _preleaps = new List<LeapData>
        {
            new LeapData(1968, 2, 4.2131700, 0.0025920),
            new LeapData(1966, 1, 4.3131700, 0.0025920),
            new LeapData(1965, 9, 3.8401300, 0.0012960),
            new LeapData(1965, 7, 3.7401300, 0.0012960),
            new LeapData(1965, 3, 3.6401300, 0.0012960),
            new LeapData(1965, 1, 3.5401300, 0.0012960),
            new LeapData(1964, 9, 3.4401300, 0.0012960),
            new LeapData(1964, 4, 3.3401300, 0.0012960),
            new LeapData(1964, 1, 3.2401300, 0.0012960),
            new LeapData(1963, 11, 1.9458580, 0.0011232),
            new LeapData(1962, 1, 1.8458580, 0.0011232),
            new LeapData(1961, 8, 1.3728180, 0.0012960),
            new LeapData(1961, 1, 1.4228180, 0.0012960),
            new LeapData(1960, 1, 1.4178180, 0.0012960),
        };

        /// <summary>
        ///  epoch year, epoch month(1-12), leaps(sec) for [1972,now].
        /// </summary>
        private static readonly List<LeapData> _leaps = new List<LeapData>
        {
            // If new leap is occured, insert it at first.
            new LeapData(2017, 1, 37),
            new LeapData(2015, 7, 36),
            new LeapData(2012, 7, 35),
            new LeapData(2009, 1, 34),
            new LeapData(2006, 1, 33),
            new LeapData(1999, 1, 32),
            new LeapData(1997, 7, 31),
            new LeapData(1996, 1, 30),
            new LeapData(1994, 7, 29),
            new LeapData(1993, 7, 28),
            new LeapData(1992, 7, 27),
            new LeapData(1991, 1, 26),
            new LeapData(1990, 1, 25),
            new LeapData(1988, 1, 24),
            new LeapData(1985, 7, 23),
            new LeapData(1983, 7, 22),
            new LeapData(1982, 7, 21),
            new LeapData(1981, 7, 20),
            new LeapData(1980, 1, 19),
            new LeapData(1979, 1, 18),
            new LeapData(1978, 1, 17),
            new LeapData(1977, 1, 16),
            new LeapData(1976, 1, 15),
            new LeapData(1975, 1, 14),
            new LeapData(1974, 1, 13),
            new LeapData(1973, 1, 12),
            new LeapData(1972, 7, 11),
            // The first is 10? Because the initial difference between TAI and UTC is 10 seconds at the start of 1972.
            // from https://en.wikipedia.org/wiki/International_Atomic_Time
            new LeapData(1972, 1, 10),
        };

        /// <summary>
        /// 将一时间数据转换为不包含跳秒因素的时刻值
        /// </summary>
        /// <param name="time">UTC时间</param>
        /// <returns>跳秒值</returns>
        protected double GetLeaps(DateTime time)
        {
            if (time.Year < 1960)                     // pre-1960 no deltas
            {
                return 0;
            }
            else if (time.Year < 1972)                // [1960-1972) pre-leap
            {
                foreach (LeapData leap in _preleaps)
                {
                    if (leap.Year > time.Year || (leap.Year == time.Year && leap.Month > time.Month)) continue;

                    TimeSpan ts = time - new DateTime(leap.Year, leap.Month, 1);
                    return leap.Delta + ts.TotalDays * leap.Rate;
                }
            }
            else                                      // [1972- leap seconds
            {
                foreach (LeapData leap in _leaps)
                {
                    if (leap.Year > time.Year || (leap.Year == time.Year && leap.Month > time.Month)) continue;

                    return leap.Leaps;
                }
            }

            return 0;
        }
        #endregion

        /// <summary>
        /// 除去跳秒等因素后的时刻值，不属于任何系统，仅是一个时刻值
        /// </summary>
        protected DateTime _moment;
        /// <summary>
        /// 当前时刻值的跳秒数
        /// </summary>
        protected double _leap;

        /// <summary>
        /// 设置当前时刻，虚函数
        /// </summary>
        /// <param name="time">时刻</param>
        public abstract void SetTime(DateTime time);

        #region 属性
        /// <summary>
        /// 
        /// </summary>
        public bool LeapYear
        {
            get
            {
                return IsLeapYear(Year);
            }
        }

        /// <summary>
        /// 时刻
        /// </summary>
        public DateTime Moment
        {
            get
            {
                return _moment;
            }
        }

        /// <summary>
        /// 年
        /// </summary>
        public int Year
        {
            get
            {
                return _moment.Year;
            }
        }

        /// <summary>
        /// 月
        /// </summary>
        public int Month
        {
            get
            {
                return _moment.Month;
            }
        }

        /// <summary>
        /// 日
        /// </summary>
        public int Day
        {
            get
            {
                return _moment.Day;
            }
        }

        /// <summary>
        /// 年积日
        /// </summary>
        public int DayOfYear
        {
            get
            {
                TimeSpan span = _moment - new DateTime(_moment.Year, 1, 1);
                return (int)Math.Ceiling(span.TotalDays);
            }
        }

        /// <summary>
        /// 周几？
        /// </summary>
        public DayOfWeek WeekDay
        {
            get
            {
                return _moment.DayOfWeek;
            }
        }

        /// <summary>
        /// 小时
        /// </summary>
        public int Hour
        {
            get
            {
                return _moment.Hour;
            }
        }

        /// <summary>
        /// 分
        /// </summary>
        public int Minute
        {
            get
            {
                return _moment.Minute;
            }
        }

        /// <summary>
        /// 秒
        /// </summary>
        public double Second
        {
            get
            {
                return _moment.Second + _moment.Millisecond * 0.0001;
            }
        }

        /// <summary>
        /// 日内秒
        /// </summary>
        public double SecondOfDay
        {
            get
            {
                return _moment.Hour * 3600 + _moment.Minute * 60 + _moment.Second + _moment.Millisecond * 0.0001;
            }
        }

        /// <summary>
        /// 周内秒
        /// </summary>
        public double SecondOfWeek
        {
            get
            {
                // 周内日数
                int diw = (int)_moment.DayOfWeek;
                double seconds = (diw - 1) * SecondInDay;     
                // 日内秒
                seconds += _moment.Hour * 3600 + _moment.Minute * 60 + _moment.Second + _moment.Millisecond * 0.0001;
                // 这几日内有无跳秒？
                seconds += GetLeaps(_moment) - GetLeaps(_moment.AddDays(-diw));
                return seconds;
            }
        }
        #endregion

        #region 时间运算
        /// <summary>
        /// 在当前实例上增加指定天数
        /// </summary>
        /// <param name="days">天</param>
        /// <Returns>新的时间对象</Returns>
        public void AddDays(double days)
        {
            _moment = _moment.AddDays(days);
        }

        /// <summary>
        /// 在当前实例上增加指定月数
        /// </summary>
        /// <param name="months">月</param>
        public void AddMonths(int months)
        {
            _moment = _moment.AddMonths(months);
        }

        /// <summary>
        /// 在当前实例上增加指定年数
        /// </summary>
        /// <param name="years">年</param>
        public void AddYears(int years)
        {
            _moment = _moment.AddYears(years);
        }

        /// <summary>
        /// 在当前实例上增加指定小时数
        /// </summary>
        /// <param name="hours">小时</param>
        public void AddHours(double hours)
        {
            _moment = _moment.AddHours(hours);
        }

        /// <summary>
        /// 在当前实例上增加指定分钟数
        /// </summary>
        /// <param name="minutes">分钟</param>
        public void AddMinutes(double minutes)
        {
            _moment = _moment.AddMinutes(minutes);
        }

        /// <summary>
        /// 在当前实例上增加指定秒数
        /// </summary>
        /// <param name="seconds">秒</param>
        public void AddSeconds(double seconds)
        {
            _moment = _moment.AddSeconds(seconds);
        }

        /// <summary>
        /// 在当前实例上增加指定周数
        /// </summary>
        /// <param name="weeks">周</param>
        public void AddWeeks(double weeks)
        {
            _moment = _moment.AddDays(weeks * 7);
        }

        /// <summary>
        /// 求解当前历元与指定历元的时间差，以秒为单位
        /// </summary>
        /// <param name="time">减数，历元</param>
        /// <returns>时间差，秒</returns>
        public double Subtract(TimeSystem time)
        {
            return Subtract(this, time);
        }

        /// <summary>
        /// 求解两历元的时间差，以秒为单位
        /// </summary>
        /// <param name="t1">被减数</param>
        /// <param name="t2">减数</param>
        /// <returns>时间差，秒</returns>
        public static double Subtract(TimeSystem t1, TimeSystem t2)
        {
            if (t1.GetType() != t2.GetType()) return double.NaN;

            return (t1.Moment - t2.Moment).TotalSeconds;
        }

        /// <summary>
        /// 将UTC时间转换成历元
        /// </summary>
        /// <param name="sys">时间系统</param>
        /// <param name="week">周，输出值</param>
        /// <returns>周内时，秒</returns>
        public double ToEpoch(Type sys, out int week)
        {
            // 如果目标系统不是GNSS时间，则返回无效数据
            if (sys.GetType().IsSubclassOf(typeof(GnssTime)))
            {
                week = -1;
                return -1;
            }

            // 如果本身就是GNSS时间，则返回自身属性值
            if (GetType().IsSubclassOf(typeof(GnssTime)))
            {
                PropertyInfo pro = GetType().GetProperty("Week");
                week = (int)pro.GetValue(this, null);
                pro = GetType().GetProperty("TOW");
                return (double)pro.GetValue(this, null);
            }

            // 根据目标类型，获取GNSS时间起算点，以计算初始跳秒数
            DateTime origin = DateTime.Now;
            switch (sys.Name)
            {
                case "GpsTime":
                    origin = GpsTime.GpsOrigin;
                    break;

                case "BdsTime":
                    origin = BdsTime.BdsOrigin;
                    break;

                case "GalileoTime":
                    origin = GalileoTime.GalileoOrigin;
                    break;

                default:
                    break;
            }

            // 加上跳秒的影响
            double zero = GetLeaps(origin);
            double leap = GetLeaps(_moment);

            TimeSpan span = _moment.AddSeconds(leap - zero) - origin;
            week = (int)Math.Floor(span.TotalDays / 7);
            return span.TotalSeconds - week * SecondInWeek;
        }
        #endregion

        #region 运算符重载
        public static double operator -(TimeSystem t1, TimeSystem t2)
        {
            if (t1.GetType() != t2.GetType()) return double.NaN;

            return (t1.Moment - t2.Moment).TotalSeconds;
        }

        public static bool operator ==(TimeSystem t1, TimeSystem t2)
        {
            if (t1.GetType() != t2.GetType()) return false;

            return t1.Moment == t2.Moment;
        }

        public static bool operator !=(TimeSystem t1, TimeSystem t2)
        {
            if (t1.GetType() != t2.GetType()) return true;

            return t1.Moment != t2.Moment;
        }

        public static bool operator <(TimeSystem t1, TimeSystem t2)
        {
            if (t1.GetType() != t2.GetType()) return false;

            return t1.Moment < t2.Moment;
        }

        public static bool operator >(TimeSystem t1, TimeSystem t2)
        {
            if (t1.GetType() != t2.GetType()) return false;

            return t1.Moment > t2.Moment;
        }

        public static bool operator <=(TimeSystem t1, TimeSystem t2)
        {
            if (t1.GetType() != t2.GetType()) return false;

            return t1.Moment <= t2.Moment;
        }

        public static bool operator >=(TimeSystem t1, TimeSystem t2)
        {
            if (t1.GetType() != t2.GetType()) return false;

            return t1.Moment >= t2.Moment;
        }
        #endregion

        /// <summary>
        /// 实现IComparable接口
        /// </summary>
        /// <param name="time">待比较时间</param>
        /// <returns>大小关系</returns>
        public int CompareTo(TimeSystem time)
        {
            return _moment.CompareTo(time._moment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() != GetType()) return false;

            TimeSystem t = (TimeSystem)obj;
            return t.Moment == _moment;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _moment.GetHashCode();
        }

        /// <summary>
        /// 重写ToString函数
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _moment.ToString("yyyy-MM-dd_HH:mm:ss.fff");
        }

        protected bool ValidateDate(int year, int month, double day)
        {
            try
            {
                DateTime time = new DateTime(year, month, (int)Math.Ceiling(day));
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected bool ValidateTime(int year, int month, int day, int hour, int minute, double second)
        {
            try
            {
                DateTime time = new DateTime(year, month, day, hour, minute, (int)Math.Ceiling(second));
                return true;
            }
            catch
            {
                return false;
            }
        }

        #region public methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public bool IsLeapYear(int year)
        {
            if (year %100 ==0)
            {
                return (year % 400 == 0);
            }
            else
            {
                return (year % 4 == 0);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public int DaysInYear(int year)
        {
            return IsLeapYear(year) ? 366 : 365;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="doy"></param>
        /// <returns></returns>
        public DateTime DoyToDate(int year, int doy)
        {
            DateTime date = new DateTime(year, 1, 1);
            return date.AddDays(doy - 1);
        }


        #endregion
    }
}
