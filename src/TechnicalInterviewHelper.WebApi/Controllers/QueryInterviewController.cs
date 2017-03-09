namespace TechnicalInterviewHelper.WebApi.Controllers
{
    using Model;
    using Services;
    using System;
    using System.Configuration;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using System.Web.Http;
    using TechnicalInterviewHelper.Model;

    [RoutePrefix("query/interview")]
    public class QueryInterviewController : ApiController
    {
        #region Repositories

        /// <summary>
        /// The query position skill
        /// </summary>
        private readonly IQueryRepository<PositionSkill, string> queryPositionSkill;

        /// <summary>
        /// The query skill
        /// </summary>
        private readonly IQueryRepository<Skill, string> querySkill;

        /// <summary>
        /// The query exercise
        /// </summary>
        private readonly IQueryRepository<Exercise, string> queryExercise;

        /// <summary>
        /// The query question
        /// </summary>
        private readonly IQueryRepository<Question, string> queryQuestion;

        #endregion Repositories

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryInterviewController"/> class.
        /// </summary>
        public QueryInterviewController()
        {
            this.querySkill = new DocumentDbQueryRepository<Skill, string>(ConfigurationManager.AppSettings["SkillCollectionId"]);
            this.queryExercise = new DocumentDbQueryRepository<Exercise, string>(ConfigurationManager.AppSettings["ExerciseCollectionId"]);
            this.queryQuestion = new DocumentDbQueryRepository<Question, string>(ConfigurationManager.AppSettings["QuestionCollectionId"]);
            this.queryPositionSkill = new DocumentDbQueryRepository<PositionSkill, string>(ConfigurationManager.AppSettings["PositionSkillCollectionId"]);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryInterviewController"/> class.
        /// </summary>
        /// <param name="querySkill">The query skill.</param>
        /// <param name="queryExercise">The query exercise.</param>
        /// <param name="queryQuestion">The query question.</param>
        /// <param name="queryPositionSkill">The query position skill.</param>
        public QueryInterviewController(
            IQueryRepository<Skill, string> querySkill,
            IQueryRepository<Exercise, string> queryExercise,
            IQueryRepository<Question, string> queryQuestion,
            IQueryRepository<PositionSkill, string> queryPositionSkill)
        {
            this.querySkill = querySkill;
            this.queryExercise = queryExercise;
            this.queryQuestion = queryQuestion;
            this.queryPositionSkill = queryPositionSkill;
        }

        #endregion Constructor

        public async Task<IHttpActionResult> Get(string positionSkillId)
        {
            if (string.IsNullOrEmpty(positionSkillId.Trim()))
            {
                return BadRequest("Cannot get an interview without an identifier of filtered skills for a position.");
            }

            var positionSkill = await this.queryPositionSkill.FindById(positionSkillId);
            if (positionSkillId == null)
            {
                return NotFound();
            }

            var interviewVM = new InterviewViewModel();

            Expression<Func<Skill, bool>> kk = (skill) => skill.Id == "1";

            foreach (var skill in positionSkill.SkillIdentifiers)
            {

            }

            throw new NotImplementedException();
        }
    }
}