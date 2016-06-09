using System;
using System.Data;
using System.Text;
using WiseQueue.Core.Common.DataContexts;
using WiseQueue.Core.Common.Entities;
using WiseQueue.Core.Common.Logging;
using WiseQueue.Core.Common.Models;
using WiseQueue.Core.Common.Specifications;
using WiseQueue.Domain.MsSql.MsSqlDataContext.QueryObjects;

namespace WiseQueue.Domain.MsSql.MsSqlDataContext
{
    /// <summary>
    /// SQL data context that will be used for working with tasks.
    /// </summary>
    public class SqlTaskDataContext : BaseLoggerObject, ITaskDataContext
    {
        #region Consts...

        /// <summary>
        /// Database schema.
        /// </summary>
        private const string schemaName = "dbo"; //TODO: move to settings.

        /// <summary>
        /// Table's name where all tasks are stored.
        /// </summary>
        private const string taskTableName = "Tasks"; //TODO: move to settings.

        #endregion

        #region Fields...

        /// <summary>
        /// The <see cref="ISqlConnectionFactory"/> instance.
        /// </summary>
        private readonly ISqlConnectionFactory connectionFactory;

        #endregion

        #region Constructors...

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

        #endregion

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

            //This command can be reused.
            InsertTaskQueryObject queryObject = new InsertTaskQueryObject(schemaName, taskTableName, logger);

            using (IDbConnection connection = connectionFactory.CreateConnection())
            {
                Int64 taskId = queryObject.Execute(taskEntity, connection);
                return taskId;
            }
        }


        /// <summary>
        /// Get available task from the storage.
        /// </summary>
        /// <param name="specification">The <see cref="TaskRequestSpecification"/> instance.</param>
        /// <returns>The <see cref="TaskEntity"/> instance if it has been found. Otherwise, null.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="specification"/> is <see langword="null" />.</exception>
        public TaskEntity GetAvailableTask(TaskRequestSpecification specification)
        {
            if (specification == null)
                throw new ArgumentNullException("specification");

           

            using (IDbConnection connection = connectionFactory.CreateConnection())
            {                
                connection.Open();
                using (connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = stringBuilder.ToString();
                        using (IDataReader rdr = command.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                Int64 id = (Int64) rdr["Id"];
                                Int64 queueId = (Int64) rdr["QueueId"];
                                TaskStates taskState = (TaskStates) (short) rdr["State"];

                                string typeDetails = (string) rdr["InstanceType"];
                                string methodDetails = (string) rdr["Method"];
                                string parameterDetails = (string) rdr["ParametersTypes"];
                                string argumentDetails = (string) rdr["Arguments"];
                                TaskActivationDetailsEntity taskActivationDetailsEntity =
                                    new TaskActivationDetailsEntity(typeDetails, methodDetails, parameterDetails,
                                        argumentDetails);

                                TaskEntity result = new TaskEntity(id, queueId, taskActivationDetailsEntity, taskState);
                                return result;
                            }
                        }
                    }
                }
                return null; //TODO: It is not a good idea. We should return something like NullTaskEntity.
            }
        }

        /// <summary>
        /// Set task's state.
        /// </summary>
        /// <param name="id">The task's identifier.</param>
        /// <param name="taskState">New task's state that we are going to set.</param>
        public void SetTaskState(Int64 id, TaskStates taskState)
        {
            const string updateStatement = "UPDATE {0}.{1} SET [State]={2}, [CompletedAt]='{3}' WHERE [Id] = {4}";

            string data = "TODO";// taskSerialization.Serialize(task.Action);
            string sqlCommand = string.Format(updateStatement, schemaName, taskTableName, (short)taskState, DateTime.UtcNow.ToString("s"), id);

            using (IDbConnection connection = connectionFactory.CreateConnection())
            {
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sqlCommand;
                    command.ExecuteNonQuery();
                }
            }
        }


        #endregion
    }
}
