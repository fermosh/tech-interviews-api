namespace TechnicalInterviewHelper.WebApi.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// A view model of a tag.
    /// </summary>
    public class TagViewModel
    {
        /// <summary>
        /// Gets or sets the skill identifier.
        /// </summary>
        /// <value>
        /// The question identifier.
        /// </value>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the skill name.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}