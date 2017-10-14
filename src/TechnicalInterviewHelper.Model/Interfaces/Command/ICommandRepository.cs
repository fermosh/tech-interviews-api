namespace TechnicalInterviewHelper.Model
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Generic repository to handle command queries (saving, e.g.).
    /// </summary>
    /// <typeparam name="T">An entity class.</typeparam>
    public interface ICommandRepository<T> where T : class
    {
        /// <summary>
        /// Saves the specified entity.
        /// </summary>
        /// <param name="entity">Entity to save.</param>
        /// <returns>A task with the saved entity.</returns>
        Task<T> Insert(T entity);

        /// <summary>
        /// Saves the specified entities.
        /// </summary>
        /// <param name="entities">Entities to save.</param>
        /// <returns>A list of resulted errors</returns>
        Task<ICollection<ErrorResult>> Insert(IEnumerable<T> entities);

        /// <summary>
        /// Updates the specified entity identifier.
        /// </summary>
        /// <param name="entity">The entity with modified information.</param>
        /// <returns>A task of void.</returns>
        Task Update(T entity);

        /// <summary>
        /// Deletes an entity
        /// </summary>
        /// <param name="id">The Id</param>
        /// <returns>A task of void.</returns>
        Task Delete(string id);
    }
}