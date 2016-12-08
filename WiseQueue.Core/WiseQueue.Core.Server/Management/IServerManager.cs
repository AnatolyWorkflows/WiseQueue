using System;
using System.Collections.Generic;
using Common.Core.Interfaces;
using WiseQueue.Core.Common.Management;
using WiseQueue.Core.Common.Models;

namespace WiseQueue.Core.Server.Management
{
    /// <summary>
    /// Interface shows that <c>object</c> is a server manager.
    /// </summary>
    public interface IServerManager : IManager, IStartStoppable
    {
        /// <summary>
        /// The server identifier.
        /// </summary>
        Int64 ServerId { get; }

        /// <summary>
        /// Get default queue.
        /// </summary>
        /// <returns>The queue.</returns>
        QueueModel GetDefaultQueue();

        /// <summary>
        /// Get all queues that available for this server,
        /// </summary>
        /// <returns>List of queues.</returns>
        IReadOnlyCollection<QueueModel> GetAvailableQueues();
    }
}
