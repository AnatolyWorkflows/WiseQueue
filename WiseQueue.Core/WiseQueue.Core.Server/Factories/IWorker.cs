using Common.Core.Interfaces;

namespace WiseQueue.Core.Server.Factories
{
    /// <summary>
    /// Interface shows that object is a worker.
    /// </summary>
    public interface IWorker: IExecutable
    {
        /// <summary>
        /// Worker name.
        /// </summary>
        string Name { get; }
    }
}
