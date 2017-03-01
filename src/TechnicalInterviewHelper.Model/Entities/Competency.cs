namespace TechnicalInterviewHelper.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// Competency entity.
    /// </summary>
    public class Competency : BaseEntity
    {
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