using System;
using WiseQueue.Core.Common.DataContexts;
using WiseQueue.Core.Common.Entities;
using WiseQueue.Core.Common.Logging;
using WiseQueue.Core.Common.Models;

namespace WiseQueue.Domain.MsSql.MsSqlDataContext
{
    public class SqlWorkflowDataContext : BaseLoggerObject, ITaskDataContext
    {
        private readonly ISqlConnectionFactory connectionFactory;

        public SqlWorkflowDataContext(ISqlConnectionFactory connectionFactory, IWiseQueueLoggerFactory loggerFactory)
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
