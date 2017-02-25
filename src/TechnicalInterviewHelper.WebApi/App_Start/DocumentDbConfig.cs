namespace TechnicalInterviewHelper.WebApi
{
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using System;
    using System.Configuration;
    using System.Threading.Tasks;

    /// <summary>
    /// A class to initialize the DocumentDB client.
    /// </summary>
    public class DocumentDbConfig
    {
        #region Private fields

        /// <summary>
        /// The end point
        /// </summary>
        private readonly string endPointUrl = ConfigurationManager.AppSettings["EndPointUrl"];

        /// <summary>
        /// The database identifier
        /// </summary>
        private readonly string databaseId = ConfigurationManager.AppSettings["DatabaseId"];

        /// <summary>
        /// The authorization key
        /// </summary>
        private readonly string authorizationKey = ConfigurationManager.AppSettings["AuthorizationKey"];

        /// <summary>
        /// The collection identifier
        /// </summary>
        private readonly string competencyCollectionId = ConfigurationManager.AppSettings["CompetencyCollectionId"];

        /// <summary>
        /// The level collection identifier
        /// </summary>
        private readonly string levelCollectionId = ConfigurationManager.AppSettings["LevelCollectionId"];

        /// <summary>
        /// The domain collection identifier
        /// </summary>
        private readonly string domainCollectionId = ConfigurationManager.AppSettings["DomainCollectionId"];

        /// <summary>
        /// The skill collection identifier
        /// </summary>
        private readonly string skillCollectionId = ConfigurationManager.AppSettings["SkillCollectionId"];

        /// <summary>
        /// The client
        /// </summary>
        private readonly DocumentClient client;

        #endregion Private fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentDbConfig"/> class.
        /// </summary>
        public DocumentDbConfig()
        {
            this.client = new DocumentClient(new Uri(this.endPointUrl), this.authorizationKey, new ConnectionPolicy { EnableEndpointDiscovery = false });
        }

        #endregion Constructor

        #region Static entrypoint

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            var documentDbInitializer = new DocumentDbConfig();
            documentDbInitializer.CreateDatabaseIfNotExistsAsync().Wait();
            documentDbInitializer.CreateCollectionIfNotExistsAsync().Wait();
        }

        #endregion Static entrypoint

        #region Initialization functions

        /// <summary>
        /// Creates the database if not exists asynchronous.
        /// </summary>
        /// <returns></returns>
        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await this.client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(this.databaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await this.client.CreateDatabaseAsync(new Database { Id = this.databaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Creates the collection if not exists asynchronous.
        /// </summary>
        /// <returns></returns>
        private async Task CreateCollectionIfNotExistsAsync()
        {
            #region Competency collection

            try
            {
                await this.client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(this.databaseId, this.competencyCollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await this.client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(this.databaseId),
                        new DocumentCollection { Id = this.competencyCollectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }

            #endregion Competency collection

            #region Level collection

            try
            {
                await this.client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(this.databaseId, this.levelCollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await this.client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(this.databaseId),
                        new DocumentCollection { Id = this.levelCollectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }

            #endregion Level collection

            #region Domain collection

            try
            {
                await this.client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(this.databaseId, this.domainCollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await this.client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(this.databaseId),
                        new DocumentCollection { Id = this.domainCollectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }

            #endregion Domain collection

            #region Skill collection

            try
            {
                await this.client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(this.databaseId, this.skillCollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await this.client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(this.databaseId),
                        new DocumentCollection { Id = this.skillCollectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }

            #endregion Skill collection
        }

        #endregion Initialization functions
    }
}