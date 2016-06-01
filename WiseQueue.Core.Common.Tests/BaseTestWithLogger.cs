using WiseQueue.Core.Common.Logging;
using WiseQueue.Domain.NLogger;

namespace WiseQueue.Core.Common.Tests
{
    /// <summary>
    /// Base class for all tests that are going to use logger.
    /// </summary>
    public abstract class BaseTestWithLogger
    {
        /// <summary>
        /// The <see cref="IWiseQueueLoggerFactory"/> instance.
        /// </summary>
        private static IWiseQueueLoggerFactory loggerFactory;

        /// <summary>
        /// The <see cref="IWiseQueueLoggerFactory"/> instance.
        /// </summary>
        protected static IWiseQueueLoggerFactory LoggerFactory
        {
            get
            {
                if (loggerFactory == null)
                    loggerFactory = new WiseQueueNLogLoggerFactory();
                return loggerFactory;
            }
        }
    }
}
