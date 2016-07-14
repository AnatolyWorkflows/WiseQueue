using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Core.BaseClasses;
using Common.Core.Logging;
using WiseQueue.Core.Common.Management;

namespace WiseQueue.Domain.Common.Management
{
    public class HistoricalManager : BaseLoggerObject, IHistoricalManager
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        public HistoricalManager(ICommonLoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        #region Implementation of IManager

        /// <summary>
        /// Start manager.
        /// </summary>
        public void Start()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Stop.
        /// </summary>
        public void Stop()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calling this function if manager should do its job.
        /// </summary>
        public void Execute()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
