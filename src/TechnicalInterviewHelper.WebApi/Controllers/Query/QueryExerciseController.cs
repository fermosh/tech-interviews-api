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

    [Route("query/exercise")]
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
        private readonly IQueryRepository<PositionSkill, string> queryPositionSkill;

        #endregion Repositories

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryExerciseController"/> class.
        /// </summary>
        public QueryExerciseController()
        {
            var exerciseCollection = ConfigurationManager.AppSettings["ExerciseCollectionId"];
            this.queryExercise = new DocumentDbQueryRepository<Exercise, string>(exerciseCollection);
            var positionSkillCollection = ConfigurationManager.AppSettings["PositionSkillCollectionId"];
            this.queryPositionSkill = new DocumentDbQueryRepository<PositionSkill, string>(positionSkillCollection);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryExerciseController"/> class.
        /// </summary>
        /// <param name="queryExercise">The query exercise.</param>
        /// <param name="queryPositionSkill">The query position skill.</param>
        public QueryExerciseController(
            IQueryRepository<Exercise, string> queryExercise,
            IQueryRepository<PositionSkill, string> queryPositionSkill)
        {
            this.queryExercise = queryExercise;
            this.queryPositionSkill = queryPositionSkill;
        }

        #endregion Constructor

        [HttpGet]
        [ActionName("All")]
        public async Task<IHttpActionResult> GetAll(string templateId)
        {
            if (string.IsNullOrEmpty(templateId?.Trim()))
            {
                return BadRequest("Cannot get exercises without a valid identifier");
            }

            var positionSkill = await this.queryPositionSkill.FindById(templateId);
            if (positionSkill == null)
            {
                return NotFound();
            }

            if (positionSkill.SkillIdentifiers == null
                ||
                positionSkill.SkillIdentifiers.Count == 0)
            {
                return BadRequest("There are no existing skill identifiers associated with the template '{templateId}'");
            }

            // Set the predicate to filter respective DataSet through SkillId field.
            var predicateToGetExercises = PredicateBuilder.New<Exercise>(false);

            foreach (var filteredSkillId in positionSkill.SkillIdentifiers)
            {
                predicateToGetExercises = predicateToGetExercises.Or(exercise => exercise.SkillId == filteredSkillId);
            }

            var exercises = await this.queryExercise.FindBy(predicateToGetExercises);

            // Proceed to create the ViewModels as part of our response.
            var exercisesVM = new List<ExerciseViewModel>();
            foreach (var exercise in exercises)
            {
                exercisesVM.Add(new ExerciseViewModel
                {
                    ExerciseId = exercise.EntityId,
                    Description = exercise.Description,
                    ProposedSolution = exercise.ProposedSolution,
                    Title = exercise.Title
                });
            }

            return Ok(exercisesVM);
        }
    }
}