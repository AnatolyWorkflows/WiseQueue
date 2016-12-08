using System;
using System.Collections.Generic;

namespace Common.Core.Interfaces.DataAccessLayer
{
    /// <summary>
    /// Interface shows, that the object is a repository for the models.
    /// </summary>
    /// <typeparam name="TEntity">Entity's type.</typeparam>
    public interface IRepository<TEntity>
        where TEntity : class, IObjectIdentity
    {
        /// <summary>
        /// Get model by identifier.
        /// </summary>
        /// <param name="objectId">The model's identifier.</param>
        /// <returns>The model, if it exists.</returns>
        TModel Get<TModel>(Int64 objectId);

        /// <summary>
        /// Get all models.
        /// </summary>
        /// <returns>List of models.</returns>
        List<TModel> GetAll<TModel>();

        /// <summary>
        /// Get objects by specification.
        /// </summary>
        /// <param name="specification">Some specification.</param>
        /// <returns>List of models.</returns>
        List<TModel> GetBySpecification<TModel>(ISpecification<TEntity> specification);

        /// <summary>
        /// Insert an model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <return>The model that has been inserted.</return>
        TModel Insert<TModel>(TModel model);

        /// <summary>
        /// Update an model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <return>The model that has been updated.</return>
        TModel Update<TModel>(TModel model);

        /// <summary>
        /// Delete an model.
        /// </summary>
        /// <param name="model">The model.</param>
        void Delete<TModel>(TModel model);
    }
}
