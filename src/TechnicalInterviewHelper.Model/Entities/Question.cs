namespace TechnicalInterviewHelper.Model
{
    using Attributes;
    using Entities;
    using Newtonsoft.Json;

    /// <summary>
    /// Question entity.
    /// </summary>
    [DocumentType(DocumentType.Questions)]
    public class Question : BaseEntity
    {
        /// <summary>
        /// Gets or sets the competency identifier.
        /// </summary>
        /// <value>
        /// The competency identifier.
        /// </value>
        [JsonProperty("competency")]
        public Tag Competency { get; set; }

        /// <summary>
        /// Gets or sets the skill identifier.
        /// </summary>
        /// <value>
        /// The skill identifier.
        /// </value>
        [JsonProperty("skill")]
        public Tag Skill { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        [JsonProperty("body")]
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the answer.
        /// </summary>
        /// <value>
        /// The answer.
        /// </value>
        [JsonProperty("answer")]
        public string Answer { get; set; }

        /// <summary>
        /// Converts the question to string
        /// </summary>
        /// <returns>The question converted to String</returns>
        public override string ToString()
        {
            string competencyId = string.Empty, skillId = string.Empty;

            if (this.Skill != null)
            {
                skillId = this.Skill.Id.ToString();
            }

            if (this.Competency != null)
            {
                competencyId = this.Competency.Id.ToString();
            }
                
            return string.Format("ID: {0}, Competency: {1}, Skill: {2}, Body: {3}", this.Id, competencyId, skillId, this.Body);
        }
    }
}