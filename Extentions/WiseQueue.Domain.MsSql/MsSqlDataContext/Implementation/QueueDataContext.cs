using System;
using System.Collections.Generic;
using WiseQueue.Core.Common.DataContexts;
using WiseQueue.Core.Common.Entities;
using WiseQueue.Core.Common.Logging;
using WiseQueue.Core.Common.Models;

namespace WiseQueue.Domain.MsSql.MsSqlDataContext.Implementation
{
    class QueueDataContext : BaseLoggerObject, IQueueDataContext
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory">The <see cref="IWiseQueueLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        public QueueDataContext(IWiseQueueLoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        #region Implementation of IQueueDataContext

        /// <summary>
        /// Get queue by its name.
        /// </summary>
        /// <param name="queueName">The queue name.</param>
        /// <returns>The <see cref="QueueEntity"/> instance.</returns>
        public QueueEntity GetQueueByName(string queueName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Insert queue.
        /// </summary>
        /// <param name="queueEntity">The <see cref="QueueEntity"/> instance.</param>
        /// <returns>The queue identifier.</returns>
        public long InsertQueue(QueueEntity queueEntity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get available queues.
        /// </summary>
        /// <returns><c>List</c> of <see cref="QueueEntity"/> instances.</returns>
        public IReadOnlyCollection<QueueEntity> GetAvailableQueues()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
