using System;

namespace WiseQueue.Core.Common.Entities.Server
{
    /// <summary>
    /// Server entity.
    /// </summary>
    public class ServerEntity
    {
        /// <summary>
        /// Server's identifier.
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// Server's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Server's description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// During <c>this</c> time next heartbeat should be sent.
        /// </summary>
        public TimeSpan HeartbeatLifetime { get; set; }
    }
}
