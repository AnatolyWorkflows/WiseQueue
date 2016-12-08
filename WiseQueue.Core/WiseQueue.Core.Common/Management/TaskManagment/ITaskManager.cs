using System;
using Common.Core.Interfaces;

namespace WiseQueue.Core.Common.Management.TaskManagment
{
    /// <summary>
    /// Interface shows that <c>object</c> is a task manager.
    /// </summary>
    public interface ITaskManager: IManager, IExecutable
    {
        /// <summary>
        /// Set on-line status.
        /// </summary>
        /// <param name="isOnline">Flag shows that server is only or not.</param>
        /// <param name="serverId">If server is on-line that here should be server identifier. Otherwise, the parameter will be ignored</param>
        void SetOnlineStatus(bool isOnline, Int64 serverId = 0);
    }
}
