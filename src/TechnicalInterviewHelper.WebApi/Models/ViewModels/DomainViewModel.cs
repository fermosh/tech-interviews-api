using Newtonsoft.Json;

namespace TechnicalInterviewHelper.WebApi.Model
{
    /// <summary>
    /// A view model to represent a domain.
    /// </summary>
    public class DomainViewModel
    {
        /// <summary>
        /// Gets or sets the domain identifier.
        /// </summary>
        /// <value>
        /// The domain identifier.
        /// </value>
        [JsonProperty("id")]
        public int DomainId { get; set; }

        /// <summary>
        /// Gets or sets the competency identifier.
        /// </summary>
        /// <value>
        /// The competency identifier.
        /// </value>
        [JsonProperty("competencyId")]
        public int CompetencyId {get;set;}

        /// <summary>
        /// Gets or sets the level identifier.
        /// </summary>
        /// <value>
        /// The level identifier.
        /// </value>
        [JsonProperty("levelId")]
        public int LevelId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}