namespace WiseQueue.Core.Common.Management
{
    /// <summary>
    /// Interface shows that <c>object</c> is a simple manager.
    /// </summary>
    public interface IManager
    {
        /// <summary>
        /// Manager name.
        /// </summary>
        string Name { get; }       
    }

    public interface IExecutableManager : IManager
    {
        /// <summary>
        /// Calling this function if manager should do its job.
        /// </summary>
        void Execute();
    }

    public interface IStartStoppableManager : IExecutableManager
    {
        /// <summary>
        /// Start manager.
        /// </summary>
        void Start();

        /// <summary>
        /// Stop.
        /// </summary>
        void Stop(); 
    }
}
