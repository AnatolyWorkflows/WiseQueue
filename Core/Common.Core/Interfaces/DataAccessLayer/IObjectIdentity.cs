using System;

namespace Common.Core.Interfaces.DataAccessLayer
{
    /// <summary>
    /// Interface shows that object has identifier.
    /// </summary>
    public interface IObjectIdentity
    {
        /// <summary>
        /// The identifier.
        /// </summary>
        Int64 Id { get; }
    }
}
