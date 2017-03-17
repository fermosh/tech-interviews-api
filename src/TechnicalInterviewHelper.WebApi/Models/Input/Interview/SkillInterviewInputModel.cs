namespace TechnicalInterviewHelper.WebApi.Model
{
    /// <summary>
    /// Input model for a filtered skill in an Interview.
    /// </summary>
    public class SkillInterviewInputModel
    {
        /// <summary>
        /// Gets or sets the skill identifier.
        /// </summary>
        /// <value>
        /// The skill identifier.
        /// </value>
        public int SkillId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Description { get; set; }
    }
}