namespace TechnicalInterviewHelper.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// Proper interface for specific operations in the PositionSkill catalog.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Model.IQueryRepository{TechnicalInterviewHelper.Model.SkillMatrixCatalog, System.String}" />
    public interface ISkillMatrixQueryRepository : IQueryRepository<SkillMatrixCatalog, string>
    {
        /// <summary>
        /// Finds the within.
        /// </summary>
        /// <param name="competencyId">The competency identifier.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>An enumeration of skills.</returns>
        Task<IEnumerable<Skill>> FindWithin(int competencyId, Expression<Func<Skill, bool>> predicate);

        /// <summary>
        /// Finds the within.
        /// </summary>
        /// <param name="competencyId">The competency identifier.</param>
        /// <param name="jobFunctionLevel">The job function level.</param>
        /// <param name="skillIds">The skill ids.</param>
        /// <returns>A bunch of tests.</returns>
        Task<IEnumerable<Skill>> FindWithinSkills(int competencyId, int jobFunctionLevel, int[] skillIds);
    }
}