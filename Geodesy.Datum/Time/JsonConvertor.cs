using Newtonsoft.Json.Converters;

namespace Geodesy.Datum.Time
{

    /// <summary>
    /// 
    /// </summary>
    public class DateJsonConverter : IsoDateTimeConverter
    {
        public DateJsonConverter()
            
        {
            base.DateTimeFormat = "yyyy-MM-dd";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TimeJsonConverter : IsoDateTimeConverter
    {
        public TimeJsonConverter()
            
        {
            base.DateTimeFormat = "HH:mm:ss";
        }
    }
}
