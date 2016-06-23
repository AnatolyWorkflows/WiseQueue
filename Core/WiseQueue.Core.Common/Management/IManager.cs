namespace WiseQueue.Core.Common.Management
{
    /// <summary>
    /// Interface shows that <c>object</c> is a simple manager.
    /// </summary>
    public interface IManager
    {
        /// <summary>
        /// Start manager.
        /// </summary>
        void Start();

        /// <summary>
        /// Stop.
        /// </summary>
        void Stop();

        /// <summary>
        /// Calling this function if manager should do its job.
        /// </summary>
        void Execute();
    }
}
