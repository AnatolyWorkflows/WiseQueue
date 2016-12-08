using WiseQueue.Core.Common;

namespace WiseQueue.Domain.Common
{
    /// <summary>
    /// This is a global configuration class.
    /// It contains all necessary methods for configure WiseQueue.
    /// </summary>
    public static class WiseQueueGlobalConfiguration
    {
        private static readonly IWiseQueueConfiguration wiseQueueConfiguration = new WiseQueueConfiguration();

        /// <summary>
        /// The <see cref="IWiseQueueConfiguration"/> instance.
        /// </summary>
        public static IWiseQueueConfiguration WiseQueueConfiguration
        {
            get { return wiseQueueConfiguration; }
        }
    }
}
