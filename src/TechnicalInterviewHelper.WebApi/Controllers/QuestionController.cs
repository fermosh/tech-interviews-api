namespace TechnicalInterviewHelper.WebApi.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using TechnicalInterviewHelper.Model;

    [RoutePrefix("api")]
    [EnableCors(origins: "*", headers: "*", methods: "GET,POST,PUT,DELETE")]
    public class QuestionController : ApiController
    {
        #region Repositories

        /// <summary>
        /// Question repository.
        /// </summary>
        private readonly IQuestionQueryRepository questionQueryRepository;

        /// <summary>
        /// The command repository
        /// </summary>
        private readonly ICommandRepository<Question> commandRepository;

        /// <summary>
        /// Template catalog repository.
        /// </summary>
        private readonly IQueryRepository<Template, string> templateQueryRepository;

        #endregion Repositories

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionController"/> class.
        /// </summary>
        /// <param name="queryQuestion">Question repository.</param>
        /// <param name="queryTemplateCatalog">Template catalog repository.</param>
        public QuestionController(
            IQuestionQueryRepository questionQueryRepository,
            ICommandRepository<Question> commandRepository,
            IQueryRepository<Template, string> templateQueryRepository
            )
        {
            this.questionQueryRepository = questionQueryRepository;
            this.commandRepository = commandRepository;
            this.templateQueryRepository = templateQueryRepository;
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

            var template = await this.templateQueryRepository.FindById(templateId);
            if (template == null)
            {
                return NotFound();
            }

            if (template.Skills == null || !template.Skills.Any())
            {
                return BadRequest($"The template '{templateId}' doesn't have associated skills.");
            }

            var questions = await this.questionQueryRepository.GetAll(template);

            return Ok(questions);
        }

        /// <summary>
        /// Gets a list of questions
        /// </summary>
        /// <returns>An HttpResult with either an error or success code with the list of questions.</returns>
        [Route("questions")]
        public async Task<IHttpActionResult> Get()
        {
            var questions = await questionQueryRepository.GetAll();

            return Ok(questions);
        }

        /// <summary>
        /// Gets a list of exercises
        /// </summary>
        /// <returns>An HttpResult with either an error or success code with the questions.</returns>
        [Route("questions/{id}")]
        public async Task<IHttpActionResult> Get(string id)
        {
            var question = await questionQueryRepository.FindById(id);

            return Ok(question);
        }

        [Route("questions")]
        [HttpPost]
        public async Task<IHttpActionResult> Post(Question question)
        {
            if (question == null)
            {
                return BadRequest("Request doesn't have a valid question to save.");
            }

            if (question.Skill == null)
            {
                return BadRequest("Input question doesn't have a skill, add it in order to save it.");
            }

            if (question.Competency == null)
            {
                return BadRequest("Input question doesn't have a competency, add it in order to save it.");
            }

            try
            {
                var result = await commandRepository.Insert(question);

                return Ok(result.Id);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("questions")]
        [HttpPut]
        public async Task<IHttpActionResult> Put(Question question)
        {
            try
            {
                await commandRepository.Update(question);

                return Ok();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("questions/{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(string id)
        {
            try
            {
                await commandRepository.Delete(id);

                return Ok();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}