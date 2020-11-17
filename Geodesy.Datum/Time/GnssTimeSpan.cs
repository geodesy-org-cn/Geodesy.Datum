using System;


namespace Geodesy.Datum.Time
{
    /// <summary>
    /// 时间段
    /// </summary>
    public class GnssTimeSpan
    {
        private GnssTime _start, _end;
        private TimeSpan _span;

        public GnssTimeSpan(GnssTime start, GnssTime end)
        {
            _start = start;
            _end = end;
            _span = _end.Moment - _start.Moment;
        }

        /// <summary>
        /// 时间段起点
        /// </summary>
        public GnssTime Start
        {
            get
            {
                return _start;
            }
            set
            {
                _start = value;
                _span = _end.Moment - _start.Moment;
            }
        }

        /// <summary>
        /// 时间段终点
        /// </summary>
        public GnssTime End
        {
            get
            {
                return _end;
            }
            set
            {
                _end = value;
                _span = _end.Moment - _start.Moment;
            }
        }

        /// <summary>
        /// 时间段所包含的总秒数
        /// </summary>
        public double TotalSeconds
        {
            get
            {
                return _span.TotalSeconds;
            }
        }

        /// <summary>
        /// 时间段所包含的总分钟数
        /// </summary>
        public double TotalMinutes
        {
            get
            {
                return _span.TotalMinutes;
            }
        }

        /// <summary>
        /// 时间段所包含的总小时数
        /// </summary>
        public double TotalHours
        {
            get
            {
                return _span.TotalHours;
            }
        }

        /// <summary>
        /// 时间段所包含的总天数
        /// </summary>
        public double TotalDays
        {
            get
            {
                return _span.TotalDays;
            }
        }

        /// <summary>
        /// 返回一个新的时段对象，其值为指定的时段对象与此实例的值之和。
        /// </summary>
        /// <param name="span">时段</param>
        /// <returns>新的时段</returns>
        public GnssTimeSpan Add(GnssTimeSpan span)
        {
            GnssTime time = _end;
            time.AddSeconds(span.TotalSeconds);
            return new GnssTimeSpan(_start, time);
        }

        /// <summary>
        /// 返回一个新的时段对象，其值为指定的时段对象与此实例的值之和。
        /// </summary>
        /// <param name="span">时段</param>
        /// <returns>新的时段</returns>
        public GnssTimeSpan Add(TimeSpan span)
        {
            GnssTime time = _end;
            time.AddSeconds(span.TotalSeconds);
            return new GnssTimeSpan(_start, time);
        }

        /// <summary>
        /// 返回一个新的时段对象，其值为指定的时段对象与此实例的值之差。
        /// </summary>
        /// <param name="span">时段</param>
        /// <returns>新的时段</returns>
        public GnssTimeSpan Subtract(GnssTimeSpan span)
        {
            GnssTime time = _end;
            time.AddSeconds(-span.TotalSeconds);
            return new GnssTimeSpan(_start, time);
        }

        /// <summary>
        /// 返回一个新的时段对象，其值为指定的时段对象与此实例的值之差。
        /// </summary>
        /// <param name="span">时段</param>
        /// <returns>新的时段</returns>
        public GnssTimeSpan Subtract(TimeSpan span)
        {
            GnssTime time = _end;
            time.AddSeconds(-span.TotalSeconds);
            return new GnssTimeSpan(_start, time);
        }

        /// <summary>
        /// 时段中是否包含某个时刻
        /// </summary>
        /// <param name="time">时刻</param>
        /// <returns>是否在其中</returns>
        public bool Contains(GnssTime time)
        {
            return (time.CompareTo(_end) <= 0) && (time.CompareTo(_start) >= 0);
        }

        /// <summary>
        /// 时段中是否包含某个时段
        /// </summary>
        /// <param name="span">时段</param>
        /// <returns>是否在其中</returns>
        public bool Contains(GnssTimeSpan span)
        {
            return (_end.CompareTo(span.End) >= 0) && (_start.CompareTo(span.Start) <= 0);
        }

        /// <summary>
        /// 获取时段的绝对值，当起点晚于终点时，两者交换即可，否则保持原状
        /// </summary>
        /// <returns>绝对正的时段</returns>
        public GnssTimeSpan Duration()
        {
            if (_end.CompareTo(_start) < 0)
                return new GnssTimeSpan(_end, _start);
            else
                return this;
        }

        /// <summary>
        /// 返回其值为此实例的相反值的新时段对象
        /// </summary>
        /// <returns>新时段对象</returns>
        public GnssTimeSpan Negate()
        {
            return new GnssTimeSpan(_end, _start);
        }
    }
}
