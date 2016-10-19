using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Common.Core.Logging;
using WiseQueue.Core.Common.Converters;
using WiseQueue.Core.Common.DataContexts;
using WiseQueue.Core.Common.Entities.Tasks;
using WiseQueue.Core.Common.Management;
using WiseQueue.Core.Common.Management.Implementation;
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
        private readonly Dictionary<Int64, TaskWrapper> activeTasks;

        private int maxRerunCount;
        private TimeSpan timeShiftAfterCrash;

        private int maxTasksPerQueue;

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="expressionConverter">The <see cref="IExpressionConverter"/> instance.</param>
        /// <param name="taskDataContext">The <see cref="ITaskDataContext"/> instance.</param>
        /// <param name="serverManager">The <see cref="IQueueManager"/> instance.</param>
        /// <param name="queueManager">The <see cref="IServerManager"/> instance.</param>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="taskManagerConfiguration"/> is <see langword="null" />.</exception>
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

            activeTasks = new Dictionary<long, TaskWrapper>();

            maxRerunCount = 3;
            maxTasksPerQueue = 4;
            timeShiftAfterCrash = TimeSpan.FromSeconds(10);
        }

        #region Implementation of ITaskManager

        /// <summary>
        /// StartTask a new <c>task</c>.
        /// </summary>
        /// <param name="task">The <c>task</c>.</param>
        /// <returns>The task's identifier.</returns>
        public Int64 StartTask(Expression<Action> task) //TODO: (task, queue)
        {
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
            logger.WriteDebug("Preparing for stopping task... Updating the task in the database...");

            taskDataContext.SetTaskState(taskId, TaskStates.Cancel);

            logger.WriteDebug("The task has been marked as cancelled. Task identifier = {0}", taskId);

            if (waitResponse)
            {
                //TODO: Wait response from the task if needed.
            }
        }

        /// <summary>
        /// Set configuration that will be used from the server side
        /// </summary>
        /// <param name="maxTasks">Max tasks that will be requested from the database in one queue.</param>
        /// <param name="maxRerunAttempts">Max re-run attempts that will be done if task has been crashed.</param>
        /// <param name="timeShift">Time that will be wait before next attempt.</param>
        public void SetServerConfiguration(int maxTasks, int maxRerunAttempts, TimeSpan timeShift)
        {
            maxTasksPerQueue = maxTasks;
            maxRerunCount = maxRerunAttempts;
            timeShiftAfterCrash = timeShift;
        }

        #endregion

        #region Implementation of IExecutable

        /// <summary>
        /// Occurs when object should do its action.
        /// </summary>
        public void Execute()
        {
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
                        ActivationData activationData = taskModel.ActivationData;
                        try
                        {
                            //TODO: Activate and run task (Smart logic)
                            var instance = Activator.CreateInstance(activationData.InstanceType);
                            MethodInfo method = activationData.Method;

                            CancellationTokenSource taskCancelTokenSource = new CancellationTokenSource();
                            CancellationToken taskCancellationToken = taskCancelTokenSource.Token;

                            Task task = Task.Run(() =>
                            {
                                Type[] argumentTypes = activationData.ArgumentTypes;
                                object[] arguments = activationData.Arguments;

                                for (int i = 0; i < argumentTypes.Length; i++)
                                {
                                    if (argumentTypes[i] == typeof(CancellationToken))
                                    {
                                        arguments[i] = taskCancellationToken;
                                        break;
                                    }
                                }
                                try
                                {
                                    method.Invoke(instance, activationData.Arguments);
                                }
                                catch (Exception ex)
                                {
                                    lock (activeTasks)
                                    {
                                        if (activeTasks.ContainsKey(taskModel.Id))
                                            activeTasks.Remove(taskModel.Id);
                                    }
                                    
                                    string msg = string.Format("There was an error during executing task: {0}.", activationData);
                                    logger.WriteError(msg, ex);

                                    RestartTask(taskModel, msg, ex);
                                }
                            }, taskCancellationToken);

                            TaskWrapper taskWrapper = new TaskWrapper(task, taskCancelTokenSource);
                            lock (activeTasks)
                            {
                                activeTasks.Add(taskModel.Id, taskWrapper);
                            }                            

                            logger.WriteDebug("The task {0} has been received.", taskModel);
                        }
                        catch (Exception ex)
                        {
                            lock (activeTasks)
                            {
                                if (activeTasks.ContainsKey(taskModel.Id))
                                    activeTasks.Remove(taskModel.Id);
                            }

                            string msg = string.Format("There was an error during executing task: {0}.", activationData);
                            RestartTask(taskModel, msg, ex);
                        }                        
                    }
                }
                else
                {
                    logger.WriteDebug("There is no new task in the storage.");
                }

                List<Int64> taskIds;
                isReceived = taskDataContext.TryGetCancelTasks(queueId, serverId, out taskIds);
                if (isReceived)
                {
                    foreach (Int64 taskId in taskIds)
                    {
                        logger.WriteTrace("The task (id = {0}) has been marked for cancelation. Cancelling...", taskId);

                        TaskWrapper taskWrapper;
                        lock (activeTasks)
                        {
                            if (activeTasks.ContainsKey(taskId) == false)
                                continue;
                            taskWrapper = activeTasks[taskId];
                            activeTasks.Remove(taskId);
                        }

                        logger.WriteTrace("The task ({0}) is running. Cancelling...", taskId);
                        taskDataContext.SetTaskState(taskId, TaskStates.Cancelling);

                        try
                        {
                            taskWrapper.TaskCancellationTokenSource.Cancel();
                        }
                        catch (Exception ex)
                        {
                            //TODO: Decide what to do.
                            logger.WriteError("there was an exception during cancelation task.", ex);

                        }

                        //TODO: Bulk update.
                        taskDataContext.SetTaskState(taskId, TaskStates.Cancelled);

                        logger.WriteTrace("The task has been canceled.");
                    }
                }
                else
                {
                    logger.WriteDebug("There is no task for cancelation.");
                }
            }

            logger.WriteDebug("All tasks have been found if existed.");
        }

        private void RestartTask(TaskModel taskModel, string msg, Exception ex)
        {
            int repeatCrashCount = taskModel.ScheduleInformation.RepeatCrashCount;
            if (repeatCrashCount <= 1)
            {
                logger.WriteTrace("All restart attempts have been expired. Task will be mark as failed.");
                //TODO: Set RepeatCrashCount to 0
                //TODO: Add message why task has been failed.
                taskDataContext.SetTaskState(taskModel.Id, TaskStates.Failed);
            }
            else
            {
                logger.WriteTrace("Task wull be restarting.");
                taskDataContext.RestartTask(taskModel.Id, timeShiftAfterCrash, taskModel.ScheduleInformation.RepeatCrashCount - 1, msg, ex);
            }
        }

        #endregion
    }
}
