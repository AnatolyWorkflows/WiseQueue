using System;
using WiseQueue.Core.Common.Entities.Tasks;

namespace WiseQueue.Core.Common.Models.Tasks
{
    public class TaskStateModel
    {
        #region Properties...

        /// <summary>
        /// Task's identifier.
        /// </summary>
        public Int64 Id { get; private set; }

        /// <summary>
        /// The queue's identifier where the task will be put.
        /// </summary>
        public Int64 QueueId { get; private set; }

        /// <summary>
        /// The server's identifier where the task will be put.
        /// </summary>
        public Int64 ServerId { get; private set; }

        /// <summary>
        /// Task's state.
        /// </summary>
        public TaskStates TaskState { get; private set; }
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="taskId">Task's identifier.</param>
        /// <param name="queueId">The queue's identifier where the task will be put.</param>
        /// <param name="serverId">The server's identifier that is used for processing the task.</param>
        /// <param name="taskState">Task's state.</param>
        /// <exception cref="ArgumentOutOfRangeException">The task identifier should be great than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The queue identifier should be great than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The server identifier should be great than 0.</exception>
        public TaskStateModel(Int64 taskId, Int64 queueId, Int64 serverId, TaskStates taskState)
        {
            if (taskId <= 0)
                throw new ArgumentOutOfRangeException("taskId", "The task identifier should be great than 0.");
            if (queueId <= 0)
                throw new ArgumentOutOfRangeException("queueId", "The queue identifier should be great than 0.");
            if (serverId <= 0)
                throw new ArgumentOutOfRangeException("serverId", "The server identifier should be great than 0.");

            Id = taskId;
            QueueId = queueId;
            ServerId = serverId;
            TaskState = taskState;
        }

        /// <exception cref="ArgumentOutOfRangeException">The queue identifier should be great than 0.</exception>
        protected TaskStateModel(Int64 queueId)
        {
            if (queueId <= 0)
                throw new ArgumentOutOfRangeException("queueId", "The queue identifier should be great than 0.");
            Id = 0;
            QueueId = queueId;
            ServerId = 0;
            TaskState = TaskStates.New;
        }

        #region Overrides of Object

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("Id: {0}; QueueId: {1}; ServerId: {2}; TaskState: {3}", Id, QueueId, ServerId, TaskState);
        }

        #endregion
    }
}
