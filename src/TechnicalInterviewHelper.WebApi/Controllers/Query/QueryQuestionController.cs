namespace TechnicalInterviewHelper.WebApi.Controllers
{
    using Model;
    using Services;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;
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
        private readonly IQueryRepository<Template, string> queryTemplateCatalog;

        #endregion Repositories

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryQuestionController"/> class.
        /// </summary>
        public QueryQuestionController()
        {
            this.queryQuestion = new QuestionDocumentDbQueryRepository(ConfigurationManager.AppSettings["QuestionCollectionId"]);
            this.queryTemplateCatalog = new DocumentDbQueryRepository<Template, string>(ConfigurationManager.AppSettings["TemplateCollectionId"]);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryQuestionController"/> class.
        /// </summary>
        /// <param name="queryQuestion">Question repository.</param>
        /// <param name="queryTemplateCatalog">Template catalog repository.</param>
        public QueryQuestionController(
            IQuestionQueryRepository queryQuestion,
            IQueryRepository<Template, string> queryTemplateCatalog)
        {
            this.queryQuestion = queryQuestion;
            this.queryTemplateCatalog = queryTemplateCatalog;
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

            var templateCatalog = await queryTemplateCatalog.FindById(templateId);
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

            var questions = await queryQuestion.GetAll(templateCatalog.CompetencyId, templateCatalog.JobFunctionLevel, templateCatalog.Skills.ToArray());

            // --------------------------------------
            // Now it's time to build the response.
            // --------------------------------------

            var questionsVM = new List<QuestionViewModel>();

            foreach (var question in questions)
            {
                questionsVM.Add(new QuestionViewModel
                {
                    Id = question.Id,
                    Body = question.Body
                });
            }

            return Ok(questionsVM);
        }

        /// <summary>
        /// Gets a list of questions
        /// </summary>
        /// <returns>An HttpResult with either an error or success code with the list of questions.</returns>
        [Route("questions")]
        [HttpGet]
        public async Task<IHttpActionResult> GetQuestions()
        {
            var questions = await queryQuestion.GetAll();

            return Ok(questions);
        }

        /// <summary>
        /// Gets a list of exercises
        /// </summary>
        /// <returns>An HttpResult with either an error or success code with the questions.</returns>
        [Route("questions/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetQuestion(string id)
        {
            var question = await queryQuestion.FindById(id);

            return Ok(question);
        }
    }
}