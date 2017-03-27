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
    public class PositionSkillViewModel
    {
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