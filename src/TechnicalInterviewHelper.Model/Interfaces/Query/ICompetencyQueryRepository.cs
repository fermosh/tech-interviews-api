namespace TechnicalInterviewHelper.Model
{
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
    }
}