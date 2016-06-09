using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using WiseQueue.Core.Common.Entities;
using WiseQueue.Core.Common.Logging;
using WiseQueue.Core.Common.Specifications;

namespace WiseQueue.Domain.MsSql.MsSqlDataContext.QueryObjects
{
    class GetAvailableTaskQueryObject : BaseQueryObject<TaskRequestSpecification, TaskEntity>
    {
        public GetAvailableTaskQueryObject(string schemaName, string taskTableName, IWiseQueueLogger logger) : base(schemaName, taskTableName, logger)
        {
        }

        #region Overrides of BaseQueryObject<TaskRequestSpecification,TaskEntity>

        protected override TaskEntity OnExecute(TaskRequestSpecification specification, IDbConnection connection)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Declare @TempTable table ([Id] [bigint], ");
            stringBuilder.Append("[State] [smallint], ");
            stringBuilder.Append("[InstanceType] [nvarchar](200), ");
            stringBuilder.Append("[Method] [nvarchar](200), ");
            stringBuilder.Append("[ParametersTypes] [nvarchar](500), ");
            stringBuilder.Append("[Arguments] [nvarchar](1000), ");
            stringBuilder.Append("[QueueId] [bigint], ");
            stringBuilder.Append("[ServerId] [bigint] NULL, ");
            stringBuilder.Append("[ExpiredAt] [datetime] NULL, ");
            stringBuilder.AppendLine("[CompletedAt] [datetime] NULL);");

            DateTime expiredAt = DateTime.UtcNow.Add(specification.Timeout);
            stringBuilder.AppendFormat("UPDATE TOP (1) {0}.{1} ", schemaName, taskTableName);
            stringBuilder.AppendFormat("SET State = {0}, ", (short)TaskStates.Pending);
            stringBuilder.AppendFormat("ServerId = {0}, ", specification.ServerId);
            stringBuilder.AppendFormat("ExpiredAt = '{0}' ", expiredAt.ToString("s"));
            stringBuilder.Append("OUTPUT inserted.* INTO @TempTable ");
            stringBuilder.AppendFormat("Where (State = {0} ", (short)TaskStates.New);
            stringBuilder.AppendFormat("OR ( (State = {0} OR State = {1}) AND ExpiredAt < '{2}')) ", (short)TaskStates.Pending, (short)TaskStates.Running, DateTime.UtcNow.ToString("s"));
            stringBuilder.AppendFormat("AND (QueueId = {0});", specification.QueueId);

            stringBuilder.AppendLine();
            stringBuilder.AppendLine("SELECT * FROM @TempTable");
            string sqlCommand = stringBuilder.ToString();

            TaskEntity taskEntity = connection.Query<TaskEntity>(sqlCommand).FirstOrDefault();
            return taskEntity;
        }

        #endregion
    }
}
