namespace TechnicalInterviewHelper.WebApi
{
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using System;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;

    /// <summary>
    /// A class to initialize the DocumentDB client.
    /// </summary>
    public class DocumentDbConfig : IDisposable
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

        #region Collections

        /// <summary>
        /// The main collection identifier
        /// </summary>
        private readonly string mainCollectionId = ConfigurationManager.AppSettings["MainCollectionId"];

        #endregion Collections

        #region Stored Procedures

        /// <summary>
        /// The Bulk Import Stored Procedure identifier
        /// </summary>
        private readonly string bulkImportStoredProcedureId = ConfigurationManager.AppSettings["BulkImportStoredProcedure"];

        /// <summary>
        /// The Bulk Import Stored Procedure identifier
        /// </summary>
        private readonly string bulkImportStoredProcedureFilePath = ConfigurationManager.AppSettings["BulkImportStoredProcedureFilePath"];

        #endregion Stored Procedures

        /// <summary>
        /// The client
        /// </summary>
        private readonly DocumentClient documentClient;

        #endregion Private fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentDbConfig"/> class.
        /// </summary>
        public DocumentDbConfig()
        {
            this.documentClient = new DocumentClient(new Uri(this.endPointUrl), this.authorizationKey, new ConnectionPolicy { EnableEndpointDiscovery = false });
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
            documentDbInitializer.CreateStoredProcedureAsync().Wait();
        }

        #endregion Static entrypoint

        #region Create Database

        /// <summary>
        /// Creates the database if not exists asynchronous.
        /// </summary>
        /// <returns></returns>
        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await this.documentClient.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(this.databaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await this.documentClient.CreateDatabaseAsync(new Database { Id = this.databaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        #endregion Create Database

        #region Create Collections

        /// <summary>
        /// Creates the collection if not exists asynchronous.
        /// </summary>
        /// <returns></returns>
        private async Task CreateCollectionIfNotExistsAsync()
        {
            var databaseUri = UriFactory.CreateDatabaseUri(this.databaseId);
            var requestOptions = new RequestOptions { OfferThroughput = 400 };

            try
            {
                var mainDocumentCollection = new DocumentCollection { Id = this.mainCollectionId };
                await this.documentClient.CreateDocumentCollectionIfNotExistsAsync(databaseUri, mainDocumentCollection, requestOptions);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Create Collections

        #region Create Stored Procedures

        /// <summary>
        /// Creates a Stored Procedure in the Main Collection
        /// </summary>
        /// <returns></returns>
        private async Task CreateStoredProcedureAsync()
        {
            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(this.databaseId, this.mainCollectionId);
            var requestOptions = new RequestOptions { OfferThroughput = 400 };

            var storedProcedure = new StoredProcedure
            {
                Id = bulkImportStoredProcedureId,
                Body = File.ReadAllText(HttpContext.Current.Server.MapPath(this.bulkImportStoredProcedureFilePath))
            };

            try
            {
                await TryDeleteStoredProcedure(documentCollectionUri, bulkImportStoredProcedureId);
                await this.documentClient.CreateStoredProcedureAsync(documentCollectionUri, storedProcedure, requestOptions);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// If a Stored Procedure is found on the DocumentCollection for the Id supplied it is deleted
        /// </summary>
        /// <param name="collectionLink">DocumentCollection to search for the Stored Procedure</param>
        /// <param name="storedProcedureId">Id of the Stored Procedure to delete</param>
        /// <returns></returns>
        private async Task TryDeleteStoredProcedure(Uri collectionLink, string storedProcedureId)
        {
            StoredProcedure storedProcedure = this.documentClient.CreateStoredProcedureQuery(collectionLink).Where(s => s.Id == storedProcedureId).AsEnumerable().FirstOrDefault();
            if (storedProcedure != null)
            {
                await this.documentClient.DeleteStoredProcedureAsync(storedProcedure.SelfLink);
            }
        }

        #endregion Create Stored Procedures

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.documentClient.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion IDisposable Support
    }
}