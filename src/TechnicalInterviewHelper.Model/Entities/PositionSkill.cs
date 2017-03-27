namespace TechnicalInterviewHelper.Model
{    
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// The way a bunch of filtered skill are saved for a queried position.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Model.BaseEntity" />
    public class PositionSkill : BaseEntity
    {
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        //// TODO: is this still needed?.
        public Position Position { get; set; }

        /// <summary>
        /// Gets or sets the position identifier.
        /// </summary>
        /// <value>
        /// The position identifier.
        /// </value>
        [JsonProperty("positionId")]
        public int PositionId { get; set; }

        /// <summary>
        /// Gets or sets the skill identifiers.
        /// </summary>
        /// <value>
        /// The skill identifiers.
        /// </value>
        [JsonProperty("skillIdentifiers")]
        public IList<int> SkillIdentifiers { get; set; }
    }
}