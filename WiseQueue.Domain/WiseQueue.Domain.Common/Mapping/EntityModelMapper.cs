using System;
using System.Collections.Generic;
using AutoMapper;
using Common.Core.BaseClasses;
using Common.Core.Interfaces.DataAccessLayer;
using Common.Core.Logging;

namespace WiseQueue.Domain.Common.Mapping
{
    class EntityModelMapper: BaseLoggerObject, IEntityModelMapper
    {
        private readonly IMapper mapper;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        public EntityModelMapper(IMapper mapper, ICommonLoggerFactory loggerFactory) : base(loggerFactory)
        {
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));

            this.mapper = mapper;
        }

        #region Implementation of IEntityModelMapper

        /// <summary>
        /// Map an entity into the model.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity.</typeparam>
        /// <typeparam name="TModel">Type of model</typeparam>
        /// <param name="entity">The entity</param>
        /// <returns>The model</returns>
        public TModel Map<TEntity, TModel>(TEntity entity)
            where TEntity : IObjectIdentity
        {
            TModel model = mapper.Map<TModel>(entity);
            return model;
        }

        /// <summary>
        /// Map a model into the entity.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity.</typeparam>
        /// <typeparam name="TModel">Type of model</typeparam>        
        /// <param name="model">The model.</param>
        /// <returns>The entity</returns>
        public TEntity Map<TEntity, TModel>(TModel model)
            where TEntity : IObjectIdentity
        {
            TEntity entity = mapper.Map<TEntity>(model);
            return entity;
        }

        /// <summary>
        /// Map list of entities into the models list.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity.</typeparam>
        /// <typeparam name="TModel">Type of model</typeparam>
        /// <param name="entities">List of entities.</param>
        /// <returns>List of models</returns>
        public List<TModel> Map<TEntity, TModel>(List<TEntity> entities) 
            where TEntity : class, IObjectIdentity 
        {
            List<TModel> models = mapper.Map<List<TModel>>(entities);
            return models;
        }

        /// <summary>
        /// Map list of models into the entities list.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity.</typeparam>
        /// <typeparam name="TModel">Type of model</typeparam>        
        /// <param name="models">List of models</param>
        /// <returns>List of entities</returns>
        public List<TEntity> Map<TEntity, TModel>(List<TModel> models) 
            where TEntity : IObjectIdentity 
            where TModel : IObjectIdentity
        {
            List<TEntity> entities = mapper.Map<List<TEntity>>(models);
            return entities;
        }

        #endregion
    }
}
