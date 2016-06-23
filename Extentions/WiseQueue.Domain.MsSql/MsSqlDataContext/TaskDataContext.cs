using System;
using System.Data;
using System.Text;
using WiseQueue.Core.Common.Converters.EntityModelConverters;
using WiseQueue.Core.Common.DataContexts;
using WiseQueue.Core.Common.Entities.Tasks;
using WiseQueue.Core.Common.Logging;
using WiseQueue.Core.Common.Models;
using WiseQueue.Core.Common.Models.Tasks;
using WiseQueue.Core.Common.Specifications;
using WiseQueue.Domain.MsSql.Utils;
using WiseQueue.Domain.MsSql.Utils.Implementation;

namespace WiseQueue.Domain.MsSql.MsSqlDataContext
{
    /// <summary>
    /// SQL data context that will be used for working with tasks.
    /// </summary>
    public class TaskDataContext : BaseLoggerObject, ITaskDataContext
    {
        #region Consts...

        /// <summary>
        /// Table's name where all tasks are stored.
        /// </summary>
        private const string taskTableName = "Tasks"; //TODO: move to settings.

        /// <summary>
        /// Insert statement.
        /// </summary>
        private const string insertStatement =
                "INSERT INTO {0}.{1} ([State], [InstanceType], [Method], [ParametersTypes], [Arguments], [QueueId]) VALUES " +
                                       "({2},     '{3}',         '{4}',         '{5}',          '{6}',      {7}); " +
                "SELECT CAST(scope_identity() AS bigint)";

        #endregion

        #region Fields...

        /// <summary>
        /// Sql settings.
        /// </summary>
        private readonly MsSqlSettings sqlSettings;

        /// <summary>
        /// Task converter.
        /// </summary>
        private readonly ITaskConverter taskConverter;

        /// <summary>
        /// The <see cref="ISqlConnectionFactory"/> instance.
        /// </summary>
        private readonly ISqlConnectionFactory connectionFactory;

        #endregion

        #region Constructors...

        /// <summary>
        /// Constructor.
        /// </summary>        
        /// <param name="sqlSettings">Sql settings.</param>
        /// <param name="taskConverter">The <see cref="ITaskConverter"/> instance.</param>
        /// <param name="connectionFactory">The <see cref="ISqlConnectionFactory"/> instance.</param>
        /// <param name="loggerFactory">The <see cref="IWiseQueueLoggerFactory"/> instance.</param>     
        /// <exception cref="ArgumentNullException"><paramref name="sqlSettings"/> is <see langword="null" />.</exception>   
        /// <exception cref="ArgumentNullException"><paramref name="taskConverter"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="connectionFactory"/> is <see langword="null" />.</exception>
        public TaskDataContext(MsSqlSettings sqlSettings, ITaskConverter taskConverter, ISqlConnectionFactory connectionFactory, IWiseQueueLoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            if (sqlSettings == null)
                throw new ArgumentNullException("sqlSettings");
            if (taskConverter == null) 
                throw new ArgumentNullException("taskConverter");
            if (connectionFactory == null)
                throw new ArgumentNullException("connectionFactory");

            this.sqlSettings = sqlSettings;
            this.taskConverter = taskConverter;
            this.connectionFactory = connectionFactory;            
        }

        #endregion

        #region Implementation of ITaskDataContext

