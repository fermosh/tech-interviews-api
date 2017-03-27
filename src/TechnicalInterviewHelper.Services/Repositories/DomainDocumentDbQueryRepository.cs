namespace TechnicalInterviewHelper.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using Model;

    /// <summary>
    /// Repository for specific operation related to domains.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Services.DocumentDbQueryRepository{TechnicalInterviewHelper.Model.DomainCatalog, System.String}" />
    /// <seealso cref="TechnicalInterviewHelper.Model.IDomainQueryRepository" />
    public class DomainDocumentDbQueryRepository : DocumentDbQueryRepository<DomainCatalog, string>, IDomainQueryRepository
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainDocumentDbQueryRepository"/> class.
        /// </summary>
        /// <param name="collectionId">The collection identifier.</param>
        public DomainDocumentDbQueryRepository(string collectionId)
            : base(collectionId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainDocumentDbQueryRepository"/> class.
        /// </summary>
        /// <param name="documentClient">The document client.</param>
        public DomainDocumentDbQueryRepository(DocumentClient documentClient)
            : base(documentClient)
        {
        }

        #endregion Constructor

        /// <summary>
        /// Finds domains that match a criteria within the collection.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        /// An enumeration of domains.
        /// </returns>
        public async Task<IEnumerable<Domain>> FindWithin(Expression<Func<Domain, bool>> predicate)
        {
            var documentQuery =
                    this.DocumentClient
                    .CreateDocumentQuery<DomainCatalog>(UriFactory.CreateDocumentCollectionUri(this.DatabaseId, this.CollectionId), new FeedOptions { MaxItemCount = -1 })
                    .SelectMany(catalog => catalog.Domains)
                    .Where(predicate)
                    .Select(domain => domain)
                    .AsDocumentQuery();

            var levels = new List<Domain>();
            while (documentQuery.HasMoreResults)
            {
                var domains = await documentQuery.ExecuteNextAsync<Domain>();
                levels.AddRange(domains);
            }

            return levels;
        }
    }
}