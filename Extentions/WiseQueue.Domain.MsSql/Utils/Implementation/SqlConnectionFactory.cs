using System;
using System.Data;
using System.Data.SqlClient;
using WiseQueue.Core.Common.Logging;
using WiseQueue.Core.Common.Models;

namespace WiseQueue.Domain.MsSql.Utils.Implementation
{
    /// <summary>
    /// Simple SQL connections factory.
    /// </summary>
    public class SqlConnectionFactory : BaseLoggerObject, ISqlConnectionFactory
    {
        #region Fields...
        
        /// <summary>
        /// MsSql settings including connection string and different timeouts.
        /// </summary>
        private readonly MsSqlSettings sqlSettings;

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sqlSettings">MsSql settings including connection string and different timeouts.</param>
        /// <param name="loggerFactory">The <see cref="IWiseQueueLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="sqlSettings"/> is <see langword="null" />.</exception>
        public SqlConnectionFactory(MsSqlSettings sqlSettings, IWiseQueueLoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            if (sqlSettings == null)
                throw new ArgumentNullException("sqlSettings");

            this.sqlSettings = sqlSettings;
        }

        #region Implementation of ISqlConnectionFactory

        /// <summary>
        /// Create a new connection.
        /// </summary>
        /// <returns>The <see cref="IDbConnection"/> instance.</returns>
        public IDbConnection CreateConnection()
        {
            logger.WriteTrace("Creating connection (Connection string = {0})...", sqlSettings.ConnectionString);

            string connectionString = sqlSettings.ConnectionString;
            IDbConnection connection = new SqlConnection(connectionString);
            logger.WriteTrace("The connection has been created. Opening...");

            connection.Open();
            logger.WriteTrace("The connection has been opened.");

            return connection;
        }

        /// <summary>
        /// Create a new database and open connection to it.
        /// </summary>
        /// <returns>The <see cref="IDbConnection"/> instance.</returns>
        /// <remarks>The database will be created only if it is not exist.</remarks>
        public IDbConnection CreateDatabaseAndConnection()
        {
            logger.WriteInfo("Creating connection and check does {0} database exist...", sqlSettings.InitialCatalog);
            string masterConnectionString = sqlSettings.MasterConnectionString;

            string recreateDatabaseSql =
                String.Format(@"if db_id('{0}') is null create database [{0}] COLLATE SQL_Latin1_General_CP1_CS_AS",
                    sqlSettings.InitialCatalog);

            logger.WriteTrace("Creating connection to the master database. Connection string = {0}", masterConnectionString);
            using (SqlConnection connection = new SqlConnection(masterConnectionString))
            {
                logger.WriteTrace("The connection has been created. Opening...");
                connection.Open();
                logger.WriteTrace("The connection has been opened. Creating a command...");

                using (var command = connection.CreateCommand())
                {
                    logger.WriteTrace("The command has been created. Sql: {0}", recreateDatabaseSql);
                    command.CommandText = recreateDatabaseSql;
                    command.ExecuteNonQuery();
                    logger.WriteTrace("The command has been executed.");
                }
            }

            logger.WriteInfo("The {0} exists.", sqlSettings.InitialCatalog);

            return CreateConnection();
        }

        /// <summary>
        /// Create command using the <c>connection</c>.
        /// </summary>
        /// <param name="connection">The <see cref="IDbConnection"/> instance.</param>
        /// <returns>The <see cref="IDbCommand"/> command.</returns>
        public IDbCommand CreateCommand(IDbConnection connection)
        {
            logger.WriteTrace("Creating a command for connection ({0})...", connection);
            IDbCommand command = connection.CreateCommand();
            logger.WriteTrace("The command has been created.");
            return command;
        }

        #endregion
    }

}
