namespace TechnicalInterviewHelper.WebApi.Model
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
        public PositionViewModel Position { get; set; }

        /// <summary>
        /// Gets or sets the skills of a particular position.
        /// </summary>
        /// <value>
        /// The skills of a particular position.
        /// </value>
        public IEnumerable<SkillForPositionViewModel> Skills { get; set; }
    }
}