namespace TechnicalInterviewHelper.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// Competency entity.
    /// </summary>
    public class Competency
    {
        /// <summary>
        /// Gets or sets the compentency identifier.
        /// </summary>
        /// <value>
        /// The compentency identifier.
        /// </value>
        [JsonProperty("competencyId")]
        public int CompentencyId { get; set; }

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