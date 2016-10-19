using System;
using WiseQueue.Core.Common;
using WiseQueue.Core.Common.Management;
using WiseQueue.Core.Server;

namespace WiseQueue.Domain.Server
{
    public static class ServerWiseQueueConfiguration
    {
        public static IWiseQueueConfiguration UseServer(this IWiseQueueConfiguration configuration,
            ServerConfiguration serverConfiguration = null)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            ServerNinjectModule ninjectModule = new ServerNinjectModule();
            configuration.AddNinjectModule(ninjectModule);

            if (serverConfiguration == null)
                serverConfiguration = new ServerConfiguration();

            configuration.BindToItself(serverConfiguration);
            
            return configuration;
        }
    }
}
