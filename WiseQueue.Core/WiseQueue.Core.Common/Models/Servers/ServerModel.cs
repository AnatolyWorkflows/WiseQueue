using System;

namespace WiseQueue.Core.Common.Models.Servers
{
    /// <summary>
    /// Server model.
    /// </summary>
    public class ServerModel
    {
        #region Properties...

        /// <summary>
        /// Server's identifier.
        /// </summary>
        public Int64 Id { get; private set; }

        /// <summary>
        /// Server's name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Server's description.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// During <c>this</c> time next heartbeat should be sent.
        /// </summary>
        public TimeSpan HeartbeatLifetime { get; private set; }

        #endregion

        #region Constructors...

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Server's <c>name</c>.</param>
        /// <param name="heartbeatLifetime">During <c>this</c> time next heartbeat should be sent.</param>
        public ServerModel(string name, TimeSpan heartbeatLifetime) : this(name, string.Empty, heartbeatLifetime)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Server's <c>name</c>.</param>
        /// <param name="description">Server's <c>description</c>.</param>
        /// <param name="heartbeatLifetime">During <c>this</c> time next heartbeat should be sent.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null" />.</exception>
        public ServerModel(string name, string description, TimeSpan heartbeatLifetime)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");


            Name = name;
            Description = description;
            HeartbeatLifetime = heartbeatLifetime;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Server's identifier.</param>
        /// <param name="name">Server's <c>name</c>.</param>
        /// <param name="heartbeatLifetime">During <c>this</c> time next heartbeat should be sent.</param>
        public ServerModel(Int64 id, string name, TimeSpan heartbeatLifetime)
            : this(id, name, string.Empty, heartbeatLifetime)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Server's identifier.</param>
        /// <param name="name">Server's <c>name</c>.</param>
        /// <param name="description">Server's <c>description</c>.</param>
        /// <param name="heartbeatLifetime">During <c>this</c> time next heartbeat should be sent.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The identifier should be great than 0.</exception>
        public ServerModel(Int64 id, string name, string description, TimeSpan heartbeatLifetime)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException("id", "The identifier should be great than 0.");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            Id = id;
            Name = name;
            Description = description;
            HeartbeatLifetime = heartbeatLifetime;
        }

        #endregion

        #region Overrides of Object

        /// <summary>
        /// Returns a string that represents the current <c>object</c>.
        /// </summary>
        /// <returns>A string that represents the current <c>object</c>.</returns>
        public override string ToString()
        {
            return string.Format("Id: {0}; Name: {1}; HeartbeatLifetime: {2}; Description: {3}", Id, Name,
                HeartbeatLifetime, Description);
        }

        #endregion
    }
}