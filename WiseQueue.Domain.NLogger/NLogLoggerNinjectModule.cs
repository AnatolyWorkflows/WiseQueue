using Ninject.Modules;
using WiseQueue.Core.Common.Logging;

namespace WiseQueue.Domain.NLogger
{
    /// <summary>
    /// Configuration for logger.
    /// </summary>
    public sealed class NLogLoggerNinjectModule : NinjectModule
    {
        #region Overrides of NinjectModule

        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IWiseQueueLoggerFactory>().To<WiseQueueNLogLoggerFactory>().InSingletonScope();
            Bind<IWiseQueueLogger>().To<WiseQueueNLogLogger>().InSingletonScope();
        }

        #endregion
    }
}
