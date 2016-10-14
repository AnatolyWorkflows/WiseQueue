using System;
using System.Collections.Generic;
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

        MethodInfo GetNonOpenMatchingMethod(Type instanceType, string methodName, Type[] argumentTypes);

        /// <summary>
        /// Serialize arguments into the array of string.
        /// </summary>
        /// <param name="arguments">List of arguments.</param>
        /// <returns>Array of string</returns>
        string[] SerializeArguments(IReadOnlyCollection<object> arguments);

        /// <summary>
        /// Deserialize arguments using method info and serialized arguments.
        /// </summary>
        /// <param name="methodInfo">The MethodInfo instance.</param>
        /// <param name="serializedArguments">Array of string that contains serialized arguments.</param>
        /// <returns>List of arguments.</returns>
        object[] DeserializeArguments(MethodInfo methodInfo, string[] serializedArguments);
    }
}
