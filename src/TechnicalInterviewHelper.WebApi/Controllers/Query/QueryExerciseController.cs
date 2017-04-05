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
    public class QueryExerciseController : ApiController
    {
        #region Repositories

        /// <summary>
        /// The Exercise repository.
        /// </summary>
        private readonly IExerciseQueryRepository queryExercise;

        /// <summary>
        /// The TemplateCatalog repository.
        /// </summary>
        private readonly IQueryRepository<Template, string> queryTemplateCatalog;

        #endregion Repositories

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryExerciseController"/> class.
        /// </summary>
        public QueryExerciseController()
        {
            this.queryExercise = new ExerciseDocumentDbQueryRepository(ConfigurationManager.AppSettings["ExerciseCollectionId"]);
            this.queryTemplateCatalog = new DocumentDbQueryRepository<Template, string>(ConfigurationManager.AppSettings["TemplateCollectionId"]);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryExerciseController"/> class.
        /// </summary>
        /// <param name="queryExercise">Exercise repository.</param>
        /// <param name="queryTemplateCatalog">TemplateCatalog repository.</param>
        public QueryExerciseController(
            IExerciseQueryRepository queryExercise,
            IQueryRepository<Template, string> queryTemplateCatalog)
        {
            this.queryExercise = queryExercise;
            this.queryTemplateCatalog = queryTemplateCatalog;
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

            var template = await this.queryTemplateCatalog.FindById(templateId);
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

            var exercises = await queryExercise.GetAll();

            // --------------------------------------
            // Now it's time to build the response.
            // --------------------------------------

            var exercisesVM = new List<ExerciseViewModel>();

            foreach (var exercise in exercises)
            {
                exercisesVM.Add(new ExerciseViewModel
                {
                    Id = exercise.Id,
                    Body = exercise.Body,
                    Solution = exercise.Solution,
                    Title = exercise.Title
                });
            }

            return Ok(exercisesVM);
        }

        /// <summary>
        /// Gets a list of exercises
        /// </summary>
        /// <returns>An HttpResult with either an error or success code with the list of exercises.</returns>
        [Route("exercises")]
        public async Task<IHttpActionResult> GetExercises()
        {
            var exercises = await queryExercise.GetAll();

            return Ok(exercises);
        }

        /// <summary>
        /// Gets a list of exercises
        /// </summary>
        /// <returns>An HttpResult with either an error or success code with the list of exercises.</returns>
        [Route("exercises/{id}")]
        public async Task<IHttpActionResult> GetExercise(string id)
        {
            var exercises = await queryExercise.FindById(id);

            return Ok(exercises);
        }
    }
}