namespace TechnicalInterviewHelper.WebApi.Controllers
{
    using System.Configuration;
    using System.Threading.Tasks;
    using System.Web.Http;
    using TechnicalInterviewHelper.Model;
    using TechnicalInterviewHelper.Services;

    public class SkillMatrixController : ApiController
    {
        private readonly IQueryRepository<Competency, string> competencyRepository;
        private readonly IQueryRepository<Level, string> levelRepository;
        private readonly IQueryRepository<Domain, string> domainRepository;
        private readonly IQueryRepository<Skill, string> skillRepository;

        #region Constructor

        public SkillMatrixController()
        {
            this.competencyRepository = new DocumentDbQueryRepository<Competency, string>(ConfigurationManager.AppSettings["CompetencyCollectionId"]);
            this.levelRepository = new DocumentDbQueryRepository<Level, string>(ConfigurationManager.AppSettings["LevelCollectionId"]);
            this.domainRepository = new DocumentDbQueryRepository<Domain, string>(ConfigurationManager.AppSettings["DomainCollectionId"]);
            this.skillRepository = new DocumentDbQueryRepository<Skill, string>(ConfigurationManager.AppSettings["SkillCollectionId"]);
        }

        public SkillMatrixController(
            IQueryRepository<Competency, string> competencyRepository,
            IQueryRepository<Level, string> levelRepository,
            IQueryRepository<Domain, string> domainRepository,
            IQueryRepository<Skill, string> skillRepository)
        {
            this.competencyRepository = competencyRepository;
            this.levelRepository = levelRepository;
            this.domainRepository = domainRepository;
            this.skillRepository = skillRepository;
        }

        #endregion Constructor

        [HttpGet]
        [ActionName("Get")]
        public async Task<SkillMatrix> GetSkillMatrix(int competencyId, int levelId, int domainId)
        {
            var skillMatrixResult = new SkillMatrix();

            var competency = await this.competencyRepository.FindById(competencyId.ToString());
            skillMatrixResult.Competency = competency;

            var level = await this.levelRepository.FindById(levelId.ToString());
            skillMatrixResult.Level = level;

            var domain = await this.domainRepository.FindById(domainId.ToString());
            skillMatrixResult.Domain = domain;

            var skills = await this.skillRepository.FindBy(skill => skill.Position.CompetencyId == competencyId);
            skillMatrixResult.Skills = skills;

            return skillMatrixResult;
        }
    }
}