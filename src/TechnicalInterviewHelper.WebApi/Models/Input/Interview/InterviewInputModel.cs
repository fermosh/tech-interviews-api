﻿namespace TechnicalInterviewHelper.WebApi.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// View model for an Interview.
    /// </summary>
    public class InterviewInputModel
    {
        /// <summary>
        /// Gets or sets the position identifier.
        /// </summary>
        /// <value>
        /// The position identifier.
        /// </value>
        public int PositionId { get; set; }

        /// <summary>
        /// Gets or sets the skills.
        /// </summary>
        /// <value>
        /// The skills.
        /// </value>
        public IEnumerable<SkillInterviewInputModel> Skills { get; set; }

        /// <summary>
        /// Gets or sets the questions.
        /// </summary>
        /// <value>
        /// The questions.
        /// </value>
        public IEnumerable<QuestionInterviewInputModel> Questions { get; set; }

        /// <summary>
        /// Gets or sets the exercises.
        /// </summary>
        /// <value>
        /// The exercises.
        /// </value>
        public IEnumerable<ExerciseInterviewInputModel> Exercises { get; set; }
    }
}