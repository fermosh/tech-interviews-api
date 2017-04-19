namespace TechnicalInterviewHelper.Model
{
    /// <summary>
    /// Help us to know what type the ID field is and can include other common properties in every entity.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface IEntity<TKey>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        TKey Id { get; set; }

        /// <summary>
        /// Gets or sets the document type identifier.
        /// </summary>
        /// <value>
        /// The document type identifier.
        /// </value>
        DocumentType DocumentTypeId { get; set; }
    }
}