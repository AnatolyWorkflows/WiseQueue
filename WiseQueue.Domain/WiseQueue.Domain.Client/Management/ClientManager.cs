using System;
using System.Linq.Expressions;
using Common.Core.Logging;
using WiseQueue.Core.Client.Managment;
using WiseQueue.Core.Common.Management;
using WiseQueue.Core.Common.Management.Implementation;
using WiseQueue.Core.Common.Management.TaskManagment;

namespace WiseQueue.Domain.Client.Management
{
    public class ClientManager : BaseManager, IClientManager
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
        public ClientManager(ITaskManager taskManager, ICommonLoggerFactory loggerFactory) : base("Client Manager", loggerFactory)
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

        /// <summary>
        /// Cancel a task that has been started.
        /// </summary>
        /// <param name="taskId">The task's identifier.</param>
        public void StopTask(Int64 taskId)
        {
            logger.WriteDebug("Stopping the task (id = {0})...", taskId);

            taskManager.StopTask(taskId);

            logger.WriteDebug("The task has been stopped (id = {0})", taskId);
        }

        #endregion        

        #region Implementation of IManager

        /// <summary>
        /// Start manager.
        /// </summary>
        public void Start()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Stop.
        /// </summary>
        public void Stop()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calling this function if manager should do its job.
        /// </summary>
        public void Execute()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
