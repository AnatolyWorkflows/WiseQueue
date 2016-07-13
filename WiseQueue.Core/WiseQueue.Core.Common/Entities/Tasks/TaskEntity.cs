using System;

namespace WiseQueue.Core.Common.Entities.Tasks
{
    /// <summary>
    /// Class contains all fields that are using to store information about a task.
    /// </summary>
    public class TaskEntity
    {
        #region Properties...

        /// <summary>
        /// Task's identifier.
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// The queue's identifier where the task will be put.
        /// </summary>
        public Int64 QueueId { get; set; }

        /// <summary>
        /// Task's state.
        /// </summary>
        public TaskStates TaskState { get; set; }

        /// <summary>
        /// Information about class that will be used for executing method.
        /// </summary>
        public string InstanceType { get; set; }

        /// <summary>
        /// Information about method that will be executed.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Parameters' types that are using in the method.
        /// </summary>
        public string ParametersTypes { get; set; }

        /// <summary>
        /// Arguments' values that are using in the method.
        /// </summary>
        public string Arguments { get; set; }

        #endregion

        #region Overrides of Object

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("Id: {0}; TaskState: {1}; QueueId: {2}; taskActivationDetails: {3}", Id, TaskState,
                QueueId, "TODO:"); //TODO: ToString().
        }

        #endregion
    }
}