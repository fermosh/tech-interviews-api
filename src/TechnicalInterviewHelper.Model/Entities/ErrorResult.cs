namespace TechnicalInterviewHelper.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// Topic entity.
    /// </summary>
    public class ErrorResult
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty("entity")]
        public string Entity { get; set; }

        /// <summary>
        /// Gets or sets the error description resulted from an operation
        /// </summary>
        /// <value>
        /// The error description
        /// </value>
        [JsonProperty("errorDescription")]
        public string ErrorDescription { get; set; }
    }
}
