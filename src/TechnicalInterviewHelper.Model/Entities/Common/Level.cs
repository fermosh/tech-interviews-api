namespace TechnicalInterviewHelper.Model
{    
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Level entity.
    /// </summary>
    public class Level
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty("level")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is eligible for asmt.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is eligible for asmt; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("isEligibleForAsmt")]
        public bool IsEligibleForAsmt { get; set; }

        /// <summary>
        /// Gets or sets the job titles.
        /// </summary>
        /// <value>
        /// The job titles.
        /// </value>
        [JsonProperty("jobTitles")]
        public IEnumerable<string> JobTitles { get; set; }
    }
}