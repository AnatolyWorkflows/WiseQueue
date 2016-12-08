using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Core.Logging;
using WiseQueue.Core.Common.DataContexts;
using WiseQueue.Core.Common.Management.Implementation;
using WiseQueue.Core.Common.Management.TaskManagment;
using WiseQueue.Core.Common.Models;
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

        /// <summary>
        /// The <see cref="IQueueDataContext"/> instance.
        /// </summary>
        private readonly IQueueDataContext queueDataContext;

        private readonly ITaskManager taskManager;

        private readonly TimeSpan heartbeatLifetime;
        private readonly TimeSpan sleepTime;

        private readonly CancellationTokenSource heartbetCancelationCancellationTokenSource;
        private readonly Task serverHeartbetTask;

        private readonly ServerModel serverModel;
        private readonly List<QueueModel> queues;

        #endregion

        #region Properties...

        /// <summary>
        /// The server identifier.
        /// </summary>
        public Int64 ServerId
        {
            get { return serverModel.Id; }
        }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="serverDataContext">The <see cref="IServerDataContext"/> instance.</param>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="serverDataContext"/> is <see langword="null"/></exception>
        public ServerManager(IServerDataContext serverDataContext, IQueueDataContext queueDataContext, ITaskManager taskManager, ICommonLoggerFactory loggerFactory)
            : base("Server Manager", loggerFactory)
        {
            if (serverDataContext == null)
                throw new ArgumentNullException(nameof(serverDataContext));
            if (queueDataContext == null)
                throw new ArgumentNullException(nameof(queueDataContext));
            if (taskManager == null)
                throw new ArgumentNullException(nameof(taskManager));


            this.serverDataContext = serverDataContext;
            this.queueDataContext = queueDataContext;
            this.taskManager = taskManager;
            heartbeatLifetime = TimeSpan.FromSeconds(15); //Move to settings.

            //5 seconds for connection to the SQL.
            sleepTime = TimeSpan.FromSeconds(heartbeatLifetime.Seconds - 10);

            heartbetCancelationCancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = heartbetCancelationCancellationTokenSource.Token;
            serverHeartbetTask = new Task(OnHeartbet, cancellationToken);

            string serverName = Guid.NewGuid().ToString(); //TODO: Server's name should be more informative than Guid :)
            string description = "This is a description"; //TODO: Server's description should be more informative than current one.
            serverModel = new ServerModel(serverName, description, heartbeatLifetime);

            //TODO: Populate queues from the configuration.
            queues = new List<QueueModel>
            {
                new QueueModel("default", "This is a default queue.")
            };
        }

        #region Implementation of IStartStoppable

        /// <summary>
        /// Start manager.
        /// </summary>
        public void Start()
        {
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
            bool serverIsOnline = false;

            while (cancellationToken.IsCancellationRequested == false)
            {
                try
                {
                    if (serverIsOnline)
                    {
                        logger.WriteDebug("Sending heartbeat for server id = {0}...", ServerId);
                        ServerHeartbeatModel serverHeartbeatModel = new ServerHeartbeatModel(ServerId, heartbeatLifetime);
                        serverDataContext.SendHeartbeat(serverHeartbeatModel);
                        logger.WriteDebug("The heartbeat has been sent.");

                        //Find servers that have been expired and delete them.
                        logger.WriteDebug("Finding and deleting servers that have been expired...");
                        int serverCount = serverDataContext.DeleteExpiredServers();
                        if (serverCount > 0)
                            logger.WriteDebug("There were(was) {0} servers that have(has) been expired. They were(was) deleted.", serverCount);
                        else
                            logger.WriteDebug("There was no any expired servers.");

                        //Execute TaskManager.
                        logger.WriteDebug("Executing task manager...");
                        taskManager.Execute();
                        logger.WriteDebug("The TaskManager has been executed.");
                    }
                    else
                    {
                        logger.WriteDebug("Preparing server for on-line...");

                        logger.WriteTrace("Inserting information about server...");
                        Int64 serverId = serverDataContext.InsertServer(serverModel);
                        logger.WriteTrace("Inserting information about queues...");
                        foreach (QueueModel queueModel in queues)
                        {
                            queueDataContext.InsertQueue(queueModel);
                        }                        
                        logger.WriteDebug("The server is on-line.");

                        serverIsOnline = true;
                        taskManager.SetOnlineStatus(true, serverId);
                    }
                }
                catch (Exception ex)
                {
                    logger.WriteError(ex, "The exception has been raised during heartbeat.");
                    serverIsOnline = false;
                    taskManager.SetOnlineStatus(false);
                }

                cancellationToken.WaitHandle.WaitOne(sleepTime);
            }            
        }


        /// <summary>
        /// Get default queue.
        /// </summary>
        /// <returns>The queue.</returns>
        public QueueModel GetDefaultQueue()
        {
            QueueModel defaultQueue = queueDataContext.GetQueueByName("default");

            return defaultQueue;
        }

        /// <summary>
        /// Get all queues that available for this server,
        /// </summary>
        /// <returns>List of queues.</returns>
        public IReadOnlyCollection<QueueModel> GetAvailableQueues()
        {
            throw new NotImplementedException();
        }
    }
}
