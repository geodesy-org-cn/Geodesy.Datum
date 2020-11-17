using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geodesy.Datum.Time
{
    /// <summary>
    /// Galileo时
    /// </summary>
    public sealed class GalileoTime : GnssTime
    {
        /// <summary>
        /// Galileo时的起算点
        /// </summary>
        public static readonly DateTime GalileoOrigin = new DateTime(1999, 8, 22);

        /// <summary>
        /// Galileo时刻的最小值
        /// </summary>
        public static GalileoTime MinValue = new GalileoTime(GalileoOrigin);

        public GalileoTime() : this(DateTime.Now) { }

        public GalileoTime(DateTime time)
        {
            _origin = GalileoOrigin;
            _zero = GetLeaps(_origin);

            if (time < _origin) throw new GeodeticException("Error Time");

            SetTime(time);
        }

        public GalileoTime(int week, double timeofweek)
        {
            _origin = GalileoOrigin;
            _zero = GetLeaps(_origin);

            if (week < 0) throw new GeodeticException("Error Time");

            SetTime(week, timeofweek);
        }

        #region 运算符重载
        public static GalileoTime operator -(GalileoTime epoch, double seconds)
        {
            return new GalileoTime(epoch._week, epoch._tow - seconds);
        }

        public static GalileoTime operator +(GalileoTime epoch, double seconds)
        {
            return new GalileoTime(epoch._week, epoch._tow + seconds);
        }
        #endregion
    }
}
