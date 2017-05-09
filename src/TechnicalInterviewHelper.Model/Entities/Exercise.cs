namespace TechnicalInterviewHelper.Model
{
    using System.Collections.Generic;
    using Attributes;
    using Entities;
    using Newtonsoft.Json;    

    /// <summary>
    /// Exercise entity.
    /// </summary>
    [DocumentType(DocumentType.Exercises)]
    public class Exercise : BaseEntity
    {
        /// <summary>
        /// Gets or sets the competency identifier.
        /// </summary>
        /// <value>
        /// The competency identifier.
        /// </value>
        [JsonProperty("competency")]
        public Tag Competency { get; set; }

        /// <summary>
        /// Gets or sets the skill identifier, supplied by a third party, at which this question belongs.
        /// </summary>
        /// <value>
        /// The skill identifier.
        /// </value>
        [JsonProperty("skills")]
        public IEnumerable<Tag> Skills { get; set; }

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
        /// Gets or sets the proposed solution.
        /// </summary>
        /// <value>
        /// The proposed solution.
        /// </value>
        [JsonProperty("solution")]
        public string Solution { get; set; }
    }
}