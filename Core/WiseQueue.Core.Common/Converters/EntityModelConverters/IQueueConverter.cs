using WiseQueue.Core.Common.Entities;
using WiseQueue.Core.Common.Models;

namespace WiseQueue.Core.Common.Converters.EntityModelConverters
{
    /// <summary>
    /// Interface shows that <c>object</c> can convert <see cref="QueueModel"/> into the <see cref="QueueEntity"/> and back.
    /// </summary>
    public interface IQueueConverter
    {
        /// <summary>
        /// Convert task <c>entity</c> into the task model.
        /// </summary>
        /// <param name="entity">The <see cref="QueueEntity"/> instance.</param>
        /// <returns>The <see cref="QueueModel"/> instance.</returns>
        QueueModel Convert(QueueEntity entity);

        /// <summary>
        /// Convert task <c>model</c> into the task entity.
        /// </summary>
        /// <param name="model">The <see cref="QueueModel"/> instance.</param>
        /// <returns>The <see cref="QueueEntity"/> instance.</returns>
        QueueEntity Convert(QueueModel model);
    }
}
