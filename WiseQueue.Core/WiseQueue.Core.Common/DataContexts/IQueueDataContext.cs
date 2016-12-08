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
        /// Insert ф queue.
        /// </summary>
        /// <param name="queue">The <see cref="QueueModel"/> instance.</param>
        /// <returns>The queue identifier.</returns>
        Int64 InsertQueue(QueueModel queue);
    }
}
