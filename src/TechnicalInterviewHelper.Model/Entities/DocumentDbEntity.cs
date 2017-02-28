namespace TechnicalInterviewHelper.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// The class implements the data type for the Id field that every entity should have in DocumentDB.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Model.IEntity{System.String}" />
    public abstract class DocumentDbEntity : IEntity<string>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}