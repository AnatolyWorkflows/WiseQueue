using System.Collections.Generic;
using WiseQueue.Core.Common.Entities;
using WiseQueue.Core.Common.Models;

namespace WiseQueue.Core.Common.Management
{
    /// <summary>
    /// Interface shows that <c>object</c> is a queues' manager.
    /// </summary>
    public interface IQueueManager
    {
        /// <summary>
        /// Get default queue.
        /// </summary>
        /// <returns>The <see cref="QueueModel"/> instance.</returns>
        QueueModel GetDefaultQueue();

        /// <summary>
        /// Check queue by name.
        /// </summary>
        /// <param name="queueName">The queue's name.</param>
        /// <returns>The <see cref="QueueModel"/> instance. </returns>
        /// <remarks>
        /// Depends on settings:
        ///     1) If there is no queue Exception will be generated.
        ///     2) If there is no queue it will be created.
        /// </remarks>
        QueueModel CheckQueueByName(string queueName);

        /// <summary>
        /// Get queues that available for current server.
        /// </summary>
        /// <returns><c>List</c> of <see cref="QueueModel"/>.</returns>
        IReadOnlyCollection<QueueModel> GetAvailableQueues();
    }

}
