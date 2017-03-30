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

    [Route("query/question")]
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
        private readonly IQueryRepository<TemplateCatalog, string> queryPositionSkill;

        #endregion Repositories

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryQuestionController"/> class.
        /// </summary>
        public QueryQuestionController()
        {
            var questionCollectionId = ConfigurationManager.AppSettings["QuestionCollectionId"];
            this.queryQuestion = new DocumentDbQueryRepository<Question, string>(questionCollectionId);

            var positionSkillCollectionId = ConfigurationManager.AppSettings["PositionSkillCollectionId"];
            this.queryPositionSkill = new DocumentDbQueryRepository<TemplateCatalog, string>(positionSkillCollectionId);
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
            this.queryPositionSkill = queryPositionSkill;
        }

        #endregion Constructor

        [HttpGet]
        [ActionName("all")]
        public async Task<IHttpActionResult> GetAll(string templateId)
        {
            return await Task.FromResult(Ok());

            /*
            if (string.IsNullOrEmpty(templateId?.Trim()))
            {
                return BadRequest("Cannot get questions without a valid identifier");
            }

            var positionSkill = await this.queryPositionSkill.FindById(templateId);
            if (positionSkill == null)
            {
                return NotFound();
            }

            if (positionSkill.Skills == null
                ||
                positionSkill.Skills.Count == 0)
            {
                return BadRequest("There are no existing skill identifiers associated with the template '{templateId}'");
            }

            // Set the predicate to filter the dataset through SkillId field.
            var predicateToGetQuestions = PredicateBuilder.New<Question>(false);
            foreach (var filteredSkillId in positionSkill.Skills)
            {
                predicateToGetQuestions = predicateToGetQuestions.Or(question => question.SkillId == filteredSkillId);
            }

            var questions = await this.queryQuestion.FindBy(predicateToGetQuestions);

            // Proceed to create the ViewModels as part of our response.
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
            */
        }
    }
}