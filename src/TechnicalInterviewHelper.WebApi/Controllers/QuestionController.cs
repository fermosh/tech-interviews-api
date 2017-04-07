namespace TechnicalInterviewHelper.WebApi.Controllers
{
    using Model;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
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
        public QuestionController()
        {
            this.questionQueryRepository = new QuestionDocumentDbQueryRepository(ConfigurationManager.AppSettings["QuestionCollectionId"]);
            this.templateQueryRepository = new DocumentDbQueryRepository<Template, string>(ConfigurationManager.AppSettings["TemplateCollectionId"]);
            this.commandRepository = new DocumentDbCommandRepository<Question>(ConfigurationManager.AppSettings["QuestionCollectionId"]);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionController"/> class.
        /// </summary>
        /// <param name="queryQuestion">Question repository.</param>
        /// <param name="queryTemplateCatalog">Template catalog repository.</param>
        public QuestionController(
            IQuestionQueryRepository questionQueryRepository,
            ICommandRepository<Question> commandRepository,
            IQueryRepository<Template, string> templateQueryRepository)
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

            if (templateId == null)
            {
                return BadRequest("Cannot get questions without a valid template identifier.");
            }

            var templateCatalog = await templateQueryRepository.FindById(templateId);
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

            var questions = await questionQueryRepository.GetAll(templateCatalog.CompetencyId, templateCatalog.JobFunctionLevel, templateCatalog.Skills.ToArray());

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