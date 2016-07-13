using System;
using Common.Core.BaseClasses;
using Common.Core.Logging;
using WiseQueue.Core.Common.Converters.EntityModelConverters;
using WiseQueue.Core.Common.Entities.Tasks;
using WiseQueue.Core.Common.Models;
using WiseQueue.Core.Common.Models.Tasks;

namespace WiseQueue.Domain.Common.Converters.EntityModelConverters
{
    /// <summary>
    /// Converting Task entity into the Task model and back.
    /// </summary>
    public class TaskConverter : BaseLoggerObject, ITaskConverter
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        public TaskConverter(ICommonLoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        #region Implementation of ITaskConverter

        /// <summary>
        /// Convert task entity into the task model.
        /// </summary>
        /// <param name="taskEntity">The <see cref="TaskEntity"/> instance.</param>
        /// <returns>The <see cref="TaskModel"/> instance.</returns>
        public TaskModel Convert(TaskEntity taskEntity)
        {
            TaskActivationDetailsModel taskActivationDetails = new TaskActivationDetailsModel(taskEntity.InstanceType,
                taskEntity.Method, taskEntity.ParametersTypes, taskEntity.Arguments);

            TaskModel taskModel = taskEntity.Id < 0
                ? new TaskModel(taskEntity.QueueId, taskActivationDetails)
                : new TaskModel(taskEntity.Id, taskEntity.QueueId, taskActivationDetails, taskEntity.TaskState);

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
                InstanceType = taskModel.TaskActivationDetails.InstanceType,
                Method = taskModel.TaskActivationDetails.Method,
                ParametersTypes = taskModel.TaskActivationDetails.ParametersTypes,
                Arguments = taskModel.TaskActivationDetails.Arguments,
                TaskState = taskModel.TaskState
            };

            logger.WriteTrace("Converting {0} into the TaskEntity has been successfully completed. Result = {1}", taskModel, entity);

            return entity;
        }

        #endregion
    }
}
