namespace TechnicalInterviewHelper.WebApi.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// A view model with information regarding an interview.
    /// </summary>
    public class InterviewViewModel
    {
        /// <summary>
        /// Gets or sets the competency identifier.
        /// </summary>
        /// <value>
        /// The competency identifier.
        /// </value>
        public int CompetencyId { get; set; }

        /// <summary>
        /// Gets or sets the skills.
        /// </summary>
        /// <value>
        /// The skills.
        /// </value>
        public IEnumerable<SkillInterviewViewModel> Skills { get; set; }

        /// <summary>
        /// Gets or sets the questions.
        /// </summary>
        /// <value>
        /// The questions.
        /// </value>
        public IEnumerable<QuestionViewModel> Questions { get; set; }

        /// <summary>
        /// Gets or sets the exercises.
        /// </summary>
        /// <value>
        /// The exercises.
        /// </value>
        public IEnumerable<ExerciseViewModel> Exercises { get; set; }
    }
}