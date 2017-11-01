namespace TechnicalInterviewHelper.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using Model;
    using TechnicalInterviewHelper.Model;
    using TechnicalInterviewHelper.Model.Entities.Comparers;

    /// <summary>
    /// API for Position-Skill operations.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/skillmatrix")]
    [EnableCors(origins: "*", headers: "*", methods: "GET")]
    public class QuerySkillMatrixController : ApiController
    {
        #region Repositories

        /// <summary>
        /// The query skill
        /// </summary>
        private readonly ISkillMatrixQueryRepository querySkillMatrix;

        /// <summary>
        /// The competency query 
        /// </summary>
        private readonly ICompetencyQueryRepository queryCompetency;

        #endregion Repositories

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="QuerySkillMatrixController"/> class.
        /// </summary>
        /// <param name="queryPositionSkill">The query skill.</param>
        public QuerySkillMatrixController(ISkillMatrixQueryRepository queryPositionSkill, ICompetencyQueryRepository queryCompetency)
        {
            this.querySkillMatrix = queryPositionSkill;
            this.queryCompetency = queryCompetency;
        }
        #endregion Constructor

        /// <summary>
        /// Gets the skill matrix by competency and level.
        /// </summary>
        /// <param name="competencyId">The competency identifier.</param>
        /// <param name="jobFunctionLevel">The job function level.</param>
        /// <returns>An HttpResult with either an error or success code with the list of skills.</returns>
        [Route("{competencyId:int}/{jobFunctionLevel:int}")]
        public async Task<IHttpActionResult> GetSkillMatrixByCompetencyAndLevel(int competencyId, int jobFunctionLevel)
        {
            // Try to locate all skills that belong to the selected competency and level Id.
            var skills = await this.querySkillMatrix
                .FindWithin(competencyId, skill => skill.CompetencyId == competencyId && skill.JobFunctionLevel == jobFunctionLevel);

            // return an http 200 status with the SkillMatrixViewModel
            return Ok(SkillMatrixViewModel.Create(competencyId, skills).Skills);
        }

        [Route("parent/{parentCompetencyId:int}/{jobFunctionLevel:int}")]
        public async Task<IHttpActionResult> GetSkillMatrixByParentCompetencyAndLevel(int parentCompetencyId, int jobFunctionLevel)
        {
            // list to store the competencies to query
            List<int> competencies = new List<int> { parentCompetencyId };
            competencies.AddRange(await this.queryCompetency.FindCompetenciesIdByParentId(parentCompetencyId));

            // hashSet to store unique skills
            var SkillsHashSet = new HashSet<Skill>(await this.querySkillMatrix.FindWithinSkills(competencies, jobFunctionLevel), new SkillComparer());

            // return an http 200 status with the SkillMatrixViewModel
            return Ok(SkillMatrixViewModel.Create(parentCompetencyId, SkillsHashSet));
        }

        [Route("{competencyId:int}")]
        public async Task<IHttpActionResult> GetSkillMatrixByCompetency(int competencyId)
        {
            var skills = await querySkillMatrix
                .FindWithinSkills(competencyId);

            // return an http 200 status with the SkillMatrixViewModel
            return Ok(SkillMatrixViewModel.Create(competencyId, skills).Skills);

        }
    }
}