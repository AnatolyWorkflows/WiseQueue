using System;
using Common.Core.BaseClasses;
using Common.Core.Logging;
using WiseQueue.Core.Common.Converters.EntityModelConverters;
using WiseQueue.Core.Common.Entities.Server;
using WiseQueue.Core.Common.Models;
using WiseQueue.Core.Common.Models.Servers;

namespace WiseQueue.Domain.Common.Converters.EntityModelConverters
{
    /// <summary>
    /// <c>Converter</c> from the server model into the server entity and back.
    /// </summary>
    public class ServerConverter: BaseLoggerObject, IServerConverter
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        public ServerConverter(ICommonLoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        #region Implementation of IServerConverter

        /// <summary>
        /// Convert server model into the server entity.
        /// </summary>
        /// <param name="serverModel">The <see cref="ServerModel"/> instance.</param>
        /// <returns>The <see cref="ServerEntity"/> instance.</returns>
        public ServerEntity Convert(ServerModel serverModel)
        {
            ServerEntity result = new ServerEntity
            {
                Id = serverModel.Id,
                Name = serverModel.Name,
                Description = serverModel.Description,
                HeartbeatLifetime = serverModel.HeartbeatLifetime
            };

            return result;
        }

        /// <summary>
        /// Convert server entity into the server model.
        /// </summary>
        /// <param name="serverEntity">The <see cref="ServerEntity"/> instance.</param>
        /// <returns>The <see cref="ServerModel"/> instance.</returns>
        public ServerModel Convert(ServerEntity serverEntity)
        {
            ServerModel result = (serverEntity.Id > 0)
                ? new ServerModel(serverEntity.Id, serverEntity.Name, serverEntity.Description,
                    serverEntity.HeartbeatLifetime)
                : new ServerModel(serverEntity.Name, serverEntity.Description, serverEntity.HeartbeatLifetime);

            return result;
        }

        #endregion
    }
}
