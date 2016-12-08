using System;

namespace WiseQueue.Domain.Common.Models.Servers
{
    /// <summary>
    /// Server model.
    /// </summary>
    public class ServerModel
    {
        #region Fields...
        /// <summary>
        /// Server's identifier.
        /// </summary>
        private Int64 id;

        /// <summary>
        /// Server's name.
        /// </summary>
        private string name;

        /// <summary>
        /// Server's description.
        /// </summary>
        private string description;

        /// <summary>
        /// During <c>this</c> time next heartbeat should be sent.
        /// </summary>
        private TimeSpan heartbeatLifetime;
        #endregion
        
        #region Properties...

        /// <summary>
        /// Server's identifier.
        /// </summary>
        public Int64 Id
        {
            get { return id; }
            set
            {
                if (id < 0)
                    throw new ArgumentException("The identifier shoyld be 0 for a new model or greate then 0 for an existing one.", "value");

               id = value;
            }
        }

        /// <summary>
        /// Server's name.
        /// </summary>
        /// <exception cref="ArgumentNullException" accessor="set"><paramref name="value"/> is <see langword="null"/></exception>
        public string Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("value");
                name = value;
            }
        }

        /// <summary>
        /// Server's description.
        /// </summary>
        /// <exception cref="ArgumentNullException" accessor="set"><paramref name="value"/> is <see langword="null"/></exception>
        public string Description
        {
            get { return description; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("value");

                description = value;
            }
        }

        /// <summary>
        /// During <c>this</c> time next heartbeat should be sent.
        /// </summary>
        /// <exception cref="ArgumentException" accessor="set">The heartbeat time should be equal or greater then 1 second</exception>
        public TimeSpan HeartbeatLifetime
        {
            get { return heartbeatLifetime; }
            set
            {
                if (heartbeatLifetime.TotalSeconds < 1)
                    throw new ArgumentException("The heartbeat time should be equal or greater then 1 second", "value");

                heartbeatLifetime = value;
            }
        }

        #endregion

        #region Constructors...

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Server's <c>name</c>.</param>
        /// <param name="heartbeatLifetime">During <c>this</c> time next heartbeat should be sent.</param>
        public ServerModel(string name, TimeSpan heartbeatLifetime) : this(name, string.Empty, heartbeatLifetime)
        {
            id = 0;
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
            id = 0;
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