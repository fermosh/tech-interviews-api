namespace TechnicalInterviewHelper.WebApi.Controllers
{
    using Services;
    using System;
    using System.Configuration;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using TechnicalInterviewHelper.Model;

    /// <summary>
    /// Has commands that affect template entities.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/questions")]
    [EnableCors(origins: "*", headers: "*", methods: "POST")]
    public class CommandQuestionController : ApiController
    {
        #region Repository

        /// <summary>
        /// The command repository
        /// </summary>
        private readonly ICommandRepository<Question> commandRepository;

        #endregion Repository

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandQuestionController"/> class.
        /// </summary>
        public CommandQuestionController()
        {
            commandRepository = new DocumentDbCommandRepository<Question>(ConfigurationManager.AppSettings["QuestionCollectionId"]);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandQuestionController"/> class.
        /// </summary>
        /// <param name="commandRepository">The question repository.</param>
        public CommandQuestionController(ICommandRepository<Question> commandRepository)
        {
            this.commandRepository = commandRepository;
        }

        #endregion Constructor

        /// <summary>
        /// Save the specified question into the database.
        /// </summary>
        /// <param name="question">The question to save.</param>
        /// <returns>An identifier of the just created record.</returns>
        /// <example>Send to "api/template" a JSON with the same structure as TemplateInputModel.</example>
        [HttpPost]
        public async Task<IHttpActionResult> Post(Question question)
        {
            if (question == null)
            {
                return BadRequest("Request doesn't have a valid question to save.");
            }

            if (question.Tag == null)
            {
                return BadRequest("Input question doesn't have a skill, add it in order to save it.");
            }

            if (question.Competency == null)
            {
                return BadRequest("Input question doesn't have a competency, add it in order to save it.");
            }

            try
            {
                question = await commandRepository.Insert(question);

                return Ok(question.Id);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}