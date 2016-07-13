using System;

namespace WiseQueue.Core.Common.Entities
{
    /// <summary>
    /// Queue entity.
    /// </summary>
    public class QueueEntity
    {
        #region Properties...

        /// <summary>
        /// Queue's identifier.
        /// </summary>
        public Int64 Id { get; set; }
        /// <summary>
        /// Queue's name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Queue's description.
        /// </summary>
        public string Description { get; set; }

        #endregion
    }
}
