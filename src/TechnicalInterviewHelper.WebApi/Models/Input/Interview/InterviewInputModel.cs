namespace TechnicalInterviewHelper.WebApi.Model
{
    using System.Collections.Generic;
    using TechnicalInterviewHelper.Model;

    /// <summary>
    /// View model for an Interview.
    /// </summary>
    public class InterviewInputModel
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
        /// Gets or sets the template identifier.
        /// </summary>
        /// <value>
        /// The template identifier.
        /// </value>
        public string TemplateId { get; set; }

        /// <summary>
        /// Gets or sets the skills.
        /// </summary>
        /// <value>
        /// The skills.
        /// </value>
        public IEnumerable<SkillInterviewInputModel> Skills { get; set; }

        /// <summary>
        /// Gets or sets the exercises.
        /// </summary>
        /// <value>
        /// The exercises.
        /// </value>
        public IEnumerable<AnsweredExerciseInputModel> Exercises { get; set; }
    }
}