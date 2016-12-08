using System;
using System.Linq;

namespace Common.Core.Interfaces.DataAccessLayer
{
    /// <summary>
    /// Interface shows that object is a data context.
    /// </summary>
    public interface IDataContext: IDisposable
    {
        /// <summary>
        /// Get a set with entities.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <returns>The IQueryable instance.</returns>
        IQueryable<TEntity> GetCollectionSet<TEntity>()
            where TEntity : class, IObjectIdentity;

        TEntity Insert<TEntity>(TEntity entity)
            where TEntity : class, IObjectIdentity;

        TEntity Update<TEntity>(TEntity entity)
            where TEntity : class, IObjectIdentity;

        void Delete<TEntity>(TEntity entity)
            where TEntity : class, IObjectIdentity;
    }
}
