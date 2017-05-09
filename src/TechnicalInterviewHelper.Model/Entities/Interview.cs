namespace TechnicalInterviewHelper.Model
{
    using Attributes;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Entity model for an interview catalog.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Model.BaseEntity" />
    [DocumentType(DocumentType.Interviews)]
    public class Interview : BaseEntity
    {
        /// <summary>
        /// Gets or sets the competency identifier.
        /// </summary>
        /// <value>
        /// The competency identifier.
        /// </value>
        [JsonProperty("competencyId")]
        public int CompetencyId { get; set; }

        /// <summary>
        /// Gets or sets the job function level.
        /// </summary>
        /// <value>
        /// The job function level.
        /// </value>
        [JsonProperty("jobFunctionLevel")]
        public int JobFunctionLevel { get; set; }

        /// <summary>
        /// Gets or sets the template identifier.
        /// </summary>
        /// <value>
        /// The template identifier.
        /// </value>
        [JsonProperty("templateId")]
        public string TemplateId { get; set; }

        /// <summary>
        /// Gets or sets the skills.
        /// </summary>
        /// <value>
        /// The skills.
        /// </value>
        [JsonProperty("skills")]
        public IEnumerable<SkillInterview> Skills { get; set; }

        /// <summary>
        /// Gets or sets the exercises.
        /// </summary>
        /// <value>
        /// The exercises.
        /// </value>
        [JsonProperty("exercises")]
        public IEnumerable<AnsweredExercise> Exercises { get; set; }
    }
}