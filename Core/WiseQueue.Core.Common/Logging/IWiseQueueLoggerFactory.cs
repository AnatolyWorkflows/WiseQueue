using System;

namespace WiseQueue.Core.Common.Logging
{
    /// <summary>
    /// Interface shows that <c>object</c> is a loggers' factory.
    /// </summary>
    public interface IWiseQueueLoggerFactory
    {
        /// <summary>
        /// Creates a logger for current <c>type</c>.
        /// </summary>
        /// <param name="type">The <c>type</c>.</param>
        /// <returns>The <see cref="IWiseQueueLogger"/> instance.</returns>
        /// <example>
        /// IWiseQueueLogger logger = factory.Create(GetType());
        /// </example>
        IWiseQueueLogger Create(Type type);

        /// <summary>
        /// Creates a logger for current <c>type</c>.
        /// </summary>
        /// <typeparam name="TType"><c>Type</c> of <c>object</c> for which logger will be created.</typeparam>
        /// <example>
        /// <c>IWiseQueueLogger</c> logger = factory.Create<SomeClass>();
        /// </example>
        /// <returns>The <see cref="IWiseQueueLogger"/> instance.</returns>
        IWiseQueueLogger Create<TType>();

        /// <summary>
        /// Creates a logger with name.
        /// </summary>
        /// <example>
        /// <c>IWiseQueueLogger</c> logger = factory.Create(GetType());
        /// </example>
        /// <param name="loggerName">The logger's name.</param>
        /// <returns>The <see cref="IWiseQueueLogger"/> instance.</returns>
        IWiseQueueLogger Create(string loggerName);
    }
}
