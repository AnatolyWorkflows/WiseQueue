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
    }
}
