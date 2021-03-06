﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Core.Logging;
using WiseQueue.Core.Common.DataContexts;
using WiseQueue.Core.Common.Management.Implementation;
using WiseQueue.Core.Common.Models.Servers;
using WiseQueue.Core.Server.Management;

namespace WiseQueue.Domain.Server.Management
{
    public class ServerManager : BaseManager, IServerManager
    {
        #region Fields...

        /// <summary>
        /// The <see cref="IServerDataContext"/> instance.
        /// </summary>
        private readonly IServerDataContext serverDataContext;

        private readonly TimeSpan heartbeatLifetime;
        private readonly TimeSpan sleepTime;

        private readonly CancellationTokenSource heartbetCancelationCancellationTokenSource;
        private readonly Task serverHeartbetTask;

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
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="serverDataContext"/> is <see langword="null"/></exception>
        public ServerManager(IServerDataContext serverDataContext, ICommonLoggerFactory loggerFactory)
            : base("Server Manager", loggerFactory)
        {
            if (serverDataContext == null)
                throw new ArgumentNullException(nameof(serverDataContext));


            this.serverDataContext = serverDataContext;
            heartbeatLifetime = TimeSpan.FromSeconds(15); //Move to settings.

            //5 seconds for connection to the SQL.
            sleepTime = TimeSpan.FromSeconds(heartbeatLifetime.Seconds - 10);

            heartbetCancelationCancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = heartbetCancelationCancellationTokenSource.Token;
            serverHeartbetTask = new Task(OnHeartbet, cancellationToken);
        }

        #region Implementation of IStartStoppable

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

            serverHeartbetTask.Start();
        }

        /// <summary>
        /// Stop.
        /// </summary>
        public void Stop()
        {
            heartbetCancelationCancellationTokenSource.Cancel(false);
            heartbetCancelationCancellationTokenSource.Dispose();
            serverDataContext.DeleteServer(ServerId);
        }

        #endregion

        private void OnHeartbet(object state)
        {
            CancellationToken cancellationToken = (CancellationToken) state;

            while (cancellationToken.IsCancellationRequested == false)
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

                cancellationToken.WaitHandle.WaitOne(sleepTime);
            }            
        }
    }
}
