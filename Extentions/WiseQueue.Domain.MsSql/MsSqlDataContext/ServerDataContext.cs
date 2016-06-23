using System;
using System.Data;
using WiseQueue.Core.Common.Converters.EntityModelConverters;
using WiseQueue.Core.Common.DataContexts;
using WiseQueue.Core.Common.Entities.Server;
using WiseQueue.Core.Common.Entities.Tasks;
using WiseQueue.Core.Common.Logging;
using WiseQueue.Core.Common.Models;
using WiseQueue.Core.Common.Models.Servers;
using WiseQueue.Domain.MsSql.Utils;
using WiseQueue.Domain.MsSql.Utils.Implementation;

namespace WiseQueue.Domain.MsSql.MsSqlDataContext
{
    /// <summary>
    /// Data context that contains all methods for working with server storage.
    /// </summary>
    class ServerDataContext: BaseLoggerObject, IServerDataContext
    {
        #region Consts...

        /// <summary>
        /// Table's name where all servers are stored.
        /// </summary>
        private const string serverTableName = "Servers"; //TODO: move to settings.
        
        /// <summary>
        /// Table's name where all tasks are stored.
        /// </summary>
        private const string taskTableName = "Tasks"; //TODO: move to settings.

        /// <summary>
        /// Insert statement.
        /// </summary>
        private const string insertStatement =
                "INSERT INTO {0}.{1} ([Name], [Description],               [ExpiredAt]                  ) VALUES " +
                                     "('{2}',     '{3}',       CAST(GETDATE() AS DATETIME)+'{4}'); " +
                "SELECT CAST(scope_identity() AS bigint)";

        /// <summary>
        /// Delete statement.
        /// </summary>
        private const string deleteStatement = "DELETE FROM {0}.{1} WHERE [Id] = {2};";

        /// <summary>
        /// Heartbeat statement.
        /// </summary>
        private const string heartbeatStatement =
            "UPDATE {0}.{1} SET [ExpiredAt]=CAST(GETDATE() AS DATETIME)+'{2}' WHERE [Id] = {3}";

        /// <summary>
        /// Delete servers that have been expired statement.
        /// </summary>
        private const string deleteExpiredServersStatement = "DECLARE @CURRENT_DATE_TIME datetime; " +
                                                             "SET @CURRENT_DATE_TIME = CAST(GETDATE() AS DATETIME); " +
                                                             "UPDATE t SET t.ServerId = NULL FROM {0}.{1} t INNER JOIN {0}.{2} s on t.ServerId = s.Id WHERE [ExpiredAt] < @CURRENT_DATE_TIME; " +
                                                             "DELETE FROM {0}.{2} WHERE [ExpiredAt] < @CURRENT_DATE_TIME;";

        #endregion

        #region Fields...

        /// <summary>
        /// The <see cref="MsSqlSettings"/> instance.
        /// </summary>
        private readonly MsSqlSettings sqlSettings;

        /// <summary>
        /// The <see cref="IServerConverter"/> instance
        /// </summary>
        private readonly IServerConverter serverConverter;

        /// <summary>
        /// The <see cref="ISqlConnectionFactory"/> instance.
        /// </summary>
        private readonly ISqlConnectionFactory connectionFactory;

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sqlSettings">The <see cref="MsSqlSettings"/> instance.</param>
        /// <param name="serverConverter">The <see cref="IServerConverter"/> instance.</param>
        /// <param name="connectionFactory">The <see cref="ISqlConnectionFactory"/> instance.</param>
        /// <param name="loggerFactory">The <see cref="IWiseQueueLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        public ServerDataContext(MsSqlSettings sqlSettings, IServerConverter serverConverter, ISqlConnectionFactory connectionFactory, IWiseQueueLoggerFactory loggerFactory) : base(loggerFactory)
        {
            if (sqlSettings == null) 
                throw new ArgumentNullException("sqlSettings");
            if (serverConverter == null)
                throw new ArgumentNullException("serverConverter");
            if (connectionFactory == null)
                throw new ArgumentNullException("connectionFactory");

            this.sqlSettings = sqlSettings;
            this.serverConverter = serverConverter;
            this.connectionFactory = connectionFactory;
        }

        #region Implementation of IServerDataContext

