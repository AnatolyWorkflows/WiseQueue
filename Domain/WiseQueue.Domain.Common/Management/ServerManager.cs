using System;
using System.Threading;
using System.Threading.Tasks;
using WiseQueue.Core.Common.DataContexts;
using WiseQueue.Core.Common.Logging;
using WiseQueue.Core.Common.Management;
using WiseQueue.Core.Common.Models;
using WiseQueue.Core.Common.Models.Servers;

namespace WiseQueue.Domain.Common.Management
{
    class ServerManager: BaseLoggerObject, IServerManager, IDisposable
    {
        private readonly IServerDataContext serverDataContext;
        private CancellationTokenSource tokenSource;

        private Task worker;

        private readonly TimeSpan heartbeatLifetime;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="serverDataContext">The <see cref="IServerDataContext"/> instance.</param>
        /// <param name="loggerFactory">The <see cref="IWiseQueueLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        public ServerManager(IServerDataContext serverDataContext, IWiseQueueLoggerFactory loggerFactory) : base(loggerFactory)
        {
            if (serverDataContext == null) 
                throw new ArgumentNullException("serverDataContext");

            this.serverDataContext = serverDataContext;

            heartbeatLifetime = TimeSpan.FromSeconds(20); //TODO: Move to settings.
        }

        private void WorkingThread(Int64 serverId, CancellationToken token)
        {
            while (token.IsCancellationRequested == false)
            {
                logger.WriteTrace("Sending heartbeat for server id = {0}...", serverId);
                ServerHeartbeatModel serverHeartbeatModel = new ServerHeartbeatModel(serverId, heartbeatLifetime);
                serverDataContext.SendHeartbeat(serverHeartbeatModel);
                logger.WriteTrace("The heartbeat has been sent.");

                //Find servers that have been expired and delete them.
                logger.WriteTrace("Finding and deleting servers that have been expired...");
                int serverCount = serverDataContext.DeleteExpiredServers();
                if (serverCount > 0 )
                    logger.WriteTrace("There were(was) {0} servers that have(has) been expired. They were(was) deleted.", serverCount);
                else
                    logger.WriteTrace("There was no any expired servers.");

                token.WaitHandle.WaitOne(15 * 1000); //TODO: Move to settings. This time should be less than heartbeatLifetime
            }

            serverDataContext.DeleteServer(serverId);
        }

        #region Implementation of IServerManager

        /// <summary>
        /// Start sever manager.
        /// </summary>
        public void Start()
        {
            string serverName = Guid.NewGuid().ToString(); //TODO: Server's name should be more informative than Guid :)
            string description = "This is a description"; //TODO: Server's description should be more informative than current one.
            
            ServerModel serverModel = new ServerModel(serverName, description, heartbeatLifetime);
            Int64 serverId = serverDataContext.InsertServer(serverModel);

            tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            worker = Task.Run(() => WorkingThread(serverId, token), token);
        }

        /// <summary>
        /// Stop server manager.
        /// </summary>
        public void Stop()
        {
            if (tokenSource != null)
            {
                tokenSource.Cancel();

                worker.Wait(); //TODO: Be should that working task will be finished every time.

                tokenSource.Dispose();
                tokenSource = null;
            }
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Stop();            
        }

        #endregion
    }
}
