using System;
using System.Linq.Expressions;
using NUnit.Framework;
using WiseQueue.Core.Tests;
using WiseQueue.Domain.MicrosoftExpressionCache;

namespace WiseQueue.Domain.Extensions.Tests
{
    [TestFixture]
    public class CachedExpressionCompilerTests: BaseTestWithLogger
    {
        [Test]
        public void CachedExpressionCompilerConstructorTest()
        {
            CachedExpressionCompiler cachedExpressionCompiler = new CachedExpressionCompiler(LoggerFactory);
            Assert.IsNotNull(cachedExpressionCompiler);
        }

        [Test]
        public void CachedExpressionCompilerConstructorWithNullParameterTest()
        {
            ArgumentNullException exception =
                Assert.Throws<ArgumentNullException>(
                    () => new CachedExpressionCompiler(null));

            Assert.AreEqual("loggerFactory", exception.ParamName);
        }

        [Test]
        public void CachedExpressionCompilerTest()
        {
            CachedExpressionCompiler cachedExpressionCompiler = new CachedExpressionCompiler(LoggerFactory);

            Expression<Func<int, bool>> expr = i => i < 5;
            var obj = cachedExpressionCompiler.GetValue(expr);

            Func<int, bool> actual = (Func<int, bool>)obj;
            Assert.IsNotNull(actual);

            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(expr.Compile().Invoke(3), actual(3));   
            }            
        }
    }
}
