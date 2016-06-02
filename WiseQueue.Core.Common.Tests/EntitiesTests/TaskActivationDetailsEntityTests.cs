using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WiseQueue.Core.Common.Entities;

namespace WiseQueue.Core.Common.Tests.EntitiesTests
{
    [TestClass]
    public class TaskActivationDetailsEntityTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TaskActivationDetailsEntityConstructorInstanceTypeIsNullTest()
        {
            string instanceType = null;
            string method = Guid.NewGuid().ToString();
            string parametersTypes = Guid.NewGuid().ToString();
            string arguments = Guid.NewGuid().ToString();
            TaskActivationDetailsEntity item = new TaskActivationDetailsEntity(instanceType, method, parametersTypes, arguments);
            Assert.Fail("The TaskActivationDetailsEntity instance has been created with wrong parameter: {0}", item);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TaskActivationDetailsEntityConstructorMethodIsNullTest()
        {
            string instanceType = Guid.NewGuid().ToString();
            string method = null;
            string parametersTypes = Guid.NewGuid().ToString();
            string arguments = Guid.NewGuid().ToString();
            TaskActivationDetailsEntity item = new TaskActivationDetailsEntity(instanceType, method, parametersTypes, arguments);
            Assert.Fail("The TaskActivationDetailsEntity instance has been created with wrong parameter: {0}", item);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TaskActivationDetailsEntityConstructorParametersTypesIsNullTest()
        {
            string instanceType = Guid.NewGuid().ToString();
            string method = Guid.NewGuid().ToString();
            string parametersTypes = null;
            string arguments = Guid.NewGuid().ToString();
            TaskActivationDetailsEntity item = new TaskActivationDetailsEntity(instanceType, method, parametersTypes, arguments);
            Assert.Fail("The TaskActivationDetailsEntity instance has been created with wrong parameter: {0}", item);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TaskActivationDetailsEntityConstructorArgumentsIsNullTest()
        {
            string instanceType = Guid.NewGuid().ToString();
            string method = Guid.NewGuid().ToString();
            string parametersTypes = Guid.NewGuid().ToString();
            string arguments = null;
            TaskActivationDetailsEntity item = new TaskActivationDetailsEntity(instanceType, method, parametersTypes, arguments);
            Assert.Fail("The TaskActivationDetailsEntity instance has been created with wrong parameter: {0}", item);
        }

        [TestMethod]
        public void TaskActivationDetailsEntityConstructorTest()
        {
            string instanceType = Guid.NewGuid().ToString();
            string method = Guid.NewGuid().ToString();
            string parametersTypes = Guid.NewGuid().ToString();
            string arguments = Guid.NewGuid().ToString();
            TaskActivationDetailsEntity item = new TaskActivationDetailsEntity(instanceType, method, parametersTypes, arguments);
            Assert.IsNotNull(item);
        }
    }
}
