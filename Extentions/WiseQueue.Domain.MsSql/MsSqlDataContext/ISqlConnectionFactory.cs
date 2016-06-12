using System.Data;

namespace WiseQueue.Domain.MsSql.MsSqlDataContext
{
    /// <summary>
    /// Interface shows that <c>object</c> is a Sql connection factory.
    /// </summary>
    public interface ISqlConnectionFactory
    {
        /// <summary>
        /// Create a new connection.
        /// </summary>
        /// <returns>The <see cref="IDbConnection"/> instance.</returns>
        IDbConnection CreateConnection();

        /// <summary>
        /// Create a new database and open connection to it.
        /// </summary>
        /// <returns>The <see cref="IDbConnection"/> instance.</returns>
        /// <remarks>The database will be created only if it is not exist.</remarks>
        IDbConnection CreateDatabaseAndConnection();
    }
}
