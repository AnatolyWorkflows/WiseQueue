using System;

namespace WiseQueue.Core.Common.Models
{
    /// <summary>
    /// Queue model.
    /// </summary>
    public class QueueModel
    {
        #region Properties...

        /// <summary>
        /// Queue's identifier.
        /// </summary>
        public Int64 Id { get; private set; }
        /// <summary>
        /// Queue's name.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Queue's description.
        /// </summary>
        public string Description { get; private set; }

        #endregion

        #region Constructors...

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Queue's <c>name</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null" />.</exception>
        public QueueModel(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            Name = name;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Queue's <c>name</c>.</param>
        /// <param name="description">Queue's <c>description</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="description"/> is <see langword="null" />.</exception>
        public QueueModel(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentNullException("description");

            Name = name;
            Description = description;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Queue's identifier.</param>
        /// <param name="name">Queue's <c>name</c>.</param>
        /// <param name="description">Queue's <c>description</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="description"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The identifier should be great than 0.</exception>
        public QueueModel(Int64 id, string name, string description)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException("id", "The identifier should be great than 0.");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentNullException("description");

            Id = id;
            Name = name;
            Description = description;
        }

        #endregion
    }
}
