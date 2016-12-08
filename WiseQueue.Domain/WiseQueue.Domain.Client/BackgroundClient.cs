using System;
using System.Linq.Expressions;
using WiseQueue.Core.Client.Managment;
using WiseQueue.Domain.Common;

namespace WiseQueue.Domain.Client
{
    public static class BackgroundClient
    {
        public static Int64 StartNewTask(Expression<Action> task)
        {
            var clientManager = WiseQueueGlobalConfiguration.WiseQueueConfiguration.Get<IClientManager>();
            return clientManager.StartTask(task);
        }
    }
}
