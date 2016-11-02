//using System;
//using NUnit.Framework;
//using WiseQueue.Core.Common.Entities.Tasks;
//using WiseQueue.Core.Common.Models.Tasks;

//namespace WiseQueue.Core.Common.Tests.ModelsTests.Tasks
//{
//    [TestFixture]
//    public class TaskModelTests
//    {
//        [Test]
//        public void TaskModelConstructorTest()
//        {
//            Int64 id = 1;
//            Int64 queueId = 1;
//            TaskActivationDetailsModel taskActivationDetails = new TaskActivationDetailsModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
//            TaskStates taskState = TaskStates.Pending;
//            TaskModel taskStateModel = new TaskStateModel(id, queueId, taskActivationDetails, taskState);
//            Assert.IsNotNull(taskModel);
//            Assert.AreEqual(id, taskModel.Id);
//            Assert.AreEqual(queueId, taskModel.QueueId);
//            Assert.AreEqual(taskState, taskModel.TaskState);
//            Assert.AreEqual(taskActivationDetails.ToString(), taskModel.TaskActivationDetails.ToString());
//        }

//        [Test]
//        public void TaskModelConstructorWithNegativeIdTest()
//        {
//            Int64 id = -1;
//            Int64 queueId = 1;
//            TaskActivationDetailsModel taskActivationDetails = new TaskActivationDetailsModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
//            TaskStates taskState = TaskStates.Pending;

//            ArgumentOutOfRangeException exception =
//                Assert.Throws<ArgumentOutOfRangeException>(
//                    () => new TaskModel(id, queueId, taskActivationDetails, taskState));

//            Assert.AreEqual("id", exception.ParamName);
//        }

//        [Test]
//        public void TaskModelConstructorWithNegativeQueueIdTest()
//        {
//            Int64 id = 1;
//            Int64 queueId = -1;
//            TaskActivationDetailsModel taskActivationDetails = new TaskActivationDetailsModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
//            TaskStates taskState = TaskStates.Pending;

//            ArgumentOutOfRangeException exception =
//                Assert.Throws<ArgumentOutOfRangeException>(
//                    () => new TaskModel(id, queueId, taskActivationDetails, taskState));

//            Assert.AreEqual("queueId", exception.ParamName);
//        }

//        [Test]
//        public void TaskModelConstructorWithOutTaskActivationDetailsModelTest()
//        {
//            Int64 id = 1;
//            Int64 queueId = 1;
//            TaskActivationDetailsModel taskActivationDetails = null;
//            TaskStates taskState = TaskStates.Pending;

//            ArgumentNullException exception =
//                Assert.Throws<ArgumentNullException>(
//                    () => new TaskModel(id, queueId, taskActivationDetails, taskState));

//            Assert.AreEqual("taskActivationDetails", exception.ParamName);
//        }

//        [Test]
//        public void NewTaskModelConstructorTest()
//        {
//            Int64 queueId = 1;
//            TaskActivationDetailsModel taskActivationDetails = new TaskActivationDetailsModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
//            TaskModel taskStateModel = new TaskStateModel(queueId, taskActivationDetails);
//            Assert.IsNotNull(taskModel);
//            Assert.AreEqual(0, taskModel.Id);
//            Assert.AreEqual(queueId, taskModel.QueueId);
//            Assert.AreEqual(TaskStates.New, taskModel.TaskState);
//            Assert.AreEqual(taskActivationDetails.ToString(), taskModel.TaskActivationDetails.ToString());
//        }

//        [Test]
//        public void NewTaskModelConstructorWithWrongQueueIdTest()
//        {
//            Int64 queueId = -1;
//            TaskActivationDetailsModel taskActivationDetails = new TaskActivationDetailsModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

//            ArgumentOutOfRangeException exception =
//                Assert.Throws<ArgumentOutOfRangeException>(
//                    () => new TaskModel(queueId, taskActivationDetails));

//            Assert.AreEqual("queueId", exception.ParamName);
//        }

//        [Test]
//        public void NewTaskModelConstructorWithNullTaskActivationDetailsModelTest()
//        {
//            Int64 queueId = 1;
//            TaskActivationDetailsModel taskActivationDetails = null;

//            ArgumentNullException exception =
//                Assert.Throws<ArgumentNullException>(
//                    // ReSharper disable once ExpressionIsAlwaysNull
//                    () => new TaskModel(queueId: queueId, taskActivationDetails: taskActivationDetails));

//            Assert.AreEqual("taskActivationDetails", exception.ParamName);
//        }
//    }
//}
