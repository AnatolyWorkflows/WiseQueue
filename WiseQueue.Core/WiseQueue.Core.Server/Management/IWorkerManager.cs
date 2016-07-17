using WiseQueue.Core.Common.Models.Tasks;

namespace WiseQueue.Core.Common.Management
{
    /// <summary>
    /// Interface shows that object is a Worker Manager.
    /// </summary>
    public interface IWorkerManager
    {
        /// <summary>
        /// Create a worker where the task will be executed.
        /// </summary>
        /// <param name="taskModel">The task.</param>
        void StartWorker(TaskModel taskModel);

        /// <summary>
        /// Stop a worker where the task is executing.
        /// </summary>
        /// <param name="taskModel">The task.</param>
        void StopWorker(TaskModel taskModel);

        /// <summary>
        /// Stop all workers.
        /// </summary>
        void StopAll();
    }
}
