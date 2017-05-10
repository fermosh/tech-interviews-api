namespace TechnicalInterviewHelper.Model
{
    /// <summary>
    /// Enum for type of document (catalog).
    /// </summary>
    public enum DocumentType
    {
        /// <summary>
        /// No valid Id
        /// </summary>
        NotValid = 0,

        /// <summary>
        /// Id for competencies
        /// </summary>
        Competencies = 1,

        /// <summary>
        /// Id for skills
        /// </summary>
        Skills = 2,

        /// <summary>
        /// Id for exercises
        /// </summary>
        Exercises = 3,

        /// <summary>
        /// Id for questions
        /// </summary>
        Questions = 4,

        /// <summary>
        /// Id for templates
        /// </summary>
        Templates = 5,

        /// <summary>
        /// Id for interviews
        /// </summary>
        Interviews = 6,

        /// <summary>
        /// Id for functions
        /// </summary>
        JobFunctions = 7
    }
}