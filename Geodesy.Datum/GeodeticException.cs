using System;

namespace Geodesy.Datum
{
    /// <summary>
    /// The exception being used for internal errors in the Geodesy Datum library.
    /// </summary>
    public class GeodeticException : Exception
    {
        private GeodeticException()
        { }

        /// <summary>
        /// New GeodesyException with a specified message
        /// </summary>
        /// <param name="message">The message for this exception</param>
        public GeodeticException(string message) : base(message)
        { }

        /// <summary>
        /// New GeodesyException with a specified message and causing inner exception
        /// </summary>
        /// <param name="message">The message for this exception</param>
        /// <param name="inner">The inner exception causing this Geodesy exception</param>
        public GeodeticException(string message, Exception inner)
            : base(message, inner)
        { }
    }
}
