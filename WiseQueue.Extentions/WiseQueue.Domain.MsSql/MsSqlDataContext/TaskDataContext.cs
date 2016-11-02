using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Common.Core.BaseClasses;
using Common.Core.Logging;
using WiseQueue.Core.Common.Converters.EntityModelConverters;
using WiseQueue.Core.Common.DataContexts;
using WiseQueue.Core.Common.Entities.Tasks;
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
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>     
        /// <exception cref="ArgumentNullException"><paramref name="sqlSettings"/> is <see langword="null" />.</exception>   
        /// <exception cref="ArgumentNullException"><paramref name="taskConverter"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="connectionFactory"/> is <see langword="null" />.</exception>
        public TaskDataContext(MsSqlSettings sqlSettings, ITaskConverter taskConverter, ISqlConnectionFactory connectionFactory, ICommonLoggerFactory loggerFactory)
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
        /// Get available task from the storage.
        /// </summary>
        /// <param name="specification">The <see cref="TaskRequestSpecification"/> instance.</param>
        /// <param name="taskModels">List of <see cref="WiseQueue.Core.Common.Models.Tasks.TaskStateModel"/> instances if it has been found</param>
        /// <returns>True if the list of TaskModel instances has been populated. Otherwise, false.</returns>
        public bool TryGetAvailableTask(TaskRequestSpecification specification, out List<TaskModel> taskModels)
        {
            if (specification == null)
                throw new ArgumentNullException("specification");

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Declare @TempTable table ([Id] [bigint], ");
            stringBuilder.Append("[QueueId] [bigint], ");
            stringBuilder.Append("[ServerId] [bigint] NULL, ");
            stringBuilder.Append("[State] [smallint], ");
            stringBuilder.Append("[CompletedAt] [datetime] NULL, ");
            stringBuilder.Append("[ExecuteAt] [datetime] NOT NULL, ");
            stringBuilder.Append("[RepeatCrashCount] [int] NOT NULL, ");
            stringBuilder.Append("[InstanceType] [nvarchar](4000), ");
            stringBuilder.Append("[Method] [nvarchar](4000), ");
            stringBuilder.Append("[ParametersTypes] [nvarchar](4000), ");
            stringBuilder.AppendLine("[Arguments] [nvarchar](4000)); ");

            stringBuilder.AppendFormat("UPDATE TOP ({0}) {1}.{2} ", specification.MaxTasks, sqlSettings.WiseQueueDefaultSchema, taskTableName);
            stringBuilder.AppendFormat("SET State = {0}, ", (short)TaskStates.Pending);
            stringBuilder.AppendFormat("ServerId = {0} ", specification.ServerId);
            //stringBuilder.Append("RepeatCount = RepeatCount - 1 ");
            stringBuilder.Append("OUTPUT inserted.* INTO @TempTable ");
            stringBuilder.AppendFormat("Where (State = {0} ", (short)TaskStates.New);
            stringBuilder.AppendFormat("OR ( (State = {0} OR State = {1}) AND [ServerId] IS NULL)) ", (short)TaskStates.Pending, (short)TaskStates.Running);
            stringBuilder.AppendFormat("AND (QueueId = {0}) AND ([ExecuteAt] <= GETUTCDATE()) AND [RepeatCrashCount] > 0;", specification.QueueId);

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
                            taskModels = new List<TaskModel>();
                            while (rdr.Read())
                            {
                                Int64 id = (Int64)rdr["Id"];
                                Int64 queueId = (Int64)rdr["QueueId"];
                                Int64 serverId = (Int64)rdr["ServerId"];
                                TaskStates taskState = (TaskStates)(short)rdr["State"];

                                int repeatCrashCount = (int)rdr["RepeatCrashCount"];

                                string typeDetails = (string)rdr["InstanceType"];
                                string methodDetails = (string)rdr["Method"];
                                string parameterDetails = (string)rdr["ParametersTypes"];
                                string argumentDetails = (string)rdr["Arguments"];
                                TaskEntity taskEntity = new TaskEntity
                                {
                                    Id = id,
                                    QueueId = queueId,
                                    ServerId = serverId,
                                    TaskState = taskState,
                                    InstanceType = typeDetails,
                                    Method = methodDetails,
                                    ParametersTypes = parameterDetails,
                                    Arguments = argumentDetails,
                                    RepeatCrashCount = repeatCrashCount
                                };

                                TaskModel taskModel = taskConverter.Convert(taskEntity);
                                taskModels.Add(taskModel);
                            }
                        }
                    }
                    if (taskModels.Count > 0)
                    {
                        transaction.Commit();
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Get task that has been marked for cancellation.
        /// </summary>
        /// <param name="specification">The <see cref="TaskRequestSpecification"/> instance.</param>
        /// <param name="taskIds">List of tasks' identifiers that have been canceled.</param>
        /// <returns>True if there is at minimum one task that has been marked for cancel.</returns>
        public bool TryGetCancelTasks(TaskRequestSpecification specification, out List<Int64> taskIds)
        {
            const string selectStatement = "select [Id] from {0}.{1} where [QueueId] = {2} and [ServerId] = {3} and [State] = {4}";

            string sqlCommand = string.Format(selectStatement, sqlSettings.WiseQueueDefaultSchema, taskTableName, specification.QueueId, specification.ServerId, (int)TaskStates.Cancel);

            using (IDbConnection connection = connectionFactory.CreateConnection())
            {
                using (IDbCommand command = connectionFactory.CreateCommand(connection))
                {
                    command.CommandText = sqlCommand;
                    using (IDataReader rdr = command.ExecuteReader())
                    {
                        taskIds = new List<Int64>();
                        while (rdr.Read())
                        {
                            Int64 id = (Int64)rdr["Id"];
                            taskIds.Add(id);
                        }
                    }
                }
            }

            return taskIds.Count > 0;
        }

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
        /// Set task's state.
        /// </summary>
        /// <param name="taskStateModel">The <see cref="WiseQueue.Core.Common.Models.Tasks.TaskStateModel"/> instance.</param>
        public void SetTaskState(TaskStateModel taskStateModel)
        {
            const string updateStatement = "UPDATE {0}.{1} SET [State]={2}, [CompletedAt]=GETUTCDATE() WHERE [Id] = {3} AND [QueueId] = {4} AND [ServerId] = {5}";

            string sqlCommand = string.Format(updateStatement, sqlSettings.WiseQueueDefaultSchema, taskTableName, (short)taskStateModel.TaskState, taskStateModel.Id, taskStateModel.QueueId, taskStateModel.ServerId);

            using (IDbConnection connection = connectionFactory.CreateConnection())
            {
                //TODO: Transaction ???
                using (IDbCommand command = connectionFactory.CreateCommand(connection))
                {
                    command.CommandText = sqlCommand;
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Try to restart the task.
        /// </summary>
        /// <param name="taskStateModel">The <see cref="TaskStateModel"/> instance.</param>
        /// <param name="timeShift">Time shift.</param>
        /// <param name="repeatCrashCount">Count of attempts that will be used for reruning this task after its crashed.</param>
        /// <param name="msg">Message that explains the restart.</param>
        /// <param name="exception">Exception that explains the restart.</param>
        public void RestartTask(TaskStateModel taskStateModel, TimeSpan timeShift, int repeatCrashCount, string msg, Exception exception)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("UPDATE {0}.{1}", sqlSettings.WiseQueueDefaultSchema, taskTableName);
            if (repeatCrashCount == 0)
                stringBuilder.AppendFormat("SET [ExecuteAt]=GETUTCDATE(), [RepeatCrashCount]=0, [State] = {0}", (short)TaskStates.Failed);
            else
                stringBuilder.AppendFormat("SET [ExecuteAt]=GETUTCDATE() +'{0}', [RepeatCrashCount]={1}, [ServerId] = NULL, [State] = {2}", timeShift, repeatCrashCount, (short)TaskStates.New);

            stringBuilder.AppendFormat("WHERE [Id] = {0} AND [QueueId] = {1} AND [ServerId] = {2}", taskStateModel.Id, taskStateModel.QueueId, taskStateModel.ServerId);

            string sqlCommand = stringBuilder.ToString();

            using (IDbConnection connection = connectionFactory.CreateConnection())
            {
                //TODO: Transaction ???
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
