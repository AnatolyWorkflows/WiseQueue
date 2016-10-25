using System;
using System.Data;
using System.Data.SqlClient;

namespace PerformanceTest.Common
{
    public class SimpleServerJob
    {
        /// <summary>
        /// Insert statement.
        /// </summary>
        private const string insertStatement =
            "INSERT INTO dbo.SimpleServerJob ([RequestId]) VALUES " +
                                                "('{0}'); ";

        public void Execute(Guid requestId, string connectionString)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = string.Format(insertStatement, requestId);
                    command.ExecuteScalar();
                }
            }
        }
    }
}
