﻿namespace TechnicalInterviewHelper.WebApi.Model
{    
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// View model with skills information.
    /// </summary>
    public class TemplateViewModel
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
        [JsonProperty("exercises")]
        public IEnumerable<object> Exercises { get; set; }

        /// <summary>
        /// Gets or sets the questions.
        /// <para/>This property will be always empty.
        /// </summary>
        /// <value>
        /// The questions.
        /// </value>
        [JsonProperty("questions")]
        public IEnumerable<object> Questions { get; set; }
    }
}