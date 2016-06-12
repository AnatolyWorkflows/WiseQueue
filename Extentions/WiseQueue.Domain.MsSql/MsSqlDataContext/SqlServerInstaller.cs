using System;
using System.Data;
using WiseQueue.Core.Common.Logging;
using WiseQueue.Core.Common.Models;
using WiseQueue.Core.Common.Utils;

namespace WiseQueue.Domain.MsSql.MsSqlDataContext
{
    class SqlServerInstaller : BaseLoggerObject, ISqlServerInstaller
    {
        /// <summary>
        /// Schema that will be used by default.
        /// </summary>
        private const string wiseQueueDefaultSchema = "WiseQueue";

        /// <summary>
        /// The <see cref="IResourceReader"/> instance.
        /// </summary>
        private readonly IResourceReader resourceReader;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="resourceReader">The <see cref="IResourceReader"/> instance.</param>
        /// <param name="loggerFactory">The <see cref="IWiseQueueLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        public SqlServerInstaller(IResourceReader resourceReader, IWiseQueueLoggerFactory loggerFactory) : base(loggerFactory)
        {
            if (resourceReader == null)
                throw new ArgumentNullException("resourceReader");
            this.resourceReader = resourceReader;
        }

        #region Implementation of ISqlServerInstaller

        /// <summary>
        /// Install WiseQueue database into the MsSql server.
        /// </summary>
        /// <param name="connection">The <see cref="IDbConnection"/> instance.</param>
        public void Install(IDbConnection connection)
        {
            Install(connection, wiseQueueDefaultSchema);
        }

        /// <summary>
        /// Install WiseQueue database into the MsSql server.
        /// </summary>
        /// <param name="connection">The <see cref="IDbConnection"/> instance.</param>
        /// <param name="schema">Name of the shema.</param>
        public void Install(IDbConnection connection, string schema)
        {
            Type currentType = GetType();
            string script = resourceReader.ReadStringResource(currentType.Assembly, "Scripts.DatabaseScripts.sql");

            script = script.Replace("#{WiseQueueSchema}", !string.IsNullOrWhiteSpace(schema) ? schema : wiseQueueDefaultSchema);
            using (var command = connection.CreateCommand())
            {
                command.CommandText = script;
                command.ExecuteNonQuery();
            }
        }

        #endregion
    }
}
