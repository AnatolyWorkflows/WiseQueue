using System;
using WiseQueue.Core.Common.Models.Servers;

namespace WiseQueue.Core.Common.DataContexts
{
    /// <summary>
    /// Interface shows that <c>object</c> is a server data context.
    /// </summary>
    public interface IServerDataContext
    {
        /// <summary>
        /// Insert a new record about a server.
        /// </summary>
        /// <param name="model">The server.</param>
        /// <returns>The server's identifier that has been inserted.</returns>
        Int64 InsertServer(ServerModel model);

        /// <summary>
        /// Delete server information form the database using server's identifier.
        /// </summary>
        /// <param name="serverId">The server identifier.</param>
        void DeleteServer(Int64 serverId);

        /// <summary>
        /// Send a heartbeat from the server.
        /// </summary>
        /// <param name="serverHeartbeatModel">The heartbeat information.</param>
        void SendHeartbeat(ServerHeartbeatModel serverHeartbeatModel);

        /// <summary>
        /// Delete servers that have been expired.
        /// </summary>
        /// <param name="currentServerId">Current server identifier. It needs because server should delete itself.</param>
        /// <returns>Count of servers that have been deleted.</returns>
        int DeleteExpiredServers(Int64 currentServerId);
    }
}
