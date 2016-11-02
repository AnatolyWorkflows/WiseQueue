using System;
using WiseQueue.Core.Common.Entities.Tasks;

namespace WiseQueue.Core.Common.Models.Tasks
{
    /// <summary>
    /// <c>Task</c> model.
    /// </summary>
    public class TaskModel: TaskStateModel
    {
        #region Properties...

        /// <summary>
        /// Task's activation details.
        /// </summary>
        public ActivationData ActivationData { get; private set; }

        public ScheduleInformation ScheduleInformation { get; private set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="queueId">The queue's identifier where the task will be put.</param>
        /// <param name="activationData">Task's activation data.</param>
        /// <param name="scheduleInformation"></param>
        /// <exception cref="ArgumentNullException"><paramref name="activationData"/> is <see langword="null" />.</exception>        
        /// <remarks>This constructor should be used only for new tasks entities.</remarks>
        public TaskModel(Int64 queueId, ActivationData activationData, ScheduleInformation scheduleInformation): base(queueId)
        {
            if (activationData == null)
                throw new ArgumentNullException("activationData");
            if (scheduleInformation == null)
                throw new ArgumentNullException(nameof(scheduleInformation));

            ActivationData = activationData;
            ScheduleInformation = scheduleInformation;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Task's identifier.</param>
        /// <param name="queueId">The queue's identifier where the task will be put.</param>
        /// <param name="serverId">The server's identifier that is used for processing the task.</param>
        /// <param name="activationData">Task's activation data.</param>
        /// <param name="taskState">Task's state.</param>
        /// <param name="scheduleInformation"></param>
        /// <exception cref="ArgumentNullException"><paramref name="activationData"/> is <see langword="null" />.</exception>
        /// <remarks>This constructor should be used only for a tasks entities that have been panding.</remarks>
        public TaskModel(Int64 id, Int64 queueId, Int64 serverId, ActivationData activationData, TaskStates taskState, ScheduleInformation scheduleInformation): base(id, queueId, serverId, taskState)
        {
            if (activationData == null)
                throw new ArgumentNullException("activationData");
            if (scheduleInformation == null)
                throw new ArgumentNullException(nameof(scheduleInformation));

            ActivationData = activationData;
            ScheduleInformation = scheduleInformation;
        }

        #region Overrides of Object

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("{0}; taskActivationDetails: {1}", base.ToString(), ActivationData);
        }

        #endregion
    }
}
