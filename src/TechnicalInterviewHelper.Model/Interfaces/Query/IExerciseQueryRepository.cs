namespace TechnicalInterviewHelper.Model
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Proper interface for specific operations in the Exercises catalog.
    /// </summary>
    /// <seealso cref="IQueryRepository{ExerciseCatalog, String}" />
    public interface IExerciseQueryRepository : IQueryRepository<Exercise, string>
    {
        /// <summary>
        /// Finds the within exercises.
        /// </summary>
        /// <param name="competencyId">The competency identifier.</param>
        /// <param name="jobFunctionLevel">The job function level.</param>
        /// <param name="skillIds">Skill identifiers to query.</param>
        /// <returns>An enumeration of exercises.</returns>
        Task<IEnumerable<Exercise>> GetAll(int competencyId, int jobFunctionLevel, int[] skillIds);
    }
}