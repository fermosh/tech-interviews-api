namespace TechnicalInterviewHelper.WebApi.Controllers
{
    using LinqKit;
    using Model;
    using Services;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using TechnicalInterviewHelper.Model;
    
    [RoutePrefix("api/template")]
    [EnableCors(origins: "*", headers: "*", methods: "get")]
    public class QueryTemplateController : ApiController
    {
        #region Repository

        /// <summary>
        /// The query skill matrix catalog
        /// </summary>
        private readonly ISkillMatrixQueryRepository querySkillMatrixCatalog;

        /// <summary>
        /// The query template catalog
        /// </summary>
        private readonly IQueryRepository<TemplateCatalog, string> queryTemplateCatalog;

        #endregion Repository

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryTemplateController"/> class.
        /// </summary>
        public QueryTemplateController()
        {
            this.querySkillMatrixCatalog = new SkillMatrixDocumentDbQueryRepository(ConfigurationManager.AppSettings["SkillCollectionId"]);
            this.queryTemplateCatalog = new DocumentDbQueryRepository<TemplateCatalog, string>(ConfigurationManager.AppSettings["TemplateCollectionId"]);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryTemplateController"/> class.
        /// </summary>
        /// <param name="querySkillMatrixCatalog">The query skill matrix catalog.</param>
        /// <param name="queryTemplateCatalog">The query template catalog.</param>
        public QueryTemplateController(
            SkillMatrixDocumentDbQueryRepository querySkillMatrixCatalog,
            IQueryRepository<TemplateCatalog, string> queryTemplateCatalog)
        {
            this.queryTemplateCatalog = queryTemplateCatalog;
            this.querySkillMatrixCatalog = querySkillMatrixCatalog;
        }

        #endregion Constructor

        /// <summary>
        /// Gets the specified template identifier.
        /// </summary>
        /// <param name="templateId">The template identifier.</param>
        /// <returns>200 HTTP status code with a template view model; otherwise, any other HTTP status code.</returns>
        [Route("{templateId:string}")]
        public async Task<IHttpActionResult> Get(string templateId)
        {
            // --------------------------------------------------------------------------------
            // Let's run some validations over the input data and the saved template as well.
            // --------------------------------------------------------------------------------

            if (string.IsNullOrEmpty(templateId?.Trim()))
            {
                return BadRequest("Cannot get a template without a valid identifier.");
            }

            var template = await this.queryTemplateCatalog.FindById(templateId);
            if (template == null)
            {
                return NotFound();
            }

            if (template.Skills == null
                ||
                template.Skills.Count() == 0)
            {
                return BadRequest($"The template with Id '{templateId}' doesn't have saved skills.");
            }

            // -------------------------------------------------------------------------------
            // Try to get all filteres skill information using its id, competency and level.
            // -------------------------------------------------------------------------------

            var filterToGetSkills = PredicateBuilder.New<Skill>(false);

            foreach (var skillId in template.Skills)
            {
                filterToGetSkills = filterToGetSkills.Or(skill => skill.Id == skillId &&
                                                                  skill.CompetencyId == template.CompetencyId &&
                                                                  skill.JobFunctionLevel == template.JobFunctionLevel);
            }

            var skillsList = await this.querySkillMatrixCatalog.FindWithin(template.CompetencyId, filterToGetSkills);

            // --------------------------------------
            // Now it's time to build the response.
            // --------------------------------------

            var skillTemplateViewModelList = new List<SkillTemplateViewModel>();

            foreach (var skill in skillsList)
            {
                // Map all the topics that the skill could have.
                var topics = new List<TopicViewModel>();
                foreach (var topic in skill.Topics)
                {
                    topics.Add(new TopicViewModel
                    {
                        Name = topic.Name,
                        IsRequired = topic.IsRequired
                    });
                }

                // Create the view model of the skill.
                var skillTemplateViewModel = new SkillTemplateViewModel
                {
                    RootId = skill.RootId,
                    DisplayOrder = skill.DisplayOrder,
                    RequiredSkillLevel = skill.RequiredSkillLevel,
                    UserSkillLevel = skill.UserSkillLevel,
                    LevelsSet = skill.LevelsSet,
                    CompetencyId = skill.CompetencyId,
                    JobFunctionLevel = skill.JobFunctionLevel,
                    Topics = topics,
                    Id = skill.Id,
                    ParentId = skill.ParentId,
                    Name = skill.Name,
                    IsSelectable = skill.IsSelectable
                };

                // Add the view model to the result list.
                skillTemplateViewModelList.Add(skillTemplateViewModel);
            }

            var templateViewModel = new TemplateViewModel
            {
                CompetencyId = template.CompetencyId,
                JobFunctionLevel = template.JobFunctionLevel,
                Skills = skillTemplateViewModelList
            };

            return Ok(templateViewModel);
        }
    }
}