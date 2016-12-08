using System;
using Common.Core.Interfaces.DataAccessLayer;
using WiseQueue.Core.Common.Entities.Tasks;

namespace WiseQueue.Core.Common.Models.Tasks
{
    /// <summary>
    /// <c>Task</c> model.
    /// </summary>
    public class TaskModel: IObjectIdentity
    {
        #region Properties...

        /// <summary>
        /// Task's identifier.
        /// </summary>
        public Int64 Id { get; private set; }

        /// <summary>
        /// Task's state.
        /// </summary>
        public TaskStates TaskState { get; private set; }

        /// <summary>
        /// Task's activation details.
        /// </summary>
        public ActivationData ActivationData { get; private set; }

        public ScheduleInformation ScheduleInformation { get; private set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="activationData">Task's activation data.</param>
        /// <exception cref="ArgumentNullException"><paramref name="activationData"/> is <see langword="null" />.</exception>        
        /// <remarks>This constructor should be used only for new tasks entities.</remarks>        
        public TaskModel(ActivationData activationData, ScheduleInformation scheduleInformation)
        {
            if (activationData == null)
                throw new ArgumentNullException("activationData");
            if (scheduleInformation == null)
                throw new ArgumentNullException(nameof(scheduleInformation));

            Id = 0;
            TaskState = TaskStates.New;
            ActivationData = activationData;
            ScheduleInformation = scheduleInformation;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="activationData">Task's activation data.</param>
        /// <remarks>This constructor should be used only for new tasks entities.</remarks>        
        public TaskModel(Int64 id, TaskStates taskState, ActivationData activationData, ScheduleInformation scheduleInformation)
        {
            if (id <= 0)
                throw new ArgumentException("Identifier should be great then 0.", "id");
            if (activationData == null)
                throw new ArgumentNullException("activationData");
            if (scheduleInformation == null)
                throw new ArgumentNullException(nameof(scheduleInformation));

            Id = id;
            TaskState = taskState;
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
            return string.Format("TaskId:{0}; TaskState: {1}; taskActivationDetails: {2}", Id, TaskState, ActivationData);
        }

        #endregion
    }
}
