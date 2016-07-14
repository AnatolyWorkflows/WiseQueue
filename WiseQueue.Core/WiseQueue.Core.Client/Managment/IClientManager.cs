using System;
using System.Linq.Expressions;
using WiseQueue.Core.Common.Management;

namespace WiseQueue.Core.Client.Managment
{
    /// <summary>
    /// Interface shows that <c>object</c> is a client manager.
    /// </summary>
    public interface IClientManager : IManager
    {
        /// <summary>
        /// Start new <c>task</c>.
        /// </summary>
        /// <param name="task">The <see cref="Expression"/> instance.</param>
        /// <returns>The task's identifier.</returns>
        Int64 StartTask(Expression<Action> task);
    }
}
