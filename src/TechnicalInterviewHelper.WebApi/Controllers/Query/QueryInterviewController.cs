namespace TechnicalInterviewHelper.WebApi.Controllers
{
    using LinqKit;
    using Model;
    using Services;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading.Tasks;
    using System.Web.Http;
    using TechnicalInterviewHelper.Model;

    [Route("query/interview")]
    public class QueryInterviewController : ApiController
    {
        #region Repositories

        /// <summary>
        /// The query position skill
        /// </summary>
        private readonly IQueryRepository<PositionSkill, string> queryPositionSkill;

        /// <summary>
        /// The query skill
        /// </summary>
        private readonly IQueryRepository<Skill, string> querySkill;


        #endregion Repositories

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryInterviewController"/> class.
        /// </summary>
        public QueryInterviewController()
        {
            this.querySkill = new DocumentDbQueryRepository<Skill, string>(ConfigurationManager.AppSettings["SkillCollectionId"]);
            this.queryPositionSkill = new DocumentDbQueryRepository<PositionSkill, string>(ConfigurationManager.AppSettings["PositionSkillCollectionId"]);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryInterviewController"/> class.
        /// </summary>
        /// <param name="querySkill">The query skill.</param>
        /// <param name="queryExercise">The query exercise.</param>
        /// <param name="queryPositionSkill">The query position skill.</param>
        public QueryInterviewController(
            IQueryRepository<Skill, string> querySkill,
            IQueryRepository<PositionSkill, string> queryPositionSkill)
        {
            this.querySkill = querySkill;
            this.queryPositionSkill = queryPositionSkill;
        }

        #endregion Constructor

        /*
        public async Task<IHttpActionResult> Get(string positionSkillId)
        {
            if (string.IsNullOrEmpty(positionSkillId?.Trim()))
            {
                return BadRequest("Cannot get an interview without an identifier of filtered skills for a position.");
            }

            var positionSkill = await this.queryPositionSkill.FindById(positionSkillId);
            if (positionSkill == null)
            {
                return NotFound();
            }

            // Set the different predicates to filter respective DataSet through SkillId field.
            var predicateToGetSkills = PredicateBuilder.New<Skill>(false);

            foreach (var filteredSkillId in positionSkill.SkillIdentifiers)
            {
                predicateToGetSkills = predicateToGetSkills.Or(skill => skill.SkillId == filteredSkillId);
            }

            var skills = await this.querySkill.FindBy(predicateToGetSkills);

            // Proceed to create the different View Models as part of our response.
            var skillsVM = new List<SkillInterviewViewModel>();
            foreach (var skill in skills)
            {
                skillsVM.Add(new SkillInterviewViewModel
                {
                    SkillId = skill.SkillId,
                    Description = skill.Description
                });
            }

            var interviewViewModelToReturn = new InterviewViewModel
            {
                CompetencyId = positionSkill.Position.CompetencyId,
                Skills = skillsVM
            };

            return Ok(interviewViewModelToReturn);
        }
        */
    }
}