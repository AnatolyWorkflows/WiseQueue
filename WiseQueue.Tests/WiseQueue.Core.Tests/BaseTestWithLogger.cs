using Common.Core.Logging;
using Common.NLogger;

namespace WiseQueue.Core.Tests
{
    /// <summary>
    /// Base class for all tests that are going to use logger.
    /// </summary>
    public abstract class BaseTestWithLogger
    {
        /// <summary>
        /// The <see cref="ICommonLoggerFactory"/> instance.
        /// </summary>
        private static ICommonLoggerFactory loggerFactory;

        /// <summary>
        /// The <see cref="ICommonLoggerFactory"/> instance.
        /// </summary>
        protected static ICommonLoggerFactory LoggerFactory
        {
            get
            {
                if (loggerFactory == null)
                    loggerFactory = new CommonNLogLoggerFactory();
                return loggerFactory;
            }
        }
    }
}
