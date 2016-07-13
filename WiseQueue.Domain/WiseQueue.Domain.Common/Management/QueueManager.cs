using System;
using System.Collections.Generic;
using Common.Core.Logging;
using WiseQueue.Core.Common.Converters.EntityModelConverters;
using WiseQueue.Core.Common.DataContexts;
using WiseQueue.Core.Common.Management;
using WiseQueue.Core.Common.Models;

namespace WiseQueue.Domain.Common.Management
{
    /// <summary>
    /// <c>Queue</c> manager. Its main responsibility is queues management.
    /// </summary>
    public class QueueManager : BaseLoggerObject, IQueueManager
    {
        #region Consts...
        /// <summary>
        /// Default queue name.
        /// </summary>
        private const string defaultQueueName = "default"; //TODO: Move to settings.
        #endregion

        #region Fields...

        /// <summary>
        /// The <see cref="IQueueConverter"/> instance.
        /// </summary>
        private readonly IQueueConverter queueConverter;

        /// <summary>
        /// The <see cref="IQueueDataContext"/> instance.
        /// </summary>
        private readonly IQueueDataContext queueDataContext;

        #endregion 

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="queueConverter">The <see cref="IQueueConverter"/> instance.</param>
        /// <param name="queueDataContext">The <see cref="IQueueDataContext"/> instance.</param>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>        
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        public QueueManager(IQueueConverter queueConverter, IQueueDataContext queueDataContext, ICommonLoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            if (queueConverter == null) 
                throw new ArgumentNullException("queueConverter");
            if (queueDataContext == null) 
                throw new ArgumentNullException("queueDataContext");

            this.queueConverter = queueConverter;
            this.queueDataContext = queueDataContext;
        }

        #region Implementation of IQueueManager

        /// <summary>
        /// Get default queue.
        /// </summary>
        /// <returns>The <see cref="QueueModel"/> instance.</returns>
        public QueueModel GetDefaultQueue()
        {
            QueueModel queueModel = queueDataContext.GetQueueByName(defaultQueueName);
            return queueModel;
        }

        /// <summary>
        /// Check queue by name.
        /// </summary>
        /// <param name="queueName">The queue's name.</param>
        /// <returns>The <see cref="QueueModel"/> instance. </returns>
        /// <remarks>
        /// Depends on settings:
        ///     1) If there is no queue Exception will be generated.
        ///     2) If there is no queue it will be created.
        /// </remarks>
        public QueueModel CheckQueueByName(string queueName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get queues that available for current server.
        /// </summary>
        /// <returns><c>List</c> of <see cref="QueueModel"/>.</returns>
        public IReadOnlyCollection<QueueModel> GetAvailableQueues()
        {
            QueueModel queueModel = queueDataContext.GetQueueByName(defaultQueueName);
            IReadOnlyCollection<QueueModel> result = new List<QueueModel>{queueModel};
            return result;
        }

        #endregion
    }
}
