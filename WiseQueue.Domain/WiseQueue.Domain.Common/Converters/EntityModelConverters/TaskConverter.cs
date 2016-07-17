using System;
using System.Reflection;
using Common.Core.BaseClasses;
using Common.Core.Logging;
using WiseQueue.Core.Common.Converters;
using WiseQueue.Core.Common.Converters.EntityModelConverters;
using WiseQueue.Core.Common.Entities.Tasks;
using WiseQueue.Core.Common.Models.Tasks;

namespace WiseQueue.Domain.Common.Converters.EntityModelConverters
{
    /// <summary>
    /// Converting Task entity into the Task model and back.
    /// </summary>
    public class TaskConverter : BaseLoggerObject, ITaskConverter
    {
        private readonly IExpressionConverter expressionConverter;
        private readonly IJsonConverter jsonConverter;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="expressionConverter">The <see cref="IExpressionConverter"/> instance.</param>
        /// <param name="jsonConverter">JSON converter.</param>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="IExpressionConverter"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="jsonConverter"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        public TaskConverter(IExpressionConverter expressionConverter, IJsonConverter jsonConverter, ICommonLoggerFactory loggerFactory) : base(loggerFactory)
        {
            if (expressionConverter == null) 
                throw new ArgumentNullException("expressionConverter");
            if (jsonConverter == null) 
                throw new ArgumentNullException("jsonConverter");

            this.expressionConverter = expressionConverter;
            this.jsonConverter = jsonConverter;
        }

        #region Implementation of ITaskConverter

        /// <summary>
        /// Convert task entity into the task model.
        /// </summary>
        /// <param name="taskEntity">The <see cref="TaskEntity"/> instance.</param>
        /// <returns>The <see cref="TaskModel"/> instance.</returns>
        public TaskModel Convert(TaskEntity taskEntity)
        {
            Type instanceType = Type.GetType(taskEntity.InstanceType, throwOnError: true, ignoreCase: true);
            Type[] argumentTypes = jsonConverter.ConvertFromJson<Type[]>(taskEntity.ParametersTypes);
            MethodInfo method = expressionConverter.GetNonOpenMatchingMethod(instanceType, taskEntity.Method, argumentTypes);

            string[] serializedArguments = jsonConverter.ConvertFromJson<string[]>(taskEntity.Arguments);
            object[] arguments = expressionConverter.DeserializeArguments(method, serializedArguments);

            ActivationData activationData = new ActivationData(instanceType, method, arguments, argumentTypes);

            TaskModel taskModel = taskEntity.Id < 0
                ? new TaskModel(taskEntity.QueueId, activationData)
                : new TaskModel(taskEntity.Id, taskEntity.QueueId, activationData, taskEntity.TaskState);

            return taskModel;
        }

        /// <summary>
        /// Convert task model into the task entity.
        /// </summary>
        /// <param name="taskModel">The <see cref="TaskModel"/> instance.</param>
        /// <returns>The <see cref="TaskEntity"/> instance.</returns>
        public TaskEntity Convert(TaskModel taskModel)
        {
            logger.WriteTrace("Converting {0} into the TaskEntity...", taskModel);
            TaskEntity entity = new TaskEntity
            {
                Id = taskModel.Id,
                QueueId = taskModel.QueueId,
                InstanceType = jsonConverter.ConvertToJson(taskModel.ActivationData.InstanceType),
                Method = jsonConverter.ConvertToJson(taskModel.ActivationData.Method),
                ParametersTypes = jsonConverter.ConvertToJson(taskModel.ActivationData.ArgumentTypes),
                Arguments = jsonConverter.ConvertToJson(taskModel.ActivationData.Arguments),
                TaskState = taskModel.TaskState
            };

            logger.WriteTrace("Converting {0} into the TaskEntity has been successfully completed. Result = {1}", taskModel, entity);

            return entity;
        }

        #endregion
    }
}
