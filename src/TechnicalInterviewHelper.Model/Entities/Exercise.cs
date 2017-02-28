namespace TechnicalInterviewHelper.Model
{
    /// <summary>
    /// Exercise entity.
    /// </summary>
    public class Exercise : DocumentDbEntity
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the complexity.
        /// </summary>
        /// <value>
        /// The complexity.
        /// </value>
        public string Complexity { get; set; }

        /// <summary>
        /// Gets or sets the solution.
        /// </summary>
        /// <value>
        /// The solution.
        /// </value>
        public string Solution { get; set; }
    }
}