namespace TechnicalInterviewHelper.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    
    /// <summary>
    /// Competency entity.
    /// </summary>
    public class Competency
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the parent identifier.
        /// </summary>
        /// <value>
        /// The parent identifier.
        /// </value>
        [JsonProperty("parentId")]
        public int? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the job functions.
        /// </summary>
        /// <value>
        /// The job functions.
        /// </value>
        [JsonProperty("jobFunctions")]
        public IEnumerable<int> JobFunctions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is selectable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is selectable; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("isSelectable")]
        public bool IsSelectable { get; set; }
    }
}