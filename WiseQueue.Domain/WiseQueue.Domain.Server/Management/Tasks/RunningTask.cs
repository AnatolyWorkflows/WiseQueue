using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Common.Core.BaseClasses;
using Common.Core.Logging;
using WiseQueue.Core.Common.Management.TaskManagment;
using WiseQueue.Domain.Common.Models.Tasks;

namespace WiseQueue.Domain.Server.Management.Tasks
{
    class RunningTask: BaseLoggerObject, IRunningTask
    {
        private readonly object instance;
        private readonly MethodInfo method;
        private readonly Type[] argumentTypes;
        private readonly object[] arguments;

        private CancellationTokenSource taskCancelTokenSource;

        public TaskModel TaskModel { get; private set; }

        public EventHandler<RunningTaskEventArg> OnCompletedEventHandler { get; set; }

        public RunningTask(object instance, TaskModel taskModel, ICommonLogger logger): base(logger)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            if (taskModel == null)
                throw new ArgumentNullException(nameof(taskModel));
            if (taskModel.ActivationData == null)
                throw new ArgumentException("taskModel.ActivationData == null", nameof(taskModel));

            this.instance = instance;
            TaskModel = taskModel;

            method = taskModel.ActivationData.Method;
            argumentTypes = taskModel.ActivationData.ArgumentTypes;
            arguments = taskModel.ActivationData.Arguments;

            taskCancelTokenSource = new CancellationTokenSource();
        }

        #region Implementation of IRunningTask

        /// <summary>
        /// Execute task.
        /// </summary>
        public void Execute()
        {
            CancellationToken taskCancellationToken = taskCancelTokenSource.Token;

            try
            {
                Task.Run(() =>
                {
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
                        method.Invoke(instance, arguments);
                        if (OnCompletedEventHandler != null)
                            OnCompletedEventHandler(this, RunningTaskEventArg.Completed);
                    }
                    catch (OperationCanceledException canceledException)
                    {
                        string msg = string.Format(
                            "The task has been canceled via OperationCanceledException: {0}.{1}.",
                            instance.GetType().FullName, method.Name);
                        logger.WriteError(msg, canceledException);
                        if (OnCompletedEventHandler != null)
                            OnCompletedEventHandler(this, RunningTaskEventArg.Cancelled);
                    }
                    catch (Exception ex)
                    {
                        string msg = string.Format("There was an error during executing task: {0}.{1}.",
                            instance.GetType().FullName, method.Name);
                        logger.WriteError(msg, ex);
                        if (OnCompletedEventHandler != null)
                            OnCompletedEventHandler(this, RunningTaskEventArg.Failed);
                    }
                }, taskCancellationToken);
            }
            catch (Exception ex)
            {
                string msg = string.Format("There was an error during executing task: {0}.{1}.",
                            instance.GetType().FullName, method.Name);
                logger.WriteError(msg, ex);
                if (OnCompletedEventHandler != null)
                    OnCompletedEventHandler(this, RunningTaskEventArg.Failed);
            }
        }

        /// <summary>
        /// Cancel task execution.
        /// </summary>
        public void Cancel()
        {
            taskCancelTokenSource.Cancel();
        }

        #endregion
    }
}
