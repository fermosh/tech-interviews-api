namespace TechnicalInterviewHelper.WebApi.Model
{
    /// <summary>
    /// A complete view model with information of the filtered skills its questions and exercies.
    /// </summary>
    public class SkillInterviewViewModel
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
        public string Name { get; set; }
    }
}