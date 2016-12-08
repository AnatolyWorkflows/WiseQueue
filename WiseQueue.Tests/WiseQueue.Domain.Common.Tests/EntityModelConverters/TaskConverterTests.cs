using NUnit.Framework;
using WiseQueue.Core.Common.Caching;
using WiseQueue.Core.Common.Converters;
using WiseQueue.Core.Common.Converters.EntityModelConverters;
using WiseQueue.Core.Common.Entities.Tasks;
using WiseQueue.Core.Tests;
using WiseQueue.Domain.Common.Converters;
using WiseQueue.Domain.Common.Converters.EntityModelConverters;
using WiseQueue.Domain.Common.Models.Tasks;
using WiseQueue.Domain.MicrosoftExpressionCache;

namespace WiseQueue.Domain.Common.Tests.EntityModelConverters
{
    public class TestTaskConverterClass
    {
        public void SimpleMethod(int k)
        {
        }

        public void MethodWithStruct(int k, TaskCancellationToken token)
        {
            
        }

        public void MethodJustClass(TaskCancellationToken token)
        {

        }
    }

    [TestFixture]
    class TaskConverterTests : BaseTestWithLogger
    {

        [Test]
        public void TaskConverterEntityToModelTest()
        {
            IJsonConverter jsonConverter = new JsonConverter(LoggerFactory);
            ICachedExpressionCompiler cachedExpressionCompiler = new CachedExpressionCompiler(LoggerFactory);
            IExpressionConverter expressionConverter = new ExpressionConverter(jsonConverter, cachedExpressionCompiler, LoggerFactory);
            ITaskConverter taskConverter = new TaskConverter(expressionConverter, jsonConverter, LoggerFactory);
            
            TestTaskConverterClass instance = new TestTaskConverterClass();

            ActivationData activationData = expressionConverter.Convert(() => instance.SimpleMethod(5));
            TaskModel taskModel = new TaskModel(1, TaskStates.New, activationData, new ScheduleInformation(1));

            TaskEntity taskEntity = taskConverter.Convert(taskModel);
            Assert.IsNotNull(taskEntity);

            TaskModel actual = taskConverter.Convert(taskEntity);
            Assert.IsNotNull(actual);

            Assert.AreEqual(taskEntity.Id, actual.Id);
            Assert.AreEqual(taskEntity.TaskState, actual.TaskState);            
        }

        [Test]
        public void TaskConverterEntityToModelWithStructureTest()
        {
            IJsonConverter jsonConverter = new JsonConverter(LoggerFactory);
            ICachedExpressionCompiler cachedExpressionCompiler = new CachedExpressionCompiler(LoggerFactory);
            IExpressionConverter expressionConverter = new ExpressionConverter(jsonConverter, cachedExpressionCompiler, LoggerFactory);
            ITaskConverter taskConverter = new TaskConverter(expressionConverter, jsonConverter, LoggerFactory);

            TestTaskConverterClass instance = new TestTaskConverterClass();

            ActivationData activationData = expressionConverter.Convert(() => instance.MethodJustClass(TaskCancellationToken.Null));

            TaskModel taskModel = new TaskModel(1, TaskStates.New, activationData, new ScheduleInformation(1));

            TaskEntity taskEntity = taskConverter.Convert(taskModel);
            Assert.IsNotNull(taskEntity);

            TaskModel actual = taskConverter.Convert(taskEntity);
            Assert.IsNotNull(actual);

            Assert.AreEqual(taskEntity.Id, actual.Id);
            Assert.AreEqual(taskEntity.TaskState, actual.TaskState);
        }
    }
}
