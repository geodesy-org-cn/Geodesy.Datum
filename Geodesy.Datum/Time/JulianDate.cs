using System;

namespace Geodesy.Datum.Time
{
    /// <summary>
    /// 简化儒略日
    /// </summary>
    public sealed class JulianDate : TimeSystem
    {
        /// <summary>
        /// 简化儒略日的起算点
        /// </summary>
        public static readonly DateTime MjdZero = new DateTime(1858, 11, 17);

        /// <summary>
        /// 以当前时刻初始化对象
        /// </summary>
        public JulianDate() : this(DateTime.Now) { }

        /// <summary>
        /// 以日期初始化对象
        /// </summary>
        /// <param name="time">日期</param>
        public JulianDate(DateTime time)
        {
            _moment = time;
        }

        /// <summary>
        /// 以儒略日初始化对象
        /// </summary>
        /// <param name="mjd">儒略日</param>
        public JulianDate(double mjd)
        {
            _moment = MjdZero.AddDays(mjd);
        }

        /// <summary>
        /// 以年月日初始化对象
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="days">日</param>
        public JulianDate(int year, int month, double days)
        {
            if (!ValidateDate(year, month, days))
                throw new GeodeticException("Error date");

            _moment = new DateTime(year, month, 1);
            _moment.AddDays(days - 1);
        }

        /// <summary>
        /// 以年月日时分秒初始化对象
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">时</param>
        /// <param name="minute">分</param>
        /// <param name="seconds">秒</param>
        public JulianDate(int year, int month, int day, int hour, int minute, double seconds)
        {
            if (!ValidateTime(year, month, day, hour, minute, seconds))
                throw new GeodeticException("Error time");

            _moment = new DateTime(year, month, day, hour, minute, 0);
            _moment.AddSeconds(seconds);
        }

        /// <summary>
        /// 儒略日
        /// </summary>
        public double JD
        {
            get
            {
                return (_moment - MjdZero).TotalDays + 2400000.5;
            }
            set
            {
                SetTime(MjdZero.AddDays(value - 2400000.5));
            }
        }

        /// <summary>
        /// 简化儒略日
        /// </summary>
        public double MJD
        {
            get
            {
                return (_moment - MjdZero).TotalDays;
            }
            set
            {
                _moment = MjdZero.AddDays(value);
            }
        }

        /// <summary>
        /// 设置儒略日
        /// </summary>
        /// <param name="mjd">儒略日值</param>
        public void SetTime(double mjd)
        {
            _moment = MjdZero.AddDays(mjd);
        }

        /// <summary>
        /// 设置儒略日
        /// </summary>
        /// <param name="time">日期</param>
        public override void SetTime(DateTime time)
        {
            _moment = time;
        }

        /// <summary>
        /// 将儒略日转换成UTC时
        /// </summary>
        /// <returns>UTC时</returns>
        public UtcTime ToUtc()
        {
            return new UtcTime(_moment);
        }

        public static JulianDate operator +(JulianDate jd, double days)
        {
            jd.AddDays(days);
            return jd;
        }

        public static JulianDate operator -(JulianDate jd, double days)
        {
            jd.AddDays(-days);
            return jd;
        }
    }
}
