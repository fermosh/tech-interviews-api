namespace TechnicalInterviewHelper.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading.Tasks;
    using System.Web.Http;
    using TechnicalInterviewHelper.Model;
    using TechnicalInterviewHelper.Services;

    public class CompetencyController : ApiController
    {
        private readonly IQueryRepository<Competency, string> Repository;

        #region Constructor

        public CompetencyController()
        {
            var collectionId = ConfigurationManager.AppSettings["CompetencyCollectionId"];
            this.Repository = new DocumentDbQueryRepository<Competency, string>(collectionId);
        }

        public CompetencyController(IQueryRepository<Competency, string> competencyRepository)
        {
            this.Repository = competencyRepository;
        }

        #endregion Constructor

        #region Get

        [HttpGet]
        [ActionName("All")]
        public async Task<IEnumerable<Competency>> GetAll()
        {
            return await this.Repository.GetAll();
        }

        #endregion Get
    }
}