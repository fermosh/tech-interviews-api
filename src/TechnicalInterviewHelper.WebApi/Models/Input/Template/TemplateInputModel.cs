namespace TechnicalInterviewHelper.WebApi.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Input information chosen by the user and composed by filtered skills belonging to a competency and level.
    /// </summary>
    public class TemplateInputModel
    {
        /// <summary>
        /// Gets or sets the competency identifier.
        /// </summary>
        /// <value>
        /// The competency identifier.
        /// </value>
        public int CompetencyId { get; set; }

        /// <summary>
        /// Gets or sets the job function level.
        /// </summary>
        /// <value>
        /// The job function level.
        /// </value>
        public int JobFunctionLevel { get; set; }

        /// <summary>
        /// Gets or sets the skill identifiers.
        /// </summary>
        /// <value>
        /// The skill identifiers.
        /// </value>
        public IEnumerable<int> Skills { get; set; }
    }
}