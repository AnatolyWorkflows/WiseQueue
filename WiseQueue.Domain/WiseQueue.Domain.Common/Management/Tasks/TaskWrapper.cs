using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WiseQueue.Domain.Common.Management
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
