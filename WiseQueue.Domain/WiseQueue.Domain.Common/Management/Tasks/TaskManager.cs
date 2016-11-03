using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Common.Core.Logging;
using WiseQueue.Core.Common.Converters;
using WiseQueue.Core.Common.DataContexts;
using WiseQueue.Core.Common.Entities.Tasks;
using WiseQueue.Core.Common.Management;
using WiseQueue.Core.Common.Management.Implementation;
using WiseQueue.Core.Common.Management.TaskManagment;
using WiseQueue.Core.Common.Models;
using WiseQueue.Core.Common.Models.Tasks;
using WiseQueue.Core.Common.Specifications;
using WiseQueue.Core.Server.Management;

namespace WiseQueue.Domain.Common.Management.Tasks
{
    /// <summary>
    /// Task manager. Its main responsibility is tasks management.
    /// </summary>
    public class TaskManager : BaseManager, ITaskManager
    {
        #region Fields...

        private readonly TaskManagerConfiguration taskManagerConfiguration;

        private readonly ITaskBuilder taskBuilder;

        /// <summary>
        /// The <see cref="IExpressionConverter"/> instance.
        /// </summary>
        private readonly IExpressionConverter expressionConverter;

        /// <summary>
        /// The <see cref="ITaskDataContext"/> instance.
        /// </summary>
        private readonly ITaskDataContext taskDataContext;

        /// <summary>
        /// The IServerManager instance.
        /// </summary>
        private readonly IServerManager serverManager;

        /// <summary>
        /// The <see cref="IQueueManager"/> instance.
        /// </summary>
        private readonly IQueueManager queueManager;

        /// <summary>
        /// List of active tasks.
        /// </summary>
        private readonly Dictionary<Int64, IRunningTask> activeTasks;

        private int maxRerunCount;
        private TimeSpan timeShiftAfterCrash;

        private int maxTasksPerQueue;
        private TaskManagerState taskManagerState;

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
        public TaskManager(TaskManagerConfiguration taskManagerConfiguration, ITaskBuilder taskBuilder, IExpressionConverter expressionConverter, ITaskDataContext taskDataContext, IServerManager serverManager, IQueueManager queueManager, ICommonLoggerFactory loggerFactory)
            : base("Task Manager", loggerFactory)
        {
            if (taskManagerConfiguration == null)
                throw new ArgumentNullException(nameof(taskManagerConfiguration));
            if (taskBuilder == null)
                throw new ArgumentNullException(nameof(taskBuilder));
            if (expressionConverter == null) 
                throw new ArgumentNullException("expressionConverter");
            if (taskDataContext == null)
                throw new ArgumentNullException("taskDataContext");
            if (serverManager == null) 
                throw new ArgumentNullException("serverManager");
            if (queueManager == null)
                throw new ArgumentNullException("queueManager");

            this.taskBuilder = taskBuilder;
            this.expressionConverter = expressionConverter;
            this.taskDataContext = taskDataContext;
            this.serverManager = serverManager;
            this.queueManager = queueManager;

            activeTasks = new Dictionary<Int64, IRunningTask>();

            maxRerunCount = taskManagerConfiguration.MaxRerunAttempts;
            maxTasksPerQueue = taskManagerConfiguration.MaxTaskPerQueue;
            timeShiftAfterCrash = taskManagerConfiguration.TimeShiftAfterCrash;
            taskManagerState = taskManagerConfiguration.State;
        }

        #region Implementation of ITaskManager

        /// <summary>
        /// StartTask a new <c>task</c>.
        /// </summary>
        /// <param name="task">The <c>task</c>.</param>
        /// <returns>The task's identifier.</returns>
        public Int64 StartTask(Expression<Action> task) //TODO: (task, queue)
        {
            if (taskManagerState == TaskManagerState.ServerOnly)
                throw new InvalidOperationException("taskManagerState == TaskManagerState.ServerOnly");

            logger.WriteDebug("Preparing a new task... Getting a default queue...");

            QueueModel defaultQueue = queueManager.GetDefaultQueue();

            logger.WriteTrace("The default queue ({0}) has been got. Converting expression into the task model...");

            ActivationData activationData = expressionConverter.Convert(task);
            ScheduleInformation schedule = new ScheduleInformation(maxRerunCount);
            TaskModel taskModel = new TaskModel(defaultQueue.Id, activationData, schedule);

            logger.WriteTrace("The expression has been converted. Inserting the task into the database...");

            Int64 taskId = taskDataContext.InsertTask(taskModel);

            logger.WriteDebug("The task has been inserted. Task identifier = {0}", taskId);

            return taskId;
        }

