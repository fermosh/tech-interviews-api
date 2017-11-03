namespace TechnicalInterviewHelper.WebApi.Model
{    
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// View model with skills information.
    /// </summary>
    public class TemplateViewModel
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
        /// Gets or sets the level.
        /// </summary>
        /// <value>
        /// The level.
        /// </value>
        [JsonProperty("level")]
        public LevelViewModel Level { get; set; }

        /// <summary>
        /// Gets or sets the name of the competency.
        /// </summary>
        /// <value>
        /// The name of the competency.
        /// </value>
        [JsonProperty("competencyName")]
        public string CompetencyName { get; set; }

        /// <summary>
        /// Gets or sets the name of the domain.
        /// </summary>
        /// <value>
        /// The name of the domain.
        /// </value>
        [JsonProperty("domain")]
        public string DomainName { get; set; }

        /// <summary>
        /// Gets or sets the skills.
        /// </summary>
        /// <value>
        /// The skills.
        /// </value>
        [JsonProperty("skills")]
        public IEnumerable<SkillTemplateViewModel> Skills { get; set; }

        /// <summary>
        /// Gets or sets the exercises.
        /// <para />This property will be always empty.
        /// </summary>
        /// <value>
        /// The exercises.
        /// </value>
        [JsonProperty("interviewExercises")]
        public IEnumerable<ExerciseTemplateViewModel> Exercises { get; set; }
    }
}