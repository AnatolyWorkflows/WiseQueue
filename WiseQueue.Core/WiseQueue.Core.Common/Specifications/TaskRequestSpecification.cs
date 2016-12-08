using System;

namespace WiseQueue.Core.Common.Specifications
{
    /// <summary>
    /// Task request specification that is using for retrieving a task for the current server.
    /// </summary>
    public class TaskRequestSpecification
    {
        #region Properties...

        /// <summary>
        /// The queue's name in which we are going to search for a new task.
        /// </summary>
        public string QueueName { get; private set; }

        /// <summary>
        /// Current server identifier.
        /// </summary>
        public Int64 ServerId { get; private set; }

        /// <summary>
        /// Max amount of tasks that will be requested.
        /// </summary>
        public int MaxTasks { get; private set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="queueName">The queue's name in which we are going to search for a new task.</param>
        /// <param name="serverId">Current server identifier.</param>
        /// <param name="maxTasks">Max amount of tasks that will be requested.</param>
        public TaskRequestSpecification(string queueName, Int64 serverId, int maxTasks = 10)
        {
            QueueName = queueName;
            ServerId = serverId;
            MaxTasks = maxTasks;
        }
    }
}
