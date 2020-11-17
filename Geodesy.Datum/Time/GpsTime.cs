using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geodesy.Datum.Time
{
    /// <summary>
    /// GPS时
    /// </summary>
    public sealed class GpsTime : GnssTime
    {
        /// <summary>
        /// GPS时的起算点
        /// </summary>
        public static readonly DateTime GpsOrigin = new DateTime(1980, 1, 6);

        /// <summary>
        /// GPS时刻的最小值
        /// </summary>
        public static readonly GpsTime MinValue = new GpsTime(GpsOrigin);

        /// <summary>
        /// 以当前时刻初始化对象
        /// </summary>
        public GpsTime() : this(DateTime.Now) { }

        /// <summary>
        /// 以指定的时间初始化对象
        /// </summary>
        /// <param name="time"></param>
        public GpsTime(DateTime time)
        {
            _origin = GpsOrigin;
            _zero = GetLeaps(_origin);

           if (time < _origin) throw new GeodeticException("Error Time");

            SetTime(time);
        }

        /// <summary>
        /// 以历元时刻初始化对象
        /// </summary>
        /// <param name="week">周</param>
        /// <param name="timeofweek">周内时</param>
        public GpsTime(int week, double timeofweek)
        {
            _origin = GpsOrigin;
            _zero = GetLeaps(_origin);

            if (week < 0 || timeofweek < 0) throw new GeodeticException("Error Time");

            SetTime(week, timeofweek);
        }

        /// <summary>
        /// 以日期初始化对象
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="days">月内天数</param>
        public GpsTime(int year, int month, double days)
        {
            if (!ValidateDate(year,month,days))
                throw new GeodeticException("Error time");

            _moment = new DateTime(year, month, 1);
            _moment.AddDays(days - 1);
            _tow = ToEpoch(_moment, out _week);
        }

        /// <summary>
        /// 以时间初始化对象
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">时</param>
        /// <param name="minute">分</param>
        /// <param name="seconds">秒</param>
        public GpsTime(int year, int month, int day, int hour, int minute, double seconds)
        {
            if (!ValidateTime(year, month, day, hour, minute, seconds))
                throw new GeodeticException("Error time");

            _moment = new DateTime(year, month, day, hour, minute, 0);
            _moment.AddSeconds(seconds);
            _tow = ToEpoch(_moment, out _week);
        }

        #region 运算符重载
        public static GpsTime operator -(GpsTime epoch, double seconds)
        {
            return new GpsTime(epoch._week, epoch._tow - seconds);
        }

        public static GpsTime operator +(GpsTime epoch, double seconds)
        {
            return new GpsTime(epoch._week, epoch._tow + seconds);
        }
        #endregion
    }
}
