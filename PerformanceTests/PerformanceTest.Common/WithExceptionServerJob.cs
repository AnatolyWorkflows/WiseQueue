using System;
using System.Data;
using System.Data.SqlClient;

namespace PerformanceTest.Common
{
    public class WithExceptionServerJob
    {
        /// <summary>
        /// Insert statement.
        /// </summary>
        private const string insertStatement =
            "INSERT INTO dbo.SimpleServerJob ([RequestId]) VALUES " +
                                                "('{0}'); ";

        private static Random random = new Random();

        public void Execute(Guid requestId, string connectionString)
        {
            if (random.Next(0, 100) < 10)
                throw new Exception("This is Random exception");

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
