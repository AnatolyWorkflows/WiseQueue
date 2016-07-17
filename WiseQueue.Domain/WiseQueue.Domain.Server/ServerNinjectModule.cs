using Ninject.Modules;
using WiseQueue.Core.Server.Management;
using WiseQueue.Domain.Server.Management;

namespace WiseQueue.Domain.Server
{
    public class ServerNinjectModule : NinjectModule
    {
        #region Overrides of NinjectModule

        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IServerManager>().To<ServerManager>().InSingletonScope();
        }

        #endregion
    }
}
