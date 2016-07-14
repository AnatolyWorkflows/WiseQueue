using System;
using WiseQueue.Core.Common.Management;

namespace WiseQueue.Core.Server.Managment
{
    /// <summary>
    /// Interface shows that <c>object</c> is a server manager.
    /// </summary>
    public interface IServerManager : IManager
    {
        /// <summary>
        /// The server identifier.
        /// </summary>
        Int64 ServerId { get; }
    }
}
