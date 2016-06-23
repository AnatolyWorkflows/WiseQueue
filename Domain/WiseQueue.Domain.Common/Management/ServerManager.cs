using System;
using WiseQueue.Core.Common.DataContexts;
using WiseQueue.Core.Common.Logging;
using WiseQueue.Core.Common.Management;
using WiseQueue.Core.Common.Models;
using WiseQueue.Core.Common.Models.Servers;

namespace WiseQueue.Domain.Common.Management
{
    class ServerManager : BaseLoggerObject, IServerManager
    {
        #region Fields...

        /// <summary>
        /// The <see cref="IServerDataContext"/> instance.
        /// </summary>
        private readonly IServerDataContext serverDataContext;

        private readonly TimeSpan heartbeatLifetime;

        #endregion


        #region Properties...

        /// <summary>
        /// The server identifier.
        /// </summary>
        public Int64 ServerId { get; private set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="serverDataContext">The <see cref="IServerDataContext"/> instance.</param>
        /// <param name="loggerFactory">The <see cref="IWiseQueueLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        public ServerManager(IServerDataContext serverDataContext, IWiseQueueLoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            if (serverDataContext == null)
                throw new ArgumentNullException("serverDataContext");

            this.serverDataContext = serverDataContext;
            heartbeatLifetime = TimeSpan.FromSeconds(15); //Move to settings.
        }

        #region Implementation of IManager

        /// <summary>
        /// Start manager.
        /// </summary>
        public void Start()
        {
            string serverName = Guid.NewGuid().ToString(); //TODO: Server's name should be more informative than Guid :)
            string description = "This is a description";
            //TODO: Server's description should be more informative than current one.

            ServerModel serverModel = new ServerModel(serverName, description, heartbeatLifetime);
            ServerId = serverDataContext.InsertServer(serverModel);
        }

        /// <summary>
        /// Stop.
        /// </summary>
        public void Stop()
        {
            serverDataContext.DeleteServer(ServerId);
        }

        /// <summary>
        /// Calling this function if manager should do its job.
        /// </summary>
        public void Execute()
        {
            logger.WriteTrace("Sending heartbeat for server id = {0}...", ServerId);
            ServerHeartbeatModel serverHeartbeatModel = new ServerHeartbeatModel(ServerId, heartbeatLifetime);
            serverDataContext.SendHeartbeat(serverHeartbeatModel);
            logger.WriteTrace("The heartbeat has been sent.");

            //Find servers that have been expired and delete them.
            logger.WriteTrace("Finding and deleting servers that have been expired...");
            int serverCount = serverDataContext.DeleteExpiredServers();
            if (serverCount > 0)
                logger.WriteTrace("There were(was) {0} servers that have(has) been expired. They were(was) deleted.",
                    serverCount);
            else
                logger.WriteTrace("There was no any expired servers.");
        }

        #endregion
    }
}
