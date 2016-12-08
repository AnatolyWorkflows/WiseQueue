using System;
using WiseQueue.Core.Common;

namespace WiseQueue.Domain.MsSql
{
    /// <summary>
    /// Configuration for MsSql server.
    /// </summary>
    public static class MsSqlWiseQueueConfiguration
    {
        /// <summary>
        /// Use <c>this</c> method if you want to use MsSql server as database.
        /// </summary>
        /// <param name="configuration">The <see cref="IWiseQueueConfiguration"/> instance.</param>
        /// <param name="connectionString">Connection string.</param>
        /// <returns>The <see cref="IWiseQueueConfiguration"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="configuration"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="connectionString"/> is <see langword="null" />.</exception>
        public static IWiseQueueConfiguration UseSqlServer(this IWiseQueueConfiguration configuration, string connectionString)
        {
            if (configuration == null) 
                throw new ArgumentNullException("configuration");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException("connectionString");

            MsSqlNinjectModule ninjectModule = new MsSqlNinjectModule(connectionString);
            configuration.AddNinjectModule(ninjectModule);

            return configuration;
        }

    }
}
