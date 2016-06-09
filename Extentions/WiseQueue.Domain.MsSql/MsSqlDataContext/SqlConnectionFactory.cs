using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using WiseQueue.Core.Common.Logging;
using WiseQueue.Core.Common.Models;

namespace WiseQueue.Domain.MsSql.MsSqlDataContext
{
    /// <summary>
    /// Simple SQL connections factory.
    /// </summary>
    public class SqlConnectionFactory : BaseLoggerObject, ISqlConnectionFactory
    {
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
        /// <exception cref="InvalidOperationException">Cannot open a connection without specifying a data source or server.orThe connection is already open.</exception>
        /// <exception cref="SqlException">A connection-level error occurred while opening the connection. If the <see cref="P:System.Data.SqlClient.SqlException.Number" /> property contains the value 18487 or 18488, this indicates that the specified password has expired or must be reset. See the <see cref="M:System.Data.SqlClient.SqlConnection.ChangePassword(System.String,System.String)" /> method for more information.The &lt;system.data.localdb&gt; tag in the app.config file has invalid or unknown elements.</exception>
        /// <exception cref="ConfigurationErrorsException">There are two entries with the same name in the <c>localdbinstances</c> section.</exception>
        public IDbConnection CreateConnection()
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            return sqlConnection;
        }

        #endregion
    }

}
