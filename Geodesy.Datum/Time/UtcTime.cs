using System;

namespace Geodesy.Datum.Time
{
    /// <summary>
    /// UTC时间
    /// </summary>
    public sealed class UtcTime : TimeSystem
    {
        /// <summary>
        /// 
        /// </summary>
        public UtcTime() : this(DateTime.Now) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        public UtcTime(DateTime time)
        {
            SetTime(time);
        }

        /// <summary>
        /// 以年月日初始化对象
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="days">日</param>
        public UtcTime(int year, int month, double days)
        {
            if (!ValidateDate(year, month, days))
                throw new GeodeticException("Error time");

            _moment = new DateTime(year, month, 1);
            _moment = _moment.AddDays(days - 1);
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
        public UtcTime(int year, int month, int day, int hour, int minute, double seconds)
        {
            if (!ValidateTime(year, month, day, hour, minute, seconds))
                throw new GeodeticException("Error time");

            _moment = new DateTime(year, month, day, hour, minute, 0);
            _moment = _moment.AddSeconds(seconds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epoch"></param>
        public UtcTime(double epoch)
        {
            int year = (int)Math.Floor(epoch);
            DateTime date = new DateTime(year, 1, 1);

            double days = (epoch - year) * DaysInYear(year);
            _moment = date.AddDays(days - 1);
        }

        /// <summary>
        /// 设置当前时刻
        /// </summary>
        /// <param name="time">时刻</param>
        public override void SetTime(DateTime time)
        {
            _moment = time;
        }

        /// <summary>
        /// 将UTC时间转换成儒略日
        /// </summary>
        /// <param name="time">UTC时间</param>
        /// <returns>儒略日</returns>
        public JulianDate ToJulian()
        {
            return new JulianDate(_moment);
        }
    }
}
