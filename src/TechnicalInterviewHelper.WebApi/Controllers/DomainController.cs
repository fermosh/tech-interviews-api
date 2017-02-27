namespace TechnicalInterviewHelper.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading.Tasks;
    using System.Web.Http;
    using TechnicalInterviewHelper.Model;
    using TechnicalInterviewHelper.Services;

    public class DomainController : ApiController
    {
        private readonly IQueryRepository<Domain, string> Repository;

        #region Constructor

        public DomainController()
        {
            this.Repository = new DocumentDbQueryRepository<Domain, string>(ConfigurationManager.AppSettings["DomainCollectionId"]);
        }

        public DomainController(IQueryRepository<Domain, string> domainRepository)
        {
            this.Repository = domainRepository;
        }

        #endregion Constructor

        #region Get

        [HttpGet]
        [ActionName("All")]
        public async Task<IEnumerable<Domain>> GetAll()
        {
            return await this.Repository.GetAll();
        }

        #endregion Get
    }
}