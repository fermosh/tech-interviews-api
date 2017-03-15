namespace TechnicalInterviewHelper.WebApi.Model
{
    /// <summary>
    /// A view model of an exercise.
    /// </summary>
    public class ExerciseViewModel
    {
        /// <summary>
        /// Gets or sets the exercise identifier.
        /// </summary>
        /// <value>
        /// The exercise identifier.
        /// </value>
        public int ExerciseId { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }
    }
}