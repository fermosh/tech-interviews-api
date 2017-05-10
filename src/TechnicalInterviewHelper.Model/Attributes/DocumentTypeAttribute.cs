namespace TechnicalInterviewHelper.Model.Attributes
{
    using System;

    /// <summary>
    /// Attribute for IEntities.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    public class DocumentTypeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTypeAttribute"/> class.
        /// </summary>
        /// <param name="documentType">Type of the document.</param>
        public DocumentTypeAttribute(DocumentType documentType)
        {
            this.DocumentType = documentType;
        }

        /// <summary>
        /// Gets or sets the type of the document.
        /// </summary>
        /// <value>
        /// The type of the document.
        /// </value>
        public DocumentType DocumentType { get; set; }
    }
}