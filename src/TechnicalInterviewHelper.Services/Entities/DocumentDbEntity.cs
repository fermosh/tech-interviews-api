namespace TechnicalInterviewHelper.Services
{
    using Model;

    /// <summary>
    /// Minimum fields that an entity model in DocumentDB should meet.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Model.IEntity{System.String}" />
    public class DocumentDbEntity : IEntity<string>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }
    }
}