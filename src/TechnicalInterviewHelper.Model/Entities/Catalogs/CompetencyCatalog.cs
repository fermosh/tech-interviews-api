namespace TechnicalInterviewHelper.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Entity to save a document with a catalog of competencies.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Model.BaseEntity" />
    public class CompetencyCatalog : BaseEntity
    {
        /// <summary>
        /// Gets or sets the competencies.
        /// </summary>
        /// <value>
        /// The competencies.
        /// </value>
        [JsonProperty("competencies")]
        public IEnumerable<Competency> Competencies { get; set; }
    }
}