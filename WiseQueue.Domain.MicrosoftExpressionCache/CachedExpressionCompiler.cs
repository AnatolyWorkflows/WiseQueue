using System;
using System.Linq.Expressions;
using WiseQueue.Core.Common.Caching;
using WiseQueue.Core.Common.Logging;
using WiseQueue.Core.Common.Models;

namespace WiseQueue.Domain.MicrosoftExpressionCache
{
    /// <summary>
    /// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
    /// https://github.com/mono/aspnetwebstack/tree/master/src/Microsoft.Web.Mvc/ExpressionUtil
    /// </summary>
    public class CachedExpressionCompiler : BaseLoggerObject, ICachedExpressionCompiler
    {
        private static readonly ParameterExpression UnusedParameterExpr = Expression.Parameter(typeof(object), "_unused");

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory">The <see cref="IWiseQueueLoggerFactory"/> instance.</param>
        public CachedExpressionCompiler(IWiseQueueLoggerFactory loggerFactory) : base(loggerFactory)
        {
        }        

        /// <summary>
        /// Evaluates an expression (not a <see cref="LambdaExpression"/>).
        /// </summary>
        /// <param name="arg">The Expression instance.</param>
        /// <returns><see cref="Expression"/> result.</returns>
        private object Evaluate(Expression arg)
        {
            if (arg == null)
                throw new ArgumentNullException("arg");

            Func<object, object> func = Wrap(arg);
            return func(null);
        }

        /// <summary>
        /// Wrap <see cref="Expression"/> into the <see cref="Func{T,TResult}"/>
        /// </summary>
        /// <param name="arg">The Expression instance.</param>
        /// <returns>The <see cref="Func{T,TResult}"/> instance.</returns>
        private Func<object, object> Wrap(Expression arg)
        {
            Expression<Func<object, object>> lambdaExpr = Expression.Lambda<Func<object, object>>(Expression.Convert(arg, typeof(object)), UnusedParameterExpr);
            return ExpressionUtil.CachedExpressionCompiler.Process(lambdaExpr);
        }

        #region Implementation of ICachedExpressionCompiler

        /// <summary>
        /// Get an <c>expression</c> result.
        /// </summary>
        /// <param name="expression">The <see cref="Expression"/> instance.</param>
        /// <returns>The value.</returns>
        public object GetValue(Expression expression)
        {
            var constantExpression = expression as ConstantExpression;

            return constantExpression != null
                    ? constantExpression.Value
                    : Evaluate(expression);
        }

        #endregion
    }
}
