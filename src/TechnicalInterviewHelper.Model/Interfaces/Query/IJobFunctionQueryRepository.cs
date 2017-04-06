namespace TechnicalInterviewHelper.Model
{
    using System.Threading.Tasks;

    /// <summary>
    /// Proper interface for specific operations in the JobFunction catalog.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Model.IQueryRepository{TechnicalInterviewHelper.Model.JobFunctionCatalog, System.String}" />
    public interface IJobFunctionQueryRepository : IQueryRepository<JobFunctionCatalog, string>
    {
        /// <summary>
        /// Finds the title of an specific level within job function catalog.
        /// </summary>
        /// <param name="jobFunctionId">The job function identifier.</param>
        /// <param name="jobFunctionLevel">The job function level.</param>
        /// <returns>The position title of the level.</returns>
        Task<string> FindJobTitleWithinLevels(int jobFunctionId, int jobFunctionLevel);
    }
}