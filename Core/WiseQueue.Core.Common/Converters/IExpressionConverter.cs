using System;
using System.Linq.Expressions;
using WiseQueue.Core.Common.Models.Tasks;

namespace WiseQueue.Core.Common.Converters
{
    /// <summary>
    /// Interface shows that object is a converter from <see cref="Expression"/> into the <see cref="TaskActivationDetailsModel"/> and back.
    /// </summary>
    public interface IExpressionConverter
    {
        /// <summary>
        /// Convert <see cref="Expression"/> into the <see cref="TaskActivationDetailsModel"/> instance.
        /// </summary>
        /// <param name="action">The <see cref="Expression"/> instance.</param>
        /// <returns>The <see cref="TaskActivationDetailsModel"/> instance.</returns>
        TaskActivationDetailsModel Convert(Expression<Action> action);
    }
}
