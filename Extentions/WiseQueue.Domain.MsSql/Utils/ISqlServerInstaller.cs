using System.Data;

namespace WiseQueue.Domain.MsSql.Utils
{
    /// <summary>
    /// Interface shows that <c>object</c> can install WiseQueue database into the MsSql server.
    /// </summary>
    public interface ISqlServerInstaller
    {
        /// <summary>
        /// Install WiseQueue database into the MsSql server.
        /// </summary>
        /// <param name="connection">The <see cref="IDbConnection"/> instance.</param>
        void Install(IDbConnection connection);

        /// <summary>
        /// Install WiseQueue database into the MsSql server.
        /// </summary>
        /// <param name="connection">The <see cref="IDbConnection"/> instance.</param>
        /// <param name="schema">Name of the shema.</param>
        void Install(IDbConnection connection, string schema);
    }
}
