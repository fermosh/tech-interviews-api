namespace TechnicalInterviewHelper.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;    

    /// <summary>
    /// Model for job function catalog.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Model.BaseEntity" />
    public class JobFunctionCatalog : BaseEntity
    {
        [JsonProperty("jobFunction")]
        public JobFunction JobFunction { get; set; }

        [JsonProperty("levels")]
        public IEnumerable<Level> Levels { get; set; }
    }
}