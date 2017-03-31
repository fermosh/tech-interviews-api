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
        [JsonProperty("questionId")]
        public string QuestionId { get; set; }

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