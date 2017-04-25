namespace TechnicalInterviewHelper.WebApi.Model
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using TechnicalInterviewHelper.Model;

    /// <summary>
    /// Encapsulates the skills belonging to a position.
    /// </summary>
    /// <remarks>
    /// This view model is like the SkillMatrix model in the UI.
    /// </remarks>
    public class SkillMatrixViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance has content.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has content; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("hasContent")]
        public bool HasContent { get; set; }

        /// <summary>
        /// Gets or sets the competency identifier.
        /// </summary>
        /// <value>
        /// The competency identifier.
        /// </value>
        [JsonProperty("competencyId")]
        public int CompetencyId { get; set; }

        /// <summary>
        /// Gets or sets the skills of a particular position.
        /// </summary>
        /// <value>
        /// The skills of a particular position.
        /// </value>
        [JsonProperty("skills")]
        public IEnumerable<SkillForPositionViewModel> Skills { get; set; }

        public static SkillMatrixViewModel Create(int competencyId, IEnumerable<Skill> skills)
        {
            // We have found documents that match the input criteria, so we proceed to include them in the response.
            var skillsVM = new List<SkillForPositionViewModel>();

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
                var skillVM = new SkillForPositionViewModel
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

                skillsVM.Add(skillVM);
            }

            var positionSkillVM = new SkillMatrixViewModel()
            {
                HasContent = skillsVM.Count > 0,
                CompetencyId = competencyId,
                Skills = skillsVM
            };

            return positionSkillVM;
        }
    }
}