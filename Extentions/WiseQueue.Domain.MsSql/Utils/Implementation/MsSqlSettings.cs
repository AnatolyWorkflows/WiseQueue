using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace WiseQueue.Domain.MsSql.Utils.Implementation
{
    /// <summary>
    /// MsSql settings that will be used in current project.
    /// </summary>
    public sealed class MsSqlSettings
    {
        /// <summary>
        /// Name of master database.
        /// </summary>
        private const string masterDatabase = "master";

        #region Properties...
        /// <summary>
        /// Connection string.
        /// </summary>
        public string ConnectionString { get; private set; }
        /// <summary>
        /// Connection string to the master database.
        /// </summary>
        public string MasterConnectionString { get; private set; }
        /// <summary>
        /// Initial catalog for WiseQueue database.
        /// </summary>
        public string InitialCatalog { get; private set; }
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="nameOrConnectionString">Name or connection string</param>
        /// <exception cref="ArgumentNullException"><paramref name="nameOrConnectionString"/> is <see langword="null" />.</exception>
        public MsSqlSettings(string nameOrConnectionString)
        {
            if (string.IsNullOrWhiteSpace(nameOrConnectionString))
                throw new ArgumentNullException("nameOrConnectionString");

            string connectionString = GetConnectionString(nameOrConnectionString);
            ConnectionString = connectionString;

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            
            //connectionString = "Data Source=(local);" +
            //                   "Initial Catalog=TestCreation;" +
            //                   "Integrated Security=SSPI;";
            InitialCatalog = builder.InitialCatalog;

            StringBuilder stringBuilder = new StringBuilder();            
            stringBuilder.AppendFormat("Data Source={0}; ", builder.DataSource);
            stringBuilder.AppendFormat("Initial Catalog={0}; ", masterDatabase);

            if (builder.IntegratedSecurity)
                stringBuilder.Append("Integrated Security=SSPI; ");

            //TODO: Login and Password

            MasterConnectionString = stringBuilder.ToString();
        }

        /// <summary>
        /// Get connection string from the <see cref="ConfigurationManager"/> if it is available.
        /// </summary>
        /// <param name="nameOrConnectionString">Name or connection string.</param>
        /// <returns>The connection string.</returns>
        private string GetConnectionString(string nameOrConnectionString)
        {
            ConnectionStringSettings connectionStringSetting = ConfigurationManager.ConnectionStrings[nameOrConnectionString];

            if (connectionStringSetting == null)
                return nameOrConnectionString;

            return connectionStringSetting.ConnectionString;
        }
    }
}
