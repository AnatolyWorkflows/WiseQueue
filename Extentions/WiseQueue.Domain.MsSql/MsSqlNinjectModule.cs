using System;
using Ninject.Modules;
using WiseQueue.Core.Common.DataContexts;
using WiseQueue.Domain.MsSql.MsSqlDataContext;
using WiseQueue.Domain.MsSql.Utils;
using WiseQueue.Domain.MsSql.Utils.Implementation;

namespace WiseQueue.Domain.MsSql
{
    /// <summary>
    /// Ninject configuration for SQL extension.
    /// </summary>
    public sealed class MsSqlNinjectModule : NinjectModule
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
        public MsSqlNinjectModule(string nameOrConnectionString)
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
            Bind<MsSqlSettings>().ToSelf().InSingletonScope().WithConstructorArgument("nameOrConnectionString", nameOrConnectionString);

            Bind<ISqlServerInstaller>().To<SqlServerInstaller>();
            Bind<ISqlConnectionFactory>().To<SqlConnectionFactory>().InSingletonScope();

            Bind<IServerDataContext>().To<ServerDataContext>();
            Bind<IQueueDataContext>().To<QueueDataContext>();
            Bind<ITaskDataContext>().To<TaskDataContext>();
        }

        #endregion
    }

}
