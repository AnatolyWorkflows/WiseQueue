using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using JsonConverter = WiseQueue.Domain.Common.Converters.JsonConverter;

namespace WiseQueue.Core.Common.Tests
{
    [TestClass]
    public class JsonConverterTests: BaseTestWithLogger
    {
        private class TestClass
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        [TestMethod]
        public void JsonConvertConstructorTest()
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            JsonConverter jsonConverter = new JsonConverter(jsonSerializerSettings, LoggerFactory);
            Assert.IsNotNull(jsonConverter);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void JsonConvertConstructorWithBadJsonSettingsTest()
        {
            JsonConverter jsonConverter = new JsonConverter(null, LoggerFactory);
            Assert.IsNull(jsonConverter);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void JsonConvertConstructorWithBadLoggerFactoryTest()
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            JsonConverter jsonConverter = new JsonConverter(jsonSerializerSettings, null);
            Assert.IsNull(jsonConverter);
        }

        [TestMethod]
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

        [TestMethod]
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
