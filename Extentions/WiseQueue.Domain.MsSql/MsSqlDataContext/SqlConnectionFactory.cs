using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using WiseQueue.Core.Common.Logging;
using WiseQueue.Core.Common.Models;

namespace WiseQueue.Domain.MsSql.MsSqlDataContext
{
    /// <summary>
    /// Simple SQL connections factory.
    /// </summary>
    public class SqlConnectionFactory : BaseLoggerObject, ISqlConnectionFactory
    {
        private const string masterDatabase = "master";

        #region Fields...
        
        /// <summary>
        /// Connection string.
        /// </summary>
        private readonly string connectionString;

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <param name="loggerFactory">The <see cref="IWiseQueueLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionString"/> is <see langword="null" />.</exception>
        public SqlConnectionFactory(string connectionString, IWiseQueueLoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException("connectionString");

            this.connectionString = connectionString;
        }

        #region Implementation of ISqlConnectionFactory

        /// <summary>
        /// Create a new connection.
        /// </summary>
        /// <returns>The <see cref="IDbConnection"/> instance.</returns>
        public IDbConnection CreateConnection()
        {
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
            //connectionString = "Data Source=(local);" +
            //                   "Initial Catalog=TestCreation;" +
            //                   "Integrated Security=SSPI;";
            StringBuilder stringBuilder = new StringBuilder();
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            stringBuilder.AppendFormat("Data Source={0}; ", builder.DataSource);
            stringBuilder.AppendFormat("Initial Catalog={0}; ", masterDatabase);
            
            if (builder.IntegratedSecurity)
                stringBuilder.Append("Integrated Security=SSPI; ");

            //TODO: Login and Password

            string masterConnectionString = stringBuilder.ToString();

            string recreateDatabaseSql =
                String.Format(@"if db_id('{0}') is null create database [{0}] COLLATE SQL_Latin1_General_CP1_CS_AS",
                    builder.InitialCatalog);

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

        #endregion
    }

}
