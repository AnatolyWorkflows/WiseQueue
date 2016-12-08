using System;
using Common.Core.Interfaces.DataAccessLayer;
using Common.Core.Logging;
using Common.Core.ResourceHelper;
using Ninject;
using Ninject.Modules;
using WiseQueue.Domain.MsSqlEntityFramework.Installer;

namespace WiseQueue.Domain.MsSqlEntityFramework
{
    /// <summary>
    /// Ninject configuration for SQL extension.
    /// </summary>
    public sealed class DependencyInjectionNinjectModule : NinjectModule
    {
        /// <summary>
        /// Name or connection string.
        /// </summary>
        private readonly string nameOrConnectionString;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="nameOrConnectionString">Name or connection string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="nameOrConnectionString"/> is <see langword="null" />.</exception>
        public DependencyInjectionNinjectModule(string nameOrConnectionString)
        {
            if (string.IsNullOrWhiteSpace(nameOrConnectionString))
                throw new ArgumentNullException("nameOrConnectionString");

            this.nameOrConnectionString = nameOrConnectionString;
        }

        #region Overrides of NinjectModule

        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            MsSqlSettings settings = new MsSqlSettings(nameOrConnectionString);

            Bind<MsSqlSettings>().ToConstant(settings);
            Bind<IDataContext>().To<WiseQueuesDataContext>();
            Bind<IDataContextFactory>().To<WiseQueuesDataContextFactory>();            

            IResourceReaderHelper resourceReader = Kernel.Get<IResourceReaderHelper>();
            ICommonLoggerFactory loggerFactory = Kernel.Get<ICommonLoggerFactory>();
            SqlServerInstaller serverInstaller = new SqlServerInstaller(settings, resourceReader, loggerFactory);

            serverInstaller.Install();
        }

        #endregion
    }

}
