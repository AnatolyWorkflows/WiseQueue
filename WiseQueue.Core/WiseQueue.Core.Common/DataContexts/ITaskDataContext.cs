using System;
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
        /// <param name="taskModel">The <see cref="TaskModel"/> instance if it has been found. Otherwise, null.</param>
        /// <returns>True if the TaskModel instance has been populated. Otherwise, false.</returns>
        bool TryGetAvailableTask(TaskRequestSpecification specification, out TaskModel taskModel);

        /// <summary>
        /// Set task's state.
        /// </summary>
        /// <param name="id">The task's identifier.</param>
        /// <param name="taskState">New task's state that we are going to set.</param>
        void SetTaskState(Int64 id, TaskStates taskState);
    }
}
