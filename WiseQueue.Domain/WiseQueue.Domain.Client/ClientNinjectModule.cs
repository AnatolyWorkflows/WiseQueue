using Ninject.Modules;
using WiseQueue.Core.Common.Management;
using WiseQueue.Domain.Client.Management;

namespace WiseQueue.Domain.Client
{
    public class ClientNinjectModule : NinjectModule
    {
        #region Overrides of NinjectModule

        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IClientManager>().To<ClientManager>().InSingletonScope();
        }

        #endregion
    }
}
