using Common.Core.Logging;
using Common.NLogger;
using Ninject.Modules;

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
            Bind<ICommonLoggerFactory>().To<CommonNLogLoggerFactory>().InSingletonScope();
            Bind<ICommonLogger>().To<CommonNLogLogger>().InSingletonScope();
        }

        #endregion
    }
}
