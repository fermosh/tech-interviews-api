namespace TechnicalInterviewHelper.Model
{
    using System.Collections.Generic;
    using Attributes;
    using Newtonsoft.Json;    

    /// <summary>
    /// The way a bunch of filtered skill are saved for a queried position.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Model.BaseEntity" />
    [DocumentType(DocumentType.Templates)]
    public class Template : BaseEntity
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
        /// Gets or sets the skill identifiers.
        /// </summary>
        /// <value>
        /// The skill identifiers.
        /// </value>
        [JsonProperty("skills")]
        public IEnumerable<int> Skills { get; set; }
    }
}