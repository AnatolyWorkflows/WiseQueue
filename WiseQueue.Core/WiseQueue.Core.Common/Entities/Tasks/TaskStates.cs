namespace WiseQueue.Core.Common.Entities.Tasks
{
    /// <summary>
    /// <c>List</c> of available task's state.
    /// </summary>
    public enum TaskStates
    {
        /// <summary>
        /// Shows that this is a new task.
        /// </summary>
        New = 1,
        /// <summary>
        /// Shows that this task has been taken into processing by one of the servers.
        /// </summary>
        Pending = 2,
        /// <summary>
        /// Shows that this task has been executed in one of the servers.
        /// </summary>
        Running = 3,
        /// <summary>
        /// Shows that task has been successfully completed.
        /// </summary>
        Successed = 4,
        /// <summary>
        /// Shows that task has been failed.
        /// </summary>
        Failed = 5,
        /// <summary>
        /// Mark task for cancelling.
        /// </summary>
        Cancel = 6,
        /// <summary>
        /// Mark task for cancellation.
        /// </summary>
        Cancelling = 7,
        /// <summary>
        /// Mark task as cancelled.
        /// </summary>
        Cancelled = 8
    }
}
