using System;

namespace Common.Domain.DataAccessLayer.Exceptions
{
    /// <summary>
    /// Base data access layer exception
    /// </summary>
    public class DataAccessLayerException: Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Message</param>
        public DataAccessLayerException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="innerException">Inner exception</param>
        public DataAccessLayerException(string message, Exception innerException): base(message, innerException)
        {            
        }
    }
}
