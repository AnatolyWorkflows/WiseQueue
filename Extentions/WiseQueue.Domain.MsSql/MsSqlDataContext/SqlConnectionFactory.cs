using System;
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
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(connectionString);
        }

        #endregion
    }

}
