using System;

namespace WiseQueue.Domain.Common.Models.Servers
{
    /// <summary>
    /// Server's heartbeat model.
    /// </summary>
    public class ServerHeartbeatModel
    {
        #region Properties...
        /// <summary>
        /// The server's identifier.
        /// </summary>
        public long ServerId { get; private set; }

        /// <summary>
        /// During <c>this</c> time next heartbeat should be sent.
        /// </summary>
        public TimeSpan HeartbeatLifetime { get; private set; }
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="serverId">The server's identifier.</param>
        /// <param name="heartbeatLifetime">During <c>this</c> time next heartbeat should be sent.</param>
        public ServerHeartbeatModel(Int64 serverId, TimeSpan heartbeatLifetime)
        {
            ServerId = serverId;
            HeartbeatLifetime = heartbeatLifetime;
        }
    }
}
