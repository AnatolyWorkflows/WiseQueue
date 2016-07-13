using System;
using System.Collections.Generic;
using WiseQueue.Core.Common.Entities;
using WiseQueue.Core.Common.Models;

namespace WiseQueue.Core.Common.DataContexts
{
    /// <summary>
    /// Interface shows that <c>object</c> is a queues' data context.
    /// </summary>
    public interface IQueueDataContext
    {
        /// <summary>
        /// Get queue by its name.
        /// </summary>
        /// <param name="queueName">The queue name.</param>
        /// <returns>The <see cref="QueueModel"/> instance.</returns>
        QueueModel GetQueueByName(string queueName);

        /// <summary>
        /// Insert queue.
        /// </summary>
        /// <param name="queueEntity">The <see cref="QueueModel"/> instance.</param>
        /// <returns>The queue identifier.</returns>
        Int64 InsertQueue(QueueModel queueEntity);

        /// <summary>
        /// Get available queues.
        /// </summary>
        /// <returns><c>List</c> of <see cref="QueueEntity"/> instances.</returns>
        IReadOnlyCollection<QueueModel> GetAvailableQueues();
    }
}
