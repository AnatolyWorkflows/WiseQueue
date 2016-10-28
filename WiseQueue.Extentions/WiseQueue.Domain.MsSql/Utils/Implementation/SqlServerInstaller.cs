using System;
using System.Data;
using Common.Core.BaseClasses;
using Common.Core.Logging;
using Common.Core.ResourceHelper;

namespace WiseQueue.Domain.MsSql.Utils.Implementation
{
    class SqlServerInstaller : BaseLoggerObject, ISqlServerInstaller
    {
        /// <summary>
        /// MsSql settings
        /// </summary>
        private readonly MsSqlSettings sqlSettings;

        /// <summary>
        /// The <see cref="IResourceReaderHelper"/> instance.
        /// </summary>
        private readonly IResourceReaderHelper resourceReaderHelper;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sqlSettings">MsSql settings.</param>
        /// <param name="resourceReaderHelper">The <see cref="IResourceReaderHelper"/> instance.</param>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="sqlSettings"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="resourceReaderHelper"/> is <see langword="null" />.</exception>
        public SqlServerInstaller(MsSqlSettings sqlSettings, IResourceReaderHelper resourceReaderHelper, ICommonLoggerFactory loggerFactory) : base(loggerFactory)
        {
            if (sqlSettings == null)
                throw new ArgumentNullException("sqlSettings");
            if (resourceReaderHelper == null)
                throw new ArgumentNullException("resourceReaderHelper");
            this.sqlSettings = sqlSettings;
            this.resourceReaderHelper = resourceReaderHelper;
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
            string script = resourceReaderHelper.ReadStringResource(currentType.Assembly, "Scripts.DatabaseScripts.sql");

            script = script.Replace("#{WiseQueueSchema}", !string.IsNullOrWhiteSpace(schema) ? schema : sqlSettings.WiseQueueDefaultSchema);
            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = script;
                    command.ExecuteNonQuery();
                }
                transaction.Commit();
            }
        }

        #endregion
    }
}
