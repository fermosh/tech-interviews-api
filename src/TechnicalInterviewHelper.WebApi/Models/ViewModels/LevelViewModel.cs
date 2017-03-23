using Newtonsoft.Json;

namespace TechnicalInterviewHelper.WebApi.Model
{
    /// <summary>
    /// A view model which represents a level.
    /// </summary>
    public class LevelViewModel
    {
        [JsonProperty("id")]
        /// <summary>
        /// Gets or sets the level identifier.
        /// </summary>
        /// <value>
        /// The level identifier.
        /// </value>
        public int LevelId { get; set; }

        [JsonProperty("competencyId")]
        /// <summary>
        /// Gets or sets the competency identifier.
        /// </summary>
        /// <value>
        /// The competency identifier.
        /// </value>
        public int CompetencyId { get; set; }

        [JsonProperty("name")]
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}