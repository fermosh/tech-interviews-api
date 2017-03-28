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

    [RoutePrefix("api/level")]
    public class QueryLevelController : ApiController
    {
        #region Repository

        private readonly ILevelQueryRepository queryLevelCatalogTwo;

        #endregion Repository

        #region Constructor

        public QueryLevelController()
        {
            this.queryLevelCatalogTwo = new LevelDocumentDbQueryRepository(ConfigurationManager.AppSettings["LevelCollectionId"]);
        }

        public QueryLevelController(ILevelQueryRepository queryLevelCatalogTwo)
        {
            this.queryLevelCatalogTwo = queryLevelCatalogTwo;
        }

        #endregion Constructor

        [HttpGet]
        [Route("all")]
        public async Task<IHttpActionResult> GetAll(int competencyId)
        {
            var levels = await this.queryLevelCatalogTwo.FindOnInternalCollection(level => level.CompetencyId == competencyId);

            if (levels.Count() == 0)
            {
                return NotFound();
            }

            var levelsVM = new List<LevelViewModel>();
            foreach (var level in levels)
            {
                levelsVM.Add(new LevelViewModel
                {
                    LevelId = level.LevelId,
                    CompetencyId = level.CompetencyId,
                    Name = level.Name,
                    Description = level.Description
                });
            }

            return Ok(levelsVM);
        }
    }
}