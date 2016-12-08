using System.Resources;
using Common.Core.ResourceHelper;
using Common.Domain.ResourceHelper;
using Ninject.Modules;
using WiseQueue.Core.Client.Managment;
using WiseQueue.Core.Common.Caching;
using WiseQueue.Core.Common.Converters;
using WiseQueue.Core.Common.Converters.EntityModelConverters;
using WiseQueue.Core.Common.Management;
using WiseQueue.Core.Common.Management.TaskManagment;
using WiseQueue.Core.Server.Management;
using WiseQueue.Domain.Client.Management;
using WiseQueue.Domain.Common.Converters;
using WiseQueue.Domain.Common.Converters.EntityModelConverters;
using WiseQueue.Domain.Common.Management;
using WiseQueue.Domain.MicrosoftExpressionCache;
using WiseQueue.Domain.Server.Management;
using WiseQueue.Domain.Server.Management.Tasks;

//using IResourceReaderHelper = Common.Core.ResourceHelper.IResourceReaderHelper;

namespace WiseQueue.Domain.Configuration
{
    class CommonNinjectModule : NinjectModule
    {
        #region Overrides of NinjectModule

        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IResourceReader>().To<ResourceReader>();

            Bind<IResourceReaderHelper>().To<ResourceReaderHelper>();

            Bind<ITaskConverter>().To<TaskConverter>();
            
            //TODO: CachedExpressionCompiler in another assembly. I guess it shouldn't be here.
            Bind<ICachedExpressionCompiler>().To<CachedExpressionCompiler>();

            Bind<IExpressionConverter>().To<ExpressionConverter>();
            Bind<IJsonConverter>().To<JsonConverter>();

            Bind<IClientManager>().To<ClientManager>().InSingletonScope();
            Bind<IServerManager>().To<ServerManager>().InSingletonScope();
            
            Bind<IMainManagerManager>().To<MainManagerManager>().InSingletonScope();
            Bind<ITaskManager>().To<TaskManager>().InSingletonScope();            

            Bind<ITaskBuilder>().To<TaskBuilder>().InSingletonScope();
        }

        #endregion
    }
}
