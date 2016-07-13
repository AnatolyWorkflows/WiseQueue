using System;
using Common.Core.BaseClasses;
using Common.Core.Logging;
using WiseQueue.Core.Common.Converters.EntityModelConverters;
using WiseQueue.Core.Common.Entities;
using WiseQueue.Core.Common.Models;

namespace WiseQueue.Domain.Common.Converters.EntityModelConverters
{
    /// <summary>
    /// Converting Queue entity into the Queue model and back.
    /// </summary>
    public class QueueConverter : BaseLoggerObject, IQueueConverter
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        public QueueConverter(ICommonLoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        #region Implementation of IQueueConverter

        /// <summary>
        /// Convert task <c>entity</c> into the task model.
        /// </summary>
        /// <param name="entity">The <see cref="QueueEntity"/> instance.</param>
        /// <returns>The <see cref="QueueModel"/> instance.</returns>
        public QueueModel Convert(QueueEntity entity)
        {
            QueueModel model = entity.Id > 0
                ? new QueueModel(entity.Id, entity.Name, entity.Description)
                : new QueueModel(entity.Name, entity.Description);

            return model;
        }

        /// <summary>
        /// Convert task <c>model</c> into the task entity.
        /// </summary>
        /// <param name="model">The <see cref="QueueModel"/> instance.</param>
        /// <returns>The <see cref="QueueEntity"/> instance.</returns>
        public QueueEntity Convert(QueueModel model)
        {
            QueueEntity entity = new QueueEntity
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description
            };

            return entity;
        }

        #endregion
    }
}
