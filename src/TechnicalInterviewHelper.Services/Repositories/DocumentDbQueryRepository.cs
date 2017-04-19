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
    public class DocumentDbQueryRepository<T, TKey> : IQueryRepository<T, TKey>, IDisposable
        where T : IEntity<TKey>
    {
        #region Protected fields

        /// <summary>
        /// The collection identifier
        /// </summary>
        protected readonly string CollectionId;

        //// public string CollectionId { get { return this.collectionId; } private set; }

        /// <summary>
        /// The document client
        /// </summary>
        protected readonly DocumentClient DocumentClient;

        //// public DocumentClient DocumentClient { get { return this.documentClient; } private set; }

        #endregion Protected fields

        #region Private fields

        /// <summary>
        /// The database identifier
        /// </summary>
        private string databaseId = ConfigurationManager.AppSettings["DatabaseId"];

        /// <summary>
        /// The database identifier
        /// </summary>
        private string endPointUrl = ConfigurationManager.AppSettings["EndPointUrl"];

        /// <summary>
        /// The authorization key
        /// </summary>
        private string authorizationKey = ConfigurationManager.AppSettings["AuthorizationKey"];

        /// <summary>
        /// To know whether the class is already disposed.
        /// </summary>
        private bool disposedValue = false;

        #endregion Private fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentDbQueryRepository{T, TKey}"/> class.
        /// </summary>
        /// <param name="collectionId">The collection identifier.</param>
        public DocumentDbQueryRepository(string collectionId)
        {
            this.CollectionId = collectionId;
            this.DocumentClient = new DocumentClient(new Uri(this.endPointUrl), this.authorizationKey, new ConnectionPolicy { EnableEndpointDiscovery = false });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentDbQueryRepository{T, TKey}"/> class.
        /// </summary>
        /// <param name="documentClient">The document client.</param>
        public DocumentDbQueryRepository(DocumentClient documentClient)
        {
            this.DocumentClient = documentClient;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets the database identifier.
        /// </summary>
        /// <value>
        /// The database identifier.
        /// </value>
        public string DatabaseId
        {
            get
            {
                return this.databaseId;
            }
        }

        #endregion Properties

        #region Get functions

        /// <summary>
        /// Finds the by.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        /// An enumeration of entities.
        /// </returns>
        public async Task<IEnumerable<T>> FindBy(Expression<Func<T, bool>> predicate)
        {
            var documentQuery =
                    this.DocumentClient
                        .CreateDocumentQuery<T>(UriFactory.CreateDocumentCollectionUri(this.databaseId, this.CollectionId), new FeedOptions { MaxItemCount = -1 })
                        .Where(predicate)
                        .AsDocumentQuery();

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
                var documentId = Convert.ToString(id);
                var competencyDocument = await this.DocumentClient.ReadDocumentAsync(UriFactory.CreateDocumentUri(this.databaseId, this.CollectionId, documentId));
                return (T)(dynamic)competencyDocument.Resource;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return default(T);
                }

                throw;
            }
            catch (Exception)
            {
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
            var documentTypeId = DocumentType.NotValid;
            switch (nameof(T))
            {
                case nameof(Competency):
                    documentTypeId = DocumentType.Competencies;
                    break;
                case nameof(SkillMatrix):
                    documentTypeId = DocumentType.Skills;
                    break;
                case nameof(Exercise):
                    documentTypeId = DocumentType.Exercises;
                    break;
                case nameof(Question):
                    documentTypeId = DocumentType.Questions;
                    break;
                case nameof(Template):
                    documentTypeId = DocumentType.Templates;
                    break;
                case nameof(InterviewCatalog):
                    documentTypeId = DocumentType.Skills;
                    break;
                case nameof(JobFunctionDocument):
                    documentTypeId = DocumentType.JobFunctions;
                    break;
                default:
                    break;
            }

            var documentQuery =
                    this.DocumentClient
                        .CreateDocumentQuery<T>(UriFactory.CreateDocumentCollectionUri(this.databaseId, this.CollectionId), new FeedOptions { MaxItemCount = -1 })
                        .Where(item => item.DocumentTypeId == documentTypeId)
                        .AsDocumentQuery();

            var competencyList = new List<T>();
            while (documentQuery.HasMoreResults)
            {
                competencyList.AddRange(await documentQuery.ExecuteNextAsync<T>());
            }

            return competencyList;
        }

        #endregion Get functions

        #region IDisposable Support

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.DocumentClient.Dispose();
                }

                this.disposedValue = true;
            }
        }

        #endregion IDisposable Support
    }
}