using System;
using WiseQueue.Core.Common.DataContexts;
using WiseQueue.Core.Common.Logging;
using WiseQueue.Core.Common.Management;
using WiseQueue.Core.Common.Models.Servers;

namespace WiseQueue.Domain.Common.Management
{
    class ServerManager : BaseMultiThreadManager, IServerManager
    {
        #region Fields...

        /// <summary>
        /// The <see cref="IServerDataContext"/> instance.
        /// </summary>
        private readonly IServerDataContext serverDataContext;

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
        }

        #region Working thread...

        /// <summary>
        /// Occurs when some work should be done in the working thread.
        /// </summary>
        protected override void OnWorkingThreadIteration()
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

        /// <summary>
        /// Occurs before exit from the working thread.
        /// </summary>
        protected override void OnWorkingThreadExit()
        {
            serverDataContext.DeleteServer(ServerId);
        }
        #endregion

        #region Start method...

        /// <summary>
        /// Occurs when manager is staring.
        /// </summary>
        protected override void OnStart()
        {
            string serverName = Guid.NewGuid().ToString(); //TODO: Server's name should be more informative than Guid :)
            string description = "This is a description";
                //TODO: Server's description should be more informative than current one.

            ServerModel serverModel = new ServerModel(serverName, description, heartbeatLifetime);
            ServerId = serverDataContext.InsertServer(serverModel);
        }

        #endregion
    }
}
