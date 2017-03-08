namespace TechnicalInterviewHelper.WebApi.Model
{
    /// <summary>
    /// A view model to represent a domain.
    /// </summary>
    public class DomainViewModel
    {
        /// <summary>
        /// Gets or sets the domain identifier.
        /// </summary>
        /// <value>
        /// The domain identifier.
        /// </value>
        public int DomainId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
    }
}