namespace TechnicalInterviewHelper.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// The class implements the data type for the Id field that every entity should have.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Model.IEntity{System.String}" />
    public abstract class BaseEntity : IEntity<string>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the document type identifier.
        /// </summary>
        /// <value>
        /// The document type identifier.
        /// </value>
        [JsonProperty("documentTypeId")]
        public DocumentType DocumentTypeId { get; set; }
    }
}