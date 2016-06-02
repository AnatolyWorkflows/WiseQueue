using System;
using NUnit.Framework;
using WiseQueue.Core.Common.Entities;

namespace WiseQueue.Core.Common.Tests.EntitiesTests
{
    [TestFixture]
    public class TaskActivationDetailsEntityTests
    {
        [Test]
        public void TaskActivationDetailsEntityConstructorInstanceTypeIsNullTest()
        {
            string instanceType = null;
            string method = Guid.NewGuid().ToString();
            string parametersTypes = Guid.NewGuid().ToString();
            string arguments = Guid.NewGuid().ToString();

            ArgumentNullException exception =
                Assert.Throws<ArgumentNullException>(
                    () => new TaskActivationDetailsEntity(instanceType, method, parametersTypes, arguments));

            Assert.AreEqual("instanceType", exception.ParamName);
        }

        [Test]
        public void TaskActivationDetailsEntityConstructorMethodIsNullTest()
        {
            string instanceType = Guid.NewGuid().ToString();
            string method = null;
            string parametersTypes = Guid.NewGuid().ToString();
            string arguments = Guid.NewGuid().ToString();

            ArgumentNullException exception =
                Assert.Throws<ArgumentNullException>(
                    () => new TaskActivationDetailsEntity(instanceType, method, parametersTypes, arguments));

            Assert.AreEqual("method", exception.ParamName);
        }

        [Test]
        public void TaskActivationDetailsEntityConstructorParametersTypesIsNullTest()
        {
            string instanceType = Guid.NewGuid().ToString();
            string method = Guid.NewGuid().ToString();
            string parametersTypes = null;
            string arguments = Guid.NewGuid().ToString();

            ArgumentNullException exception =
                Assert.Throws<ArgumentNullException>(
                    () => new TaskActivationDetailsEntity(instanceType, method, parametersTypes, arguments));

            Assert.AreEqual("parametersTypes", exception.ParamName);
        }

        [Test]
        public void TaskActivationDetailsEntityConstructorArgumentsIsNullTest()
        {
            string instanceType = Guid.NewGuid().ToString();
            string method = Guid.NewGuid().ToString();
            string parametersTypes = Guid.NewGuid().ToString();
            string arguments = null;

            ArgumentNullException exception =
                Assert.Throws<ArgumentNullException>(
                    () => new TaskActivationDetailsEntity(instanceType, method, parametersTypes, arguments));

            Assert.AreEqual("arguments", exception.ParamName);
        }

        [Test]
        public void TaskActivationDetailsEntityConstructorTest()
        {
            string instanceType = Guid.NewGuid().ToString();
            string method = Guid.NewGuid().ToString();
            string parametersTypes = Guid.NewGuid().ToString();
            string arguments = Guid.NewGuid().ToString();
            TaskActivationDetailsEntity item = new TaskActivationDetailsEntity(instanceType, method, parametersTypes,
                arguments);
            Assert.IsNotNull(item);
        }
    }
}
