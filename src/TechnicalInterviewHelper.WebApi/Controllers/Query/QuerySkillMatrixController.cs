namespace TechnicalInterviewHelper.WebApi.Controllers
{    
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using Model;
    using TechnicalInterviewHelper.Model;

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

        #endregion Repositories

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="QuerySkillMatrixController"/> class.
        /// </summary>
        /// <param name="queryPositionSkill">The query skill.</param>
        public QuerySkillMatrixController(ISkillMatrixQueryRepository queryPositionSkill)
        {
            this.querySkillMatrix = queryPositionSkill;
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
            var skills = await this.querySkillMatrix.FindWithin(competencyId, skill => skill.CompetencyId == competencyId &&
                                                                                         skill.JobFunctionLevel == jobFunctionLevel);

            // We have found documents that match the input criteria, so we proceed to include them in the response.
            var skillsVM = new List<SkillForPositionViewModel>();

            foreach (var skill in skills)
            {
                // Map all the topics that the skill could have.
                var topics = new List<TopicViewModel>();
                foreach (var topic in skill.Topics)
                {
                    topics.Add(new TopicViewModel
                    {
                        Name = topic.Name,
                        IsRequired = topic.IsRequired
                    });
                }

                // Create the view model of the skill.
                var skillVM = new SkillForPositionViewModel
                {
                    RootId = skill.RootId,
                    DisplayOrder = skill.DisplayOrder,
                    RequiredSkillLevel = skill.RequiredSkillLevel,
                    UserSkillLevel = skill.UserSkillLevel,
                    LevelsSet = skill.LevelsSet,
                    CompetencyId = skill.CompetencyId,
                    JobFunctionLevel = skill.JobFunctionLevel,
                    Topics = topics,
                    Id = skill.Id,
                    ParentId = skill.ParentId,
                    Name = skill.Name,
                    IsSelectable = skill.IsSelectable
                };

                skillsVM.Add(skillVM);
            }

            var positionSkillVM = new SkillMatrixViewModel()
            {
                HasContent = skillsVM.Count() > 0,
                CompetencyId = competencyId,
                Skills = skillsVM
            };

            return Ok(positionSkillVM);
        }
    }
}