//using System;
//using NUnit.Framework;
//using WiseQueue.Core.Common.Converters.EntityModelConverters;
//using WiseQueue.Core.Common.Entities.Tasks;
//using WiseQueue.Core.Common.Models.Tasks;
//using WiseQueue.Core.Tests;
//using WiseQueue.Domain.Common.Converters.EntityModelConverters;

//namespace WiseQueue.Domain.Common.Tests.EntityModelConverters
//{
//    [TestFixture]
//    class TaskConverterTests : BaseTestWithLogger
//    {
//        [Test]
//        public void TaskConverterEntityToModelTest()
//        {
//            ITaskConverter taskConverter = new TaskConverter(LoggerFactory);

//            TaskEntity taskEntity = new TaskEntity
//            {
//                Id = 23,
//                QueueId = 2,
//                TaskState = TaskStates.Failed,
//                InstanceType = Guid.NewGuid().ToString(),
//                Method = Guid.NewGuid().ToString(),
//                ParametersTypes = Guid.NewGuid().ToString(),
//                Arguments = Guid.NewGuid().ToString()
//            };

//            TaskModel actual = taskConverter.Convert(taskEntity);

//            Assert.AreEqual(taskEntity.Id, actual.Id);
//            Assert.AreEqual(taskEntity.QueueId, actual.QueueId);
//            Assert.AreEqual(taskEntity.TaskState, actual.TaskState);
//            Assert.AreEqual(taskEntity.InstanceType, actual.TaskActivationDetails.InstanceType);
//            Assert.AreEqual(taskEntity.Method, actual.TaskActivationDetails.Method);
//            Assert.AreEqual(taskEntity.ParametersTypes, actual.TaskActivationDetails.ParametersTypes);
//            Assert.AreEqual(taskEntity.Arguments, actual.TaskActivationDetails.Arguments);
//        }

//        [Test]
//        public void TaskConverterModelToEntityTest()
//        {
//            ITaskConverter taskConverter = new TaskConverter(LoggerFactory);

//            TaskModel taskModel = new TaskModel(23, 2,
//                new TaskActivationDetailsModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
//                    Guid.NewGuid().ToString(), Guid.NewGuid().ToString()), TaskStates.Pending);
            
//            TaskEntity actual = taskConverter.Convert(taskModel);

//            Assert.AreEqual(taskModel.Id, actual.Id);
//            Assert.AreEqual(taskModel.QueueId, actual.QueueId);
//            Assert.AreEqual(taskModel.TaskState, actual.TaskState);
//            Assert.AreEqual(taskModel.TaskActivationDetails.InstanceType, actual.InstanceType);
//            Assert.AreEqual(taskModel.TaskActivationDetails.Method, actual.Method);
//            Assert.AreEqual(taskModel.TaskActivationDetails.ParametersTypes, actual.ParametersTypes);
//            Assert.AreEqual(taskModel.TaskActivationDetails.Arguments, actual.Arguments);
//        }

//    }
//}
