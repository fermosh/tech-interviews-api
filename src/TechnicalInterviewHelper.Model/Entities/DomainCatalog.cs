namespace TechnicalInterviewHelper.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// An entity that has a list of domains.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Model.BaseEntity" />
    public class DomainCatalog : BaseEntity
    {
        /// <summary>
        /// Gets or sets the domains.
        /// </summary>
        /// <value>
        /// The domains.
        /// </value>
        [JsonProperty("domains")]
        public IEnumerable<Domain> Domains { get; set; }
    }
}