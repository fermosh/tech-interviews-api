namespace TechnicalInterviewHelper.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Encapsulates the skills belonging to a position.
    /// </summary>
    public class PositionSkillViewModel
    {
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public PositionInputModel Position { get; set; }

        /// <summary>
        /// Gets or sets the skills.
        /// </summary>
        /// <value>
        /// The skills.
        /// </value>
        public IEnumerable<SkillViewModel> Skills { get; set; }
    }
}