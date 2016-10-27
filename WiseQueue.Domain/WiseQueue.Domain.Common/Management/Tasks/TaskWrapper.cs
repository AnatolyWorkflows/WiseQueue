using System.Threading;
using System.Threading.Tasks;

namespace WiseQueue.Domain.Common.Management.Tasks
{
    class TaskWrapper
    {
        public Task Task { get; }

        public CancellationTokenSource TaskCancellationTokenSource { get; }

        public TaskWrapper(Task task, CancellationTokenSource taskCancellationTokenSource)
        {
            Task = task;
            TaskCancellationTokenSource = taskCancellationTokenSource;
        }
    }
}
