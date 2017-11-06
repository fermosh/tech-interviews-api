namespace TechnicalInterviewHelper.WebApi.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Input information chosen by the user and composed by filtered skills belonging to a competency and level.
    /// </summary>
    public class TemplateInputModel
    {
        /// <summary>
        /// Gets or sets the template name.
        /// </summary>
        /// <value>
        /// The template name.
        /// </value>
        [JsonProperty("name")]
        public string Name { get; set; }

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
        /// Gets or sets the skill identifiers.
        /// </summary>
        /// <value>
        /// The skill identifiers.
        /// </value>
        [JsonProperty("skills")]
        public IEnumerable<SkillTemplateInputModel> Skills { get; set; }

        /// <summary>
        /// Gets or sets the exercises identifiers.
        /// </summary>
        /// <value>
        /// The exercises.
        /// </value>
        [JsonProperty("exercises")]
        public IEnumerable<string> Exercises { get; set; }
    }
}