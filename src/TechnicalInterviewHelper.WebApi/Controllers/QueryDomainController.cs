namespace TechnicalInterviewHelper.WebApi.Controllers
{
    using Model;
    using Services;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using TechnicalInterviewHelper.Model;

    [RoutePrefix("query/domain")]
    public class QueryDomainController : ApiController
    {
        #region Repository

        private readonly IQueryRepository<Domain, string> queryDomain;

        #endregion Repository

        #region Constructor

        public QueryDomainController()
        {
            this.queryDomain = new DocumentDbQueryRepository<Domain, string>(ConfigurationManager.AppSettings["DomainCollectionId"]);
        }

        public QueryDomainController(IQueryRepository<Domain, string> queryDomain)
        {
            this.queryDomain = queryDomain;
        }

        #endregion Constructor

        [HttpGet]
        [ActionName("all")]
        public async Task<IHttpActionResult> GetAllDomainsOfCompetencyAndLevel(int competencyId, int levelId)
        {
            var domains = await this.queryDomain.FindBy(domain => domain.CompetencyId == competencyId && domain.LevelId == levelId);
            if (domains.Count() == 0)
            {
                return NotFound();
            }

            // TODO: temporal solution to the ID property value.
            var count = 0;

            var domainsVM = new List<DomainViewModel>();
            foreach (var domain in domains)
            {
                count++;

                domainsVM.Add(new DomainViewModel
                {
                    DomainId = count,
                    Name = domain.Name
                });
            }

            return Ok(domainsVM);
        }
    }
}