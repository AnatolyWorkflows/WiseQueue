using WiseQueue.Core.Common.Entities.Tasks;
using WiseQueue.Core.Common.Models.Tasks;

namespace WiseQueue.Core.Common.Converters.EntityModelConverters
{
    /// <summary>
    /// Interface shows that <c>object</c> can convert <see cref="TaskModel"/> into the <see cref="TaskEntity"/> and back.
    /// </summary>
    public interface ITaskConverter
    {
        /// <summary>
        /// Convert task entity into the task model.
        /// </summary>
        /// <param name="taskEntity">The <see cref="TaskEntity"/> instance.</param>
        /// <returns>The <see cref="TaskModel"/> instance.</returns>
        TaskModel Convert(TaskEntity taskEntity);

        /// <summary>
        /// Convert task model into the task entity.
        /// </summary>
        /// <param name="taskModel">The <see cref="TaskModel"/> instance.</param>
        /// <returns>The <see cref="TaskEntity"/> instance.</returns>
        TaskEntity Convert(TaskModel taskModel);
    }
}
