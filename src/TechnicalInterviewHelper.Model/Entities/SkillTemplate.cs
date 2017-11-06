namespace TechnicalInterviewHelper.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Model for a skill in an Template.
    /// </summary>
    public class SkillTemplate
    {
        /// <summary>
        /// Gets or sets the skill identifier.
        /// </summary>
        /// <value>
        /// The skill identifier.
        /// </value>
        [JsonProperty("id")]
        public int SkillId { get; set; }

        /// <summary>
        /// Gets or sets the question identifiers.
        /// </summary>
        /// <value>
        /// The questions.
        /// </value>
        [JsonProperty("questions")]
        public IEnumerable<string> Questions { get; set; }
    }
}
