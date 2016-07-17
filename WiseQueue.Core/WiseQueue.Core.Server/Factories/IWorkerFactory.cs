using WiseQueue.Core.Common.Models.Tasks;

namespace WiseQueue.Core.Server.Factories
{
    /// <summary>
    /// Interface shows that object can create workers.
    /// </summary>
    public interface IWorkerFactory
    {
        /// <summary>
        /// Create a worker using the <see cref="TaskModel"/> instance.
        /// </summary>
        /// <param name="taskModel">Information that will be used for creating the worker.</param>
        /// <returns>The worker.</returns>
        IWorker CreateWorker(TaskModel taskModel);
    }
}
