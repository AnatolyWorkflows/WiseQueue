using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using Common.Core.BaseClasses;
using Common.Core.Logging;
using Common.Core.ResourceHelper;

namespace WiseQueue.Domain.MsSqlEntityFramework.Installer
{
    class SqlServerInstaller : BaseLoggerObject
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
        public void Install()
        {
            using (IDbConnection connection = new SqlConnection(sqlSettings.ConnectionString))
            {
                connection.Open();
                Install(connection, sqlSettings.WiseQueueDefaultSchema);
            }
        }

        /// <summary>
        /// Install WiseQueue database into the MsSql server.
        /// </summary>
        /// <param name="connection">The <see cref="IDbConnection"/> instance.</param>
        /// <param name="schema">Name of the shema.</param>
        private void Install(IDbConnection connection, string schema)
        {
            Type currentType = GetType();
            string script = resourceReaderHelper.ReadStringResource(currentType.Assembly, "Installer.Scripts.DatabaseScripts.sql");

            script = script.Replace("#{WiseQueueSchema}", !string.IsNullOrWhiteSpace(schema) ? schema : sqlSettings.WiseQueueDefaultSchema);
            //We have transaction inside the script.
            //using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
            //{
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    using (var command = connection.CreateCommand())
                    {
                        //command.Transaction = transaction;
                        command.CommandText = script;
                        command.ExecuteNonQuery();
                        break;
                    }
                }
                catch (DbException ex)
                {
                    if (ex.ErrorCode == 1205)
                    {
                        logger.WriteWarning("Deadlock occurred during automatic migration execution. Retrying (Attempts: {0} of 10)...", i);
                        Thread.Sleep(10*1000); //TODO: To settings
                    }
                    else
                    {
                        logger.WriteError(ex, "Installing process has been failed.");
                        throw;
                    }
                }
            }
            //transaction.Commit();
            //}
        }

        #endregion
    }
}
