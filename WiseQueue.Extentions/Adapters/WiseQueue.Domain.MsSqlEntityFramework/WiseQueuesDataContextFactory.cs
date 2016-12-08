using System;
using System.Data.Common;
using System.Data.SqlClient;
using Common.Core.BaseClasses;
using Common.Core.Interfaces.DataAccessLayer;
using Common.Core.Logging;

namespace WiseQueue.Domain.MsSqlEntityFramework
{
    class WiseQueuesDataContextFactory: BaseLoggerObject, IDataContextFactory
    {
        private readonly MsSqlSettings sqlSettings;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        public WiseQueuesDataContextFactory(MsSqlSettings sqlSettings, ICommonLoggerFactory loggerFactory) : base(loggerFactory)
        {
            if (sqlSettings == null)
                throw new ArgumentNullException(nameof(sqlSettings));

            this.sqlSettings = sqlSettings;
        }

        #region Implementation of IDataContextFactory

        /// <summary>
        /// Create a new data context.
        /// </summary>
        /// <returns>The IDataContext instance.</returns>
        public IDataContext CreateDataContext()
        {
            DbConnection existingConnection = new SqlConnection(sqlSettings.ConnectionString);
            return new WiseQueuesDataContext(existingConnection);
        }

        #endregion
    }
}
