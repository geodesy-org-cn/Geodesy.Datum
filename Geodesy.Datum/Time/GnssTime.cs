using System;

namespace Geodesy.Datum.Time
{
    /// <summary>
    /// 卫星导航系统时间
    /// </summary>
    public abstract class GnssTime : TimeSystem
    {
        /// <summary>
        /// GNSS时间系统的起算点
        /// </summary>
        protected DateTime _origin;
        /// <summary>
        /// 时间起算点的跳秒数
        /// </summary>
        protected double _zero;

        /// <summary>
        /// GNSS历元的周
        /// </summary>
        protected int _week;
        /// <summary>
        /// GNSS历元的周内时
        /// </summary>
        protected double _tow;

        #region 属性
        /// <summary>
        /// 时间起点
        /// </summary>
        public DateTime Origin
        {
            get
            {
                return _origin;
            }
        }

        /// <summary>
        /// 整周数
        /// </summary>
        public int Week
        {
            get
            {
                _tow = ToEpoch(_moment, out _week);
                return _week;
            }
        }

        /// <summary>
        /// 周内时，以秒为单位
        /// </summary>
        public double TOW
        {
            get
            {
                _tow = ToEpoch(_moment, out _week);
                return _tow;
            }
        }
        #endregion

        #region 时间转换
        /// <summary>
        /// 将UTC时间转换成历元
        /// </summary>
        /// <param name="time">UTC时间</param>
        /// <param name="week">周，输出值</param>
        /// <returns>周内时</returns>
        public double ToEpoch(DateTime time, out int week)
        {
            if (time < _origin)
            {
                week = -1;
                return -1;
            }

            // 加上跳秒的影响
            double leap = GetLeaps(time);
            time.AddSeconds(leap - _zero);
            TimeSpan span = time - _origin;
            
            week = (int)Math.Floor(span.TotalDays / 7);
            return span.TotalSeconds - week * SecondInWeek;
        }

        /// <summary>
        /// 将历元转换成UTC时间
        /// </summary>
        /// <param name="week">周</param>
        /// <param name="tow">周内时</param>
        /// <returns>UTC时间</returns>
        public DateTime FromEpoch(int week, double tow)
        {
            if (week < 0 || tow < 0) return DateTime.MinValue;

            DateTime time = _origin;
            time = time.AddDays(week * 7);
            time = time.AddSeconds(tow);

            // 减去跳秒的影响
            double leap = GetLeaps(time);
            time.AddSeconds(_zero - leap);

            return time;
        }

        /// <summary>
        /// 将历元转换成UTC时间
        /// </summary>
        /// <returns>UTC时间</returns>
        public UtcTime ToUtc()
        {
            return new UtcTime(_moment);
        }

        /// <summary>
        /// 将历元转换成TAI时间
        /// </summary>
        /// <returns></returns>
        public TaiTime ToTai()
        {
            return new TaiTime(_moment);
        }

        /// <summary>
        /// 将历元转换成儒略日
        /// </summary>
        /// <returns>儒略日</returns>
        public JulianDate ToJulian()
        {
            return new JulianDate(_moment);
        }
        #endregion

        /// <summary>
        /// 设置历元时刻
        /// </summary>
        /// <param name="time">UTC时间</param>
        public override void SetTime(DateTime time)
        {
            _moment = time;
            _tow = ToEpoch(time, out _week);
        }

        /// <summary>
        /// 设置历元时间
        /// </summary>
        /// <param name="week">周</param>
        /// <param name="tow">周内时</param>
        public void SetTime(int week, double tow)
        {
            _week = week;
            _tow = tow;
            _moment = FromEpoch(week, tow);
        }

        /// <summary>
        /// 获取历元时间
        /// </summary>
        /// <param name="week">整周数</param>
        /// <returns>周内时，秒</returns>
        public double GetEpoch(out int week)
        {
            _tow = ToEpoch(_moment, out _week);
            week = _week;
            return _tow;
        }
    }
}
