using System;
using WiseQueue.Core.Common;

namespace WiseQueue.Domain.Server
{
    public static class ServerWiseQueueConfiguration
    {
        public static IWiseQueueConfiguration UseServer(
          this IWiseQueueConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            ServerNinjectModule ninjectModule = new ServerNinjectModule();
            configuration.AddNinjectModule(ninjectModule);

            return configuration;
        }
    }
}
