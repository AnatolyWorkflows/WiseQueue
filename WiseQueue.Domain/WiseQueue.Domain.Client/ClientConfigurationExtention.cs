using System;
using WiseQueue.Core.Client;
using WiseQueue.Core.Common;

namespace WiseQueue.Domain.Client
{
    /// <summary>
    /// This is a global configuration class.
    /// It contains all necessary methods for configure WiseQueue.
    /// </summary>
    public static class ClientConfigurationExtention
    {
        public static IWiseQueueConfiguration UseClient(this IWiseQueueConfiguration configuration, ClientConfiguration clientConfiguration = null)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            if (clientConfiguration == null)
                clientConfiguration = ClientConfiguration.Default;

            configuration.BindToItself(clientConfiguration);
            configuration.AddNinjectModule(new ClientNinjectModule());

            return configuration;
        }
    }
}
