namespace TechnicalInterviewHelper.Model
{
    using System.Collections.Generic;

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
        public int SkillId { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Position Position { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the topics.
        /// </summary>
        /// <value>
        /// The topics.
        /// </value>
        public List<Topic> Topics { get; set; }
    }
}