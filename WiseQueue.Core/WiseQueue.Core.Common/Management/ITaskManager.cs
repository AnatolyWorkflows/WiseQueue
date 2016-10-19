using System;
using System.Linq.Expressions;
using Common.Core.Interfaces;

namespace WiseQueue.Core.Common.Management
{
    /// <summary>
    /// Interface shows that <c>object</c> is a task manager.
    /// </summary>
    public interface ITaskManager: IManager, IExecutable
    {
        /// <summary>
        /// StartTask a new <c>task</c>.
        /// </summary>
        /// <param name="task">The <c>task</c>.</param>
        /// <returns>The task's identifier.</returns>
        Int64 StartTask(Expression<Action> task);

        /// <summary>
        /// Stop a task that has been started.
        /// </summary>
        /// <param name="taskId">The task's identifier.</param>
        /// <param name="waitResponse">Falg shows that method will be wating until task is cancelled.</param>
        void StopTask(Int64 taskId, bool waitResponse = false);

        /// <summary>
        /// Set configuration that will be used from the server side
        /// </summary>
        /// <param name="maxTaskPerQueue">Max tasks that will be requested from the database in one queue.</param>
        /// <param name="maxRerunAttempts">Max re-run attempts that will be done if task has been crashed.</param>
        /// <param name="timeShiftAfterCrash">Time that will be wait before next attempt.</param>
        void SetServerConfiguration(int maxTaskPerQueue, int maxRerunAttempts, TimeSpan timeShiftAfterCrash);
    }
}
