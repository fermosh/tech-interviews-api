namespace TechnicalInterviewHelper.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// Entity model for an answered question.
    /// </summary>
    public class AnsweredQuestion
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
        /// Gets or sets the skill identifier.
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
        /// Gets or sets the answer.
        /// </summary>
        /// <value>
        /// The answer.
        /// </value>
        [JsonProperty("answer")]
        public string Answer { get; set; }

        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        /// <value>
        /// The rating.
        /// </value>
        [JsonProperty("rating")]
        public float Rating { get; set; }
    }
}