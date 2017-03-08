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

    [RoutePrefix("query/competency")]
    public class QueryCompetencyController : ApiController
    {
        #region Repository

        private readonly IQueryRepository<Competency, string> queryCompetency;

        #endregion Repository

        #region Constructor

        public QueryCompetencyController()
        {
            var collectionId = ConfigurationManager.AppSettings["CompetencyCollectionId"];
            this.queryCompetency = new DocumentDbQueryRepository<Competency, string>(collectionId);
        }

        public QueryCompetencyController(IQueryRepository<Competency, string> competencyRepository)
        {
            this.queryCompetency = competencyRepository;
        }

        #endregion Constructor

        [HttpGet]
        [ActionName("all")]
        public async Task<IHttpActionResult> GetAll()
        {
            var competencies = await this.queryCompetency.GetAll();
            if (competencies.Count() == 0)
            {
                return NotFound();
            }

            // Temporal solution to the ID property value.
            var counter = 0;

            var competenciesVM = new List<CompetencyViewModel>();
            foreach (var competency in competencies)
            {
                counter++;
                competenciesVM.Add(new CompetencyViewModel
                {
                    CompetencyId = counter,
                    Name = competency.Name
                });
            }

            return Ok(competenciesVM);
        }
    }
}