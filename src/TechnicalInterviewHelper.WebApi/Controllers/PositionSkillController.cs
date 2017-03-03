namespace TechnicalInterviewHelper.WebApi.Controllers
{
    using Model;
    using Services;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class PositionSkillController : ApiController
    {
        private readonly IQueryRepository<Skill, string> skillRepository;

        public PositionSkillController()
        {
            this.skillRepository = new DocumentDbQueryRepository<Skill, string>(ConfigurationManager.AppSettings["SkillCollectionId"]);
        }

        public async Task<PositionSkillViewModel> Get(PositionInputModel positionToFind)
        {
            var skillViewModelList = new List<SkillViewModel>();

            var skillsBelongingToPosition = await this.skillRepository.FindBy(
                    skill =>
                        skill.CompetencyId == positionToFind.CompetencyId &&
                        skill.LevelId == positionToFind.LevelId &&
                        skill.DomainId == positionToFind.DomainId);

            foreach(var skill in skillsBelongingToPosition)
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