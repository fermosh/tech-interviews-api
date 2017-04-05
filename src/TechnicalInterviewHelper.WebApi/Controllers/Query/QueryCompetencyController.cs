namespace TechnicalInterviewHelper.WebApi.Controllers
{
    using Model;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using TechnicalInterviewHelper.Model;

    [RoutePrefix("api/competency")]
    [EnableCors(origins: "*", headers: "*", methods: "GET")]
    public class QueryCompetencyController : ApiController
    {
        #region Repository

        private readonly IQueryRepository<CompetencyCatalog, string> queryCompetency;

        #endregion Repository

        #region Constructor

        public QueryCompetencyController(IQueryRepository<CompetencyCatalog, string> competencyRepository)
        {
            this.queryCompetency = competencyRepository;
        }

        #endregion Constructor

        [Route("all")]
        public async Task<IHttpActionResult> GetAll()
        {
            var competencyCatalogs = await this.queryCompetency.GetAll();
            if (competencyCatalogs.Count() == 0)
            {
                return NotFound();
            }

            var competenciesVM = new List<CompetencyViewModel>();
            foreach (var competencyCatalog in competencyCatalogs)
            {
                foreach (var competency in competencyCatalog.Competencies)
                {
                    competenciesVM.Add(new CompetencyViewModel
                    {
                        Id = competency.Id,
                        ParentId = competency.ParentId,
                        Name = competency.Name,
                        Code = competency.Code
                    });
                }
            }

            return Ok(competenciesVM);
        }
    }
}