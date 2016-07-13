using System;
using Common.Core.Logging;

namespace Common.Core.BaseClasses
{
    /// <summary>
    /// Base class for objects that should have logger.
    /// </summary>
    public abstract class BaseLoggerObject
    {
        /// <summary>
        /// The <see cref="ICommonLogger"/> instance.
        /// </summary>
        protected readonly ICommonLogger logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        protected BaseLoggerObject(ICommonLoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
                throw new ArgumentNullException("loggerFactory");

            logger = loggerFactory.Create(GetType());
        }
    }
}
