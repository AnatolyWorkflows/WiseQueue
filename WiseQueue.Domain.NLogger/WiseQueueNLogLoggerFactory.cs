using System;
using WiseQueue.Core.Common.Logging;

namespace WiseQueue.Domain.NLogger
{
    /// <summary>
    /// Factory that can create loggers.
    /// </summary>
    public sealed class WiseQueueNLogLoggerFactory : IWiseQueueLoggerFactory
    {
        /// <summary>
        /// Creates a logger of the given <see cref="Type"/>
        /// </summary>
        /// <param name="loggerType">The type.</param>
        /// <returns>The <see cref="IWiseQueueLogger"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="loggerType"/> is <see langword="null" />.</exception>
        /// <example>
        /// factory.Create(GetType());
        /// </example>
        public IWiseQueueLogger Create(Type loggerType)
        {
            if (loggerType == null) 
                throw new ArgumentNullException("loggerType");

            string name = loggerType.FullName;
            WiseQueueNLogLogger result = new WiseQueueNLogLogger(name);
            return result;
        }

        /// <summary>
        /// Creates a logger of the given <see cref="Type"/>
        /// </summary>
        /// <typeparam name="TType">Type of object that is going to use logger.</typeparam>
        /// <returns>The <see cref="IWiseQueueLogger"/> instance.</returns>
        /// <example>
        /// factory.Create<SomeClass>();
        /// </example>
        public IWiseQueueLogger Create<TType>()
        {
            Type loggerType = typeof(TType);
            string name = loggerType.FullName;
            WiseQueueNLogLogger result = new WiseQueueNLogLogger(name);
            return result;
        }

        /// <summary>
        /// Creates a logger with name.
        /// </summary>
        /// <example>
        /// <c>IWiseQueueLogger</c> logger = factory.Create(GetType());
        /// </example>
        /// <param name="loggerName">The logger's name.</param>
        /// <returns>The <see cref="IWiseQueueLogger"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="loggerName"/> is <see langword="null" />.</exception>
        public IWiseQueueLogger Create(string loggerName)
        {
            if (string.IsNullOrWhiteSpace(loggerName)) 
                throw new ArgumentNullException("loggerName");

            WiseQueueNLogLogger result = new WiseQueueNLogLogger(loggerName);
            return result;
        }
    }
}
