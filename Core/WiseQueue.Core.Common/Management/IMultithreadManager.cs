using System;

namespace WiseQueue.Core.Common.Management
{
    /// <summary>
    /// Interface shows that object has thread where some work will be done.
    /// </summary>
    public interface IMultithreadManager : IDisposable
    {
        /// <summary>
        /// Start manager.
        /// </summary>
        void Start();

        /// <summary>
        /// Stop manager.
        /// </summary>
        void Stop();
    }
}
