﻿namespace TechnicalInterviewHelper.Model
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
        /// <param name="competencyId">The competency identifier.</param>
        /// <param name="jobFunctionLevel">The job function level.</param>
        /// <param name="skillIds">Skill identifiers to query.</param>
        /// <returns>An enumeration of questions.</returns>
        Task<IEnumerable<Question>> GetAll(int competencyId, int jobFunctionLevel, int[] skillIds);
    }
}