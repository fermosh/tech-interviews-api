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

    [RoutePrefix("api/competency")]
    public class QueryCompetencyController : ApiController
    {
        #region Repository

        private readonly IQueryRepository<CompetencyCatalog, string> queryCompetency;

        #endregion Repository

        #region Constructor

        public QueryCompetencyController()
        {
            var collectionId = ConfigurationManager.AppSettings["CompetencyCollectionId"];
            this.queryCompetency = new DocumentDbQueryRepository<CompetencyCatalog, string>(collectionId);
        }

        public QueryCompetencyController(IQueryRepository<CompetencyCatalog, string> competencyRepository)
        {
            this.queryCompetency = competencyRepository;
        }

        #endregion Constructor

        [HttpGet]
        [Route("all")]
        public async Task<IHttpActionResult> GetAll()
        {
            var competencyCatalogs = await this.queryCompetency.GetAll();
            if (competencyCatalogs.Count() == 0)
            {
                return NotFound();
            }

            // TODO: we have to merge all the docs for competency catalog in order to have only one of it.
            var competencies = competencyCatalogs.First().Competencies;

            if (competencies.Count() == 0)
            {
                return NotFound();
            }

            var competenciesVM = new List<CompetencyViewModel>();
            foreach (var competency in competencies)
            {
                competenciesVM.Add(new CompetencyViewModel
                {
                    CompetencyId = competency.CompentencyId,
                    Name = competency.Name
                });
            }

            return Ok(competenciesVM);
        }
    }
}