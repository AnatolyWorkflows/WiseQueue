using System;
using WiseQueue.Core.Common.Entities;

namespace WiseQueue.Core.Common.DataContexts
{
    /// <summary>
    /// Interface shows that <c>object</c> is a task data context.
    /// </summary>
    public interface ITaskDataContext
    {
        /// <summary>
        /// Insert a task into the storage.
        /// </summary>
        /// <param name="taskEntity">The <see cref="TaskEntity"/> instance.</param>
        /// <returns>The task's identifier.</returns>
        Int64 InsertTask(TaskEntity taskEntity);

        /// <summary>
        /// Get available task from the storage.
        /// </summary>
        /// <returns>The <see cref="TaskEntity"/> instance if it has been found. Otherwise, null.</returns>
        TaskEntity GetAvailableTask();
    }
}
