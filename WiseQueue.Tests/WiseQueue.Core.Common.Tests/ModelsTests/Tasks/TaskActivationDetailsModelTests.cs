//using System;
//using System.Reflection;
//using System.Reflection.Emit;
//using NUnit.Framework;
//using WiseQueue.Core.Common.Models.Tasks;

//namespace WiseQueue.Core.Common.Tests.ModelsTests.Tasks
//{
//    [TestFixture]
//    public class ActivationDataTests
//    {
//        [Test]
//        public void ActivationDataConstructorInstanceTypeIsNullTest()
//        {
//            Type instanceType = null;
//            MethodInfo method = GetType().GetMethod("ActivationDataConstructorInstanceTypeIsNullTest");
//            Type[] parametersTypes = Guid.NewGuid().ToString();
//            string arguments = Guid.NewGuid().ToString();

//            ArgumentNullException exception =
//                Assert.Throws<ArgumentNullException>(
//                    () => new ActivationData(instanceType, method, parametersTypes, arguments));

//            Assert.AreEqual("instanceType", exception.ParamName);
//        }

//        [Test]
//        public void ActivationDataConstructorMethodIsNullTest()
//        {
//            string instanceType = Guid.NewGuid().ToString();
//            string method = null;
//            string parametersTypes = Guid.NewGuid().ToString();
//            string arguments = Guid.NewGuid().ToString();

//            ArgumentNullException exception =
//                Assert.Throws<ArgumentNullException>(
//                    () => new ActivationData(instanceType, method, parametersTypes, arguments));

//            Assert.AreEqual("method", exception.ParamName);
//        }

//        [Test]
//        public void ActivationDataConstructorParametersTypesIsNullTest()
//        {
//            string instanceType = Guid.NewGuid().ToString();
//            string method = Guid.NewGuid().ToString();
//            string parametersTypes = null;
//            string arguments = Guid.NewGuid().ToString();

//            ArgumentNullException exception =
//                Assert.Throws<ArgumentNullException>(
//                    () => new ActivationData(instanceType, method, parametersTypes, arguments));

//            Assert.AreEqual("parametersTypes", exception.ParamName);
//        }

//        [Test]
//        public void ActivationDataConstructorArgumentsIsNullTest()
//        {
//            string instanceType = Guid.NewGuid().ToString();
//            string method = Guid.NewGuid().ToString();
//            string parametersTypes = Guid.NewGuid().ToString();
//            string arguments = null;

//            ArgumentNullException exception =
//                Assert.Throws<ArgumentNullException>(
//                    () => new ActivationData(instanceType, method, parametersTypes, arguments));

//            Assert.AreEqual("arguments", exception.ParamName);
//        }

//        [Test]
//        public void ActivationDataConstructorTest()
//        {
//            string instanceType = Guid.NewGuid().ToString();
//            string method = Guid.NewGuid().ToString();
//            string parametersTypes = Guid.NewGuid().ToString();
//            string arguments = Guid.NewGuid().ToString();
//            ActivationData item = new ActivationData(instanceType, method, parametersTypes,
//                arguments);
//            Assert.IsNotNull(item);
//        }
//    }
//}
