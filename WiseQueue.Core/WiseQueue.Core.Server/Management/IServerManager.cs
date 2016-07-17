using System;
using Common.Core.Interfaces;
using WiseQueue.Core.Common.Management;

namespace WiseQueue.Core.Server.Management
{
    /// <summary>
    /// Interface shows that <c>object</c> is a server manager.
    /// </summary>
    public interface IServerManager : IManager, IStartStoppable, IExecutable
    {
        /// <summary>
        /// The server identifier.
        /// </summary>
        Int64 ServerId { get; }
    }
}
