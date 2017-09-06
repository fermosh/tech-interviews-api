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

        /// <summary>
        /// Gets or sets the proposed solution.
        /// </summary>
        /// <value>
        /// The proposed solution.
        /// </value>
        [JsonProperty("answer")]
        public string Answer { get; set; }

        /// <summary>
        /// Gets or sets the competency
        /// </summary>
        /// <value>
        /// The competency.
        /// </value>
        [JsonProperty("competency")]
        public CompetencyViewModel Competency { get; set; }

        /// <summary>
        /// Gets or sets the question body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        [JsonProperty("skill")]
        public TagViewModel Tag { get; set; }

    }
}