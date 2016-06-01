using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WiseQueue.Core.Tests;
using WiseQueue.Domain.MicrosoftExpressionCache;

namespace WiseQueue.Domain.Extensions.Tests
{
    [TestClass]
    public class CachedExpressionCompilerTests: BaseTestWithLogger
    {
        [TestMethod]
        public void CachedExpressionCompilerConstructorTest()
        {
            CachedExpressionCompiler cachedExpressionCompiler = new CachedExpressionCompiler(LoggerFactory);
            Assert.IsNotNull(cachedExpressionCompiler);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CachedExpressionCompilerConstructorWithNullParameterTest()
        {
            CachedExpressionCompiler cachedExpressionCompiler = new CachedExpressionCompiler(null);
            Assert.IsNull(cachedExpressionCompiler);
        }

        [TestMethod]
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
