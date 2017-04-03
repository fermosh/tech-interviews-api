namespace TechnicalInterviewHelper.Model
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Proper interface for specific operations in the Exercises catalog.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Model.IQueryRepository{TechnicalInterviewHelper.Model.ExerciseCatalog, System.String}" />
    public interface IExerciseQueryRepository : IQueryRepository<ExerciseCatalog, string>
    {
        /// <summary>
        /// Finds the within exercises.
        /// </summary>
        /// <param name="competencyId">The competency identifier.</param>
        /// <param name="jobFunctionLevel">The job function level.</param>
        /// <param name="skillIds">Skill identifiers to query.</param>
        /// <returns>An enumeration of exercises.</returns>
        Task<IEnumerable<ExerciseCatalog>> FindWithinExercises(int competencyId, int jobFunctionLevel, int[] skillIds);
    }
}