        /// <summary>
        /// Insert a task into the storage.
        /// </summary>
        /// <param name="taskModel">The <see cref="TaskModel"/> instance.</param>
        /// <returns>The task's identifier.</returns>
        /// <exception cref="ArgumentException">Task's identifier should be 0.</exception>
        public Int64 InsertTask(TaskModel taskModel)
        {
            logger.WriteTrace("Inserting {0} into the database...", taskModel);

            if (taskModel.Id != 0)
                throw new ArgumentException("Task's identifier should be 0. Now it is " + taskModel.Id, "taskModel");

            logger.WriteTrace("Converting {0} into the TaskEntity...", taskModel);
            TaskEntity taskEntity = taskConverter.Convert(taskModel);
            logger.WriteTrace("{0} has been converted. Generating sql command for {1}...", taskModel, taskEntity);

            string instanceType = taskEntity.InstanceType;
            string method = taskEntity.Method;
            string parametersTypes = taskEntity.ParametersTypes;
            string arguments = taskEntity.Arguments;
            Int64 queueId = taskEntity.QueueId;
            string sqlCommand = string.Format(insertStatement, sqlSettings.WiseQueueDefaultSchema, taskTableName, (short)taskEntity.TaskState, instanceType, method, parametersTypes, arguments, queueId);

            logger.WriteTrace("The SqlCommand has been generated. Result: {0}", sqlCommand);

            logger.WriteTrace("Executing sql command...");
            using (IDbConnection connection = connectionFactory.CreateConnection())
            {
                using (IDbCommand command = connectionFactory.CreateCommand(connection))
                {
                    command.CommandText = sqlCommand;
                    Int64 taskId = (Int64)command.ExecuteScalar();

                    logger.WriteTrace("The command has been executed. TaskId = {0}", taskId);
                    return taskId;
                }
            }            
        }

        /// <summary>
        /// Get available task from the storage.
        /// </summary>
        /// <param name="specification">The <see cref="TaskRequestSpecification"/> instance.</param>
        /// <param name="taskModel">The <see cref="TaskModel"/> instance if it has been found. Otherwise, null.</param>
        /// <returns>True if the TaskModel instance has been populated. Otherwise, false.</returns>
        public bool TryGetAvailableTask(TaskRequestSpecification specification, out TaskModel taskModel)
        {
            taskModel = null;

            if (specification == null)
                throw new ArgumentNullException("specification");

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Declare @TempTable table ([Id] [bigint], ");
            stringBuilder.Append("[QueueId] [bigint], ");
            stringBuilder.Append("[ServerId] [bigint] NULL, ");
            stringBuilder.Append("[State] [smallint], ");
            stringBuilder.Append("[CompletedAt] [datetime] NULL, ");
            stringBuilder.Append("[InstanceType] [nvarchar](200), ");
            stringBuilder.Append("[Method] [nvarchar](200), ");
            stringBuilder.Append("[ParametersTypes] [nvarchar](500), ");
            stringBuilder.AppendLine("[Arguments] [nvarchar](1000)); ");

            stringBuilder.AppendFormat("UPDATE TOP (1) {0}.{1} ", sqlSettings.WiseQueueDefaultSchema, taskTableName);
            stringBuilder.AppendFormat("SET State = {0}, ", (short)TaskStates.Pending);
            stringBuilder.AppendFormat("ServerId = {0} ", specification.ServerId);
            stringBuilder.Append("OUTPUT inserted.* INTO @TempTable ");
            stringBuilder.AppendFormat("Where (State = {0} ", (short)TaskStates.New);
            stringBuilder.AppendFormat("OR ( (State = {0} OR State = {1}) AND [ServerId] IS NULL)) ", (short)TaskStates.Pending, (short)TaskStates.Running);
            stringBuilder.AppendFormat("AND (QueueId = {0});", specification.QueueId);

            stringBuilder.AppendLine();
            stringBuilder.AppendLine("SELECT * FROM @TempTable");

            using (IDbConnection connection = connectionFactory.CreateConnection())
            {                
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    using (IDbCommand command = connectionFactory.CreateCommand(connection, transaction))
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
                                TaskEntity taskEntity = new TaskEntity
                                {
                                    Id = id,
                                    QueueId = queueId,
                                    TaskState = taskState,
                                    InstanceType = typeDetails,
                                    Method = methodDetails,
                                    ParametersTypes = parameterDetails,
                                    Arguments = argumentDetails
                                };

                                taskModel = taskConverter.Convert(taskEntity);                                
                            }
                        }
                    }
                    if (taskModel != null)
                    {
                        transaction.Commit();
                        return true;
                    }
                }
                
                return false;
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
            string sqlCommand = string.Format(updateStatement, sqlSettings.WiseQueueDefaultSchema, taskTableName, (short)taskState, DateTime.UtcNow.ToString("s"), id);

            using (IDbConnection connection = connectionFactory.CreateConnection())
            {
                using (IDbCommand command = connectionFactory.CreateCommand(connection))
                {
                    command.CommandText = sqlCommand;
                    command.ExecuteNonQuery();
                }
            }
        }


        #endregion
    }
}
