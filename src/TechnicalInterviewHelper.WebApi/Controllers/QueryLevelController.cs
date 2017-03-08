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

    [RoutePrefix("query/level")]
    public class QueryLevelController : ApiController
    {
        #region Repository

        private readonly IQueryRepository<Level, string> queryLevel;

        #endregion Repository

        #region Constructor

        public QueryLevelController()
        {
            this.queryLevel = new DocumentDbQueryRepository<Level, string>(ConfigurationManager.AppSettings["LevelCollectionId"]);
        }

        public QueryLevelController(IQueryRepository<Level, string> queryLevel)
        {
            this.queryLevel = queryLevel;
        }

        #endregion Constructor

        [HttpGet]
        [ActionName("all")]
        public async Task<IHttpActionResult> GetAllLevelsOfCompetency(int competencyId)
        {
            var levels = await this.queryLevel.FindBy(level => level.CompetencyId == competencyId);
            if (levels.Count() == 0)
            {
                return NotFound();
            }

            // Temporal solution to the ID property value.
            var counter = 0;

            var levelsVM = new List<LevelViewModel>();
            foreach (var level in levels)
            {
                counter++;
                levelsVM.Add(new LevelViewModel
                {
                    LevelId = counter,
                    Name = level.Name
                });
            }

            return Ok(levelsVM);
        }
    }
}