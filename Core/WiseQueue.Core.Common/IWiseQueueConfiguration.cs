using System;
using System.ComponentModel;
using Ninject;
using Ninject.Modules;

namespace WiseQueue.Core.Common
{
    public interface IWiseQueueConfiguration: IDisposable
    {
        void AddNinjectModule(NinjectModule ninjectModule);

        TComponent Get<TComponent>();
    }
}
