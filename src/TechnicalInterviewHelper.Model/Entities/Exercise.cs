namespace TechnicalInterviewHelper.Model
{
    /// <summary>
    /// Exercise entity.
    /// </summary>
    public class Exercise : BaseEntity
    {
        /// <summary>
        /// Gets or sets the skill identifier, supplied by a third party, at which this question belongs.
        /// </summary>
        /// <value>
        /// The skill identifier.
        /// </value>
        public int SkillId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the complexity.
        /// </summary>
        /// <value>
        /// The complexity.
        /// </value>
        public string Complexity { get; set; }

        /// <summary>
        /// Gets or sets the proposed solution.
        /// </summary>
        /// <value>
        /// The proposed solution.
        /// </value>
        public string ProposedSolution { get; set; }

        /// <summary>
        /// Gets or sets the solution.
        /// </summary>
        /// <value>
        /// The solution.
        /// </value>
        public string CapturedSolution { get; set; }

        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        /// <value>
        /// The rating.
        /// </value>
        public float CapturedRating { get; set; }
    }
}