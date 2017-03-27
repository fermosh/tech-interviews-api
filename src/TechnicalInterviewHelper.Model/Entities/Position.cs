namespace TechnicalInterviewHelper.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// To get information about a position.
    /// </summary>
    public class Position : BaseEntity
    {
        /// <summary>
        /// Gets or sets the position identifier.
        /// </summary>
        /// <value>
        /// The position identifier.
        /// </value>
        [JsonProperty("positionId")]
        public int PositionId { get; set; }

        /// <summary>
        /// Gets or sets the competency identifier.
        /// </summary>
        /// <value>
        /// The competency identifier.
        /// </value>
        [JsonProperty("competencyId")]
        public int CompetencyId { get; set; }

        /// <summary>
        /// Gets or sets the level identifier.
        /// </summary>
        /// <value>
        /// The level identifier.
        /// </value>
        [JsonProperty("levelId")]
        public int LevelId { get; set; }

        /// <summary>
        /// Gets or sets the domain identifier.
        /// </summary>
        /// <value>
        /// The domain identifier.
        /// </value>
        [JsonProperty("domainId")]
        public int DomainId { get; set; }
    }
}