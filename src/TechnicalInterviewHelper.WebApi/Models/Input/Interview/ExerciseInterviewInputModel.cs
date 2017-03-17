namespace TechnicalInterviewHelper.WebApi.Model
{
    public class ExerciseInterviewInputModel
    {
        /// <summary>
        /// Gets or sets the exercise identifier.
        /// </summary>
        /// <value>
        /// The exercise identifier.
        /// </value>
        public string ExerciseId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the captured solution.
        /// </summary>
        /// <value>
        /// The captured solution.
        /// </value>
        public string CapturedSolution { get; set; }

        /// <summary>
        /// Gets or sets the captured rating.
        /// </summary>
        /// <value>
        /// The captured rating.
        /// </value>
        public float CapturedRating { get; set; }
    }
}