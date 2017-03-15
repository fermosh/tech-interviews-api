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
    [RoutePrefix("query/positionskill")]
    public class QueryPositionSkillController : ApiController
    {
        #region Repository

        /// <summary>
        /// The query skill
        /// </summary>
        private readonly IQueryRepository<Skill, string> querySkill;

        #endregion Repository

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryPositionSkillController"/> class.
        /// </summary>
        public QueryPositionSkillController()
        {
            this.querySkill = new DocumentDbQueryRepository<Skill, string>(ConfigurationManager.AppSettings["SkillCollectionId"]);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryPositionSkillController"/> class.
        /// </summary>
        /// <param name="querySkill">The query for skills.</param>
        public QueryPositionSkillController(IQueryRepository<Skill, string> querySkill)
        {
            this.querySkill = querySkill;
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

            var skillsBelongingToPosition = await this.querySkill.FindBy(
                    skill =>
                        skill.CompetencyId == positionToFind.CompetencyId &&
                        skill.LevelId == positionToFind.LevelId &&
                        skill.DomainId == positionToFind.DomainId);

            // Exit early when there are not skills to return.
            if (skillsBelongingToPosition.Count() == 0)
            {
                return NotFound();
            }

            // We have found documents that match the input criteria, so we proceed to include them in the response.
            var skillsVM = new List<SkillForPositionViewModel>();

            foreach (var skill in skillsBelongingToPosition)
            {
                skillsVM.Add(
                    new SkillForPositionViewModel
                    {
                        Name = skill.Name,
                        SkillId = skill.EntityId,
                        ParentSkillId = skill.ParentId,
                        HasChildren = false,
                        SkillLevel = skill.LevelSet
                    });
            }

            var positionVM = new PositionViewModel
            {
                CompetencyId = positionToFind.CompetencyId,
                LevelId = positionToFind.LevelId,
                DomainId = positionToFind.DomainId
            };

            var positionSkillVM = new PositionSkillViewModel()
            {
                Position = positionVM,
                Skills = skillsVM
            };

            return Ok(positionSkillVM);
        }
    }
}