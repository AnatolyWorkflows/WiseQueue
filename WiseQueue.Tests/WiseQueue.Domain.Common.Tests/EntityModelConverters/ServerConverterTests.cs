using System;
using NUnit.Framework;
using WiseQueue.Core.Common.Converters.EntityModelConverters;
using WiseQueue.Core.Common.Entities.Server;
using WiseQueue.Core.Common.Models.Servers;
using WiseQueue.Core.Tests;
using WiseQueue.Domain.Common.Converters.EntityModelConverters;

namespace WiseQueue.Domain.Common.Tests.EntityModelConverters
{
    [TestFixture]
    class ServerConverterTests : BaseTestWithLogger
    {
        [Test]
        public void ServerConverterEntityToModelTest()
        {
            IServerConverter converter = new ServerConverter(LoggerFactory);

            ServerEntity entity = new ServerEntity
            {
                Id = 23,
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                HeartbeatLifetime = TimeSpan.FromSeconds(5)
            };

            ServerModel actual = converter.Convert(entity);

            Assert.AreEqual(entity.Id, actual.Id);
            Assert.AreEqual(entity.Name, actual.Name);
            Assert.AreEqual(entity.Description, actual.Description);
            Assert.AreEqual(entity.HeartbeatLifetime, actual.HeartbeatLifetime);
        }

        [Test]
        public void ServerConverterModelToEntityTest()
        {
            IServerConverter converter = new ServerConverter(LoggerFactory);

            ServerModel serverModel = new ServerModel(23, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
                TimeSpan.FromSeconds(67));

            ServerEntity actual = converter.Convert(serverModel);

            Assert.AreEqual(serverModel.Id, actual.Id);
            Assert.AreEqual(serverModel.Name, actual.Name);
            Assert.AreEqual(serverModel.Description, actual.Description);
            Assert.AreEqual(serverModel.HeartbeatLifetime, actual.HeartbeatLifetime);
        }

    }
}
