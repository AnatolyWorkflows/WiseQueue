namespace WiseQueue.Core.Common.Management
{
    /// <summary>
    /// Interface shows that <c>object</c> is a server manager.
    /// </summary>
    public interface IServerManager
    {
        /// <summary>
        /// Start sever manager.
        /// </summary>
        void Start();

        /// <summary>
        /// Stop server manager.
        /// </summary>
        void Stop();
    }
}
