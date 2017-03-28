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

    [RoutePrefix("api/domain")]
    public class QueryDomainController : ApiController
    {
        #region Repository

        /// <summary>
        /// The query domain
        /// </summary>
        private readonly IDomainQueryRepository queryDomain;

        #endregion Repository

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryDomainController"/> class.
        /// </summary>
        public QueryDomainController()
        {
            this.queryDomain = new DomainDocumentDbQueryRepository(ConfigurationManager.AppSettings["DomainCollectionId"]);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryDomainController"/> class.
        /// </summary>
        /// <param name="queryDomain">The query domain.</param>
        public QueryDomainController(IDomainQueryRepository queryDomain)
        {
            this.queryDomain = queryDomain;
        }

        #endregion Constructor

        /// <summary>
        /// Gets all domains that belong to certain competency and level.
        /// </summary>
        /// <param name="competencyId">The competency identifier.</param>
        /// <param name="levelId">The level identifier.</param>
        /// <returns>A list of domains that belong to certain competency and level.</returns>
        [HttpGet]
        [Route("all")]
        public async Task<IHttpActionResult> GetAll(int competencyId, int levelId)
        {
            var domains = await this.queryDomain.FindWithin(item => item.CompetencyId == competencyId && item.LevelId == levelId);

            if (domains.Count() == 0)
            {
                return NotFound();
            }

            var domainsVM = new List<DomainViewModel>();
            foreach (var domain in domains)
            {
                domainsVM.Add(new DomainViewModel
                {
                    CompetencyId = domain.CompetencyId,
                    LevelId = domain.LevelId,
                    DomainId = domain.DomainId,
                    Name = domain.Name
                });
            }

            return Ok(domainsVM);
        }
    }
}