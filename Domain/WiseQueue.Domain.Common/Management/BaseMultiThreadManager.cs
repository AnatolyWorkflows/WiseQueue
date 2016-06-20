using System;
using System.Threading;
using System.Threading.Tasks;
using WiseQueue.Core.Common.Logging;
using WiseQueue.Core.Common.Management;
using WiseQueue.Core.Common.Models;

namespace WiseQueue.Domain.Common.Management
{
    public abstract class BaseMultiThreadManager : BaseLoggerObject, IMultithreadManager
    {
        #region Fields...
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
        protected readonly TimeSpan heartbeatLifetime; //TODO: Should be different for each manager.
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory">The <see cref="IWiseQueueLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        protected BaseMultiThreadManager(IWiseQueueLoggerFactory loggerFactory) : base(loggerFactory)
        {            
            heartbeatLifetime = TimeSpan.FromSeconds(20); //TODO: Move to settings.
        }

        #region Working thread...

        /// <summary>
        /// Occurs when some work should be done in the working thread.
        /// </summary>
        protected abstract void OnWorkingThreadIteration();

        /// <summary>
        /// Occurs before exit from the working thread.
        /// </summary>
        /// <remarks>Should be overrode in children classes if needed.</remarks>
        protected virtual void OnWorkingThreadExit()
        {            
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

                token.WaitHandle.WaitOne(15 * 1000); //TODO: Move to settings. This time should be less than heartbeatLifetime
            }

            logger.WriteTrace("Exiting working thread...");

            try
            {
                OnWorkingThreadExit();                
            }
            catch (Exception ex)
            {
                logger.WriteError(ex, "There was an exception during exiting from the working thread. Skip it and exit anyway.");
            }

            logger.WriteTrace("The thread has been exited.");
        }
        #endregion

        #region Implementation of IMultithreadManager

        /// <summary>
        /// Occurs when manager is staring.
        /// </summary>
        /// <remarks>Should be overrode in children classes if needed.</remarks>
        protected virtual void OnStart()
        {            
        }

        /// <summary>
        /// Start manager.
        /// </summary>
        public void Start()
        {
            try
            {
                logger.WriteInfo("Starting manager...");

                OnStart();

                tokenSource = new CancellationTokenSource();
                CancellationToken token = tokenSource.Token;
                worker = Task.Run(() => WorkingThread(token), token);                

                logger.WriteInfo("The manager has been started.");
            }
            catch (Exception ex)
            {
                logger.WriteError(ex, "There was an exception during starting manager.");
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
                logger.WriteInfo("Stopping manager...");

                try
                {
                    logger.WriteTrace("Stopping working thread...");
                    tokenSource.Cancel();
                    worker.Wait(); //TODO: Be should that working task will be finished every time.
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
