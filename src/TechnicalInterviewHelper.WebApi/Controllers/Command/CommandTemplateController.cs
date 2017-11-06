namespace TechnicalInterviewHelper.WebApi.Controllers
{
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using Model;
    using TechnicalInterviewHelper.Model;

    /// <summary>
    /// Has commands that affect template entities.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/templates")]
    [EnableCors(origins: "*", headers: "*", methods: "POST,PUT")]
    public class CommandTemplateController : ApiController
    {
        #region Repository

        /// <summary>
        /// The command repository
        /// </summary>
        private readonly ICommandRepository<Template> commandRepository;

        /// <summary>
        /// The query template catalog
        /// </summary>
        private readonly IQueryRepository<Template, string> queryRepository;

        #endregion Repository

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="QuerySkillMatrixController"/> class.
        /// </summary>
        /// <param name="skillRepository">The skill repository.</param>
        public CommandTemplateController(
            ICommandRepository<Template> commandRepository,
            IQueryRepository<Template, string> queryRepository)
        {
            this.commandRepository = commandRepository;
            this.queryRepository = queryRepository;
        }

        #endregion Constructor

        /// <summary>
        /// Save the specified template and its skills identifiers into a database.
        /// </summary>
        /// <param name="templateInput">The template with skills to save.</param>
        /// <returns>An identifier of the just created record.</returns>
        /// <example>Send to "api/template" a JSON with the same structure as TemplateInputModel.</example>
        [Route("")]
        [HttpPost]
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
                List<SkillTemplate> skills = new List<SkillTemplate>();
                foreach (var skillInput in templateInput.Skills)
                {
                    SkillTemplate skill = new SkillTemplate();
                    skill.SkillId = skillInput.SkillId;
                    skill.Questions = skillInput.Questions;
                    skills.Add(skill);
                }

                var templateToSave = new Template()
                {
                    Name = templateInput.Name,
                    DocumentTypeId = DocumentType.Templates,
                    CompetencyId = templateInput.CompetencyId,
                    JobFunctionLevel = templateInput.JobFunctionLevel,
                    Skills = skills,
                    Exercises = templateInput.Exercises
                };

                var documentCreatedForPositionSkill = await this.commandRepository.Insert(templateToSave);

                return Ok(documentCreatedForPositionSkill.Id);
            }
            catch (System.Exception)
            {
                return InternalServerError();
            }
        }

        /// <summary>
        /// Update an existing template.
        /// </summary>
        /// <param name="templateInput">he template with skills to update.</param>
        /// <returns></returns>
        [Route("")]
        public async Task<IHttpActionResult> Put(Template template)
        {
            try
            {
                template.DocumentTypeId = DocumentType.Templates;
                template.Name = template.Name.Trim();

                // Validate Template Name
                var existingTemplateName = await this.queryRepository.FindBy(
                        t => t.CompetencyId == template.CompetencyId
                             && t.JobFunctionLevel == template.JobFunctionLevel
                             && t.Id != template.Id
                             && t.Name.ToLower() == template.Name.ToLower()
                    );
                if (existingTemplateName != null && existingTemplateName.Count() > 0)
                {
                    return Ok(new ErrorResult() { Entity = "Template", ErrorDescription = "Duplicated Template Name" });
                }
                
                // Update Template
                await this.commandRepository.Update(template);

                return Ok(template.Id);
            }
            catch (System.Exception)
            {
                return InternalServerError();
            }
        }
    }
}