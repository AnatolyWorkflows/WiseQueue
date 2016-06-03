using System;
using WiseQueue.Core.Common.DataContexts;
using WiseQueue.Core.Common.Entities;
using WiseQueue.Core.Common.Logging;
using WiseQueue.Core.Common.Models;

namespace WiseQueue.Domain.MsSql.MsSqlDataContext
{
    /// <summary>
    /// SQL data context that will be used for working with tasks.
    /// </summary>
    public class SqlTaskDataContext : BaseLoggerObject, ITaskDataContext
    {
        /// <summary>
        /// The <see cref="ISqlConnectionFactory"/> instance.
        /// </summary>
        private readonly ISqlConnectionFactory connectionFactory;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionFactory">The <see cref="ISqlConnectionFactory"/> instance.</param>
        /// <param name="loggerFactory">The <see cref="IWiseQueueLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionFactory"/> is <see langword="null" />.</exception>
        public SqlTaskDataContext(ISqlConnectionFactory connectionFactory, IWiseQueueLoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            if (connectionFactory == null)
                throw new ArgumentNullException("connectionFactory");

            this.connectionFactory = connectionFactory;
        }

        #region Implementation of ITaskDataContext

        /// <summary>
        /// Insert a task into the storage.
        /// </summary>
        /// <param name="taskEntity">The <see cref="TaskEntity"/> instance.</param>
        /// <returns>The task's identifier.</returns>
        public long InsertTask(TaskEntity taskEntity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get available task from the storage.
        /// </summary>
        /// <returns>The <see cref="TaskEntity"/> instance if it has been found. Otherwise, null.</returns>
        public TaskEntity GetAvailableTask()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
