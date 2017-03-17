namespace TechnicalInterviewHelper.WebApi.Model
{
    /// <summary>
    /// View model which represents a skill.
    /// </summary>
    public class SkillForPositionViewModel
    {
        /// <summary>
        /// Gets or sets the skill identifier.
        /// </summary>
        /// <value>
        /// The skill identifier.
        /// </value>
        public string SkillId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has children.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has children; otherwise, <c>false</c>.
        /// </value>
        public bool HasChildren { get; set; }

        /*
        //
        // All these might be in another view model class.
        //
        public int RootId { get; set; }

        public int DisplayOrder { get; set; }

        public int RequiredSkillLevel { get; set; }

        public int UserSkillLevel { get; set; }

        public int LevelSet { get; set; }

        public int CompetencyId { get; set; }

        public int jobFunctionLevel { get; set; }

        public IEnumerable<Topic> Topics { get; set; }

        public IEnumerable<Question> Questions { get; set; }

        public IEnumerable<Exercise> Exercises { get; set; }

        public bool IsSelectable { get; set; }

        public string Priority { get; set; }

        public string StartingFrom { get; set; }
        */
    }
}