﻿namespace TechnicalInterviewHelper.Services
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading.Tasks;

    using Microsoft.Azure.Documents.Client;
    using Model;

    /// <summary>
    /// Implements the generic command repository (saving, e.g.).
    /// </summary>
    /// <typeparam name="T">Entity to apply the command operations.</typeparam>
    /// <seealso cref="TechnicalInterviewHelper.Model.ICommandRepository{T, TKey}" />
    public class DocumentDbCommandRepository<T> : ICommandRepository<T>, IDisposable
            where T : BaseEntity
    {
        #region fields

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

        /// <summary>
        /// Name of the Stored Procedure used to perform bulk imports, it inserts a document if it does not exist, if it does then updates it
        /// </summary>
        private string questionsBulkImportSPId = ConfigurationManager.AppSettings["QuestionsBulkImportSPId"];

        /// <summary>
        /// To know whether the class is already disposed.
        /// </summary>
        private bool disposedValue = false;        

        #endregion fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentDbCommandRepository{T}"/> class.
        /// </summary>
        /// <param name="collectionId">The collection identifier.</param>
        public DocumentDbCommandRepository(string collectionId)
        {
            this.collectionId = collectionId;
            this.documentClient = new DocumentClient(new Uri(this.endPointUrl), this.authorizationKey, new ConnectionPolicy { EnableEndpointDiscovery = false });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentDbCommandRepository{T}"/> class.
        /// </summary>
        /// <param name="documentClient">The document client.</param>
        /// <param name="collectionId">The collection identifier.</param>
        public DocumentDbCommandRepository(DocumentClient documentClient, string collectionId)
        {
            this.collectionId = collectionId;
            this.documentClient = documentClient;
        }

        #endregion Constructor

        #region Insert command

        /// <summary>
        /// Inserts the specified entity at the DocumentDB collection.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// The key type of the saved entity.
        /// </returns>
        public async Task<T> Insert(T entity)
        {
            var documentCreated = await this.documentClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(this.databaseId, this.collectionId), entity);
            entity.Id = documentCreated.Resource.Id;
            return entity;
        }

        /// <summary>
        /// Inserts an IEnumerable of entities
        /// </summary>
        /// <param name="entities">The IEnumerable of entities</param>
        /// <returns>A list of ErrorResult</returns>
        public async Task<ICollection<ErrorResult>> Insert(IEnumerable<T> entities)
        {
            List<ErrorResult> errorResults = null;

            try
            {
                var documentsCreated = await this.documentClient.ExecuteStoredProcedureAsync<int>(UriFactory.CreateStoredProcedureUri(this.databaseId, this.collectionId, this.questionsBulkImportSPId), entities);
            }
            catch (Exception e)
            {
                foreach (var entity in entities)
                {
                    try
                    {
                        var resultEntity = await this.Insert(entity);
                    }
                    catch (Exception internalEx)
                    {
                        if (errorResults == null)
                        {
                            errorResults = new List<ErrorResult>();
                        }

                        errorResults.Add(new ErrorResult
                        {
                            Entity = entity.ToString(),
                            ErrorDescription = internalEx.ToString()
                        });
                    }
                }
            }

            return errorResults;
        }

        #endregion Insert command

        #region Udpate command

        /// <summary>
        /// Updates a document with the specified entity identifier.
        /// </summary>
        /// <param name="entity">The entity with modified information.</param>
        /// <returns>A task of void.</returns>
        public async Task Update(T entity)
        {
            var updated = await this.documentClient.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(this.databaseId, this.collectionId, entity.Id), entity);
        }

        #endregion Udpate command

        #region Delete command

        /// <summary>
        /// Delete a document as an asynchronous operation from the Azure DocumentDB database service.
        /// </summary>
        /// <param name="id">The id</param>
        /// <returns>A task of void.</returns>
        public async Task Delete(string id)
        {
            var deleted = await this.documentClient.DeleteDocumentAsync(UriFactory.CreateDocumentUri(this.databaseId, this.collectionId, id));
        }

        #endregion

        #region IDisposable support

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
                    this.documentClient.Dispose();
                }

                this.disposedValue = true;
            }
        }

        #endregion IDisposable support
    }
}