namespace TechnicalInterviewHelper.WebApi.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// A view model of an exercise.
    /// </summary>
    public class ExerciseViewModel
    {
        /// <summary>
        /// Gets or sets the exercise identifier.
        /// </summary>
        /// <value>
        /// The exercise identifier.
        /// </value>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the proposed solution.
        /// </summary>
        /// <value>
        /// The proposed solution.
        /// </value>
        [JsonProperty("solution")]
        public string Solution { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [JsonProperty("Body")]
        public string Body { get; set; }
    }
}