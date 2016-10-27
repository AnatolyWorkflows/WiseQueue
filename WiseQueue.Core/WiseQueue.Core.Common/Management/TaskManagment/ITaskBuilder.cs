using WiseQueue.Core.Common.Models.Tasks;

namespace WiseQueue.Core.Common.Management.TaskManagment
{
    /// <summary>
    /// Interface shows that object can build tasks.
    /// </summary>
    public interface ITaskBuilder
    {
        IRunningTask Build(TaskModel taskModel);
    }
}
