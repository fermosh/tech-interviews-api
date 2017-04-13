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
    public class ExerciseController : ApiController
    {
        #region Repositories

        /// <summary>
        /// exercise repository.
        /// </summary>
        private readonly IExerciseQueryRepository exerciseQueryRepository;

        /// <summary>
        /// The command repository
        /// </summary>
        private readonly ICommandRepository<Exercise> commandRepository;

        /// <summary>
        /// Template catalog repository.
        /// </summary>
        private readonly IQueryRepository<Template, string> templateQueryRepository;

        #endregion Repositories

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ExerciseController"/> class.
        /// </summary>
        /// <param name="queryQuestion">Question repository.</param>
        /// <param name="queryTemplateCatalog">Template catalog repository.</param>
        public ExerciseController(
            IExerciseQueryRepository exerciseQueryRepository,
            ICommandRepository<Exercise> commandRepository,
            IQueryRepository<Template, string> templateQueryRepository)
        {
            this.exerciseQueryRepository = exerciseQueryRepository;
            this.commandRepository = commandRepository;
            this.templateQueryRepository = templateQueryRepository;
        }

        #endregion Constructor

        [Route("templates/{templateId}/exercises")]
        public async Task<IHttpActionResult> GetExercisesByTemplate(string templateId)
        {
            // --------------------------------------------------------------------------------
            // Let's run some validations over the input data and the saved template as well.
            // --------------------------------------------------------------------------------

            if (string.IsNullOrEmpty(templateId?.Trim()))
            {
                return BadRequest("Cannot get exercises without a valid template identifier.");
            }

            var template = await this.templateQueryRepository.FindById(templateId);
            if (template == null)
            {
                return NotFound();
            }

            if (template.Skills == null
                ||
                template.Skills.Count() == 0)
            {
                return BadRequest($"The template '{templateId}' doesn't have associated skills.");
            }

            // -------------------------------------------------------------------------------
            // Try to get all filteres skill information using its id, competency and level.
            // -------------------------------------------------------------------------------

            var exercises = await exerciseQueryRepository.GetAll(template.CompetencyId, template.JobFunctionLevel, template.Skills.ToArray());

            return Ok(exercises);
        }

        /// <summary>
        /// Gets a list of exercises
        /// </summary>
        /// <returns>An HttpResult with either an error or success code with the list of exercises.</returns>
        [Route("exercises")]
        public async Task<IHttpActionResult> Get()
        {
            var exercises = await exerciseQueryRepository.GetAll();

            return Ok(exercises);
        }

        /// <summary>
        /// Gets a list of exercises
        /// </summary>
        /// <returns>An HttpResult with either an error or success code with the exercises.</returns>
        [Route("exercises/{id}")]
        public async Task<IHttpActionResult> Get(string id)
        {
            var exercise = await exerciseQueryRepository.FindById(id);

            return Ok(exercise);
        }

        [Route("exercises")]
        [HttpPost]
        public async Task<IHttpActionResult> Post(Exercise exercise)
        {
            if (exercise == null)
            {
                return BadRequest("Request doesn't have a valid exercise to save.");
            }

            if (exercise.Skills == null || !exercise.Skills.Any())
            {
                return BadRequest("Input exercise doesn't have a skill, add it in order to save it.");
            }

            if (exercise.Competency == null)
            {
                return BadRequest("Input exercise doesn't have a competency, add it in order to save it.");
            }

            try
            {
                var result = await commandRepository.Insert(exercise);

                return Ok(result.Id);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("exercises")]
        [HttpPut]
        public async Task<IHttpActionResult> Put(Exercise exercise)
        {
            try
            {
                await commandRepository.Update(exercise);

                return Ok();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("exercises/{id}")]
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