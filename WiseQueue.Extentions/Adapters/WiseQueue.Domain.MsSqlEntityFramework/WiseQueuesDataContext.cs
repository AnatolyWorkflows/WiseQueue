using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using Common.Core.Interfaces.DataAccessLayer;

namespace WiseQueue.Domain.MsSqlEntityFramework
{
    class WiseQueuesDataContext : DbContext, IDataContext
    {

        public WiseQueuesDataContext(DbConnection existingConnection): base(existingConnection, true)
        {            
        }

        #region Implementation of IDataContext

        /// <summary>
        /// Get a set with entities.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <returns>The IQueryable instance.</returns>
        public IQueryable<TEntity> GetCollectionSet<TEntity>()
            where TEntity : class, IObjectIdentity
        {
            DbSet<TEntity> collection = Set<TEntity>();
            return collection;
        }

        public TEntity Insert<TEntity>(TEntity entity)
            where TEntity : class, IObjectIdentity
        {
            DbSet<TEntity> collection = Set<TEntity>();
            TEntity result = collection.Add(entity);
            return result;
        }

        public TEntity Update<TEntity>(TEntity entity)
            where TEntity : class, IObjectIdentity
        {
            DbSet<TEntity> collection = Set<TEntity>();
            TEntity result = collection.Attach(entity);
            return result;
        }

        public void Delete<TEntity>(TEntity model)
            where TEntity : class, IObjectIdentity
        {
            Entry(model).State = EntityState.Deleted;
        }

        #endregion
    }
}