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

    [RoutePrefix("api/exercise")]
    [EnableCors(origins: "*", headers: "*", methods: "get")]
    public class QueryExerciseController : ApiController
    {
        #region Repositories

        /// <summary>
        /// The query exercise
        /// </summary>
        private readonly IQueryRepository<Exercise, string> queryExercise;

        /// <summary>
        /// The query position skill
        /// </summary>
        private readonly IQueryRepository<TemplateCatalog, string> queryTemplateCatalog;

        #endregion Repositories

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryExerciseController"/> class.
        /// </summary>
        public QueryExerciseController()
        {
            this.queryExercise = new DocumentDbQueryRepository<Exercise, string>(ConfigurationManager.AppSettings["ExerciseCollectionId"]);
            this.queryTemplateCatalog = new DocumentDbQueryRepository<TemplateCatalog, string>(ConfigurationManager.AppSettings["TemplateCollectionId"]);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryExerciseController"/> class.
        /// </summary>
        /// <param name="queryExercise">The query exercise.</param>
        /// <param name="queryPositionSkill">The query position skill.</param>
        public QueryExerciseController(
            IQueryRepository<Exercise, string> queryExercise,
            IQueryRepository<TemplateCatalog, string> queryPositionSkill)
        {
            this.queryExercise = queryExercise;
            this.queryTemplateCatalog = queryPositionSkill;
        }

        #endregion Constructor

        [HttpGet]
        [Route("all/{templateId:int}")]
        public async Task<IHttpActionResult> GetAll(string templateId)
        {
            // --------------------------------------------------------------------------------
            // Let's run some validations over the input data and the saved template as well.
            // --------------------------------------------------------------------------------

            if (string.IsNullOrEmpty(templateId?.Trim()))
            {
                return BadRequest("Cannot get exercises without a valid template identifier.");
            }

            var positionSkill = await this.queryTemplateCatalog.FindById(templateId);
            if (positionSkill == null)
            {
                return NotFound();
            }

            if (positionSkill.Skills == null
                ||
                positionSkill.Skills.Count() == 0)
            {
                return BadRequest($"The template '{templateId}' doesn't have associated skills.");
            }

            // -------------------------------------------------------------------------------
            // Try to get all filteres skill information using its id, competency and level.
            // -------------------------------------------------------------------------------

            var filterToGetSkills = PredicateBuilder.New<Exercise>(false);

            foreach (var skillId in positionSkill.Skills)
            {
                filterToGetSkills = filterToGetSkills.Or(exercise => exercise.SkillId == skillId);
            }

            var exercises = await this.queryExercise.FindBy(filterToGetSkills);

            // --------------------------------------
            // Now it's time to build the response.
            // --------------------------------------

            var exercisesVM = new List<ExerciseViewModel>();

            foreach (var exercise in exercises)
            {
                exercisesVM.Add(new ExerciseViewModel
                {
                    ExerciseId = exercise.Id,
                    Description = exercise.Description,
                    ProposedSolution = exercise.ProposedSolution,
                    Title = exercise.Title
                });
            }

            return Ok(exercisesVM);
        }
    }
}