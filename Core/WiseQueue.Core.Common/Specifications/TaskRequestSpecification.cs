using System;

namespace WiseQueue.Core.Common.Specifications
{
    /// <summary>
    /// Task request specification that is using for retrieving a task for the current server.
    /// </summary>
    public class TaskRequestSpecification
    {
        #region Fields...
        /// <summary>
        /// The queue's identifier in which we are going to search for a new task.
        /// </summary>
        private readonly Int64 queueId;
        /// <summary>
        /// Current server identifier.
        /// </summary>
        private readonly Int64 serverId;
        #endregion

        #region Properties...

        /// <summary>
        /// The queue's identifier in which we are going to search for a new task.
        /// </summary>
        public Int64 QueueId
        {
            get { return queueId; }
        }

        /// <summary>
        /// Current server identifier.
        /// </summary>
        public Int64 ServerId
        {
            get { return serverId; }
        }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="queueId">The queue's identifier in which we are going to search for a new task.</param>
        /// <param name="serverId">Current server identifier.</param>
        public TaskRequestSpecification(Int64 queueId, Int64 serverId)
        {
            this.queueId = queueId;
            this.serverId = serverId;
        }
    }
}
