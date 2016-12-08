using System;
using WiseQueue.Core.Common;
using WiseQueue.Core.Common.Management.TaskManagment;
using WiseQueue.Core.Server;

namespace WiseQueue.Domain.Server
{
    /// <summary>
    /// This is a global configuration class.
    /// It contains all necessary methods for configure WiseQueue.
    /// </summary>
    public static class ServerConfigurationExtention
    {
        public static IWiseQueueConfiguration UseServer(this IWiseQueueConfiguration configuration, ServerConfiguration serverConfiguration = null, TaskManagerConfiguration taskManagerConfiguration = null)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            if (serverConfiguration == null)
                serverConfiguration = ServerConfiguration.Default;

            if (taskManagerConfiguration == null)
                taskManagerConfiguration = new TaskManagerConfiguration
                {
                    MaxRerunAttempts = 4,
                    MaxTaskPerQueue = 5,
                    TimeShiftAfterCrash = TimeSpan.FromSeconds(30)
                };

            configuration.BindToItself(serverConfiguration);
            configuration.BindToItself(taskManagerConfiguration);
            configuration.AddNinjectModule(new ServerNinjectModule());

            return configuration;
        }
    }
}
