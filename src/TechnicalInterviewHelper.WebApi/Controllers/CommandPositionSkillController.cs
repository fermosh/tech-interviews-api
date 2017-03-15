namespace TechnicalInterviewHelper.WebApi.Controllers
{
    using Model;
    using Services;
    using System.Configuration;
    using System.Threading.Tasks;
    using System.Web.Http;
    using TechnicalInterviewHelper.Model;

    /// <summary>
    /// Has commands that works over PositionSkill entities.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("command/positionskill")]
    public class CommandPositionSkillController : ApiController
    {
        #region Repository

        /// <summary>
        /// The command repository
        /// </summary>
        private readonly ICommandRepository<PositionSkill> commandRepository;

        #endregion Repository

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryPositionSkillController"/> class.
        /// </summary>
        public CommandPositionSkillController()
        {
            this.commandRepository = new DocumentDbCommandRepository<PositionSkill>(ConfigurationManager.AppSettings["SkillCollectionId"]);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryPositionSkillController"/> class.
        /// </summary>
        /// <param name="skillRepository">The skill repository.</param>
        public CommandPositionSkillController(ICommandRepository<PositionSkill> commandRepository)
        {
            this.commandRepository = commandRepository;
        }

        #endregion Constructor

        /// <summary>
        /// Posts the specified position skills to save.
        /// </summary>
        /// <param name="positionSkillToSave">The position skill to save.</param>
        /// <returns>A string as the ID of the just created document, error otherwise.</returns>
        public async Task<IHttpActionResult> Post(PositionSkillInputModel positionSkillToSave)
        {
            if (positionSkillToSave.Position == null)
            {
                return BadRequest("Request doesn't have a position to link with the skills.");
            }

            if (positionSkillToSave.SkillIdentifiers.Count == 0)
            {
                return BadRequest("Cannot save a position without skills, add at least one of them.");
            }

            try
            {
                var position = new Position
                {
                    CompetencyId = positionSkillToSave.Position.CompetencyId,
                    LevelId = positionSkillToSave.Position.LevelId,
                    DomainId = positionSkillToSave.Position.DomainId
                };

                var positionSkillDocumentToSave = new PositionSkill()
                {
                    Position = position,
                    SkillIdentifiers = positionSkillToSave.SkillIdentifiers
                };

                var documentCreatedForPositionSkill = await this.commandRepository.Insert(positionSkillDocumentToSave);

                return Ok(documentCreatedForPositionSkill.Id);
            }
            catch (System.Exception)
            {
                return InternalServerError();
            }
        }
    }
}