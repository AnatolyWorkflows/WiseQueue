using System;
using System.Linq.Expressions;
using WiseQueue.Core.Client.Managment;

namespace WiseQueue.Domain.Configuration
{
    public static class BackgroundClient
    {
        /// <summary>
        /// Start a new task.
        /// </summary>
        /// <param name="action">The task as expression.</param>
        /// <returns>The task's identifier.</returns>
        public static Int64 StartNewTask(Expression<Action> action)
        {
            IClientManager clientManager = WiseQueueGlobalConfiguration.WiseQueueConfiguration.Get<IClientManager>();
            Int64 taskId = clientManager.StartTask(action);
            return taskId;
        }

        /// <summary>
        /// Cancel a task that has been started.
        /// </summary>
        /// <param name="taskId">The task's identifier.</param>
        public static void CancelTask(Int64 taskId)
        {
            IClientManager clientManager = WiseQueueGlobalConfiguration.WiseQueueConfiguration.Get<IClientManager>();
            clientManager.StopTask(taskId);
        }
    }
}
