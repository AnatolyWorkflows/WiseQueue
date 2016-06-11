using Ninject;
using Ninject.Infrastructure.Disposal;
using Ninject.Modules;
using WiseQueue.Core.Common;

namespace WiseQueue.Domain.Common
{
    public class WiseQueueConfiguration: DisposableObject, IWiseQueueConfiguration
    {
        private readonly IKernel kernel;

        public WiseQueueConfiguration()
        {
            kernel = new StandardKernel();
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

        #endregion

        #region Overrides of DisposableObject

        /// <summary>Releases resources held by the object.</summary>
        public override void Dispose(bool disposing)
        {
            if (!kernel.IsDisposed)
                kernel.Dispose();

            base.Dispose(disposing);
        }

        #endregion
    }
}
