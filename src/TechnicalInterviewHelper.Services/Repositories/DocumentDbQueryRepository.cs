namespace TechnicalInterviewHelper.Services
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using Model;

    /// <summary>
    /// Implements the query operations over a No-SQL Document Db data source.
    /// </summary>
    /// <typeparam name="T">An entity class.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <seealso cref="TechnicalInterviewHelper.Model.IQueryRepository{T, TKey}" />
    public class DocumentDbQueryRepository<T, TKey> : IQueryRepository<T, TKey>
        where T : class
    {
        #region Private fields

        /// <summary>
        /// The collection identifier
        /// </summary>
        private readonly string collectionId;

        /// <summary>
        /// The document client
        /// </summary>
        private readonly DocumentClient documentClient;

        /// <summary>
        /// The database identifier
        /// </summary>
        private string endPointUrl = ConfigurationManager.AppSettings["EndPointUrl"];

        /// <summary>
        /// The authorization key
        /// </summary>
        private string authorizationKey = ConfigurationManager.AppSettings["AuthorizationKey"];

        /// <summary>
        /// The database identifier
        /// </summary>
        private string databaseId = ConfigurationManager.AppSettings["DatabaseId"];

        #endregion Private fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentDbQueryRepository{T, TKey}"/> class.
        /// </summary>
        /// <param name="collectionId">The collection identifier.</param>
        public DocumentDbQueryRepository(string collectionId)
        {
            this.collectionId = collectionId;
            this.documentClient = new DocumentClient(new Uri(this.endPointUrl), this.authorizationKey, new ConnectionPolicy { EnableEndpointDiscovery = false });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentDbQueryRepository{T, TKey}"/> class.
        /// </summary>
        /// <param name="documentClient">The document client.</param>
        public DocumentDbQueryRepository(DocumentClient documentClient)
        {
            this.documentClient = documentClient;
        }

        #endregion Constructor

        /// <summary>
        /// Finds the by.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        /// An enumeration of entities.
        /// </returns>
        public async Task<IEnumerable<T>> FindBy(Expression<Func<T, bool>> predicate)
        {
            var documentQuery = this.documentClient.CreateDocumentQuery<T>(UriFactory.CreateDocumentCollectionUri(this.databaseId, this.collectionId), new FeedOptions { MaxItemCount = -1 }).Where(predicate).AsDocumentQuery();

            var competencyList = new List<T>();
            while (documentQuery.HasMoreResults)
            {
                competencyList.AddRange(await documentQuery.ExecuteNextAsync<T>());
            }

            return competencyList;
        }

        /// <summary>
        /// Finds the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// An entity.
        /// </returns>
        public async Task<T> FindById(TKey id)
        {
            try
            {
                var mm = Convert.ToString(id);

                var competencyDocument = await this.documentClient.ReadDocumentAsync(UriFactory.CreateDocumentUri(this.databaseId, this.collectionId, mm));

                return (T)(dynamic)competencyDocument;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return default(T);
                }

                throw;
            }
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>
        /// All entities in the collection.
        /// </returns>
        public async Task<IEnumerable<T>> GetAll()
        {
            var documentQuery = this.documentClient.CreateDocumentQuery(UriFactory.CreateDocumentCollectionUri(this.databaseId, this.collectionId), new FeedOptions { MaxItemCount = -1 }).AsDocumentQuery();

            var competencyList = new List<T>();
            while (documentQuery.HasMoreResults)
            {
                competencyList.AddRange(await documentQuery.ExecuteNextAsync<T>());
            }

            return competencyList;
        }
    }
}