namespace TechnicalInterviewHelper.WebApi.Controllers
{
    using LinqKit;
    using Model;
    using Services;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using TechnicalInterviewHelper.Model;

    [RoutePrefix("api/question")]
    [EnableCors(origins: "*", headers: "*", methods: "get")]
    public class QueryQuestionController : ApiController
    {
        #region Repositories

        /// <summary>
        /// The query question
        /// </summary>
        private readonly IQueryRepository<Question, string> queryQuestion;

        /// <summary>
        /// The query position skill
        /// </summary>
        private readonly IQueryRepository<TemplateCatalog, string> queryTemplateCatalog;

        #endregion Repositories

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryQuestionController"/> class.
        /// </summary>
        public QueryQuestionController()
        {
            this.queryQuestion = new DocumentDbQueryRepository<Question, string>(ConfigurationManager.AppSettings["QuestionCollectionId"]);
            this.queryTemplateCatalog = new DocumentDbQueryRepository<TemplateCatalog, string>(ConfigurationManager.AppSettings["TemplateCollectionId"]);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryQuestionController"/> class.
        /// </summary>
        /// <param name="queryQuestion">The query question.</param>
        /// <param name="queryPositionSkill">The query position skill.</param>
        public QueryQuestionController(
            IQueryRepository<Question, string> queryQuestion,
            IQueryRepository<TemplateCatalog, string> queryPositionSkill)
        {
            this.queryQuestion = queryQuestion;
            this.queryTemplateCatalog = queryPositionSkill;
        }

        #endregion Constructor

        [HttpGet]
        [Route("all")]
        public async Task<IHttpActionResult> GetAll(string templateId)
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

            var filterToGetSkills = PredicateBuilder.New<Question>(false);

            foreach (var skillId in templateCatalog.Skills)
            {
                filterToGetSkills = filterToGetSkills.Or(question => question.SkillId == skillId);
            }

            var questions = await this.queryQuestion.FindBy(filterToGetSkills);

            // --------------------------------------
            // Now it's time to build the response.
            // --------------------------------------

            var questionsVM = new List<QuestionViewModel>();

            foreach (var question in questions)
            {
                questionsVM.Add(new QuestionViewModel
                {
                    QuestionId = question.Id,
                    Description = question.Description
                });
            }

            return Ok(questionsVM);
        }
    }
}