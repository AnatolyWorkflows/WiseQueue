using System;
using NUnit.Framework;
using WiseQueue.Core.Common.Entities;
using WiseQueue.Core.Common.Entities.Tasks;

namespace WiseQueue.Core.Common.Tests.EntitiesTests
{
    [TestFixture]
    public class TaskEntityTests
    {
        [Test]
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

        [Test]
        public void TaskEntityConstructorWithNegativeIdTest()
        {
            Int64 id = -1;
            Int64 queueId = 1;
            TaskActivationDetailsEntity taskActivationDetails = new TaskActivationDetailsEntity(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            TaskStates taskState = TaskStates.Pending;

            ArgumentOutOfRangeException exception =
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => new TaskEntity(id, queueId, taskActivationDetails, taskState));

            Assert.AreEqual("id", exception.ParamName);
        }

        [Test]
        public void TaskEntityConstructorWithNegativeQueueIdTest()
        {
            Int64 id = 1;
            Int64 queueId = -1;
            TaskActivationDetailsEntity taskActivationDetails = new TaskActivationDetailsEntity(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            TaskStates taskState = TaskStates.Pending;

            ArgumentOutOfRangeException exception =
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => new TaskEntity(id, queueId, taskActivationDetails, taskState));

            Assert.AreEqual("queueId", exception.ParamName);
        }

        [Test]
        public void TaskEntityConstructorWithOutTaskActivationDetailsEntityTest()
        {
            Int64 id = 1;
            Int64 queueId = 1;
            TaskActivationDetailsEntity taskActivationDetails = null;
            TaskStates taskState = TaskStates.Pending;

            ArgumentNullException exception =
                Assert.Throws<ArgumentNullException>(
                    () => new TaskEntity(id, queueId, taskActivationDetails, taskState));

            Assert.AreEqual("taskActivationDetails", exception.ParamName);
        }

        [Test]
        public void TaskEntityConstructorWithWrongStateTest()
        {
            Int64 id = 1;
            Int64 queueId = 1;
            TaskActivationDetailsEntity taskActivationDetails = new TaskActivationDetailsEntity(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            TaskStates taskState = TaskStates.New;

            ArgumentException exception =
                Assert.Throws<ArgumentException>(
                    () => new TaskEntity(id, queueId, taskActivationDetails, taskState));

            Assert.AreEqual("taskState", exception.ParamName);
        }


        [Test]
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

        [Test]
        public void NewTaskEntityConstructorWithWrongQueueIdTest()
        {
            Int64 queueId = -1;
            TaskActivationDetailsEntity taskActivationDetails = new TaskActivationDetailsEntity(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            ArgumentOutOfRangeException exception =
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => new TaskEntity(queueId, taskActivationDetails));

            Assert.AreEqual("queueId", exception.ParamName);
        }

        [Test]
        public void NewTaskEntityConstructorWithNullTaskActivationDetailsEntityTest()
        {
            Int64 queueId = 1;
            TaskActivationDetailsEntity taskActivationDetails = null;

            ArgumentNullException exception =
                Assert.Throws<ArgumentNullException>(
                    () => new TaskEntity(queueId, taskActivationDetails));

            Assert.AreEqual("taskActivationDetails", exception.ParamName);
        }
    }
}
