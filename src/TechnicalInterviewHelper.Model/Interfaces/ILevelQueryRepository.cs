namespace TechnicalInterviewHelper.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// This is under test.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Model.IQueryRepository{TechnicalInterviewHelper.Model.LevelCatalog, System.String}" />
    public interface ILevelQueryRepository : IQueryRepository<LevelCatalog, string>
    {
        /// <summary>
        /// Finds the on internal collection.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>It returns something.</returns>
        Task<IEnumerable<Level>> FindOnInternalCollection(Expression<Func<Level, bool>> predicate);
    }
}