using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using WiseQueue.Core.Common.Converters.EntityModelConverters;
using WiseQueue.Core.Common.Entities;
using WiseQueue.Core.Common.Models;
using WiseQueue.Core.Tests;
using WiseQueue.Domain.Common.Converters.EntityModelConverters;

namespace WiseQueue.Domain.Common.Tests.EntityModelConverters
{
    [TestFixture]
    class QueueConverterTests : BaseTestWithLogger
    {
        [Test]
        public void QueueConverterEntityToModelTest()
        {
            QueueEntity entity = new QueueEntity
            {
                Id = 23,
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString()
            };

            IQueueConverter converter = new QueueConverter(LoggerFactory);

            QueueModel actual = converter.Convert(entity);

            Assert.AreEqual(entity.Id, actual.Id);
            Assert.AreEqual(entity.Name, actual.Name);
            Assert.AreEqual(entity.Description, actual.Description);
        }

        [Test]
        public void QueueConverterModelToEntityTest()
        {
            QueueModel model = new QueueModel(23, Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            IQueueConverter converter = new QueueConverter(LoggerFactory);

            QueueEntity actual = converter.Convert(model);

            Assert.AreEqual(model.Id, actual.Id);
            Assert.AreEqual(model.Name, actual.Name);
            Assert.AreEqual(model.Description, actual.Description);
        }
    }
}
