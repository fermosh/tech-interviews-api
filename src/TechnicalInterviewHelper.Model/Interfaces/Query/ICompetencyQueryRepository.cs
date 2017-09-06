namespace TechnicalInterviewHelper.Model
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Proper interface for specific operations in the competency documents.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Model.IQueryRepository{TechnicalInterviewHelper.Model.CompetencyDocument, System.String}" />
    public interface ICompetencyQueryRepository : IQueryRepository<CompetencyDocument, string>
    {
        /// <summary>
        /// Finds a competency.
        /// </summary>
        /// <param name="competencyId">The competency identifier.</param>
        /// <returns>A competency that belongs to the passed identifier.</returns>
        Task<Competency> FindCompetency(int competencyId);

        /// <summary>
        /// Finds child Competencies from a given parent Competency id
        /// </summary>
        /// <param name="parentCompetencyId">The parent competency identifier</param>
        /// <returns>A competency collection that belongs to the passed parent competency identifier</returns>
        Task<IEnumerable<Competency>> FindCompetenciesByParentId(int parentCompetencyId);

        /// <summary>
        /// Finds child Competencies from a given parent Competency id
        /// </summary>
        /// <param name="parentCompetencyId">The parent competency identifier</param>
        /// <returns>A competency collection that belongs to the passed parent competency identifier</returns>
        Task<IEnumerable<int>> FindCompetenciesIdByParentId(int parentCompetencyId);
    }
}