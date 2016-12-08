using System;
using Ninject.Modules;

namespace WiseQueue.Core.Common
{
    public interface IWiseQueueConfiguration: IDisposable
    {
        void AddNinjectModule(NinjectModule ninjectModule);

        void BindToItself<TObject>(TObject obj);

        TComponent Get<TComponent>();
    }
}
