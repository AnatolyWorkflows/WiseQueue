using System;
using WiseQueue.Core.Client;
using WiseQueue.Core.Common;
using WiseQueue.Core.Server;

namespace WiseQueue.Domain.Configuration
{
    /// <summary>
    /// This is a global configuration class.
    /// It contains all necessary methods for configure WiseQueue.
    /// </summary>
    public static class WiseQueueGlobalConfiguration
    {
        /// <summary>
        /// The <see cref="IWiseQueueConfiguration"/> instance.
        /// </summary>
        private static readonly IWiseQueueConfiguration wiseQueueConfiguration = new WiseQueueConfiguration();

        /// <summary>
        /// The <see cref="IWiseQueueConfiguration"/> instance.
        /// </summary>
        public static IWiseQueueConfiguration WiseQueueConfiguration
        {
            get { return wiseQueueConfiguration; }
        }

        public static IWiseQueueConfiguration UseClient(this IWiseQueueConfiguration configuration, ClientConfiguration clientConfiguration = null)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            if (clientConfiguration == null)
                clientConfiguration = ClientConfiguration.Default;

            configuration.BindToItself(clientConfiguration);

            return configuration;
        }

        public static IWiseQueueConfiguration UseServer(this IWiseQueueConfiguration configuration, ServerConfiguration serverConfiguration = null)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            if (serverConfiguration == null)
                serverConfiguration = ServerConfiguration.Default;

            configuration.BindToItself(serverConfiguration);

            return configuration;
        }
    }
}
