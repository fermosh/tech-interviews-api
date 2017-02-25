namespace TechnicalInterviewHelper.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading.Tasks;
    using System.Web.Http;
    using TechnicalInterviewHelper.Model;
    using TechnicalInterviewHelper.Services;

    public class LevelController : ApiController
    {
        private readonly IQueryRepository<Level, string> Repository;

        #region Constructor

        public LevelController()
        {
            this.Repository = new DocumentDbQueryRepository<Level, string>(ConfigurationManager.AppSettings["LevelCollectionId"]);
        }

        public LevelController(IQueryRepository<Level, string> levelRepository)
        {
            this.Repository = levelRepository;
        }

        #endregion Constructor

        #region Get

        [HttpGet]
        [ActionName("All")]
        public async Task<IEnumerable<Level>> GetAllLevels()
        {
            return await this.Repository.GetAll();
        }

        #endregion Get
    }
}