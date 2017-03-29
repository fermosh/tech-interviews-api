namespace TechnicalInterviewHelper.WebApi.Model
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// Encapsulates the skills belonging to a position.
    /// </summary>
    /// <remarks>
    /// This view model is like the SkillMatrix model in the UI.
    /// </remarks>
    public class SkillMatrixViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance has content.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has content; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("hasContent")]
        public bool HasContent { get; set; }

        /// <summary>
        /// Gets or sets the competency identifier.
        /// </summary>
        /// <value>
        /// The competency identifier.
        /// </value>
        [JsonProperty("competencyId")]
        public int CompetencyId { get; set; }

        /// <summary>
        /// Gets or sets the skills of a particular position.
        /// </summary>
        /// <value>
        /// The skills of a particular position.
        /// </value>
        [JsonProperty("skills")]
        public IEnumerable<SkillForPositionViewModel> Skills { get; set; }
    }
}