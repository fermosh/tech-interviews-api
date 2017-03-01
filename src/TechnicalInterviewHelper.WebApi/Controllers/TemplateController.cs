namespace TechnicalInterviewHelper.WebApi.Controllers
{
    using Model;
    using Services;
    using System.Configuration;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class TemplateController : ApiController
    {
        #region Private fields

        /// <summary>
        /// The collection identifier
        /// </summary>
        private readonly string collectionId;

        /// <summary>
        /// The query repository
        /// </summary>
        private readonly IQueryRepository<SkillMatrix, string> queryRepository;

        /// <summary>
        /// The command repository
        /// </summary>
        private readonly ICommandRepository<SkillMatrix> commandRepository;

        #endregion Private fields

        #region Constructor

        public TemplateController()
        {
            this.collectionId = ConfigurationManager.AppSettings["SkillCollectionId"];
            this.queryRepository = new DocumentDbQueryRepository<SkillMatrix, string>(collectionId);
            this.commandRepository = new DocumentDbCommandRepository<SkillMatrix>(collectionId);
        }

        public TemplateController(
            string collectionId,
            IQueryRepository<SkillMatrix, string> queryRepository,
            ICommandRepository<SkillMatrix> commandRepository)
        {
            this.collectionId = collectionId;
            this.queryRepository = queryRepository;
            this.commandRepository = commandRepository;
        }

        #endregion Constructor

        #region Get functions

        public async Task<SkillMatrix> Get(string templateId)
        {
            return await this.queryRepository.FindById(templateId);
        }

        public async Task<SkillMatrix> Post(SkillMatrix template)
        {
            return await this.commandRepository.Insert(template);
        }

        public async Task Put(SkillMatrix template)
        {
            var templateId = template.Id.Trim();
            if (string.IsNullOrEmpty(templateId))
            {
                await Task.FromResult(BadRequest());
            }

            await this.commandRepository.Update(template);
        }

        #endregion Get functions
    }
}