namespace TechnicalInterviewHelper.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// SkillMatrix data transfer object.
    /// </summary>
    public class SkillMatrix : DocumentDbEntity
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance has content.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has content; otherwise, <c>false</c>.
        /// </value>
        public bool HasContent { get; set; }

        /// <summary>
        /// Gets or sets the competency.
        /// </summary>
        /// <value>
        /// The competency.
        /// </value>
        public Competency Competency { get; set; }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>
        /// The level.
        /// </value>
        public Level Level { get; set; }

        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>
        /// The domain.
        /// </value>
        public Domain Domain { get; set; }

        /// <summary>
        /// Gets or sets the skills.
        /// </summary>
        /// <value>
        /// The skills.
        /// </value>
        public IEnumerable<Skill> Skills { get; set; }
    }
}