using System;

namespace Geodesy.Datum.Time
{
    /// <summary>
    /// BDS时
    /// </summary>
    public sealed class BdsTime : GnssTime
    {
        /// <summary>
        /// BDS时的起算点
        /// </summary>
        public static readonly DateTime BdsOrigin = new DateTime(2006, 1, 1);

        /// <summary>
        /// BDS时刻的最小值
        /// </summary>
        public static BdsTime MinValue = new BdsTime(BdsOrigin);

        public BdsTime() : this(DateTime.Now) { }

        public BdsTime(DateTime time)
        {
            _origin = BdsOrigin;
            _zero = GetLeaps(_origin);

            if (time < _origin) throw new GeodeticException("Error Time");

            SetTime(time);
        }

        public BdsTime(int week, double tow)
        {
            _origin = BdsOrigin;
            _zero = GetLeaps(_origin);

            if (week < 0) throw new GeodeticException("Error Time");

            SetTime(week, tow);
        }

        #region 运算符重载
        public static BdsTime operator -(BdsTime epoch, double seconds)
        {
            return new BdsTime(epoch._week, epoch._tow - seconds);
        }

        public static BdsTime operator +(BdsTime epoch, double seconds)
        {
            return new BdsTime(epoch._week, epoch._tow + seconds);
        }
        #endregion
    }
}
