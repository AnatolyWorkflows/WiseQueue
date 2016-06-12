using System;
using Ninject.Modules;
using WiseQueue.Core.Common.DataContexts;
using WiseQueue.Domain.MsSql.MsSqlDataContext;

namespace WiseQueue.Domain.MsSql
{
    /// <summary>
    /// Ninject configuration for SQL extension.
    /// </summary>
    public sealed class MsSqlNinjectModule : NinjectModule
    {
        /// <summary>
        /// Connection string.
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionString"/> is <see langword="null" />.</exception>
        public MsSqlNinjectModule(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException("connectionString");

            this.connectionString = connectionString;
        }

        #region Overrides of NinjectModule

        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<ISqlServerInstaller>().To<SqlServerInstaller>();
            Bind<ISqlConnectionFactory>().To<SqlConnectionFactory>().InSingletonScope().WithConstructorArgument("connectionString", connectionString);

            Bind<IQueueDataContext>().To<QueueDataContext>();
            Bind<ITaskDataContext>().To<SqlTaskDataContext>();
        }

        #endregion
    }

}
