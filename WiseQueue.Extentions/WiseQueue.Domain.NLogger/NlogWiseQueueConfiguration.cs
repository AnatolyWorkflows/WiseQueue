using System;
using WiseQueue.Core.Common;

namespace WiseQueue.Domain.NLogger
{
    public static class NlogWiseQueueConfiguration
    {
        public static IWiseQueueConfiguration UseNLog(this IWiseQueueConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");
            
            NLogLoggerNinjectModule ninjectModule = new NLogLoggerNinjectModule();
            configuration.AddNinjectModule(ninjectModule);

            return configuration;
        }
    }
}
