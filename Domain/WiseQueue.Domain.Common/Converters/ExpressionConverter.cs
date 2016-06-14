using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using WiseQueue.Core.Common.Caching;
using WiseQueue.Core.Common.Converters;
using WiseQueue.Core.Common.Logging;
using WiseQueue.Core.Common.Models;
using WiseQueue.Core.Common.Models.Tasks;

namespace WiseQueue.Domain.Common.Converters
{
    /// <summary>
    /// <c>Convert</c> <see cref="Expression"/> into the <see cref="TaskActivationDetailsModel"/> and back.
    /// </summary>
    public class ExpressionConverter : BaseLoggerObject, IExpressionConverter
    {
        #region Fields...
        /// <summary>
        /// The <see cref="IJsonConverter"/> instance.
        /// </summary>
        private readonly IJsonConverter jsonConverter;

        /// <summary>
        /// The <see cref="ICachedExpressionCompiler"/> instance.
        /// </summary>
        private readonly ICachedExpressionCompiler cachedExpressionCompiler;
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="jsonConverter">The <see cref="IJsonConverter"/> instance.</param>
        /// <param name="cachedExpressionCompiler">The <see cref="ICachedExpressionCompiler"/> instance.</param>
        /// <param name="loggerFactory">The <see cref="IWiseQueueLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        public ExpressionConverter(IJsonConverter jsonConverter, ICachedExpressionCompiler cachedExpressionCompiler, IWiseQueueLoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            if (jsonConverter == null) throw new ArgumentNullException("jsonConverter");
            if (cachedExpressionCompiler == null) throw new ArgumentNullException("cachedExpressionCompiler");
            this.jsonConverter = jsonConverter;
            this.cachedExpressionCompiler = cachedExpressionCompiler;
        }

        #region Implementation of IExpressionConverter

        #region Serialize helpful methods...

        /// <summary>
        /// Serialize arguments into JSON format.
        /// </summary>
        /// <param name="expressions">Arguments.</param>
        /// <returns><c>List</c> of string that contain JSON information about arguments.</returns>
        private string[] SerializeArguments(IReadOnlyCollection<Expression> expressions)
        {
            string[] result = new string[expressions.Count];
            for (int i = 0; i < expressions.Count; i++)
            {
                Expression expression = expressions.ElementAt(i);
                var item = cachedExpressionCompiler.GetValue(expression);
                string jsonItem = jsonConverter.ConvertToJson(item);
                result[i] = jsonItem;
            }

            return result;
        }
        #endregion

        /// <summary>
        /// Convert <see cref="Expression"/> into the <see cref="TaskActivationDetailsModel"/> instance.
        /// </summary>
        /// <param name="action">The <see cref="Expression"/> instance.</param>
        /// <returns>The <see cref="TaskActivationDetailsModel"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">Expression body should be of type `MethodCallExpression`</exception>
        /// <exception cref="InvalidOperationException">Expression object should be not null.</exception>
        public TaskActivationDetailsModel Convert(Expression<Action> action)
        {
            logger.WriteTrace("Converting Expression<Action> into the TaskActivationDetailsModel...");

            if (action == null)
                throw new ArgumentNullException("action");

            MethodCallExpression callExpression = action.Body as MethodCallExpression;
            if (callExpression == null)
                throw new ArgumentException("Expression body should be of type `MethodCallExpression`", "action");

            Type type;

            if (callExpression.Object != null)
            {
                var objectValue = cachedExpressionCompiler.GetValue(callExpression.Object);
                if (objectValue == null)
                    throw new InvalidOperationException("Expression object should be not null.");

                type = objectValue.GetType();
            }
            else
            {
                type = callExpression.Method.DeclaringType;
            }

            string[] arguments = SerializeArguments(callExpression.Arguments);

            MethodInfo method = callExpression.Method;

            string typeDetails = type.AssemblyQualifiedName;
            string methodDetails = method.Name;
            string parameterDetails = jsonConverter.ConvertToJson(method.GetParameters().Select(x => x.ParameterType).ToArray());
            string argumentDetails = jsonConverter.ConvertToJson(arguments);
            TaskActivationDetailsModel result = new TaskActivationDetailsModel(typeDetails, methodDetails, parameterDetails, argumentDetails);

            logger.WriteTrace("Converting Expression<Action> into the TaskActivationDetailsModel has been successfully completed.");

            return result;
        }

        #endregion
    }
}
