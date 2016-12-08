using System.Resources;
using AutoMapper;
using Common.Core.Interfaces.DataAccessLayer;
using Common.Core.ResourceHelper;
using Common.Domain.ResourceHelper;
using Ninject.Modules;
using WiseQueue.Core.Common.Caching;
using WiseQueue.Core.Common.Converters;
using WiseQueue.Core.Common.Converters.EntityModelConverters;
using WiseQueue.Domain.Common.Converters;
using WiseQueue.Domain.Common.Converters.EntityModelConverters;
using WiseQueue.Domain.Common.Mapping;
using WiseQueue.Domain.MicrosoftExpressionCache;

//using IResourceReaderHelper = Common.Core.ResourceHelper.IResourceReaderHelper;

namespace WiseQueue.Domain.Common
{
    class CommonNinjectModule : NinjectModule
    {
        #region Overrides of NinjectModule

        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            IMapper mapper = AutoMapperConfiguration.CreateMapper();
            Bind<IMapper>().ToConstant(mapper);
            Bind<IEntityModelMapper>().To<EntityModelMapper>();

            Bind<IResourceReader>().To<ResourceReader>();

            Bind<IResourceReaderHelper>().To<ResourceReaderHelper>();

            Bind<ITaskConverter>().To<TaskConverter>();
            
            //TODO: CachedExpressionCompiler in another assembly. I guess it shouldn't be here.
            Bind<ICachedExpressionCompiler>().To<CachedExpressionCompiler>();

            Bind<IExpressionConverter>().To<ExpressionConverter>();
            Bind<IJsonConverter>().To<JsonConverter>();
        }

        #endregion
    }
}
