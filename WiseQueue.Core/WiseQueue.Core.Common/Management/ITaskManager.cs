using System;
using System.Linq.Expressions;
using Common.Core.Interfaces;

namespace WiseQueue.Core.Common.Management
{
    /// <summary>
    /// Interface shows that <c>object</c> is a task manager.
    /// </summary>
    public interface ITaskManager: IManager, IExecutable
    {
        /// <summary>
        /// StartTask a new <c>task</c>.
        /// </summary>
        /// <param name="task">The <c>task</c>.</param>
        /// <returns>The task's identifier.</returns>
        Int64 StartTask(Expression<Action> task);
    }
}
