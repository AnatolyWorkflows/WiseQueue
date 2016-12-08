using System;
using WiseQueue.Core.Common.Entities.Tasks;

namespace WiseQueue.Core.Common.Models.Tasks
{
    public class TaskStateChangeModel
    {
        #region Properties...

        /// <summary>
        /// Task's identifier.
        /// </summary>
        public Int64 Id { get; private set; }

        /// <summary>
        /// The server's identifier where the task will be put.
        /// </summary>
        public Int64? ServerId { get; private set; }

        /// <summary>
        /// Maximum state that can be changed to the new one.
        /// </summary>
        public TaskStates CheckState { get; private set; }

        /// <summary>
        /// New task's state.
        /// </summary>
        public TaskStates NewState { get; private set; }
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="taskId">Task's identifier.</param>
        /// <param name="serverId">The server's identifier that is used for processing the task.</param>
        /// <param name="newState">New task's state.</param>
        /// <exception cref="ArgumentOutOfRangeException">The task identifier should be great than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The queue identifier should be great than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The server identifier should be great than 0.</exception>
        public TaskStateChangeModel(Int64 taskId, Int64 serverId, TaskStates newState)
        {
            if (taskId <= 0)
                throw new ArgumentOutOfRangeException("taskId", "The task identifier should be great than 0.");
            if (serverId <= 0)
                throw new ArgumentOutOfRangeException("serverId", "The server identifier should be great than 0.");

            switch (newState)
            {
                case TaskStates.New:
                case TaskStates.Cancel:
                    CheckState = TaskStates.Running;
                    break;
                case TaskStates.Cancelled:
                case TaskStates.Successed:
                case TaskStates.Failed:
                    CheckState = TaskStates.Cancel;
                    break;
                default:
                    throw new NotImplementedException("newState = "+ newState);
            }

            Id = taskId;
            ServerId = serverId;
            NewState = newState;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="taskId">Task's identifier.</param>
        /// <param name="newState">New task's state.</param>
        /// <exception cref="ArgumentOutOfRangeException">The task identifier should be great than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The queue identifier should be great than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The server identifier should be great than 0.</exception>
        public TaskStateChangeModel(Int64 taskId, TaskStates newState)
        {
            if (taskId <= 0)
                throw new ArgumentOutOfRangeException("taskId", "The task identifier should be great than 0.");

            Id = taskId;
            NewState = newState;
            CheckState = TaskStates.Running;
        }


        #region Overrides of Object

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            if (ServerId.HasValue)
                return string.Format("Id: {0}; ServerId: {1}; NewState: {2}, CheckState: {3}", Id, ServerId, NewState, CheckState);

            return string.Format("Id: {0}; NewState: {1}, CheckState: {2}", Id, NewState, CheckState);
        }

        #endregion
    }
}
