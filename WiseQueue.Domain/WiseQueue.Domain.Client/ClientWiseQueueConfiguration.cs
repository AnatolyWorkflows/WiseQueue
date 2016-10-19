using System;
using WiseQueue.Core.Client;
using WiseQueue.Core.Common;

namespace WiseQueue.Domain.Client
{
    public static class ClientWiseQueueConfiguration
    {
        public static IWiseQueueConfiguration UseClient(this IWiseQueueConfiguration configuration, ClientConfiguration clientConfiguration)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            ClientNinjectModule ninjectModule = new ClientNinjectModule();
            configuration.AddNinjectModule(ninjectModule);

            return configuration;
        }
    }
}
