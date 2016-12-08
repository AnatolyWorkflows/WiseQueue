using System;
using System.Reflection;
using Common.Core.BaseClasses;
using Common.Core.Logging;
using WiseQueue.Core.Common.Management.TaskManagment;
using WiseQueue.Domain.Common.Models.Tasks;

namespace WiseQueue.Domain.Server.Management.Tasks
{
    public class TaskBuilder: BaseLoggerObject, ITaskBuilder
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        public TaskBuilder(ICommonLoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        #region Implementation of ITaskBuilder

        public IRunningTask Build(TaskModel taskModel)
        {
            ActivationData activationData = taskModel.ActivationData;

            //TODO: Activate and run task (Smart logic)
            var instance = Activator.CreateInstance(activationData.InstanceType);
            MethodInfo method = activationData.Method;

            IRunningTask runningTask = new RunningTask(instance, taskModel, logger);

            return runningTask;
        }

        #endregion
    }
}
