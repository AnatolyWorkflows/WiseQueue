using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
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
            string connectionString = sqlSettings.ConnectionString;
            IDbConnection connection = new SqlConnection(connectionString);
            connection.Open();

            return connection;
        }

        /// <summary>
        /// Create a new database and open connection to it.
        /// </summary>
        /// <returns>The <see cref="IDbConnection"/> instance.</returns>
        /// <remarks>The database will be created only if it is not exist.</remarks>
        public IDbConnection CreateDatabaseAndConnection()
        {
            string masterConnectionString = sqlSettings.MasterConnectionString;

            string recreateDatabaseSql =
                String.Format(@"if db_id('{0}') is null create database [{0}] COLLATE SQL_Latin1_General_CP1_CS_AS",
                    sqlSettings.InitialCatalog);

            using (SqlConnection connection = new SqlConnection(masterConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = recreateDatabaseSql;
                    command.ExecuteNonQuery();
                }
            }

            return CreateConnection();
        }

        /// <summary>
        /// Create command using the <c>connection</c>.
        /// </summary>
        /// <param name="connection">The <see cref="IDbConnection"/> instance.</param>
        /// <returns>The <see cref="IDbCommand"/> command.</returns>
        public IDbCommand CreateCommand(IDbConnection connection)
        {
            IDbCommand command = connection.CreateCommand();
            return command;
        }

        #endregion
    }

}
