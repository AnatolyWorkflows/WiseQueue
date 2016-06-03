using System;
using System.Data;
using System.Data.SqlClient;
using WiseQueue.Core.Common.Logging;
using WiseQueue.Core.Common.Models;

namespace WiseQueue.Domain.MsSql.MsSqlDataContext
{
    public class SqlConnectionFactory : BaseLoggerObject, ISqlConnectionFactory
    {
        private readonly string connectionString;

        public SqlConnectionFactory(string connectionString, IWiseQueueLoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException("connectionString");

            this.connectionString = connectionString;
        }

        #region Implementation of ISqlConnectionFactory

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(connectionString);
        }

        #endregion
    }

}
