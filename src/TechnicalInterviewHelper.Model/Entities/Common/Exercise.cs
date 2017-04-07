namespace TechnicalInterviewHelper.Model
{
    using System.Collections.Generic;
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
        [JsonProperty("skills")]
        public IEnumerable<int> Skills { get; set; }

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
        [JsonProperty("body")]
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
        [JsonProperty("solution")]
        public string Solution { get; set; }
    }
}