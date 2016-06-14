using System.Data;

namespace WiseQueue.Domain.MsSql.Utils
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

        /// <summary>
        /// Create command using the <c>connection</c>.
        /// </summary>
        /// <param name="connection">The <see cref="IDbConnection"/> instance.</param>
        /// <returns>The <see cref="IDbCommand"/> command.</returns>
        IDbCommand CreateCommand(IDbConnection connection);
    }
}
