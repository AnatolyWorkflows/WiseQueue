using System;
using Common.Core.Logging;
using WiseQueue.Core.Common.Management;
using WiseQueue.Core.Common.Management.Implementation;

namespace WiseQueue.Domain.Common.Management
{
    public class HistoricalManager : BaseManager, IHistoricalManager
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">Value cannot be null or empty.</exception>
        public HistoricalManager(ICommonLoggerFactory loggerFactory) : base("HistoricalManager", loggerFactory)
        {
        }

        #region Implementation of IExecutableManager

        /// <summary>
        /// Calling this function if manager should do its job.
        /// </summary>
        public void Execute()
        {
            //TODO: Do some historical activities.
            throw new NotImplementedException();
        }

        #endregion
    }
}
