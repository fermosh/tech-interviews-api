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
        /// <param name="template">The template.</param>
        /// <returns>An enumeration of exercises.</returns>
        Task<IEnumerable<Exercise>> GetAll(Template template);

        /// <summary>
        /// Select all those exercises by Ids.
        /// </summary>
        /// <param name="ids">The Ids.</param>
        /// <returns>An enumeration of exercises.</returns>
        Task<IEnumerable<Exercise>> FindByIds(List<string> ids);
    }
}