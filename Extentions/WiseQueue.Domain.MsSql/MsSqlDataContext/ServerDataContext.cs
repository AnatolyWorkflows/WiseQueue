using System;
using WiseQueue.Core.Common.DataContexts;
using WiseQueue.Core.Common.Logging;
using WiseQueue.Core.Common.Models;
using WiseQueue.Core.Common.Models.Servers;

namespace WiseQueue.Domain.MsSql.MsSqlDataContext
{
    class ServerDataContext: BaseLoggerObject, IServerDataContext
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory">The <see cref="IWiseQueueLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        public ServerDataContext(IWiseQueueLoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        #region Implementation of IServerDataContext

        /// <summary>
        /// Insert a new record about a server.
        /// </summary>
        /// <param name="serverModel">The server.</param>
        /// <returns>The server's identifier that has been inserted.</returns>
        public long InsertServer(ServerModel serverModel)
        {
            return 5;
        }

        /// <summary>
        /// Delete server information form the database using server's identifier.
        /// </summary>
        /// <param name="serverId">The server identifier.</param>
        public void DeleteServer(long serverId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Send a heartbeat from the server.
        /// </summary>
        /// <param name="serverHeartbeatModel">The heartbeat information.</param>
        public void SendHeartbeat(ServerHeartbeatModel serverHeartbeatModel)
        {
            
        }

        #endregion
    }
}
