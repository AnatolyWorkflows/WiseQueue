using Ninject;
using Ninject.Infrastructure.Disposal;
using Ninject.Modules;
using WiseQueue.Core.Client;
using WiseQueue.Core.Common;
using WiseQueue.Core.Common.Management;
using WiseQueue.Core.Server;
using WiseQueue.Core.Server.Management;

namespace WiseQueue.Domain.Configuration
{
    public class WiseQueueConfiguration: DisposableObject, IWiseQueueConfiguration
    {
        private readonly IKernel kernel;

        public WiseQueueConfiguration()
        {
            kernel = new StandardKernel();
            kernel.Load(new CommonNinjectModule());
        }

        #region Implementation of IWiseQueueConfiguration

        public void AddNinjectModule(NinjectModule ninjectModule)
        {
            kernel.Load(ninjectModule);
        }

        public void BindToItself<TObject>(TObject obj)
        {
            kernel.Bind<TObject>().ToConstant(obj);
        }

        public TComponent Get<TComponent>()
        {
            TComponent result = kernel.Get<TComponent>();            
            return result;
        }

        public void Activate()
        {
            IMainManagerManager mainManager = kernel.Get<IMainManagerManager>();
            
            IManager manager = kernel.Get<IServerManager>();
            mainManager.Register(manager);

            ServerConfiguration serverConfiguration = kernel.TryGet<ServerConfiguration>();
            ClientConfiguration clientConfiguration = kernel.TryGet<ClientConfiguration>();

            TaskManagerConfiguration taskManagerConfiguration = new TaskManagerConfiguration();

            if (serverConfiguration != null)
            {
                taskManagerConfiguration.MaxTaskPerQueue = serverConfiguration.MaxTaskPerQueue;
                taskManagerConfiguration.State = TaskManagerState.ServerOnly;
            }

            if (clientConfiguration != null)
            {
                taskManagerConfiguration.MaxRerunAttempts = clientConfiguration.MaxRerunAttempts;
                taskManagerConfiguration.TimeShiftAfterCrash = clientConfiguration.TimeShiftAfterCrash;
                taskManagerConfiguration.State = TaskManagerState.ClientOnly;
            }

            if (serverConfiguration != null && clientConfiguration != null)
                taskManagerConfiguration.State = TaskManagerState.Both;

            BindToItself(taskManagerConfiguration);

            manager = kernel.Get<ITaskManager>();
            mainManager.Register(manager);                

            mainManager.Start();
        }

        #endregion

        #region Overrides of DisposableObject

        /// <summary>Releases resources held by the object.</summary>
        public override void Dispose(bool disposing)
        {
            if (!kernel.IsDisposed)
            {
                IMainManagerManager mainManager = kernel.Get<IMainManagerManager>();
                mainManager.Stop();

                kernel.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
