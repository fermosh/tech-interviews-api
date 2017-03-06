namespace TechnicalInterviewHelper.WebApi.Controllers
{    
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Model;
    using Services;

    /// <summary>
    /// API for Position-Skill operations.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("query/positionskill")]
    public class QueryPositionSkillController : ApiController
    {
        #region Repository

        /// <summary>
        /// The query repository
        /// </summary>
        private readonly IQueryRepository<Skill, string> queryRepository;

        #endregion Repository

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryPositionSkillController"/> class.
        /// </summary>
        public QueryPositionSkillController()
        {
            this.queryRepository = new DocumentDbQueryRepository<Skill, string>(ConfigurationManager.AppSettings["SkillCollectionId"]);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryPositionSkillController"/> class.
        /// </summary>
        /// <param name="skillRepository">The skill repository.</param>
        public QueryPositionSkillController(IQueryRepository<Skill, string> queryRepository)
        {
            this.queryRepository = queryRepository;
        }

        #endregion Constructor

        /// <summary>
        /// Gets the specified position to find.
        /// </summary>
        /// <param name="positionToFind">The position to find.</param>
        /// <returns></returns>
        public async Task<PositionSkillViewModel> Get(PositionInputModel positionToFind)
        {
            var skillViewModelList = new List<SkillViewModel>();

            var skillsBelongingToPosition = await this.queryRepository.FindBy(
                    skill =>
                        skill.CompetencyId == positionToFind.CompetencyId &&
                        skill.LevelId == positionToFind.LevelId &&
                        skill.DomainId == positionToFind.DomainId);

            foreach (var skill in skillsBelongingToPosition)
            {
                skillViewModelList.Add(new SkillViewModel
                {
                    Name = skill.Name,
                    SkillId = skill.Id,
                    ParentSkillId = skill.ParentId,
                    HasChildren = false,
                    SkillLevel = skill.LevelSet
                });
            }

            var positionSkillVM = new PositionSkillViewModel()
            {
                Position = positionToFind,
                Skills = skillViewModelList
            };

            return positionSkillVM;
        }
    }
}