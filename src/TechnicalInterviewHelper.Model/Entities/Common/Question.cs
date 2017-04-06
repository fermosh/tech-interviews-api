namespace TechnicalInterviewHelper.Model
{
    using Newtonsoft.Json;
    using TechnicalInterviewHelper.Model.Entities.Common;

    /// <summary>
    /// Question entity.
    /// </summary>
    public class Question : BaseEntity
    {
        /// <summary>
        /// Gets or sets the competency identifier.
        /// </summary>
        /// <value>
        /// The competency identifier.
        /// </value>
        [JsonProperty("competency")]
        [JsonConverter(typeof(ConcreteListTypeConverter<ITag, Skill>))]
        public ITag Competency { get; set; }

        /// <summary>
        /// Gets or sets the skill identifier.
        /// </summary>
        /// <value>
        /// The skill identifier.
        /// </value>
        [JsonProperty("tag")]
        public Skill Tag { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        [JsonProperty("body")]
        public string Body { get; set; }
    }
}