        public void StopTask(Int64 taskId, bool waitResponse = false)
        {
            if (taskManagerState == TaskManagerState.ServerOnly)
                throw new InvalidOperationException("taskManagerState == TaskManagerState.ServerOnly");

            logger.WriteDebug("Preparing for stopping task... Updating the task in the database...");

            taskDataContext.StopTask(taskId);

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
            if (taskManagerState == TaskManagerState.ClientOnly)
            {
                logger.WriteDebug("The server has client algorithm only.");
                return;
            }

            logger.WriteDebug("Searching for new tasks in available queue...");

            IReadOnlyCollection<QueueModel> queues = queueManager.GetAvailableQueues();
            foreach (QueueModel queueModel in queues)
            {
                Int64 queueId = queueModel.Id;
                Int64 serverId = serverManager.ServerId;
                TaskRequestSpecification specification = new TaskRequestSpecification(queueId, serverId, maxTasksPerQueue);
                List<TaskModel> taskModels;

                bool isReceived = taskDataContext.TryGetAvailableTask(specification, out taskModels);

                if (isReceived)
                {
                    foreach (TaskModel taskModel in taskModels)
                    {
                        IRunningTask currentTask = null;
                        try
                        {
                            currentTask = taskBuilder.Build(taskModel);
                            
                            lock (activeTasks)
                            {
                                activeTasks.Add(taskModel.Id, currentTask);
                            }

                            currentTask.OnCompletedEventHandler += OnCompletedEventHandler;
                            currentTask.Execute(); //TODO: Provide global Cancelation Token.                                                            

                            logger.WriteDebug("The task {0} has been received.", taskModel);
                        }
                        catch (Exception ex)
                        {
                            string msg = string.Format("There was an error during executing task: {0}.", taskModel);
                            logger.WriteError(ex, msg);
                            RestartTask(taskModel, msg, ex);
                        }
                    }
                }
                else
                {
                    logger.WriteDebug("There is no new task in the storage.");
                }

                List<Int64> taskIds;
                TaskRequestSpecification cancelationSpecification = new TaskRequestSpecification(queueId, serverId);
                isReceived = taskDataContext.TryGetCancelTasks(cancelationSpecification, out taskIds);
                if (isReceived)
                {
                    foreach (Int64 taskId in taskIds)
                    {
                        logger.WriteTrace("The task (id = {0}) has been marked for cancelation. Cancelling...", taskId);

                        IRunningTask runningTask;
                        lock (activeTasks)
                        {
                            if (activeTasks.ContainsKey(taskId) == false)
                                continue;
                            runningTask = activeTasks[taskId];
                        }

                        logger.WriteTrace("The task ({0}) is running. Cancelling...", taskId);
                        try
                        {
                            runningTask.Cancel();
                        }
                        catch (Exception ex)
                        {
                            //TODO: Decide what to do.
                            logger.WriteError("there was an exception during cancelation task.", ex);
                        }
                                               
                        logger.WriteTrace("The task has been marked for canceling.");
                    }
                }
                else
                {
                    logger.WriteDebug("There is no task for cancelation.");
                }
            }

            logger.WriteDebug("All tasks have been found if existed.");
        }

        private void OnCompletedEventHandler(object sender, RunningTaskEventArg eventArg)
        {
            IRunningTask currentTask = (IRunningTask) sender;
            currentTask.OnCompletedEventHandler -= OnCompletedEventHandler;

            TaskStates taskState;
            Int64 taskId = currentTask.TaskModel.Id;

            switch (eventArg)
            {
                case RunningTaskEventArg.Completed:
                    logger.WriteTrace("The task has been completed: {0}", taskId);
                    taskState = TaskStates.Successed;
                    break;
                case RunningTaskEventArg.Cancelled:
                    logger.WriteTrace("The task has been canceled: {0}", taskId);
                    taskState = TaskStates.Cancelled;
                    break;
                case RunningTaskEventArg.Failed:
                    logger.WriteTrace("The task has been failed: {0}", taskId);
                    taskState = TaskStates.Failed;
                    break;
                default:
                    throw new NotImplementedException();
            }
            
            if (taskState == TaskStates.Successed || taskState == TaskStates.Cancelled)
            {
                lock (activeTasks)
                {
                    if (activeTasks.ContainsKey(taskId) == false)
                        return;
                    activeTasks.Remove(taskId);
                }

                TaskStateModel taskStateModel = new TaskStateModel(taskId, currentTask.TaskModel.QueueId, currentTask.TaskModel.ServerId, taskState);
                taskDataContext.SetTaskState(taskStateModel);
            }
            else
            {
                //TODO: correct message and details.
                RestartTask(currentTask.TaskModel, "Exception inside", new Exception("TODO"));
            }
        }

        private void RestartTask(TaskModel taskModel, string msg, Exception ex)
        {
            int repeatCrashCount = taskModel.ScheduleInformation.RepeatCrashCount - 1;
            //if (repeatCrashCount <= 1)
            //{
            //    logger.WriteTrace("All restart attempts have been expired. Task will be mark as failed.");
            //    //TODO: Set RepeatCrashCount to 0
            //    //TODO: Add message why task has been failed.
            //    TaskStateModel taskStateModel = new TaskStateModel(taskModel.Id, taskModel.QueueId, taskModel.ServerId, TaskStates.Failed);
            //    taskDataContext.SetTaskState(taskStateModel);
            //}
            //else
            //{
            TaskStateModel taskStateModel;
            if (repeatCrashCount > 0)
            {
                logger.WriteTrace("Task will be restarted.");
                taskStateModel = new TaskStateModel(taskModel.Id, taskModel.QueueId, taskModel.ServerId, TaskStates.New);
            }
            else
            {
                logger.WriteTrace("Task will be failed.");
                taskStateModel = new TaskStateModel(taskModel.Id, taskModel.QueueId, taskModel.ServerId, TaskStates.Failed);
            }
            taskDataContext.RestartTask(taskStateModel, timeShiftAfterCrash, repeatCrashCount, msg, ex);
            //}
        }

        #endregion
    }
}
