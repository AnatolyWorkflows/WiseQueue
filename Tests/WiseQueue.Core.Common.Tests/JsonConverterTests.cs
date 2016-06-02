using System;
using Newtonsoft.Json;
using NUnit.Framework;
using WiseQueue.Core.Tests;
using Assert = NUnit.Framework.Assert;
using JsonConverter = WiseQueue.Domain.Common.Converters.JsonConverter;

namespace WiseQueue.Core.Common.Tests
{
    [TestFixture]
    public class JsonConverterTests: BaseTestWithLogger
    {
        private class TestClass
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        [Test]
        public void JsonConvertConstructorTest()
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            JsonConverter jsonConverter = new JsonConverter(jsonSerializerSettings, LoggerFactory);
            Assert.IsNotNull(jsonConverter);
        }

        [Test]
        public void JsonConvertConstructorWithBadJsonSettingsTest()
        {
            ArgumentNullException exception =
                Assert.Throws<ArgumentNullException>(
                    () => new JsonConverter(null, LoggerFactory));

            Assert.AreEqual("jsonSerializerSettings", exception.ParamName);
        }

        [Test]
        public void JsonConvertConstructorWithBadLoggerFactoryTest()
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();

            ArgumentNullException exception =
                Assert.Throws<ArgumentNullException>(
                    () => new JsonConverter(jsonSerializerSettings, null));

            Assert.AreEqual("loggerFactory", exception.ParamName);
        }

        [Test]
        public void JsonConvertTest()
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();           
            JsonConverter jsonConverter = new JsonConverter(jsonSerializerSettings, LoggerFactory);

            TestClass expected = new TestClass
            {
                Id = 2,
                Name = Guid.NewGuid().ToString()
            };

            string jsonData = jsonConverter.ConvertToJson(expected);
            Assert.IsNotNull(jsonData);

            object obj = jsonConverter.ConvertFromJson(jsonData, typeof(TestClass));
            
            TestClass actual = (TestClass) obj;

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
        }

        [Test]
        public void JsonGenericConvertTest()
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            JsonConverter jsonConverter = new JsonConverter(jsonSerializerSettings, LoggerFactory);

            TestClass expected = new TestClass
            {
                Id = 2,
                Name = Guid.NewGuid().ToString()
            };

            string jsonData = jsonConverter.ConvertToJson(expected);
            Assert.IsNotNull(jsonData);

            TestClass actual = jsonConverter.ConvertFromJson<TestClass>(jsonData);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
        }
    }
}
