namespace TechnicalInterviewHelper.WebApi.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Input information with filtered skills chosen by the user.
    /// </summary>
    public class PositionSkillInputModel
    {
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public PositionInputModel Position { get; set; }

        /// <summary>
        /// Gets or sets the skill identifiers.
        /// </summary>
        /// <value>
        /// The skill identifiers.
        /// </value>
        public IList<int> SkillIdentifiers { get; set; }
    }
}