using System;
using System.Collections.Generic;
using WiseQueue.Core.Common.Entities.Tasks;
using WiseQueue.Core.Common.Models.Tasks;
using WiseQueue.Core.Common.Specifications;

namespace WiseQueue.Core.Common.DataContexts
{
    /// <summary>
    /// Interface shows that <c>object</c> is a tasks' data context.
    /// </summary>
    public interface ITaskDataContext
    {
        /// <summary>
        /// Insert a task into the storage.
        /// </summary>
        /// <param name="taskEntity">The <see cref="TaskModel"/> instance.</param>
        /// <returns>The task's identifier.</returns>
        Int64 InsertTask(TaskModel taskEntity);

        /// <summary>
        /// Get available task from the storage.
        /// </summary>
        /// <param name="specification">The <see cref="TaskRequestSpecification"/> instance.</param>
        /// <param name="taskModels">List of <see cref="TaskModel"/> instances if it has been found</param>
        /// <returns>True if the list of TaskModel instances has been populated. Otherwise, false.</returns>
        bool TryGetAvailableTask(TaskRequestSpecification specification, out List<TaskModel> taskModels);

        /// <summary>
        /// Set task's state.
        /// </summary>
        /// <param name="id">The task's identifier.</param>
        /// <param name="taskState">New task's state that we are going to set.</param>
        void SetTaskState(Int64 id, TaskStates taskState);

        /// <summary>
        /// Get task that has been marked for cancellation.
        /// </summary>
        /// <param name="queueId">The queue identifier.</param>
        /// <param name="serverId">The server identifier.</param>
        /// <param name="taskIds">List of tasks' identifiers that have been canceled.</param>
        /// <returns>True if there is at minimum one task that has been marked for cancel.</returns>
        bool TryGetCancelTasks(Int64 queueId, Int64 serverId, out List<Int64> taskIds);
    }
}
