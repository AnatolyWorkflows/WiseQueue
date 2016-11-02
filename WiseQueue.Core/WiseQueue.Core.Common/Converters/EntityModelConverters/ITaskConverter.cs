using WiseQueue.Core.Common.Entities.Tasks;
using WiseQueue.Core.Common.Models.Tasks;

namespace WiseQueue.Core.Common.Converters.EntityModelConverters
{
    /// <summary>
    /// Interface shows that <c>object</c> can convert <see cref="WiseQueue.Core.Common.Models.Tasks.TaskStateModel"/> into the <see cref="TaskEntity"/> and back.
    /// </summary>
    public interface ITaskConverter
    {
        /// <summary>
        /// Convert task entity into the task model.
        /// </summary>
        /// <param name="taskEntity">The <see cref="TaskEntity"/> instance.</param>
        /// <returns>The <see cref="WiseQueue.Core.Common.Models.Tasks.TaskStateModel"/> instance.</returns>
        TaskModel Convert(TaskEntity taskEntity);

        /// <summary>
        /// Convert task model into the task entity.
        /// </summary>
        /// <param name="taskStateModel <see cref="WiseQueue.Core.Common.Models.Tasks.TaskStateModel"/> instance.</param>
        /// <returns>The <see cref="TaskEntity"/> instance.</returns>
        TaskEntity Convert(TaskModel taskStateModel);
    }
}
