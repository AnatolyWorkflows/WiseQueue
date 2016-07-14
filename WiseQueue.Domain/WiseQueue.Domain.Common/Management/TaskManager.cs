using System;
using System.Linq.Expressions;
using Common.Core.Logging;
using WiseQueue.Core.Common.Converters;
using WiseQueue.Core.Common.DataContexts;
using WiseQueue.Core.Common.Management;
using WiseQueue.Core.Common.Management.Implementation;
using WiseQueue.Core.Common.Models;
using WiseQueue.Core.Common.Models.Tasks;
using WiseQueue.Core.Common.Specifications;

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

            TaskActivationDetailsModel taskActivationDetails = expressionConverter.Convert(task);
            TaskModel taskModel = new TaskModel(defaultQueue.Id, taskActivationDetails);

            logger.WriteTrace("The expression has been converted. Inserting the task into the database...");

            Int64 taskId = taskDataContext.InsertTask(taskModel);

            logger.WriteDebug("The task has been inserted. Task identifier = {0}", taskId);

            return taskId;
        }

        #endregion


        #region Implementation of IManager

        /// <summary>
        /// Start manager.
        /// </summary>
        public void Start()
        {            
        }

        /// <summary>
        /// Stop.
        /// </summary>
        public void Stop()
        {
        }

        /// <summary>
        /// Calling this function if manager should do its job.
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
            }

            logger.WriteDebug("All tasks have been found if existed.");
        }

        #endregion
    }
}
