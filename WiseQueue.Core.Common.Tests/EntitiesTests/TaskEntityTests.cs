using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WiseQueue.Core.Common.Entities;

namespace WiseQueue.Core.Common.Tests.EntitiesTests
{
    [TestClass]
    public class TaskEntityTests
    {
        [TestMethod]
        public void TaskEntityConstructorTest()
        {
            Int64 id = 1;
            Int64 queueId = 1;
            TaskActivationDetailsEntity taskActivationDetails = new TaskActivationDetailsEntity(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            TaskStates taskState = TaskStates.Pending;
            TaskEntity taskEntity = new TaskEntity(id, queueId, taskActivationDetails, taskState);
            Assert.IsNotNull(taskEntity);
            Assert.AreEqual(id, taskEntity.Id);
            Assert.AreEqual(queueId, taskEntity.QueueId);
            Assert.AreEqual(taskState, taskEntity.TaskState);
            Assert.AreEqual(taskActivationDetails.ToString(), taskEntity.TaskActivationDetails.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TaskEntityConstructorWithNegativeIdTest()
        {
            Int64 id = -1;
            Int64 queueId = 1;
            TaskActivationDetailsEntity taskActivationDetails = new TaskActivationDetailsEntity(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            TaskStates taskState = TaskStates.Pending;
            TaskEntity taskEntity = new TaskEntity(id, queueId, taskActivationDetails, taskState);
            Assert.Fail("The TaskEntity instance has been created with wrong parameter: {0}", taskEntity);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TaskEntityConstructorWithNegativeQueueIdTest()
        {
            Int64 id = 1;
            Int64 queueId = -1;
            TaskActivationDetailsEntity taskActivationDetails = new TaskActivationDetailsEntity(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            TaskStates taskState = TaskStates.Pending;
            TaskEntity taskEntity = new TaskEntity(id, queueId, taskActivationDetails, taskState);
            Assert.Fail("The TaskEntity instance has been created with wrong parameter: {0}", taskEntity);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TaskEntityConstructorWithOutTaskActivationDetailsEntityTest()
        {
            Int64 id = 1;
            Int64 queueId = 1;
            TaskActivationDetailsEntity taskActivationDetails = null;
            TaskStates taskState = TaskStates.Pending;
            TaskEntity taskEntity = new TaskEntity(id, queueId, taskActivationDetails, taskState);
            Assert.Fail("The TaskEntity instance has been created with wrong parameter: {0}", taskEntity);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TaskEntityConstructorWithWrongStateTest()
        {
            Int64 id = 1;
            Int64 queueId = 1;
            TaskActivationDetailsEntity taskActivationDetails = new TaskActivationDetailsEntity(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            TaskStates taskState = TaskStates.New;
            TaskEntity taskEntity = new TaskEntity(id, queueId, taskActivationDetails, taskState);
            Assert.Fail("The TaskEntity instance has been created with wrong parameter: {0}", taskEntity);
        }


        [TestMethod]
        public void NewTaskEntityConstructorTest()
        {
            Int64 queueId = 1;
            TaskActivationDetailsEntity taskActivationDetails = new TaskActivationDetailsEntity(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            TaskEntity taskEntity = new TaskEntity(queueId, taskActivationDetails);
            Assert.IsNotNull(taskEntity);
            Assert.AreEqual(0, taskEntity.Id);
            Assert.AreEqual(queueId, taskEntity.QueueId);
            Assert.AreEqual(TaskStates.New, taskEntity.TaskState);
            Assert.AreEqual(taskActivationDetails.ToString(), taskEntity.TaskActivationDetails.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NewTaskEntityConstructorWithWrongQueueIdTest()
        {
            Int64 queueId = -1;
            TaskActivationDetailsEntity taskActivationDetails = new TaskActivationDetailsEntity(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            TaskEntity taskEntity = new TaskEntity(queueId, taskActivationDetails);
            Assert.Fail("The TaskEntity instance has been created with wrong parameter: {0}", taskEntity);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewTaskEntityConstructorWithNullTaskActivationDetailsEntityTest()
        {
            Int64 queueId = 1;
            TaskActivationDetailsEntity taskActivationDetails = null;
            TaskEntity taskEntity = new TaskEntity(queueId, taskActivationDetails);
            Assert.Fail("The TaskEntity instance has been created with wrong parameter: {0}", taskEntity);
        }
    }
}
