namespace TechnicalInterviewHelper.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// Proper interface for specific operations in the domain catalog.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Model.IQueryRepository{TechnicalInterviewHelper.Model.DomainCatalog, System.String}" />
    public interface IDomainQueryRepository : IQueryRepository<DomainCatalog, string>
    {
        /// <summary>
        /// Finds within a document of a collection.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>An enumeration of domains.</returns>
        Task<IEnumerable<Domain>> FindWithin(Expression<Func<Domain, bool>> predicate);
    }
}