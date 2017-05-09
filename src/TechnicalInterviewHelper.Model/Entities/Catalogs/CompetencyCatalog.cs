namespace TechnicalInterviewHelper.Model
{
    using System.Collections.Generic;
    using Attributes;
    using Newtonsoft.Json;    

    /// <summary>
    /// Entity that models a competency with all its information.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Model.BaseEntity" />
    [DocumentType(DocumentType.Competencies)]
    public class CompetencyDocument : BaseEntity
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