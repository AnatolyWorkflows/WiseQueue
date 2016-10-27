using System;
using WiseQueue.Core.Common.Models.Tasks;

namespace WiseQueue.Core.Common.Management.TaskManagment
{
    /// <summary>
    /// Interface shows that object is a running task and contains all neccessary methods.
    /// </summary>
    public interface IRunningTask
    {
        TaskModel TaskModel { get; }

        EventHandler<RunningTaskEventArg> OnCompletedEventHandler { get; set; }

        /// <summary>
        /// Execute task.
        /// </summary>
        void Execute();

        /// <summary>
        /// Cancel task execution.
        /// </summary>
        void Cancel();
    }
}
