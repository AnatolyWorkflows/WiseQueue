using System;
using Ninject.Modules;
using WiseQueue.Core.Common.DataContexts;
using WiseQueue.Domain.MsSql.MsSqlDataContext;

namespace WiseQueue.Domain.MsSql
{
    public sealed class MsSqlNinjectModule : NinjectModule
    {
        private readonly string connectionString;

        public MsSqlNinjectModule(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException("connectionString");

            this.connectionString = connectionString;
        }

        #region Overrides of NinjectModule

        /// <summary>Loads the module into the kernel.</summary>
        public override void Load()
        {
            Bind<ISqlConnectionFactory>().To<SqlConnectionFactory>().InSingletonScope().WithConstructorArgument("connectionString", connectionString);
            Bind<ITaskDataContext>().To<SqlWorkflowDataContext>();
        }

        #endregion
    }

}
