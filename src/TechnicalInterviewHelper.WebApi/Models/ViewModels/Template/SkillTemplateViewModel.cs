namespace TechnicalInterviewHelper.WebApi.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using TechnicalInterviewHelper.Model;

    /// <summary>
    /// A view model with skill information.
    /// </summary>
    public class SkillTemplateViewModel
    {
        /// <summary>
        /// Gets or sets the root identifier.
        /// </summary>
        /// <value>
        /// The root identifier.
        /// </value>
        [JsonProperty("rootId")]
        public int? RootId { get; set; }

        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        /// <value>
        /// The display order.
        /// </value>
        [JsonProperty("displayOrder")]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the required skill level.
        /// </summary>
        /// <value>
        /// The required skill level.
        /// </value>
        [JsonProperty("requiredSkillLevel")]
        public int RequiredSkillLevel { get; set; }

        /// <summary>
        /// Gets or sets the user skill level.
        /// </summary>
        /// <value>
        /// The user skill level.
        /// </value>
        [JsonProperty("userSkillLevel")]
        public int UserSkillLevel { get; set; }

        /// <summary>
        /// Gets or sets the levels set.
        /// Note that this property has a typo since our data source has it too.
        /// </summary>
        /// <value>
        /// The levels set.
        /// </value>
        [JsonProperty("levelsSet")]
        public int LevelsSet { get; set; }

        /// <summary>
        /// Gets or sets the competency identifier.
        /// </summary>
        /// <value>
        /// The competency identifier.
        /// </value>
        [JsonProperty("competencyId")]
        public int CompetencyId { get; set; }

        /// <summary>
        /// Gets or sets the job function level.
        /// </summary>
        /// <value>
        /// The job function level.
        /// </value>
        [JsonProperty("jobFunctionLevel")]
        public int JobFunctionLevel { get; set; }

        /// <summary>
        /// Gets or sets the topics.
        /// </summary>
        /// <value>
        /// The topics.
        /// </value>
        [JsonProperty("topics")]
        public IList<TopicViewModel> Topics { get; set; }

        /// <summary>
        /// Gets or sets the questions.
        /// <para/>This property will be always empty.
        /// </summary>
        /// <value>
        /// The questions.
        /// </value>
        [JsonProperty("interviewQuestions")]
        public IEnumerable<object> Questions { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the parent identifier.
        /// </summary>
        /// <value>
        /// The parent identifier.
        /// </value>
        [JsonProperty("parentId")]
        public int? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is selectable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is selectable; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("isSelectable")]
        public bool IsSelectable { get; set; }
    }
}