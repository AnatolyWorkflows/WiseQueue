using System;
using System.Collections.Generic;
using Common.Core.BaseClasses;
using Common.Core.Interfaces.DataAccessLayer;
using Common.Core.Logging;
using Common.Domain.DataAccessLayer.Exceptions;

namespace Common.Domain.DataAccessLayer
{
    /// <summary>
    /// This is a base class for all repositories.
    /// </summary>
    /// <typeparam name="TEntity">Entity's type.</typeparam>
    public abstract class BaseRepository<TEntity>: BaseLoggerObject, IRepository<TEntity>
        where TEntity : class, IObjectIdentity
    {
        protected readonly IDataContextFactory dataContextFactory;

        protected readonly IEntityModelMapper entityModelMapper;

        protected BaseRepository(IDataContextFactory dataContextFactory, IEntityModelMapper entityModelMapper, ICommonLogger logger): base(logger)
        {
            if (dataContextFactory == null)
                throw new ArgumentNullException(nameof(dataContextFactory));
            if (entityModelMapper == null)
                throw new ArgumentNullException(nameof(entityModelMapper));

            this.dataContextFactory = dataContextFactory;
            this.entityModelMapper = entityModelMapper;
        }

        #region Implementation of IRepository<TModel,TEntity>

        #region Read operations...

        #region Abstract methods...
        /// <summary>
        /// Occurs when model by identifier from the repository is needed.
        /// </summary>
        /// <param name="objectId">The model's identifier.</param>
        /// <returns>The model, if it exists.</returns>
        protected abstract TModel OnGet<TModel>(Int64 objectId);

        /// <summary>
        /// Occurs when all models are required.
        /// </summary>
        /// <returns>List of models.</returns>
        protected abstract List<TModel> OnGetAll<TModel>();

        /// <summary>
        /// Occurs when all models according to specification are required.
        /// </summary>
        /// <param name="specification">Some specification.</param>
        /// <returns>List of models.</returns>
        protected abstract List<TModel> OnGetBySpecification<TModel>(ISpecification<TEntity> specification);
        #endregion

        /// <summary>
        /// Get model by identifier.
        /// </summary>
        /// <param name="objectId">The model's identifier.</param>
        /// <returns>The model, if it exists.</returns>
        public TModel Get<TModel>(Int64 objectId)
        {
            try
            {
                return OnGet<TModel>(objectId);
            }
            catch (Exception ex)
            {
                throw new DataAccessLayerException("There was a problem during retrieving a model", ex);
            }
        }

        /// <summary>
        /// Get all models.
        /// </summary>
        /// <returns>List of models.</returns>
        public List<TModel> GetAll<TModel>()
        {
            try
            {
                return OnGetAll<TModel>();
            }
            catch (Exception ex)
            {
                throw new DataAccessLayerException("There was a problem during retrieving models", ex);
            }
        }       

        /// <summary>
        /// Get objects by specification.
        /// </summary>
        /// <param name="specification">Some specification.</param>
        /// <returns>List of models.</returns>
        public List<TModel> GetBySpecification<TModel>(ISpecification<TEntity> specification)
        {
            try
            {
                return OnGetBySpecification<TModel>(specification);
            }
            catch (Exception ex)
            {
                throw new DataAccessLayerException("There was a problem during retrieving models", ex);
            }
        }

        #endregion

        #region Modificators...

        #region Abstract methods...

        /// <summary>
        /// Occurs during inserting a new model.
        /// </summary>
        /// <param name="model">The model</param>
        /// <return>The model that has been inserted.</return>
        protected abstract TModel OnInsert<TModel>(TModel model);

        /// <summary>
        /// Occurs during updating an existing model.
        /// </summary>
        /// <param name="model">The model</param>
        /// <return>The model that has been updated.</return>
        protected abstract TModel OnUpdate<TModel>(TModel model);

        /// <summary>
        /// Occurs during deleting an existing model.
        /// </summary>
        /// <param name="model">The model.</param>
        protected abstract void OnDelete<TModel>(TModel model);
        #endregion

        /// <summary>
        /// Insert an model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <return>The model's identifier.</return>
        public TModel Insert<TModel>(TModel model)
        {
            try
            {
                return OnInsert(model);
            }
            catch (Exception ex)
            {
                throw new DataAccessLayerException("There was a problem during inserting the model", ex);
            }
        }

        /// <summary>
        /// Update an model.
        /// </summary>
        /// <param name="model">The model.</param>
        public TModel Update<TModel>(TModel model)
        {
            try
            {
                return OnUpdate(model);
            }
            catch (Exception ex)
            {
                throw new DataAccessLayerException("There was a problem during updating the model", ex);
            }
        }

        /// <summary>
        /// Delete an model.
        /// </summary>
        /// <param name="model">The model.</param>
        public void Delete<TModel>(TModel model)
        {
            try
            {
                OnDelete(model);
            }
            catch (Exception ex)
            {
                throw new DataAccessLayerException("There was a problem during deleting the model", ex);
            }
        }        

        #endregion

        #endregion
    }
}
