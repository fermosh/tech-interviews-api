namespace TechnicalInterviewHelper.WebApi.Controllers
{    
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using Model;
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
        private readonly IQueryRepository<TemplateCatalog, string> queryTemplateCatalog;

        /// <summary>
        /// The query skill matrix catalog
        /// </summary>
        private readonly ISkillMatrixQueryRepository querySkillMatrixCatalog;

        #endregion Repositories

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryExerciseController"/> class.
        /// </summary>
        /// <param name="queryExercise">Exercise repository.</param>
        /// <param name="queryTemplateCatalog">TemplateCatalog repository.</param>
        public QueryExerciseController(
            IExerciseQueryRepository queryExercise,
            IQueryRepository<TemplateCatalog, string> queryTemplateCatalog,
            ISkillMatrixQueryRepository querySkillMatrixCatalog)
        {
            this.queryExercise = queryExercise;
            this.queryTemplateCatalog = queryTemplateCatalog;
            this.querySkillMatrixCatalog = querySkillMatrixCatalog;
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

            var exercises = await this.queryExercise.FindWithinExercises(template.CompetencyId, template.JobFunctionLevel, template.Skills.ToArray());

            // -------------------------------------------------------------------------------
            // Try to get skill information for Tag property.
            // -------------------------------------------------------------------------------

            var skillsList = await this.querySkillMatrixCatalog.FindWithinSkills(template.CompetencyId, template.JobFunctionLevel, template.Skills.ToArray());

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
                    Solution = exercise.Solution,
                    Title = exercise.Title,
                    Tags = skillsList.Where(s => exercise.Skills.Contains(s.Id)).Select(s => new TagViewModel() { SkillId = s.Id, Name = s.Name }).ToList()
                });
            }

            return Ok(exercisesVM);
        }
    }
}