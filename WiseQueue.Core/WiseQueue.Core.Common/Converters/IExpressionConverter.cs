using System;
using System.Linq.Expressions;
using System.Reflection;
using WiseQueue.Core.Common.Models.Tasks;

namespace WiseQueue.Core.Common.Converters
{
    /// <summary>
    /// Interface shows that object is a converter from <see cref="Expression"/> into the <see cref="ActivationData"/> and back.
    /// </summary>
    public interface IExpressionConverter
    {
        /// <summary>
        /// Convert <see cref="Expression"/> into the <see cref="ActivationData"/> instance.
        /// </summary>
        /// <param name="action">The <see cref="Expression"/> instance.</param>
        /// <returns>The <see cref="ActivationData"/> instance.</returns>
        ActivationData Convert(Expression<Action> action);

        MethodInfo GetNonOpenMatchingMethod(Type instanceType, string method, Type[] argumentTypes);
        object[] DeserializeArguments(MethodInfo method, string[] serializedArguments);
    }
}
