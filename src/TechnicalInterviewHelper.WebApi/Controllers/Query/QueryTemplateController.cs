namespace TechnicalInterviewHelper.WebApi.Controllers
{    
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using Model;
    using TechnicalInterviewHelper.Model;

    [RoutePrefix("api/templates")]
    [EnableCors(origins: "*", headers: "*", methods: "GET")]
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
        private readonly IQueryRepository<Template, string> queryTemplateCatalog;

        /// <summary>
        /// The query job function
        /// </summary>
        private readonly IJobFunctionQueryRepository queryJobFunction;

        /// <summary>
        /// The query competency
        /// </summary>
        private readonly ICompetencyQueryRepository queryCompetency;

        #endregion Repository

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryTemplateController"/> class.
        /// </summary>
        /// <param name="querySkillMatrixCatalog">The query skill matrix catalog.</param>
        /// <param name="queryTemplateCatalog">The query template catalog.</param>
        public QueryTemplateController(
            ISkillMatrixQueryRepository querySkillMatrixCatalog,
            IQueryRepository<Template, string> queryTemplateCatalog,
            IJobFunctionQueryRepository queryJobFunction,
            ICompetencyQueryRepository queryCompetency)
        {
            this.queryTemplateCatalog = queryTemplateCatalog;
            this.querySkillMatrixCatalog = querySkillMatrixCatalog;
            this.queryJobFunction = queryJobFunction;
            this.queryCompetency = queryCompetency;
        }

        #endregion Constructor

        /// <summary>
        /// Gets the all templates.
        /// </summary>
        /// <returns>
        /// 200 HTTP status code with all templates view models; otherwise, any other HTTP status code.
        /// </returns>
        /// <example>api/template/all</example>
        [Route("all")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var templates = await queryTemplateCatalog.GetAll();

            if (templates == null)
            {
                return NotFound();
            }

            var templateViewModelsList = new List<TemplateViewModel>();

            foreach (var template in templates)
            {
                var competencyAndJobInfo = await GetCompetencyAndJobInfoAsync(template);

                templateViewModelsList.Add(new TemplateViewModel
                {
                    Id = template.Id,
                    CompetencyId = template.CompetencyId,
                    JobFunctionLevel = template.JobFunctionLevel,
                    Level = new LevelViewModel
                    {
                        Id = template.JobFunctionLevel,
                        Name = $"L{template.JobFunctionLevel}",
                        Description = competencyAndJobInfo.JobDescription
                    },
                    CompetencyName = competencyAndJobInfo.CompetencyName,
                    DomainName = competencyAndJobInfo.DomainName,
                    Skills = Enumerable.Repeat(new SkillTemplateViewModel(), template.Skills.Count()), // Fake Skills, we just need to know how many skills the template has
                    Exercises = new List<object>()
                });
            }

            return Ok(templateViewModelsList);
        }

        /// <summary>
        /// Gets the specified template identifier.
        /// </summary>
        /// <param name="templateId">The template identifier.</param>
        /// <returns>
        /// 200 HTTP status code with a template view model; otherwise, any other HTTP status code.
        /// </returns>
        /// <example>api/template/04278e7f-2d35-49b8-a8f9-ebd0d794c434</example>
        [Route("{templateId}")]
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

            if (template.Skills == null || template.Skills.Count() == 0)
            {
                return BadRequest($"The template with Id '{templateId}' doesn't have saved skills.");
            }

            var competencyAndJobInfo = await GetCompetencyAndJobInfoAsync(template);
            var skillsList = await this.querySkillMatrixCatalog.FindWithinSkills(template.CompetencyId, template.JobFunctionLevel, template.Skills.ToArray());
            var skillTemplateViewModelList = GetSkillsTemplateViewModels(skillsList);

            var levelViewModel = new LevelViewModel
            {
                Id = template.JobFunctionLevel,
                Name = $"L{template.JobFunctionLevel}",
                Description = competencyAndJobInfo.JobDescription
            };

            var templateViewModel = new TemplateViewModel
            {
                CompetencyId = template.CompetencyId,
                JobFunctionLevel = template.JobFunctionLevel,
                Level = levelViewModel,
                CompetencyName = competencyAndJobInfo.CompetencyName,
                DomainName = competencyAndJobInfo.DomainName,
                Skills = skillTemplateViewModelList,
                Exercises = new List<object>()
            };

            return Ok(templateViewModel);
        }


        /// <summary>
        /// Builds and returns a list of SkillTemplateViewModel based on the Skills of a given Template
        /// </summary>
        /// <param name="skills">The template's skills.</param>
        /// <returns>
        /// Enumerable of SkillTemplateViewModel
        /// </returns>
        private IEnumerable<SkillTemplateViewModel> GetSkillsTemplateViewModels(IEnumerable<Skill> skills)
        {
            var templateSkillsViewModelList = new List<SkillTemplateViewModel>();

            foreach (var skill in skills)
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
                    IsSelectable = skill.IsSelectable,
                    Questions = new List<object>()
                };

                // Add the view model to the result list.
                templateSkillsViewModelList.Add(skillTemplateViewModel);
            }

            return templateSkillsViewModelList;
        }

        /// <summary>
        /// Gets the Competency and Job Information for a given template
        /// </summary>
        /// <returns>
        /// Competency and Job Information
        /// </returns>
        private async Task<CompetencyAndJobInfo> GetCompetencyAndJobInfoAsync(Template template)
        {
            var competencyAndJobInfo = new CompetencyAndJobInfo();

            // Try to find the competency that has the job function identifier.
            var competency = await queryCompetency.FindCompetency(template.CompetencyId);

            if (competency != null)
            {
                competencyAndJobInfo.DomainName = competency.Name;
            }

            while (competency != null && !competency.JobFunctions.Any() && competency.ParentId.HasValue)
            {
                competency = await queryCompetency.FindCompetency(competency.ParentId.Value);
            }

            if (competency != null)
            {
                competencyAndJobInfo.CompetencyName = competency.Name;

                if (competency.JobFunctions.Any())
                {
                    var jobFunctionId = competency.JobFunctions.First();

                    competencyAndJobInfo.JobDescription = await queryJobFunction.FindJobTitleByLevel(jobFunctionId, template.JobFunctionLevel);
                }
            }

            return competencyAndJobInfo;
        }
    }
}