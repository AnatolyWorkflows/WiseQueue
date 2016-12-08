using System;
using System.Collections.Generic;
using Common.Core.Logging;
using WiseQueue.Core.Common.Converters;
using WiseQueue.Core.Common.DataContexts;
using WiseQueue.Core.Common.Entities.Tasks;
using WiseQueue.Core.Common.Management.Implementation;
using WiseQueue.Core.Common.Management.TaskManagment;
using WiseQueue.Core.Common.Models;
using WiseQueue.Core.Common.Models.Tasks;
using WiseQueue.Core.Common.Specifications;
using WiseQueue.Core.Server.Management;

namespace WiseQueue.Domain.Server.Management.Tasks
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
        /// List of active tasks.
        /// </summary>
        private readonly Dictionary<Int64, IRunningTask> activeTasks;

        private int maxRerunCount;
        private TimeSpan timeShiftAfterCrash;

        private int maxTasksPerQueue;

        private Int64 serverId;

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="expressionConverter">The <see cref="IExpressionConverter"/> instance.</param>
        /// <param name="taskDataContext">The <see cref="ITaskDataContext"/> instance.</param>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="expressionConverter"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="taskDataContext"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="serverManager"/> is <see langword="null" />.</exception>
        public TaskManager(TaskManagerConfiguration taskManagerConfiguration, ITaskBuilder taskBuilder, IExpressionConverter expressionConverter, ITaskDataContext taskDataContext, IServerManager serverManager, ICommonLoggerFactory loggerFactory)
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

            this.taskBuilder = taskBuilder;
            this.expressionConverter = expressionConverter;
            this.taskDataContext = taskDataContext;
            this.serverManager = serverManager;

            activeTasks = new Dictionary<Int64, IRunningTask>();

            maxRerunCount = taskManagerConfiguration.MaxRerunAttempts;
            maxTasksPerQueue = taskManagerConfiguration.MaxTaskPerQueue;
            timeShiftAfterCrash = taskManagerConfiguration.TimeShiftAfterCrash;
        }

        #region Implementation of IExecutable

        /// <summary>
        /// Occurs when object should do its action.
        /// </summary>
        public void Execute()
        {
            logger.WriteDebug("Searching for new tasks in available queue...");

            IReadOnlyCollection<QueueModel> queues = serverManager.GetAvailableQueues();
            foreach (QueueModel queueModel in queues)
            {
                string queueName = queueModel.Name;
                TaskRequestSpecification specification = new TaskRequestSpecification(queueName, serverId, maxTasksPerQueue);
                List<TaskModel> taskModels;

                bool isReceived = taskDataContext.TryGetAvailableTask(specification, out taskModels);

                if (isReceived)
                {
                    foreach (TaskModel taskModel in taskModels)
                    {
                        try
                        {
                            IRunningTask currentTask = taskBuilder.Build(taskModel);
                            
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
                            RestartTask(serverId, taskModel, msg, ex);
                        }
                    }
                }
                else
                {
                    logger.WriteDebug("There is no new task in the storage.");
                }

                List<Int64> taskIds;
                TaskRequestSpecification cancelationSpecification = new TaskRequestSpecification(queueName, serverId);
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

        /// <summary>
        /// Set on-line status.
        /// </summary>
        /// <param name="isOnline">Flag shows that server is only or not.</param>
        /// <param name="newServerId">If server is on-line that here should be server identifier. Otherwise, the parameter will be ignored</param>
        public void SetOnlineStatus(bool isOnline, Int64 newServerId = 0)
        {
            if (isOnline)
                serverId = newServerId;
            else
                serverId = 0;
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

            try
            {
                if (taskState == TaskStates.Successed || taskState == TaskStates.Cancelled)
                {
                    TaskStateChangeModel taskStateChangeModel = new TaskStateChangeModel(taskId, serverId, taskState);
                    taskDataContext.SetTaskState(taskStateChangeModel);
                }
                else
                {
                    //TODO: correct message and details.
                    RestartTask(serverId, currentTask.TaskModel, "Exception inside", new Exception("TODO"));
                }

                lock (activeTasks)
                {
                    if (activeTasks.ContainsKey(taskId) == false)
                        return;
                    activeTasks.Remove(taskId);
                }
            }
            catch (Exception ex)
            {
                logger.WriteError(ex, "The task hasn't been mark as {0} => Put it into the shadowTaskList.");
                //TODO: Put task into the shadowList => it will deleted later.
            }
        }

        private void RestartTask(Int64 taskServerId, TaskModel taskModel, string msg, Exception ex)
        {
            int repeatCrashCount = taskModel.ScheduleInformation.RepeatCrashCount - 1;

            TaskStateChangeModel taskStateChangeModel;
            if (repeatCrashCount > 0)
            {
                logger.WriteTrace("Task will be restarted.");
                taskStateChangeModel = new TaskStateChangeModel(taskModel.Id, taskServerId, TaskStates.New);
                taskDataContext.RestartTask(taskStateChangeModel, timeShiftAfterCrash, msg, ex);
            }
            else
            {
                logger.WriteTrace("Task will be failed.");
                taskStateChangeModel = new TaskStateChangeModel(taskModel.Id, taskServerId, TaskStates.Failed);
                taskDataContext.SetTaskState(taskStateChangeModel);
            }
        }

        #endregion
    }
}
