using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

        #endregion

        #region Fields...

        /// <summary>
        /// Sql settings.
        /// </summary>
        private readonly MsSqlSettings sqlSettings;

        /// <summary>
        /// The <see cref="ISqlConnectionFactory"/> instance.
        /// </summary>
        private readonly ISqlConnectionFactory connectionFactory;

        private ITaskConverter taskConverter;

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
                throw new ArgumentNullException(nameof(taskConverter));
            if (connectionFactory == null)
                throw new ArgumentNullException("connectionFactory");

            this.sqlSettings = sqlSettings;
            this.connectionFactory = connectionFactory;
            this.taskConverter = taskConverter;
        }

        #endregion

        #region Implementation of ITaskDataContext

        /// <summary>
        /// Get available task from the storage.
        /// </summary>
        /// <param name="specification">The <see cref="TaskRequestSpecification"/> instance.</param>
        /// <param name="taskModels">List of <see cref="TaskStateChangeModel"/> instances if it has been found</param>
        /// <returns>True if the list of TaskModel instances has been populated. Otherwise, false.</returns>
        public bool TryGetAvailableTask(TaskRequestSpecification specification, out List<TaskModel> taskModels)
        {
            if (specification == null)
                throw new ArgumentNullException("specification");

            using (IDbConnection connection = connectionFactory.CreateConnection())
            {
                using (SqlTransaction transaction = (SqlTransaction) connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    using (SqlCommand command = (SqlCommand) connectionFactory.CreateCommand(connection, transaction))
                    {
                        command.Transaction = transaction;
                        command.CommandText = string.Format("{0}.SelectTasks", sqlSettings.WiseQueueDefaultSchema);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@MaxRows", specification.MaxTasks);
                        command.Parameters.AddWithValue("@UpdatedTaskState", (short)TaskStates.Running);
                        command.Parameters.AddWithValue("@ServerId", specification.ServerId);
                        command.Parameters.AddWithValue("@QueueName", specification.QueueName);
                        command.Parameters.AddWithValue("@MaxTaskState", (short)TaskStates.Pending);

                        using (SqlDataReader rdr = command.ExecuteReader())
                        {
                            taskModels = new List<TaskModel>();
                            while (rdr.Read())
                            {
                                Int64 id = (Int64) rdr["Id"];
                                string queueName = (string) rdr["QueueName"];
                                Int64 serverId = (Int64) rdr["ServerId"];
                                TaskStates taskState = (TaskStates) (short) rdr["State"];

                                int repeatCrashCount = (int) rdr["RepeatCrashCount"];

                                string typeDetails = (string) rdr["InstanceType"];
                                string methodDetails = (string) rdr["Method"];
                                string parameterDetails = (string) rdr["ParametersTypes"];
                                string argumentDetails = (string) rdr["Arguments"];
                                
                                TaskEntity taskEntity = new TaskEntity
                                {
                                    Id = id,
                                    QueueName = queueName,
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
            const string selectStatement =
                "select TOP ({5}) [Id] from {0}.{1} where [QueueName] = {2} and [ServerId] = {3} and [State] = {4}";

            string sqlCommand = string.Format(selectStatement, sqlSettings.WiseQueueDefaultSchema, taskTableName,
                specification.QueueName, specification.ServerId, (int) TaskStates.Cancel, specification.MaxTasks);

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
                            Int64 id = (Int64) rdr["Id"];
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
        public Int64 InsertTask(string queueName, TaskModel taskModel)
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

            try
            {
                using (IDbConnection connection = connectionFactory.CreateConnection())
                {
                    using (SqlTransaction transaction = (SqlTransaction) connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            using (SqlCommand command = (SqlCommand) connectionFactory.CreateCommand(connection))
                            {
                                command.Transaction = transaction;
                                command.CommandText = string.Format("{0}.InsertTask", sqlSettings.WiseQueueDefaultSchema);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@QueueName", taskEntity.QueueName);
                                command.Parameters.AddWithValue("@State", taskEntity.TaskState);
                                command.Parameters.AddWithValue("@InstanceType", instanceType);
                                command.Parameters.AddWithValue("@Method", method);
                                command.Parameters.AddWithValue("@ParametersTypes", parametersTypes);
                                command.Parameters.AddWithValue("@Arguments", arguments);
                                command.Parameters.AddWithValue("@RetTaskId", Int64.MaxValue).Direction = ParameterDirection.Output;

                                logger.WriteTrace("Executing stored procedure {0}...", command.CommandText);

                                command.ExecuteNonQuery();
                                Int64 taskId = (Int64)command.Parameters["@RetTaskId"].Value;

                                if (taskId <= 0) //TODO: Not a good practice raise general exception.
                                    throw new Exception("Thre was a problem during inserting a new task: taskId = " + taskId);

                                transaction.Commit();
                                logger.WriteTrace("The stored procedure has been executed. TaskId = {0}", taskId);
                                return taskId;
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.WriteError(ex, "The task hasn't been inserted.");
                            try
                            {
                                transaction.Rollback();
                            }
                            catch (Exception rollBackException)
                            {
                                logger.WriteError(rollBackException,
                                    "There was an exception during rolling transaction back.");
                            }

                            throw new Exception("The task hasn't been inserted.", ex); //TODO: This exception will be catched a couple of lines below :)
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.WriteError(ex, "The task hasn't been inserted.");
                throw new Exception("The task hasn't been inserted.", ex);
            }
        }

        /// <summary>
        /// Set task's state.
        /// </summary>
        /// <param name="taskStateChangeModel">The <see cref="TaskStateChangeModel"/> instance.</param>
        public void SetTaskState(TaskStateChangeModel taskStateChangeModel)
        {
            try
            {
                using (IDbConnection connection = connectionFactory.CreateConnection())
                {
                    using (SqlTransaction transaction = (SqlTransaction)connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            using (SqlCommand command = (SqlCommand) connectionFactory.CreateCommand(connection))
                            {
                                command.Transaction = transaction;
                                command.CommandText = string.Format("{0}.SetTaskState", sqlSettings.WiseQueueDefaultSchema);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@Id", taskStateChangeModel.Id);
                                command.Parameters.AddWithValue("@ServerId", taskStateChangeModel.ServerId);
                                command.Parameters.AddWithValue("@State", (short) taskStateChangeModel.NewState);
                                command.Parameters.AddWithValue("@CheckState", (short)taskStateChangeModel.CheckState);
                                command.Parameters.AddWithValue("@UpdatedRowCount", -5).Direction = ParameterDirection.Output;

                                logger.WriteTrace("Executing stored procedure {0}...", command.CommandText);

                                command.ExecuteNonQuery();
                                int updatedRowCount = (int)command.Parameters["@UpdatedRowCount"].Value;

                                if (updatedRowCount != 1) //TODO: Not a good practice raise general exception.
                                    throw new Exception("Thre was a problem during changing state: updatedRowCount = "+ updatedRowCount);

                                transaction.Commit();

                                logger.WriteTrace("The stored procedure has been executed.");
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.WriteError(ex, "The task state hasn't been changed.");
                            try
                            {
                                transaction.Rollback();
                            }
                            catch (Exception rollBackException)
                            {
                                logger.WriteError(rollBackException,
                                    "There was an exception during rolling transaction back.");
                            }

                            throw new Exception("The task state hasn't been changed.", ex); //TODO: This exception will be catched a couple of lines below :)
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.WriteError(ex, "The task hasn't been inserted.");
                throw new Exception("The task state hasn't been changed.", ex);
            }
        }

        /// <summary>
        /// Try to restart the task.
        /// </summary>
        /// <param name="taskStateChangeModel">The <see cref="TaskStateChangeModel"/> instance.</param>
        /// <param name="timeShift">Time shift.</param>
        /// <param name="msg">Message that explains the restart.</param>
        /// <param name="exception">Exception that explains the restart.</param>
        public void RestartTask(TaskStateChangeModel taskStateChangeModel, TimeSpan timeShift, string msg, Exception exception)
        {
            try
            {
                using (IDbConnection connection = connectionFactory.CreateConnection())
                {
                    using (SqlTransaction transaction = (SqlTransaction)connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            using (SqlCommand command = (SqlCommand)connectionFactory.CreateCommand(connection))
                            {
                                command.Transaction = transaction;
                                command.CommandText = string.Format("{0}.RestartTask", sqlSettings.WiseQueueDefaultSchema);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@Id", taskStateChangeModel.Id);
                                command.Parameters.AddWithValue("@ServerId", taskStateChangeModel.ServerId);
                                command.Parameters.AddWithValue("@TimeShift", timeShift.ToString());
                                command.Parameters.AddWithValue("@State", (short)taskStateChangeModel.NewState);
                                command.Parameters.AddWithValue("@CheckState", (short)taskStateChangeModel.CheckState);
                                command.Parameters.AddWithValue("@UpdatedRowCount", -5).Direction = ParameterDirection.Output;

                                logger.WriteTrace("Executing stored procedure {0}...", command.CommandText);

                                command.ExecuteNonQuery();
                                int updatedRowCount = (int)command.Parameters["@UpdatedRowCount"].Value;
                                
                                if (updatedRowCount != 1) //TODO: Not a good practice raise general exception.
                                    throw new Exception("Thre was a problem during restaring the task: updatedRowCount = " + updatedRowCount);

                                transaction.Commit();

                                logger.WriteTrace("The stored procedure has been executed.");
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.WriteError(ex, "The task state hasn't been restarted.");
                            try
                            {
                                transaction.Rollback();
                            }
                            catch (Exception rollBackException)
                            {
                                logger.WriteError(rollBackException,
                                    "There was an exception during rolling transaction back.");
                            }

                            throw new Exception("The task state hasn't been restarted.", ex); //TODO: This exception will be catched a couple of lines below :)
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.WriteError(ex, "The task state hasn't been restarted.");
                throw new Exception("The task state hasn't been changed.", ex);
            }
        }

        #endregion
    }
}
