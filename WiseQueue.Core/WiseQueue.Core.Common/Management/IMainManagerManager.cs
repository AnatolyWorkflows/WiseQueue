using System;

namespace WiseQueue.Core.Common.Management
{
    /// <summary>
    /// Interface shows that <c>object</c> is a main manager that controlling all others managers.
    /// </summary>
    public interface IMainManagerManager : IDisposable
    {
        /// <summary>
        /// Start manager.
        /// </summary>
        void Start();

        /// <summary>
        /// Stop manager.
        /// </summary>
        void Stop();

        /// <summary>
        /// Register a new manager.
        /// </summary>
        /// <param name="manager">The IManager instance.</param>
        [Obsolete("This method will be removed because it should be automatic registration.")]
        void Register(IManager manager); //TODO: This should be automatic
    }
}
