using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Common.Core.BaseClasses;
using Common.Core.Interfaces;
using Common.Core.Logging;
using WiseQueue.Core.Common.Management;
using WiseQueue.Core.Common.Models;

namespace WiseQueue.Domain.Common.Management
{
    public sealed class MainManagerManager : BaseLoggerObject, IMainManagerManager
    {
        #region Fields...
        /// <summary>
        /// List of managers that will be controlling by this object.
        /// </summary>
        private readonly List<IManager> managers;

        /// <summary>
        /// Cancellation token source.
        /// </summary>
        private CancellationTokenSource tokenSource;

        /// <summary>
        /// <c>Task</c> where all work will be run.
        /// </summary>
        private Task worker;

        /// <summary>
        /// Sending heartbeat interval.
        /// </summary>
        private readonly TimeSpan heartbeatLifetime; //TODO: Should be different for each manager.
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        public MainManagerManager(ICommonLoggerFactory loggerFactory) : base(loggerFactory)
        {
            managers = new List<IManager>();
            heartbeatLifetime = TimeSpan.FromSeconds(20); //TODO: Move to settings.
        }

        #region Working thread...

        /// <summary>
        /// Occurs when some work should be done in the working thread.
        /// </summary>
        private void OnWorkingThreadIteration()
        {            
            TimeSpan totalExecutionTime = TimeSpan.Zero;
            foreach (IManager manager in managers)
            {
                logger.WriteTrace("Executing {0}...", manager);
                try
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    if (manager is IExecutable)
                        (manager as IExecutable).Execute();
                    stopwatch.Stop();
                    totalExecutionTime = totalExecutionTime.Add(stopwatch.Elapsed);
                    logger.WriteTrace("The {0} has been executed. Time: {1}", manager, stopwatch.Elapsed);

                    if (stopwatch.Elapsed.TotalSeconds > 5.0f)
                    {
                        for (int i = 0; i < 50; i++)
                        {
                            logger.WriteError("!!!!!!!!!!!!!!!!!!! stopwatch.Elapsed.TotalSeconds > 5.0f");
                        }                        
                    }

                }
                catch (Exception ex)
                {
                    logger.WriteError(ex, "There was an exception during {0} executing.", manager);
                }
            }
        }

        /// <summary>
        /// Occurs before exit from the working thread.
        /// </summary>
        /// <remarks>Should be overrode in children classes if needed.</remarks>
        private void OnWorkingThreadExit()
        {
            try
            {
                //TODO: Do some stuff before exit.
            }
            catch (Exception ex)
            {
                logger.WriteError(ex, "There was an exception during exiting from the working thread. Skip it and exit anyway.");
            }
        }

        /// <summary>
        /// Working thread.
        /// </summary>
        /// <param name="token">The cancellation <c>token</c>.</param>
        private void WorkingThread(CancellationToken token)
        {
            while (token.IsCancellationRequested == false)
            {
                logger.WriteTrace("Running execution of single step in the working thread...");

                try
                {
                    OnWorkingThreadIteration();
                    logger.WriteTrace("The step has been executed.");
                }
                catch (Exception ex)
                {
                    logger.WriteError(ex, "There was an exception during executing. Do nothing.");
                }

                token.WaitHandle.WaitOne(1 * 1000); //TODO: Move to settings. This time should be less than heartbeatLifetime
            }

            logger.WriteTrace("Exiting working thread...");

            OnWorkingThreadExit();                

            logger.WriteTrace("The thread has been exited.");
        }
        #endregion

        #region Implementation of IMultithreadManager

        /// <summary>
        /// Start manager.
        /// </summary>
        public void Start()
        {
            try
            {
                logger.WriteInfo("Starting main manager...");

                foreach (IManager manager in managers)
                {
                    if (manager is IStartStoppable)
                    {
                        logger.WriteTrace("Starting {0}...", manager);
                        (manager as IStartStoppable).Start();
                        logger.WriteTrace("The {0} has been started.", manager);
                    }
                }

                tokenSource = new CancellationTokenSource();
                CancellationToken token = tokenSource.Token;
                worker = Task.Run(() => WorkingThread(token), token);                

                logger.WriteInfo("The main manager has been started.");
            }
            catch (Exception ex)
            {
                logger.WriteError(ex, "There was an exception during starting main manager.");
                Stop();
                throw;
            }
        }        

        /// <summary>
        /// Stop manager.
        /// </summary>
        public void Stop()
        {            
            if (tokenSource != null)
            {
                logger.WriteInfo("Stopping main manager...");

                try
                {
                    logger.WriteTrace("Stopping working thread...");
                    tokenSource.Cancel();
                    worker.Wait(); //TODO: Be should that working task will be finished every time.

                    foreach (IManager manager in managers)
                    {
                        if (manager is IStartStoppable)
                        {
                            logger.WriteTrace("Stopping {0}...", manager);
                            (manager as IStartStoppable).Stop();
                            logger.WriteTrace("The {0} has been stopped.", manager);
                        }
                    }

                    logger.WriteTrace("The working thread has been stopped.");
                }
                catch (Exception ex)
                {
                    logger.WriteError(ex, "There was an exception during st0pping the working thread. Skip it and continue anyway.");
                }

                tokenSource.Dispose();
                tokenSource = null;

                logger.WriteInfo("The manager has been stopped.");
            }           
        }

        /// <summary>
        /// Register a new manager.
        /// </summary>
        /// <param name="manager">The IManager instance.</param>
        [Obsolete("This method will be removed because it should be automatic registration.")]
        public void Register(IManager manager)
        {
            managers.Add(manager);
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Stop();  
        }

        #endregion
    }
}
