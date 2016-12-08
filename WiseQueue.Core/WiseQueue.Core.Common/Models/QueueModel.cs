using System;

namespace WiseQueue.Core.Common.Models
{
    /// <summary>
    /// Queue model.
    /// </summary>
    public class QueueModel
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
        #endregion

        #region Properties...

        /// <summary>
        /// Server's identifier.
        /// </summary>
        /// <exception cref="ArgumentException" accessor="set">The identifier should be 0 for a new model or great then 0 for an existing one.</exception>
        public Int64 Id
        {
            get { return id; }
            set
            {
                if (id < 0)
                    throw new ArgumentException("The identifier should be 0 for a new model or great then 0 for an existing one.", "value");

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

        #endregion

        #region Constructors...

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Queue's <c>name</c>.</param>
        public QueueModel(string name): this(name, string.Empty)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Queue's <c>name</c>.</param>
        /// <param name="description">Queue's <c>description</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null" />.</exception>
        public QueueModel(string name, string description)
        {
            Name = name;
            Description = description;
        }

        #endregion

        #region Overrides of Object

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("Id: {0}; Name: {1}; Description: {2}", Id, Name, Description);
        }

        #endregion
    }
}
