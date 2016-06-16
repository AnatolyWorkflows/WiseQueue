using WiseQueue.Core.Common.Entities.Server;
using WiseQueue.Core.Common.Models.Servers;

namespace WiseQueue.Core.Common.Converters.EntityModelConverters
{
    /// <summary>
    /// Interface shows that <c>object</c> can convert <see cref="ServerModel"/> into the <see cref="ServerEntity"/> and back.
    /// </summary>
    public interface IServerConverter
    {
        /// <summary>
        /// Convert server model into the server entity.
        /// </summary>
        /// <param name="serverModel">The <see cref="ServerModel"/> instance.</param>
        /// <returns>The <see cref="ServerEntity"/> instance.</returns>
        ServerEntity Convert(ServerModel serverModel);

        /// <summary>
        /// Convert server entity into the server model.
        /// </summary>
        /// <param name="serverEntity">The <see cref="ServerEntity"/> instance.</param>
        /// <returns>The <see cref="ServerModel"/> instance.</returns>
        ServerModel Convert(ServerEntity serverEntity);
    }
}
