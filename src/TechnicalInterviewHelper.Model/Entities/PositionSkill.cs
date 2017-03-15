namespace TechnicalInterviewHelper.Model
{
    using System.Collections.Generic;

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
        public Position Position { get; set; }

        /// <summary>
        /// Gets or sets the skill identifiers.
        /// </summary>
        /// <value>
        /// The skill identifiers.
        /// </value>
        public IList<int> SkillIdentifiers { get; set; }
    }
}