namespace TechnicalInterviewHelper.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;    

    /// <summary>
    /// Model for job function document.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Model.BaseEntity" />
    public class JobFunctionDocument : BaseEntity
    {
        /// <summary>
        /// Gets or sets the job function.
        /// </summary>
        /// <value>
        /// The job function.
        /// </value>
        [JsonProperty("jobFunction")]
        public JobFunction JobFunction { get; set; }

        /// <summary>
        /// Gets or sets the levels.
        /// </summary>
        /// <value>
        /// The levels.
        /// </value>
        [JsonProperty("levels")]
        public IEnumerable<Level> Levels { get; set; }
    }
}