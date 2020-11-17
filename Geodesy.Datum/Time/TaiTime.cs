using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geodesy.Datum.Time
{
    public sealed class TaiTime : TimeSystem
    {
        public TaiTime() : this(DateTime.Now) { }

        public TaiTime(DateTime time)
        {
            _moment = time;
        }

        /// <summary>
        /// 以年月日初始化对象
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="days">日</param>
        public TaiTime(int year, int month, double days)
        {
            if (!ValidateDate(year, month, days))
                throw new GeodeticException("Error time");

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
        public TaiTime(int year, int month, int day, int hour, int minute, double seconds)
        {
            if (!ValidateTime(year, month, day, hour, minute, seconds))
                throw new GeodeticException("Error time");

            _moment = new DateTime(year, month, day, hour, minute, 0);
            _moment.AddSeconds(seconds);
        }

        public override void SetTime(DateTime time)
        {
            _moment = time;
        }

        #region 运算符重载
        public static TaiTime operator -(TaiTime time, double seconds)
        {
            return new TaiTime(time._moment.AddSeconds(-seconds));
        }

        public static TaiTime operator +(TaiTime time, double seconds)
        {
            return new TaiTime(time._moment.AddSeconds(seconds));
        }
        #endregion
    }
}
