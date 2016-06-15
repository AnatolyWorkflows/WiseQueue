using System;

namespace WiseQueue.Core.Common.Entities.Server
{
    /// <summary>
    /// Server heartbeat entity.
    /// </summary>
    public class ServerHeartbeatEntity
    {
        /// <summary>
        /// The server's identifier.
        /// </summary>
        public long ServerId { get; set; }

        /// <summary>
        /// During <c>this</c> time next heartbeat should be sent.
        /// </summary>
        public TimeSpan HeartbeatLifetime { get; set; }
    }
}