        /// <summary>
        /// Insert a new record about a server.
        /// </summary>
        /// <param name="model">The server.</param>
        /// <returns>The server's identifier that has been inserted.</returns>
        public Int64 InsertServer(ServerModel model)
        {
            logger.WriteTrace("Inserting {0} into the database...", model);

            if (model.Id != 0)
                throw new ArgumentException("Model's identifier should be 0. Now it is " + model.Id, "model");

            logger.WriteTrace("Converting {0} into the entity...", model);
            ServerEntity entity = serverConverter.Convert(model);
            logger.WriteTrace("{0} has been converted. Generating sql command for {1}...", model, entity);

            string sqlCommand = string.Format(insertStatement, sqlSettings.WiseQueueDefaultSchema, serverTableName, entity.Name, entity.Description, entity.HeartbeatLifetime);

            logger.WriteTrace("The SqlCommand has been generated. Result: {0}", sqlCommand);

            logger.WriteTrace("Executing sql command...");
            using (IDbConnection connection = connectionFactory.CreateConnection())
            {
                using (IDbCommand command = connectionFactory.CreateCommand(connection))
                {
                    command.CommandText = sqlCommand;
                    Int64 result = (Int64)command.ExecuteScalar();

                    logger.WriteTrace("The command has been executed. Entity identifier is {0}", result);
                    return result;
                }
            }          
        }

        /// <summary>
        /// Delete server information form the database using server's identifier.
        /// </summary>
        /// <param name="serverId">The server identifier.</param>
        public void DeleteServer(Int64 serverId)
        {
            logger.WriteTrace("Deleting server by its identifier ({0}) from the database...", serverId);

            if (serverId <= 0)
                throw new ArgumentException("Server's identifier should be greate than 0. Now it is " + serverId, "serverId");

            string sqlCommand = string.Format(deleteStatement, sqlSettings.WiseQueueDefaultSchema, serverTableName, serverId);

            logger.WriteTrace("The SqlCommand has been generated. Result: {0}", sqlCommand);

            logger.WriteTrace("Executing sql command...");
            using (IDbConnection connection = connectionFactory.CreateConnection())
            {
                using (IDbCommand command = connectionFactory.CreateCommand(connection))
                {
                    command.CommandText = sqlCommand;
                    int affectedRows = command.ExecuteNonQuery();

                    logger.WriteTrace("The command has been executed. The entity has been deleted. Affected rows = {0}.", affectedRows);
                }
            }       
        }

        /// <summary>
        /// Send a heartbeat from the server.
        /// </summary>
        /// <param name="serverHeartbeatModel">The heartbeat information.</param>
        public void SendHeartbeat(ServerHeartbeatModel serverHeartbeatModel)
        {
            logger.WriteTrace("Sending heartbeat from the server ({0}) to the database...", serverHeartbeatModel);

            if (serverHeartbeatModel.ServerId <= 0)
                throw new ArgumentException("Server's identifier should be greate than 0. Now it is " + serverHeartbeatModel.ServerId, "serverHeartbeatModel");

            string sqlCommand = string.Format(heartbeatStatement, sqlSettings.WiseQueueDefaultSchema, serverTableName, serverHeartbeatModel.HeartbeatLifetime, serverHeartbeatModel.ServerId);

            logger.WriteTrace("The SqlCommand has been generated. Result: {0}", sqlCommand);

            logger.WriteTrace("Executing sql command...");
            using (IDbConnection connection = connectionFactory.CreateConnection())
            {
                using (IDbCommand command = connectionFactory.CreateCommand(connection))
                {
                    command.CommandText = sqlCommand;
                    int affectedRows = command.ExecuteNonQuery();

                    logger.WriteTrace("The command has been executed. The entity has been deleted. Affected rows = {0}.", affectedRows);
                }
            }                 
        }


        /// <summary>
        /// Delete servers that have been expired.
        /// </summary>
        /// <param name="currentServerId">Current server identifier. It needs because server should delete itself.</param>
        /// <returns>Count of servers that have been deleted.</returns>
        public int DeleteExpiredServers(Int64 currentServerId)
        {
            logger.WriteTrace("Deleting servers that have been expired from the database...");

            string sqlCommand = string.Format(deleteExpiredServersStatement, sqlSettings.WiseQueueDefaultSchema, taskTableName, serverTableName);

            logger.WriteTrace("The SqlCommand has been generated. Result: {0}", sqlCommand);

            logger.WriteTrace("Executing sql command...");
            using (IDbConnection connection = connectionFactory.CreateConnection())
            {
                using (IDbCommand command = connectionFactory.CreateCommand(connection))
                {
                    command.CommandText = sqlCommand;
                    int affectedRows = command.ExecuteNonQuery();

                    logger.WriteTrace("The command has been executed. The entity has been deleted. Affected rows = {0}.", affectedRows);

                    return affectedRows;
                }
            }              
        }

        #endregion
    }
}
