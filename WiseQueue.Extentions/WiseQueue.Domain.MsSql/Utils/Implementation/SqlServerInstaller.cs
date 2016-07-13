using System;
using System.Data;
using Common.Core.Logging;
using WiseQueue.Core.Common.Models;
using WiseQueue.Core.Common.Utils;

namespace WiseQueue.Domain.MsSql.Utils.Implementation
{
    class SqlServerInstaller : BaseLoggerObject, ISqlServerInstaller
    {
        /// <summary>
        /// MsSql settings
        /// </summary>
        private readonly MsSqlSettings sqlSettings;

        /// <summary>
        /// The <see cref="IResourceReader"/> instance.
        /// </summary>
        private readonly IResourceReader resourceReader;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sqlSettings">MsSql settings.</param>
        /// <param name="resourceReader">The <see cref="IResourceReader"/> instance.</param>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="sqlSettings"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="resourceReader"/> is <see langword="null" />.</exception>
        public SqlServerInstaller(MsSqlSettings sqlSettings, IResourceReader resourceReader, ICommonLoggerFactory loggerFactory) : base(loggerFactory)
        {
            if (sqlSettings == null)
                throw new ArgumentNullException("sqlSettings");
            if (resourceReader == null)
                throw new ArgumentNullException("resourceReader");
            this.sqlSettings = sqlSettings;
            this.resourceReader = resourceReader;
        }

        #region Implementation of ISqlServerInstaller

        /// <summary>
        /// Install WiseQueue database into the MsSql server.
        /// </summary>
        /// <param name="connection">The <see cref="IDbConnection"/> instance.</param>
        public void Install(IDbConnection connection)
        {
            Install(connection, sqlSettings.WiseQueueDefaultSchema);
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

            script = script.Replace("#{WiseQueueSchema}", !string.IsNullOrWhiteSpace(schema) ? schema : sqlSettings.WiseQueueDefaultSchema);
            using (var command = connection.CreateCommand())
            {
                command.CommandText = script;
                command.ExecuteNonQuery();
            }
        }

        #endregion
    }
}
