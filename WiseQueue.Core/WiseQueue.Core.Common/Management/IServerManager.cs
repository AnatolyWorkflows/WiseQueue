using System;
using Common.Core.Interfaces;

namespace WiseQueue.Core.Common.Management
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
