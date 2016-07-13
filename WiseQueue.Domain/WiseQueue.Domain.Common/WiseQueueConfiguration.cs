using Ninject;
using Ninject.Infrastructure.Disposal;
using Ninject.Modules;
using WiseQueue.Core.Common;
using WiseQueue.Core.Common.Management;

namespace WiseQueue.Domain.Common
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

        public TComponent Get<TComponent>()
        {
            TComponent result = kernel.Get<TComponent>();
            return result;
        }

        public void Activate()
        {
            IMainManagerManager mainManager = kernel.Get<IMainManagerManager>();
            
            IManager manager = kernel.TryGet<IServerManager>();
            if (manager != null)
                mainManager.Register(manager);

            manager = kernel.TryGet<ITaskManager>();
            if (manager != null)
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
