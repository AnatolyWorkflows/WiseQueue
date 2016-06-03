using System;
using System.Data;
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
        /// <exception cref="ArgumentException">Task's identifier should be 0.</exception>
        public Int64 InsertTask(TaskEntity taskEntity)
        {
            if (taskEntity.Id != 0)
                throw new ArgumentException("Task's identifier should be 0. Now it is " + taskEntity.Id, "taskEntity");

            const string insertStatement =
                "INSERT INTO dbo.Tasks ([State], [InstanceType], [Method], [ParametersTypes], [Arguments], [QueueId]) VALUES " +
                                       "({0},     '{1}',         '{2}',         '{3}',          '{4}',      {5}); " +
                "SELECT CAST(scope_identity() AS bigint)";

            string instanceType = taskEntity.TaskActivationDetails.InstanceType;
            string method = taskEntity.TaskActivationDetails.Method;
            string parametersTypes = taskEntity.TaskActivationDetails.ParametersTypes;
            string arguments = taskEntity.TaskActivationDetails.Arguments;
            Int64 queueId = taskEntity.QueueId;
            string sqlCommand = string.Format(insertStatement, (short)taskEntity.TaskState, instanceType, method, parametersTypes, arguments, queueId);

            using (IDbConnection connection = connectionFactory.CreateConnection())
            {
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sqlCommand;
                    Int64 jobId = (Int64)command.ExecuteScalar();
                    return jobId;
                }
            }
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
