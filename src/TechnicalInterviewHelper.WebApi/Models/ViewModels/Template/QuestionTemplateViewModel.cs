namespace TechnicalInterviewHelper.WebApi.Model
{
    using Newtonsoft.Json;

    public class QuestionTemplateViewModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        [JsonProperty("body")]
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the answer.
        /// </summary>
        /// <value>
        /// The answer.
        /// </value>
        [JsonProperty("answer")]
        public string Answer { get; set; }
    }
}