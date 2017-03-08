namespace TechnicalInterviewHelper.WebApi.Controllers
{
    using Services;
    using System.Configuration;
    using System.Web.Http;
    using TechnicalInterviewHelper.Model;

    [RoutePrefix("skill")]
    public class SkillsController : ApiController
    {
        #region Private fields

        /// <summary>
        /// The collection identifier
        /// </summary>
        private readonly string collectionId;

        /// <summary>
        /// The query repository
        /// </summary>
        private readonly IQueryRepository<Skill, string> queryRepository;

        /// <summary>
        /// The command repository
        /// </summary>
        private readonly ICommandRepository<Skill> commandRepository;

        #endregion Private fields

        #region Constructor

        public SkillsController()
        {
            this.collectionId = ConfigurationManager.AppSettings["SkillCollectionId"];
            this.queryRepository = new DocumentDbQueryRepository<Skill, string>(collectionId);
            this.commandRepository = new DocumentDbCommandRepository<Skill>(collectionId);
        }

        public SkillsController(
            string collectionId,
            IQueryRepository<Skill, string> queryRepository,
            ICommandRepository<Skill> commandRepository)
        {
            this.collectionId = collectionId;
            this.queryRepository = queryRepository;
            this.commandRepository = commandRepository;
        }

        #endregion Constructor
    }
}