using System;
using Common.Core.BaseClasses;
using Common.Core.Logging;
using WiseQueue.Core.Common.Converters;
using WiseQueue.Core.Common.Models.Tasks;
using WiseQueue.Core.Server.Factories;

namespace WiseQueue.Domain.Server.Factories
{
    class WorkerFactory : BaseLoggerObject, IWorkerFactory
    {
        /// <summary>
        /// The <see cref="IExpressionConverter"/> instance.
        /// </summary>
        private readonly IExpressionConverter expressionConverter;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="expressionConverter">The <see cref="IExpressionConverter"/> instance.</param>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="expressionConverter"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        public WorkerFactory(IExpressionConverter expressionConverter, ICommonLoggerFactory loggerFactory) : base(loggerFactory)
        {
            if (expressionConverter == null)
                throw new ArgumentNullException("expressionConverter");

            this.expressionConverter = expressionConverter;
        }

        #region Implementation of IWorkerFactory

        /// <summary>
        /// Create a worker using the <see cref="TaskModel"/> instance.
        /// </summary>
        /// <param name="taskModel">Information that will be used for creating the worker.</param>
        /// <returns>The worker.</returns>
        public IWorker CreateWorker(TaskModel taskModel)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
