using System;
using System.Data.SqlClient;
using System.Reflection;
using WiseQueue.Core.Common.Converters;
using WiseQueue.Core.Common.Entities.Tasks;
using WiseQueue.Core.Common.Models.Tasks;

namespace WiseQueue.Domain.MsSql.Converters
{
    class TaskModelBuilder
    {
        private readonly IJsonConverter jsonConverter;
        private readonly IExpressionConverter expressionConverter;

        public TaskModelBuilder(IJsonConverter jsonConverter, IExpressionConverter expressionConverter)
        {
            if (jsonConverter == null)
                throw new ArgumentNullException(nameof(jsonConverter));
            if (expressionConverter == null)
                throw new ArgumentNullException(nameof(expressionConverter));

            this.jsonConverter = jsonConverter;
            this.expressionConverter = expressionConverter;
        }


        public TaskModel Build(SqlDataReader rdr)
        {
            Int64 id = (Int64)rdr["Id"];
            string queueName = (string)rdr["QueueName"];
            //Int64 serverId = (Int64)rdr["ServerId"];
            TaskStates taskState = (TaskStates)(short)rdr["State"];

            int repeatCrashCount = (int)rdr["RepeatCrashCount"];

            string typeDetails = (string)rdr["InstanceType"];
            string methodDetails = (string)rdr["Method"];
            string parameterDetails = (string)rdr["ParametersTypes"];
            string argumentDetails = (string)rdr["Arguments"];

            string instanceTypeJson = jsonConverter.ConvertFromJson<string>(typeDetails);
            Type instanceType = Type.GetType(instanceTypeJson, throwOnError: true, ignoreCase: true);
            Type[] argumentTypes = jsonConverter.ConvertFromJson<Type[]>(parameterDetails);
            MethodInfo method = expressionConverter.GetNonOpenMatchingMethod(instanceType, methodDetails, argumentTypes);

            string[] serializedArguments = jsonConverter.ConvertFromJson<string[]>(argumentDetails);
            object[] arguments = expressionConverter.DeserializeArguments(method, serializedArguments);

            ActivationData activationData = new ActivationData(instanceType, method, arguments, argumentTypes);
            ScheduleInformation scheduleInformation = new ScheduleInformation(repeatCrashCount);

            TaskModel taskModel = new TaskModel(id, taskState, activationData, scheduleInformation);

            return taskModel;
        }
    }
}
