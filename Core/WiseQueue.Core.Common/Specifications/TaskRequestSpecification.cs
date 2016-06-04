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
        /// <summary>
        /// <c>Timeout</c> when task that was <c>fixed</c> for the <see cref="serverId"/> will be expired.
        /// </summary>
        private readonly TimeSpan timeout;
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

        /// <summary>
        /// <c>Timeout</c> when task that was <c>fixed</c> for the <see cref="serverId"/> will be expired.
        /// </summary>
        public TimeSpan Timeout
        {
            get { return timeout; }
        }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="queueId">The queue's identifier in which we are going to search for a new task.</param>
        /// <param name="serverId">Current server identifier.</param>
        /// <param name="timeout"><c>Timeout</c> when jobs fixing for the <see cref="serverId"/> will be expired.</param>
        public TaskRequestSpecification(int queueId, Int64 serverId, TimeSpan timeout)
        {
            this.queueId = queueId;
            this.serverId = serverId;
            this.timeout = timeout;
        }

    }
}
