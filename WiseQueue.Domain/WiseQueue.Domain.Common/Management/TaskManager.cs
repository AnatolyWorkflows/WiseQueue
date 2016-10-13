using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Common.Core.Logging;
using WiseQueue.Core.Common;
using WiseQueue.Core.Common.Converters;
using WiseQueue.Core.Common.DataContexts;
using WiseQueue.Core.Common.Entities.Tasks;
using WiseQueue.Core.Common.Management;
using WiseQueue.Core.Common.Management.Implementation;
using WiseQueue.Core.Common.Models;
using WiseQueue.Core.Common.Models.Tasks;
using WiseQueue.Core.Common.Specifications;
using WiseQueue.Core.Server.Management;

namespace WiseQueue.Domain.Common.Management
{
    /// <summary>
    /// Task manager. Its main responsibility is tasks management.
    /// </summary>
    public class TaskManager : BaseManager, ITaskManager
    {
        #region Fields...
        /// <summary>
        /// The <see cref="IExpressionConverter"/> instance.
        /// </summary>
        private readonly IExpressionConverter expressionConverter;

        /// <summary>
        /// The <see cref="ITaskDataContext"/> instance.
        /// </summary>
        private readonly ITaskDataContext taskDataContext;

        private readonly IServerManager serverManager;

        /// <summary>
        /// The <see cref="IQueueManager"/> instance.
        /// </summary>
        private readonly IQueueManager queueManager;
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="expressionConverter">The <see cref="IExpressionConverter"/> instance.</param>
        /// <param name="taskDataContext">The <see cref="ITaskDataContext"/> instance.</param>
        /// <param name="serverManager">The <see cref="IQueueManager"/> instance.</param>
        /// <param name="queueManager">The <see cref="IServerManager"/> instance.</param>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="expressionConverter"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="taskDataContext"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="serverManager"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="queueManager"/> is <see langword="null" />.</exception>
        public TaskManager(IExpressionConverter expressionConverter, ITaskDataContext taskDataContext, IServerManager serverManager, IQueueManager queueManager, ICommonLoggerFactory loggerFactory)
            : base("Task Manager", loggerFactory)
        {
            if (expressionConverter == null) 
                throw new ArgumentNullException("expressionConverter");
            if (taskDataContext == null)
                throw new ArgumentNullException("taskDataContext");
            if (serverManager == null) 
                throw new ArgumentNullException("serverManager");
            if (queueManager == null)
                throw new ArgumentNullException("queueManager");

            this.expressionConverter = expressionConverter;
            this.taskDataContext = taskDataContext;
            this.serverManager = serverManager;
            this.queueManager = queueManager;
        }

        #region Implementation of ITaskManager

        /// <summary>
        /// StartTask a new <c>task</c>.
        /// </summary>
        /// <param name="task">The <c>task</c>.</param>
        /// <returns>The task's identifier.</returns>
        public Int64 StartTask(Expression<Action> task)
        {
            logger.WriteDebug("Preparing a new task... Getting a default queue...");

            QueueModel defaultQueue = queueManager.GetDefaultQueue();

            logger.WriteTrace("The default queue ({0}) has been got. Converting expression into the task model...");

            ActivationData activationData = expressionConverter.Convert(task);
            TaskModel taskModel = new TaskModel(defaultQueue.Id, activationData);

            logger.WriteTrace("The expression has been converted. Inserting the task into the database...");

            Int64 taskId = taskDataContext.InsertTask(taskModel);

            logger.WriteDebug("The task has been inserted. Task identifier = {0}", taskId);

            return taskId;
        }

        public void StopTask(Int64 taskId, bool waitResponse = false)
        {
            logger.WriteDebug("Preparing for stopping task... Getting a default queue...");

            QueueModel defaultQueue = queueManager.GetDefaultQueue();

            logger.WriteTrace("The default queue ({0}) has been got. Updating the task in the database...");

            taskDataContext.SetTaskState(taskId, TaskStates.Cancel);

            logger.WriteDebug("The task has been marked as cancelled. Task identifier = {0}", taskId);

            if (waitResponse)
            {
                //TODO: Wait response from the task if needed.
            }
        }

        #endregion


        #region Implementation of IExecutable

        /// <summary>
        /// Occurs when object should do its action.
        /// </summary>
        public void Execute()
        {
            logger.WriteDebug("Searching for new tasks in available queue...");

            var queues = queueManager.GetAvailableQueues();
            foreach (QueueModel queueModel in queues)
            {
                Int64 queueId = queueModel.Id;
                Int64 serverId = serverManager.ServerId;
                TaskRequestSpecification specification = new TaskRequestSpecification(queueId, serverId);
                TaskModel taskModel;

                bool isReceived = taskDataContext.TryGetAvailableTask(specification, out taskModel);

                if (isReceived)
                {
                    //TODO: Activate and run task.

                    logger.WriteDebug("The task {0} has been received.", taskModel);
                }
                else
                {
                    logger.WriteDebug("There is no new task in the storage.");
                }

                MethodResult<Int64> methodResult = taskDataContext.GetCancelTask(queueId, serverId);
                if (methodResult.HasError)
                {
                    logger.WriteError(
                        "There was an error during geting tasks that have been marked for cancellation: ",
                        methodResult.ErrorMsg);
                }
                else
                {
                    Int64 taskId = methodResult.Result;
                    if (taskId <= 0)
                    {
                        logger.WriteTrace("There is no task for cncelling.");
                        continue;
                    }
                    logger.WriteTrace("The task (id = {0}) has been marked for cancelation. Cancelling...", taskId);
                    taskDataContext.SetTaskState(taskId, TaskStates.Cancelling);

                    //TODO: Cancel task.

                    taskDataContext.SetTaskState(taskId, TaskStates.Cancelled);
                }
            }

            logger.WriteDebug("All tasks have been found if existed.");
        }

        #endregion
    }
}
