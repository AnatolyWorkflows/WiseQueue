using System;
using System.Collections.Generic;
using System.Linq;
using Common.Core.Interfaces.DataAccessLayer;
using Common.Core.Logging;
using Common.Domain.DataAccessLayer.Exceptions;

namespace Common.Domain.DataAccessLayer
{
    public class BaseWiseRepository<TEntity> : BaseRepository<TEntity>
        where TEntity : class, IObjectIdentity
    {
        public BaseWiseRepository(IDataContextFactory dataContextFactory, IEntityModelMapper entityModelMapper, ICommonLogger logger) : base(dataContextFactory, entityModelMapper, logger)
        {
        }

        #region Overrides of BaseRepository<TModel,TEntity>

        /// <summary>
        /// Occurs when model by identifier from the repository is needed.
        /// </summary>
        /// <param name="objectId">The model's identifier.</param>
        /// <returns>The model, if it exists.</returns>
        protected override TModel OnGet<TModel>(Int64 objectId)
        {
            using (IDataContext dataContext = dataContextFactory.CreateDataContext())
            {
                IQueryable<TEntity> entities = dataContext.GetCollectionSet<TEntity>();
                TEntity entity = entities.SingleOrDefault(x => x.Id == objectId);

                if (entity == null)
                    throw new ModelNotFoundException("Entity not found");

                TModel model = entityModelMapper.Map<TEntity, TModel>(entity);
                return model;
            }
        }

        /// <summary>
        /// Occurs when all models are required.
        /// </summary>
        /// <returns>List of models.</returns>
        protected override List<TModel> OnGetAll<TModel>()
        {
            using (IDataContext dataContext = dataContextFactory.CreateDataContext())
            {
                IQueryable<TEntity> dataSet = dataContext.GetCollectionSet<TEntity>();
                List<TEntity> entities = dataSet.ToList();

                List<TModel> models = entityModelMapper.Map<TEntity, TModel>(entities);
                return models;
            }
        }

        /// <summary>
        /// Occurs when all models according to specification are required.
        /// </summary>
        /// <param name="specification">Some specification.</param>
        /// <returns>List of models.</returns>
        protected override List<TModel> OnGetBySpecification<TModel>(ISpecification<TEntity> specification)
        {
            using (IDataContext dataContext = dataContextFactory.CreateDataContext())
            {
                IQueryable<TEntity> dataSet = dataContext.GetCollectionSet<TEntity>();
                List<TEntity> entities = dataSet.Where(specification.IsSatisfiedBy()).ToList();

                List<TModel> models = entityModelMapper.Map<TEntity, TModel>(entities);
                return models;
            }
        }


        /// <summary>
        /// Occurs during inserting a new model.
        /// </summary>
        /// <param name="model">The model</param>
        /// <return>The model that has been inserted.</return>
        protected override TModel OnInsert<TModel>(TModel model)
        {
            using (IDataContext dataContext = dataContextFactory.CreateDataContext())
            {
                TEntity entity = entityModelMapper.Map<TEntity, TModel>(model);
                TEntity insertedEntity = dataContext.Insert<TEntity>(entity);

                TModel insertedModel = entityModelMapper.Map<TEntity, TModel>(insertedEntity);
                return insertedModel;
            }
        }

        /// <summary>
        /// Occurs during updating an existing model.
        /// </summary>
        /// <param name="model">The model</param>
        protected override TModel OnUpdate<TModel>(TModel model)
        {
            using (IDataContext dataContext = dataContextFactory.CreateDataContext())
            {
                TEntity entity = entityModelMapper.Map<TEntity, TModel>(model);
                TEntity insertedEntity = dataContext.Update(entity);

                TModel insertedModel = entityModelMapper.Map<TEntity, TModel>(insertedEntity);
                return insertedModel;
            }
        }


        /// <summary>
        /// Occurs during deleting an existing model.
        /// </summary>
        /// <param name="model">The model.</param>
        protected override void OnDelete<TModel>(TModel model)
        {
            using (IDataContext dataContext = dataContextFactory.CreateDataContext())
            {
                TEntity entity = entityModelMapper.Map<TEntity, TModel>(model);
                dataContext.Delete(entity);
            }
        }

        #endregion
    }
}
