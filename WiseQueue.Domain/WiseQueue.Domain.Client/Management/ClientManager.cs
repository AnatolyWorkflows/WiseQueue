using System;
using System.Linq.Expressions;
using Common.Core.Logging;
using WiseQueue.Core.Client.Managment;
using WiseQueue.Core.Common.Converters;
using WiseQueue.Core.Common.DataContexts;
using WiseQueue.Core.Common.Entities.Tasks;
using WiseQueue.Core.Common.Management.Implementation;
using WiseQueue.Core.Common.Repositories;
using WiseQueue.Domain.Common.Models;
using WiseQueue.Domain.Common.Models.Tasks;

namespace WiseQueue.Domain.Client.Management
{
    public class ClientManager : BaseManager, IClientManager
    {
        private readonly ITaskRepository taskRepository;

        /// <summary>
        /// The <see cref="ITaskDataContext"/> instance.
        /// </summary>
        private readonly ITaskDataContext taskDataContext;

        private readonly IQueueDataContext queueDataContext;

        /// <summary>
        /// The <see cref="IExpressionConverter"/> instance.
        /// </summary>
        private readonly IExpressionConverter expressionConverter;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="taskDataContext">The <see cref="ITaskDataContext"/> instance.</param>
        /// <param name="expressionConverter">The <see cref="IExpressionConverter"/> instance.</param>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        public ClientManager(ITaskRepository taskRepository, ITaskDataContext taskDataContext, IQueueDataContext queueDataContext, IExpressionConverter expressionConverter, ICommonLoggerFactory loggerFactory) : base("Client Manager", loggerFactory)
        {
            if (taskRepository == null)
                throw new ArgumentNullException(nameof(taskRepository));
            if (taskDataContext == null)
                throw new ArgumentNullException("taskDataContext");
            if (queueDataContext == null)
                throw new ArgumentNullException(nameof(queueDataContext));
            if (expressionConverter == null)
                throw new ArgumentNullException(nameof(expressionConverter));

            this.taskRepository = taskRepository;
            this.taskDataContext = taskDataContext;
            this.queueDataContext = queueDataContext;
            this.expressionConverter = expressionConverter;
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

            string queueName = "default";
            logger.WriteTrace("Getting queue identifier by name {0}...", queueName);

            QueueModel queue = queueDataContext.GetQueueByName(queueName);

            ActivationData activationData = expressionConverter.Convert(task);
            int maxRerunCount = 3; //TODO: Settigns
            ScheduleInformation schedule = new ScheduleInformation(maxRerunCount);
            TaskModel taskModel = new TaskModel(activationData, schedule);

            logger.WriteTrace("The expression has been converted. Inserting the task into the database...");

            //Int64 taskId = taskDataContext.InsertTask(queueName, taskModel);
            taskModel = taskRepository.Insert(taskModel);
            Int64 taskId = taskModel.Id;

            logger.WriteDebug("The task has been started. Task identifier = {0}", taskId);

            return taskId;
        }

        /// <summary>
        /// Cancel a task that has been started.
        /// </summary>
        /// <param name="taskId">The task's identifier.</param>
        public void StopTask(Int64 taskId)
        {
            logger.WriteDebug("Stopping the task (id = {0})...", taskId);

            TaskStateChangeModel taskStateChangeModel = new TaskStateChangeModel(taskId, TaskStates.Cancel);
            taskDataContext.SetTaskState(taskStateChangeModel);

            logger.WriteDebug("The task has been marked as canceled. Task identifier = {0}", taskId);

            //TODO: Wait response from the task if needed.

            logger.WriteDebug("The task has been stopped (id = {0})", taskId);
        }

        #endregion       
    }
}
