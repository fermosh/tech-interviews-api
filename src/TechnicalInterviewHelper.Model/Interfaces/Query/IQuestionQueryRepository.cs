namespace TechnicalInterviewHelper.Model
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Proper interface for specific operations in the Questions catalog.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Model.IQueryRepository{TechnicalInterviewHelper.Model.Question, System.String}" />
    public interface IQuestionQueryRepository : IQueryRepository<Question, string>
    {
        /// <summary>
        /// Select all those questions that have skill id as one of its values.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <returns>An enumeration of questions.</returns>
        Task<IEnumerable<Question>> GetAll(Template template);

        /// <summary>
        /// Select all those questions by Ids.
        /// </summary>
        /// <param name="ids">The Ids.</param>
        /// <returns>An enumeration of questions.</returns>
        Task<IEnumerable<Question>> FindByIds(List<string> ids);
    }
}