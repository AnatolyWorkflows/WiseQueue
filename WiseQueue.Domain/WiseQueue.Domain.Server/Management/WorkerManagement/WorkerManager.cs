using System;
using Common.Core.Logging;
using WiseQueue.Core.Common.Management;
using WiseQueue.Core.Common.Management.Implementation;
using WiseQueue.Core.Common.Models.Tasks;
using WiseQueue.Core.Server.Factories;

namespace WiseQueue.Domain.Server.Management.WorkerManagement
{
    class WorkerManager: BaseManager, IWorkerManager
    {
        /// <summary>
        /// Worker factory.
        /// </summary>
        private readonly IWorkerFactory workerFactory;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Manager <c>name</c>.</param>
        /// <param name="workerFactory">Worker factory.</param>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="workerFactory"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">Value cannot be null or empty.</exception>
        public WorkerManager(string name, IWorkerFactory workerFactory, ICommonLoggerFactory loggerFactory) : base(name, loggerFactory)
        {
            if (workerFactory == null) 
                throw new ArgumentNullException("workerFactory");

            this.workerFactory = workerFactory;
        }

        #region Implementation of IWorkerManager

        /// <summary>
        /// Create a worker where the task will be executed.
        /// </summary>
        /// <param name="taskModel">The task.</param>
        public void StartWorker(TaskModel taskModel)
        {
            IWorker worker = workerFactory.CreateWorker(taskModel);
        }

        /// <summary>
        /// Stop a worker where the task is executing.
        /// </summary>
        /// <param name="taskModel">The task.</param>
        public void StopWorker(TaskModel taskModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Stop all workers.
        /// </summary>
        public void StopAll()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
