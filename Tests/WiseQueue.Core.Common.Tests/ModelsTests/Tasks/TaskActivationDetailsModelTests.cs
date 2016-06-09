using System;
using NUnit.Framework;
using WiseQueue.Core.Common.Models.Tasks;

namespace WiseQueue.Core.Common.Tests.ModelsTests.Tasks
{
    [TestFixture]
    public class TaskActivationDetailsModelTests
    {
        [Test]
        public void TaskActivationDetailsModelConstructorInstanceTypeIsNullTest()
        {
            string instanceType = null;
            string method = Guid.NewGuid().ToString();
            string parametersTypes = Guid.NewGuid().ToString();
            string arguments = Guid.NewGuid().ToString();

            ArgumentNullException exception =
                Assert.Throws<ArgumentNullException>(
                    () => new TaskActivationDetailsModel(instanceType, method, parametersTypes, arguments));

            Assert.AreEqual("instanceType", exception.ParamName);
        }

        [Test]
        public void TaskActivationDetailsModelConstructorMethodIsNullTest()
        {
            string instanceType = Guid.NewGuid().ToString();
            string method = null;
            string parametersTypes = Guid.NewGuid().ToString();
            string arguments = Guid.NewGuid().ToString();

            ArgumentNullException exception =
                Assert.Throws<ArgumentNullException>(
                    () => new TaskActivationDetailsModel(instanceType, method, parametersTypes, arguments));

            Assert.AreEqual("method", exception.ParamName);
        }

        [Test]
        public void TaskActivationDetailsModelConstructorParametersTypesIsNullTest()
        {
            string instanceType = Guid.NewGuid().ToString();
            string method = Guid.NewGuid().ToString();
            string parametersTypes = null;
            string arguments = Guid.NewGuid().ToString();

            ArgumentNullException exception =
                Assert.Throws<ArgumentNullException>(
                    () => new TaskActivationDetailsModel(instanceType, method, parametersTypes, arguments));

            Assert.AreEqual("parametersTypes", exception.ParamName);
        }

        [Test]
        public void TaskActivationDetailsModelConstructorArgumentsIsNullTest()
        {
            string instanceType = Guid.NewGuid().ToString();
            string method = Guid.NewGuid().ToString();
            string parametersTypes = Guid.NewGuid().ToString();
            string arguments = null;

            ArgumentNullException exception =
                Assert.Throws<ArgumentNullException>(
                    () => new TaskActivationDetailsModel(instanceType, method, parametersTypes, arguments));

            Assert.AreEqual("arguments", exception.ParamName);
        }

        [Test]
        public void TaskActivationDetailsModelConstructorTest()
        {
            string instanceType = Guid.NewGuid().ToString();
            string method = Guid.NewGuid().ToString();
            string parametersTypes = Guid.NewGuid().ToString();
            string arguments = Guid.NewGuid().ToString();
            TaskActivationDetailsModel item = new TaskActivationDetailsModel(instanceType, method, parametersTypes,
                arguments);
            Assert.IsNotNull(item);
        }
    }
}
