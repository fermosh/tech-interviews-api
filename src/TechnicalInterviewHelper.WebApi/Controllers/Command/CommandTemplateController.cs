namespace TechnicalInterviewHelper.WebApi.Controllers
{    
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using Model;
    using TechnicalInterviewHelper.Model;

    /// <summary>
    /// Has commands that affect template entities.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/template")]
    [EnableCors(origins: "*", headers: "*", methods: "POST")]
    public class CommandTemplateController : ApiController
    {
        #region Repository

        /// <summary>
        /// The command repository
        /// </summary>
        private readonly ICommandRepository<TemplateCatalog> commandRepository;

        #endregion Repository

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="QuerySkillMatrixController"/> class.
        /// </summary>
        /// <param name="skillRepository">The skill repository.</param>
        public CommandTemplateController(ICommandRepository<TemplateCatalog> commandRepository)
        {
            this.commandRepository = commandRepository;
        }

        #endregion Constructor

        /// <summary>
        /// Save the specified template and its skills identifiers into a database.
        /// </summary>
        /// <param name="templateInput">The template with skills to save.</param>
        /// <returns>An identifier of the just created record.</returns>
        /// <example>Send to "api/template" a JSON with the same structure as TemplateInputModel.</example>
        [Route("")]
        public async Task<IHttpActionResult> Post(TemplateInputModel templateInput)
        {
            if (templateInput == null)
            {
                return BadRequest("Request doesn't have a valid template to save.");
            }

            if (templateInput.Skills == null
                ||
                templateInput.Skills.Count() == 0)
            {
                return BadRequest("Input template doesn't have skills, add some ones in order to save it.");
            }

            try
            {
                var templateToSave = new TemplateCatalog()
                {
                    CompetencyId = templateInput.CompetencyId,
                    JobFunctionLevel = templateInput.JobFunctionLevel,
                    Skills = templateInput.Skills
                };

                var documentCreatedForPositionSkill = await this.commandRepository.Insert(templateToSave);

                return Ok(documentCreatedForPositionSkill.Id);
            }
            catch (System.Exception)
            {
                return InternalServerError();
            }
        }
    }
}