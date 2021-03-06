﻿namespace TechnicalInterviewHelper.Model
{
    using System.Collections.Generic;
    using Attributes;
    using Newtonsoft.Json;    

    /// <summary>
    /// Entity which models a document with skills.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Model.BaseEntity" />
    [DocumentType(DocumentType.Skills)]
    public class SkillMatrix : BaseEntity
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance has content.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has content; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("hasContent")]
        public bool HasContent { get; set; }

        /// <summary>
        /// Gets or sets the competency identifier.
        /// </summary>
        /// <value>
        /// The competency identifier.
        /// </value>
        [JsonProperty("competencyId")]
        public int CompetencyId { get; set; }

        /// <summary>
        /// Gets or sets the skills.
        /// </summary>
        /// <value>
        /// The skills.
        /// </value>
        [JsonProperty("skills")]
        public IEnumerable<Skill> Skills { get; set; }
    }
}