using System;
using System.Collections.Generic;
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
        /// Get available task from the storage.
        /// </summary>
        /// <param name="specification">The <see cref="TaskRequestSpecification"/> instance.</param>
        /// <param name="taskModels">List of <see cref="WiseQueue.Core.Common.Models.Tasks.TaskStateModel"/> instances if it has been found</param>
        /// <returns>True if the list of TaskModel instances has been populated. Otherwise, false.</returns>
        bool TryGetAvailableTask(TaskRequestSpecification specification, out List<TaskModel> taskModels);

        /// <summary>
        /// Get task that has been marked for cancellation.
        /// </summary>
        /// <param name="specification">The <see cref="TaskRequestSpecification"/> instance.</param>
        /// <param name="taskIds">List of tasks' identifiers that have been canceled.</param>
        /// <returns>True if there is at minimum one task that has been marked for cancel.</returns>
        bool TryGetCancelTasks(TaskRequestSpecification specification, out List<Int64> taskIds);

        /// <summary>
        /// Insert a task into the storage.
        /// </summary>
        /// <param name="taskModel">The <see cref="TaskModel"/> instance.</param>
        /// <returns>The task's identifier.</returns>
        Int64 InsertTask(TaskModel taskModel);

        /// <summary>
        /// Set task's state.
        /// </summary>
        /// <param name="taskStateModel">The <see cref="TaskStateModel"/> instance.</param>
        void SetTaskState(TaskStateModel taskStateModel);

        /// <summary>
        /// Stop task by its identifier.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        void StopTask(Int64 taskId);

        /// <summary>
        /// Try to restart the task.
        /// </summary>
        /// <param name="taskStateModel">The <see cref="TaskStateModel"/> instance.</param>
        /// <param name="timeShift">Time shift.</param>
        /// <param name="repeatCrashCount">Count of attempts that will be used for reruning this task after its crashed.</param>
        /// <param name="msg">Message that explains the restart.</param>
        /// <param name="exception">Exception that explains the restart.</param>
        void RestartTask(TaskStateModel taskStateModel, TimeSpan timeShift, int repeatCrashCount, string msg, Exception exception);
    }
}
