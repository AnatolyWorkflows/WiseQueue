using Ninject.Modules;
using WiseQueue.Core.Common.Management.TaskManagment;
using WiseQueue.Core.Server.Management;
using WiseQueue.Domain.Server.Management;
using WiseQueue.Domain.Server.Management.Tasks;

namespace WiseQueue.Domain.Server
{
    class ServerNinjectModule : NinjectModule
    {
        #region Overrides of NinjectModule

        /// <summary>Loads the module into the kernel.</summary>
        public override void Load()
        {
            Bind<IServerManager>().To<ServerManager>().InSingletonScope();
            Bind<ITaskBuilder>().To<TaskBuilder>().InSingletonScope();
            Bind<ITaskManager>().To<TaskManager>().InSingletonScope();
        }

        #endregion
    }
}
