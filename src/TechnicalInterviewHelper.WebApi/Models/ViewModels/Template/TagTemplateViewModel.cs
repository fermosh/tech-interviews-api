﻿namespace TechnicalInterviewHelper.WebApi.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// Tag entity.
    /// </summary>
    public class TagTemplateViewModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}