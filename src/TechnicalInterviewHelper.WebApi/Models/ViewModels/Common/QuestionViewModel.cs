namespace TechnicalInterviewHelper.WebApi.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// A view model of a question.
    /// </summary>
    public class QuestionViewModel
    {
        /// <summary>
        /// Gets or sets the question identifier.
        /// </summary>
        /// <value>
        /// The question identifier.
        /// </value>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the question body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        [JsonProperty("body")]
        public string Body { get; set; }
    }
}