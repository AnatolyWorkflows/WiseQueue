using System;
using WiseQueue.Core.Common;

namespace WiseQueue.Domain.MsSql
{
    public static class MsSqlWiseQueueConfiguration
    {
        public static IWiseQueueConfiguration UseSqlServer(
           this IWiseQueueConfiguration configuration,
           string connectionString)
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
