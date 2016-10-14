using WiseQueue.Core.Common;

namespace WiseQueue.Domain.Common.Management.Tasks
{
    public class TaskCancellationToken : ITaskCancellationToken
    {
        public static TaskCancellationToken Null
        {
            get { return new TaskCancellationToken(); }
        }
    }
}
