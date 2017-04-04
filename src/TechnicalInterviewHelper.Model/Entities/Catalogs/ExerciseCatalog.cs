namespace TechnicalInterviewHelper.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// Exercise entity.
    /// </summary>
    public class ExerciseCatalog : BaseEntity
    {
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
        /// Gets or sets the skill identifier, supplied by a third party, at which this question belongs.
        /// </summary>
        /// <value>
        /// The skill identifier.
        /// </value>
        [JsonProperty("skillId")]
        public int SkillId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the complexity.
        /// </summary>
        /// <value>
        /// The complexity.
        /// </value>
        [JsonProperty("complexity")]
        public string Complexity { get; set; }

        /// <summary>
        /// Gets or sets the proposed solution.
        /// </summary>
        /// <value>
        /// The proposed solution.
        /// </value>
        [JsonProperty("proposedSolution")]
        public string ProposedSolution { get; set; }
    }
}