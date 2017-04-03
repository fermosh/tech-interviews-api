namespace TechnicalInterviewHelper.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// Generic repository to query data entities.
    /// </summary>
    /// <typeparam name="T">An entity class.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface IQueryRepository<T, in TKey> where T : class
    {
        /// <summary>
        /// Finds the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>An entity.</returns>
        Task<T> FindById(TKey id);

        /// <summary>
        /// Finds the by.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>An enumeration of entities.</returns>
        /// TODO: We'll have to include a sorting predicate paramater.
        Task<IEnumerable<T>> FindBy(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>All entities in the collection.</returns>
        Task<IEnumerable<T>> GetAll();
    }
}