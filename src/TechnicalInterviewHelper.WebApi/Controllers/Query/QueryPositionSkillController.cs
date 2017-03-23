namespace TechnicalInterviewHelper.WebApi.Controllers
{
    using Model;
    using Services;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using TechnicalInterviewHelper.Model;

    /// <summary>
    /// API for Position-Skill operations.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [Route("positionskill")]
    public class QueryPositionSkillController : ApiController
    {
        #region Repositories

        /// <summary>
        /// The query skill
        /// </summary>
        private readonly IQueryRepository<Skill, string> querySkill;

        /// <summary>
        /// The query position
        /// </summary>
        private readonly IQueryRepository<Position, string> queryPosition;

        #endregion Repositories

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryPositionSkillController"/> class.
        /// </summary>
        public QueryPositionSkillController()
        {
            this.querySkill = new DocumentDbQueryRepository<Skill, string>(ConfigurationManager.AppSettings["SkillCollectionId"]);
            this.queryPosition = new DocumentDbQueryRepository<Position, string>(ConfigurationManager.AppSettings["PositionCollectionId"]);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryPositionSkillController"/> class.
        /// </summary>
        /// <param name="querySkill">The query skill.</param>
        /// <param name="queryPosition">The query position.</param>
        public QueryPositionSkillController(
            IQueryRepository<Skill, string> querySkill,
            IQueryRepository<Position, string> queryPosition)
        {
            this.querySkill = querySkill;
            this.queryPosition = queryPosition;
        }

        #endregion Constructor

        /// <summary>
        /// Gets the specified position to find.
        /// </summary>
        /// <param name="positionToFind">The position to find.</param>
        /// <returns>An HttpResult with either an error or success code with the list of skills.</returns>
        [HttpGet]
        [ActionName("all")]
        public async Task<IHttpActionResult> GetAll(PositionInputModel positionToFind)
        {
            if (positionToFind == null)
            {
                return BadRequest("A position is required in order to get its skills.");
            }

            // Try to locate a position given the competency, level and domain identifiers.
            var positionList = await this.queryPosition.FindBy(
                    item => item.CompetencyId == positionToFind.CompetencyId
                            && item.LevelId == positionToFind.LevelId
                            && item.DomainId == positionToFind.DomainId);

            var position = positionList.SingleOrDefault();
            if (position == null)
            {
                return NotFound();
            }

            // Try to locate all skills that belong to the selected position id.
            var skills = await this.querySkill.FindBy(
                    item => item.PositionId == position.PositionId);

            // Exit early when there are not skills to return.
            if (skills.Count() == 0)
            {
                return NotFound();
            }

            // We have found documents that match the input criteria, so we proceed to include them in the response.
            var skillsVM = new List<SkillForPositionViewModel>();

            foreach (var skill in skills)
            {
                skillsVM.Add(
                    new SkillForPositionViewModel
                    {
                        SkillId = skill.SkillId,
                        ParentId = skill.ParentSkillId,
                        Name = skill.Description,
                    });
            }

            var positionSkillVM = new PositionSkillViewModel()
            {
                Skills = skillsVM
            };

            return Ok(positionSkillVM);
        }
    }
}