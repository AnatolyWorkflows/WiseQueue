using System;
using WiseQueue.Core.Common.Entities.Tasks;

namespace WiseQueue.Core.Common.Models.Tasks
{
    /// <summary>
    /// <c>Task</c> model.
    /// </summary>
    public class TaskModel
    {
        #region Fields...
        /// <summary>
        /// Task's identifier.
        /// </summary>
        private readonly Int64 id;
        /// <summary>
        /// The queue's identifier where the task will be put.
        /// </summary>
        private readonly Int64 queueId;
        /// <summary>
        /// Task's state.
        /// </summary>
        private readonly TaskStates taskState;
        #endregion

        #region Properties...

        /// <summary>
        /// Task's identifier.
        /// </summary>
        public Int64 Id
        {
            get { return id; }
        }

        /// <summary>
        /// The queue's identifier where the task will be put.
        /// </summary>
        public Int64 QueueId
        {
            get { return queueId; }
        }

        /// <summary>
        /// Task's activation details.
        /// </summary>
        public ActivationData ActivationData { get; private set; }

        /// <summary>
        /// Task's state.
        /// </summary>
        public TaskStates TaskState
        {
            get { return taskState; }
        }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="queueId">The queue's identifier where the task will be put.</param>
        /// <param name="activationData">Task's activation data.</param>
        /// <exception cref="ArgumentOutOfRangeException">The queue identifier should be greate than 0.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="activationData"/> is <see langword="null" />.</exception>        
        /// <remarks>This constructor should be used only for new tasks entities.</remarks>
        public TaskModel(Int64 queueId, ActivationData activationData)
        {
            if (queueId <= 0)
                throw new ArgumentOutOfRangeException("queueId", "The queue identifier should be greate than 0.");
            if (activationData == null)
                throw new ArgumentNullException("activationData");

            this.queueId = queueId;
            ActivationData = activationData;
            taskState = TaskStates.New;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Task's identifier.</param>
        /// <param name="queueId">The queue's identifier where the task will be put.</param>
        /// <param name="activationData">Task's activation data.</param>
        /// <param name="taskState">Task's state.</param>
        /// <exception cref="ArgumentOutOfRangeException">The task identifier should be greate than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The queue identifier should be greate than 0.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="activationData"/> is <see langword="null" />.</exception>
        /// <remarks>This constructor should be used only for a tasks entities that have been panding.</remarks>
        public TaskModel(Int64 id, Int64 queueId, ActivationData activationData, TaskStates taskState)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException("id", "The task identifier should be greate than 0.");
            if (queueId <= 0)
                throw new ArgumentOutOfRangeException("queueId", "The queue identifier should be greate than 0.");
            if (activationData == null)
                throw new ArgumentNullException("activationData");

            this.id = id;
            this.queueId = queueId;
            ActivationData = activationData;
            this.taskState = taskState;
        }

        #region Overrides of Object

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("Id: {0}; TaskState: {1}; QueueId: {2}; taskActivationDetails: {3}", id, taskState, queueId, ActivationData);
        }

        #endregion
    }
}
