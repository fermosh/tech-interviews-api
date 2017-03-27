namespace TechnicalInterviewHelper.Model
{    
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Skill entity.
    /// </summary>
    public class Skill : BaseEntity
    {
        /// <summary>
        /// Gets or sets the skill identifier supplied by a third party.
        /// </summary>
        /// <value>
        /// The skill identifier.
        /// </value>
        [JsonProperty("skillId")]
        public int SkillId { get; set; }

        /// <summary>
        /// Gets or sets the parent skill identifier.
        /// </summary>
        /// <value>
        /// The parent skill identifier.
        /// </value>
        [JsonProperty("parentSkillId")]
        public int ParentSkillId { get; set; }

        /// <summary>
        /// Gets or sets the position identifier.
        /// </summary>
        /// <value>
        /// The position identifier.
        /// </value>
        [JsonProperty("positionId")]
        public int PositionId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the topics.
        /// </summary>
        /// <value>
        /// The topics.
        /// </value>
        [JsonProperty("topics")]
        public IList<Topic> Topics { get; set; }
    }
}