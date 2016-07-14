using System;
using Common.Core.BaseClasses;
using Common.Core.Logging;

namespace WiseQueue.Core.Common.Management.Implementation
{
    /// <summary>
    /// Base manager.
    /// </summary>
    public abstract class BaseManager: BaseLoggerObject, IManager
    {
        /// <summary>
        /// Manager name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Manager <c>name</c>.</param>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">Value cannot be null or empty.</exception>
        protected BaseManager(string name, ICommonLoggerFactory loggerFactory) : base(loggerFactory)
        {
            if (string.IsNullOrEmpty(name)) 
                throw new ArgumentException("Value cannot be null or empty.", "name");

            Name = name;
        }
    }
}
