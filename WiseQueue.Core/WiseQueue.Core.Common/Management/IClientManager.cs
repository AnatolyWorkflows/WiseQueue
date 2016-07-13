using System;
using System.Linq.Expressions;

namespace WiseQueue.Core.Common.Management
{
    /// <summary>
    /// Interface shows that <c>object</c> is a client manager.
    /// </summary>
    public interface IClientManager
    {
        /// <summary>
        /// Start new <c>task</c>.
        /// </summary>
        /// <param name="task">The <see cref="Expression"/> instance.</param>
        /// <returns>The task's identifier.</returns>
        Int64 StartTask(Expression<Action> task);
    }
}
