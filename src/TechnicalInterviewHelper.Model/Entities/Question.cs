namespace TechnicalInterviewHelper.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// Question entity.
    /// </summary>
    public class Question : BaseEntity
    {
        /// <summary>
        /// Gets or sets the skill identifier, supplied by a third party, at which this question belongs.
        /// </summary>
        /// <value>
        /// The skill identifier.
        /// </value>
        [JsonProperty("skillId")]
        public int SkillId { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the captured answer.
        /// </summary>
        /// <value>
        /// The captured answer.
        /// </value>
        [JsonProperty("capturedAnswer")]
        public string CapturedAnswer { get; set; }

        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        /// <value>
        /// The rating.
        /// </value>
        [JsonProperty("capturedRating")]
        public int CapturedRating { get; set; }
    }
}