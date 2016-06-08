using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WiseQueue.Core.Common.Logging;

namespace WiseQueue.Domain.MsSql.MsSqlDataContext.QueryObjects
{
    abstract class BaseQueryObject<TRequest, TResponse>
    {
        protected readonly string schemaName;
        protected readonly string taskTableName;
        protected readonly IWiseQueueLogger logger;

        protected BaseQueryObject(string schemaName, string taskTableName, IWiseQueueLogger logger)
        {
            if (string.IsNullOrWhiteSpace(schemaName))
                throw new ArgumentNullException("schemaName");
            if (string.IsNullOrWhiteSpace(taskTableName))
                throw new ArgumentNullException("taskTableName");
            if (logger == null)
                throw new ArgumentNullException("logger");

            this.schemaName = schemaName;
            this.taskTableName = taskTableName;
            this.logger = logger;
        }

        public TResponse Execute(TRequest request, IDbConnection connection)
        {
            TResponse response = OnExecute(request, connection);
            return response;
        }

        protected abstract TResponse OnExecute(TRequest request, IDbConnection connection);
    }
}
