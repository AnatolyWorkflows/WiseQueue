using System;
using System.Linq.Expressions;
using Common.Core.BaseClasses;
using Common.Core.Logging;
using WiseQueue.Core.Common.Management;
using WiseQueue.Core.Common.Models;
using WiseQueue.Domain.Common;

namespace WiseQueue.Domain.Client.Management
{
    public class ClientManager : BaseLoggerObject, IClientManager
    {
        /// <summary>
        /// The <see cref="ITaskManager"/> instance.
        /// </summary>
        private readonly ITaskManager taskManager;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="taskManager">The <see cref="ITaskManager"/> instance.</param>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        public ClientManager(ITaskManager taskManager, ICommonLoggerFactory loggerFactory) : base(loggerFactory)
        {
            if (taskManager == null)
                throw new ArgumentNullException("taskManager");
            this.taskManager = taskManager;
        }

        #region Implementation of IClientManager

        /// <summary>
        /// Start new <c>task</c>.
        /// </summary>
        /// <param name="task">The <see cref="Expression"/> instance.</param>
        /// <returns>The task's identifier.</returns>
        public Int64 StartTask(Expression<Action> task)
        {
            logger.WriteDebug("Starting a new task...");

            Int64 taskId = taskManager.StartTask(task);

            logger.WriteDebug("The task has been put into the database. Task identifier = {0}", taskId);
            return taskId;
        }

        #endregion

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
    }
}
