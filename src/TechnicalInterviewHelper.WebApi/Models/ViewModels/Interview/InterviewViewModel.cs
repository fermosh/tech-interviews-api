namespace TechnicalInterviewHelper.WebApi.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// A view model with information regarding an interview.
    /// </summary>
    public class InterviewViewModel
    {
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public PositionViewModel Position { get; set; }

        /// <summary>
        /// Gets or sets the skills.
        /// </summary>
        /// <value>
        /// The skills.
        /// </value>
        public IEnumerable<SkillForInterviewViewModel> Skills { get; set; }

        /// <summary>
        /// Gets or sets the exercises.
        /// </summary>
        /// <value>
        /// The exercises.
        /// </value>
        public IEnumerable<ExerciseViewModel> Exercises { get; set; }
    }
}