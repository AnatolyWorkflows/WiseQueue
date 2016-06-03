using System;
using WiseQueue.Core.Common.Logging;

namespace WiseQueue.Core.Common.Models
{
    /// <summary>
    /// Base class for objects that should have logger.
    /// </summary>
    public abstract class BaseLoggerObject
    {
        /// <summary>
        /// The <see cref="IWiseQueueLogger"/> instance.
        /// </summary>
        protected readonly IWiseQueueLogger logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory">The <see cref="IWiseQueueLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        protected BaseLoggerObject(IWiseQueueLoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
                throw new ArgumentNullException("loggerFactory");

            logger = loggerFactory.Create(GetType());
        }
    }
}
