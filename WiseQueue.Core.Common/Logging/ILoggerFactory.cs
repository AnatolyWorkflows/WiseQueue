using System;

namespace WiseQueue.Core.Common.Logging
{
    /// <summary>
    /// Interface shows that <c>object</c> is a loggers' factory.
    /// </summary>
    public interface ILoggerFactory
    {
        /// <summary>
        /// Creates a logger for current <c>type</c>.
        /// </summary>
        /// <example>
        /// <c>ILogger</c> logger = factory.Create(GetType());
        /// </example>
        /// <param name="type">The <c>type</c>.</param>
        /// <returns>The <see cref="ILogger"/> instance.</returns>
        ILogger Create(Type type);

        /// <summary>
        /// Creates a logger for current <c>type</c>.
        /// </summary>
        /// <typeparam name="TType"><c>Type</c> of <c>object</c> for which logger will be created.</typeparam>
        /// <example>
        /// <c>ILogger</c> logger = factory.Create<SomeClass>();
        /// </example>
        /// <returns>The <see cref="ILogger"/> instance.</returns>
        ILogger Create<TType>();
    }
}
