namespace TechnicalInterviewHelper.Model
{    
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Entity for interview.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Model.BaseEntity" />
    public class Interview : BaseEntity
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
        /// Gets or sets the skills.
        /// </summary>
        /// <value>
        /// The skills.
        /// </value>
        [JsonProperty("skills")]
        public IEnumerable<Skill> Skills { get; set; }

        /// <summary>
        /// Gets or sets the questions.
        /// </summary>
        /// <value>
        /// The questions.
        /// </value>
        [JsonProperty("questions")]
        public IEnumerable<Question> Questions { get; set; }

        /// <summary>
        /// Gets or sets the exercises.
        /// </summary>
        /// <value>
        /// The exercises.
        /// </value>
        [JsonProperty("exercises")]
        public IEnumerable<Exercise> Exercises { get; set; }
    }
}