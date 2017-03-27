namespace TechnicalInterviewHelper.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;    

    /// <summary>
    /// Entity with a list of levels.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Model.BaseEntity" />
    public class LevelCatalog : BaseEntity
    {
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