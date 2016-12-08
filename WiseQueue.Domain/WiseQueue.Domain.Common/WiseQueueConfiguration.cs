using Ninject;
using Ninject.Infrastructure.Disposal;
using Ninject.Modules;
using WiseQueue.Core.Common;

namespace WiseQueue.Domain.Common
{
    public class WiseQueueConfiguration: DisposableObject,  IWiseQueueConfiguration
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
        #endregion
    }
}
