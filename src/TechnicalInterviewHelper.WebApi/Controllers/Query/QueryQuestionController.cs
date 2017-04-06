namespace TechnicalInterviewHelper.WebApi.Controllers
{    
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using Model;
    using TechnicalInterviewHelper.Model;

    [RoutePrefix("api")]
    [EnableCors(origins: "*", headers: "*", methods: "GET")]
    public class QueryQuestionController : ApiController
    {
        #region Repositories

        /// <summary>
        /// Question repository.
        /// </summary>
        private readonly IQuestionQueryRepository queryQuestion;

        /// <summary>
        /// Template catalog repository.
        /// </summary>
        private readonly IQueryRepository<TemplateCatalog, string> queryTemplateCatalog;

        /// <summary>
        /// The query skill matrix catalog
        /// </summary>
        private readonly ISkillMatrixQueryRepository querySkillMatrixCatalog;

        #endregion Repositories

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryQuestionController"/> class.
        /// </summary>
        /// <param name="queryQuestion">Question repository.</param>
        /// <param name="queryTemplateCatalog">Template catalog repository.</param>
        public QueryQuestionController(
            IQuestionQueryRepository queryQuestion,
            IQueryRepository<TemplateCatalog, string> queryTemplateCatalog,
            ISkillMatrixQueryRepository querySkillMatrixCatalog)
        {
            this.queryQuestion = queryQuestion;
            this.queryTemplateCatalog = queryTemplateCatalog;
            this.querySkillMatrixCatalog = querySkillMatrixCatalog;
        }

        #endregion Constructor

        [Route("templates/{templateId}/questions")]
        public async Task<IHttpActionResult> GetQuestionsByTemplate(string templateId)
        {
            // --------------------------------------------------------------------------------
            // Let's run some validations over the input data and the saved template as well.
            // --------------------------------------------------------------------------------

            if (string.IsNullOrEmpty(templateId?.Trim()))
            {
                return BadRequest("Cannot get questions without a valid template identifier.");
            }

            var templateCatalog = await this.queryTemplateCatalog.FindById(templateId);
            if (templateCatalog == null)
            {
                return NotFound();
            }

            if (templateCatalog.Skills == null
                ||
                templateCatalog.Skills.Count() == 0)
            {
                return BadRequest($"The template '{templateId}' doesn't have associated skills.");
            }

            // -------------------------------------------------------------------------------
            // Try to get all filteres skill information using its id, competency and level.
            // -------------------------------------------------------------------------------

            var questions = await this.queryQuestion.FindWithinQuestions(templateCatalog.CompetencyId, templateCatalog.JobFunctionLevel, templateCatalog.Skills.ToArray());

            // -------------------------------------------------------------------------------
            // Try to get skill information for Tag property.
            // -------------------------------------------------------------------------------

            var skillsList = await this.querySkillMatrixCatalog.FindWithinSkills(templateCatalog.CompetencyId, templateCatalog.JobFunctionLevel, templateCatalog.Skills.ToArray());

            // --------------------------------------
            // Now it's time to build the response.
            // --------------------------------------

            var questionsVM = new List<QuestionViewModel>();

            foreach (var question in questions)
            {
                var skill = skillsList.FirstOrDefault(s => s.Id == question.SkillId);
                questionsVM.Add(new QuestionViewModel
                {
                    QuestionId = question.Id,
                    Description = question.Description,
                    Answer = question.Answer,
                    Tag = new TagViewModel
                    {
                        SkillId = question.SkillId,
                        Name = skill != null ? skill.Name : string.Empty
                    }
                });
            }

            return Ok(questionsVM);
        }
    }
}