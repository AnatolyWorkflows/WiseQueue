using System;
using System.Data;
using System.Linq;
using Dapper;
using WiseQueue.Core.Common.Entities;
using WiseQueue.Core.Common.Logging;

namespace WiseQueue.Domain.MsSql.MsSqlDataContext.QueryObjects
{
    class InsertTaskQueryObject: BaseQueryObject<TaskEntity, Int64>
    {
        const string insertStatement =
               "INSERT INTO {0}.{1} ([State], [InstanceType], [Method], [ParametersTypes], [Arguments], [QueueId]) VALUES " +
                                      "({2},     '{3}',         '{4}',         '{5}',          '{6}',      {7}); " +
               "SELECT CAST(scope_identity() AS bigint)";

        public InsertTaskQueryObject(string schemaName, string taskTableName, IWiseQueueLogger logger) : base(schemaName, taskTableName, logger)
        {
        }

        #region Overrides of BaseQueryObject<TaskEntity,long>

        protected override Int64 OnExecute(TaskEntity taskEntity, IDbConnection connection)
        {
            string instanceType = taskEntity.TaskActivationDetails.InstanceType;
            string method = taskEntity.TaskActivationDetails.Method;
            string parametersTypes = taskEntity.TaskActivationDetails.ParametersTypes;
            string arguments = taskEntity.TaskActivationDetails.Arguments;
            Int64 queueId = taskEntity.QueueId;
            string sqlCommand = string.Format(insertStatement, schemaName, taskTableName, (short)taskEntity.TaskState, instanceType, method, parametersTypes, arguments, queueId);

            Int64 taskId = connection.Query<Int64>(sqlCommand).First();
            return taskId;
        }

        #endregion
    }
}
