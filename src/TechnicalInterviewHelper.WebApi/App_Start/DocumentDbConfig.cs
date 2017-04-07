namespace TechnicalInterviewHelper.WebApi.App_Start
{
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using System;
    using System.Configuration;
    using System.Threading.Tasks;

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
        /// The collection identifier
        /// </summary>
        private readonly string competencyCollectionId = ConfigurationManager.AppSettings["CompetencyCollectionId"];

        /// <summary>
        /// The level collection identifier
        /// </summary>
        private readonly string questionCollectionId = ConfigurationManager.AppSettings["QuestionCollectionId"];

        /// <summary>
        /// The domain collection identifier
        /// </summary>
        private readonly string exerciseCollectionId = ConfigurationManager.AppSettings["ExerciseCollectionId"];

        /// <summary>
        /// The skill collection identifier
        /// </summary>
        private readonly string skillCollectionId = ConfigurationManager.AppSettings["SkillCollectionId"];

        /// <summary>
        /// The template collection identifier
        /// </summary>
        private readonly string templateCollectionId = ConfigurationManager.AppSettings["TemplateCollectionId"];

        /// <summary>
        /// The interview collection identifier
        /// </summary>
        private readonly string interviewCollectionId = ConfigurationManager.AppSettings["InterviewCollectionId"];

        #endregion Collections

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
            #region Competency collection

            try
            {
                await this.documentClient.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(this.databaseId, this.competencyCollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await this.documentClient.CreateDocumentCollectionAsync(
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

            #region Question collection

            try
            {
                await this.documentClient.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(this.databaseId, this.questionCollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await this.documentClient.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(this.databaseId),
                        new DocumentCollection { Id = this.questionCollectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }

            #endregion Question collection

            #region Exercise collection

            try
            {
                await this.documentClient.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(this.databaseId, this.exerciseCollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await this.documentClient.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(this.databaseId),
                        new DocumentCollection { Id = this.exerciseCollectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }

            #endregion Skill collection

            #region Skill collection

            try
            {
                await this.documentClient.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(this.databaseId, this.skillCollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await this.documentClient.CreateDocumentCollectionAsync(
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

            #region Template collection

            try
            {
                await this.documentClient.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(this.databaseId, this.templateCollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await this.documentClient.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(this.databaseId),
                        new DocumentCollection { Id = this.templateCollectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }

            #endregion Template collection

            #region Interview collection

            try
            {
                await this.documentClient.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(this.databaseId, this.interviewCollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await this.documentClient.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(this.databaseId),
                        new DocumentCollection { Id = this.interviewCollectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }

            #endregion Interview collection
        }

        #endregion Create Collections

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