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
        private readonly IQueryRepository<TemplateCatalog, string> queryTemplateCatalog;

        #endregion Repositories

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryQuestionController"/> class.
        /// </summary>
        /// <param name="queryQuestion">Question repository.</param>
        /// <param name="queryTemplateCatalog">Template catalog repository.</param>
        public QueryQuestionController(
            IQuestionQueryRepository queryQuestion,
            IQueryRepository<TemplateCatalog, string> queryTemplateCatalog)
        {
            this.queryQuestion = queryQuestion;
            this.queryTemplateCatalog = queryTemplateCatalog;
        }

        #endregion Constructor

        [Route("template/{templateId}/questions")]
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