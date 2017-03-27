namespace TechnicalInterviewHelper.Services
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using Model;

    /// <summary>
    /// This code is still under test.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Services.ILevelQueryRepository" />
    public class LevelDocumentDbQueryRepository : DocumentDbQueryRepository<LevelCatalog, string>, ILevelQueryRepository
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelDocumentDbQueryRepository"/> class.
        /// </summary>
        /// <param name="collectionId">The collection identifier.</param>
        public LevelDocumentDbQueryRepository(string collectionId)
            : base(collectionId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelDocumentDbQueryRepository"/> class.
        /// </summary>
        /// <param name="documentClient">The document client.</param>
        public LevelDocumentDbQueryRepository(DocumentClient documentClient)
            : base(documentClient)
        {
        }

        #endregion Constructor

        /// <summary>
        /// Finds the on internal collection.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>It returns something.</returns>
        /// <exception cref="System.NotImplementedException">Throws exception.</exception>
        public async Task<IEnumerable<Level>> FindOnInternalCollection(Expression<Func<Level, bool>> predicate)
        {
            var documentQuery =
                    this.DocumentClient
                    .CreateDocumentQuery<LevelCatalog>(UriFactory.CreateDocumentCollectionUri(this.DatabaseId, this.CollectionId), new FeedOptions { MaxItemCount = -1 })
                    .SelectMany(catalog => catalog.Levels)
                    .Where(predicate)
                    .Select(level => level)
                    .AsDocumentQuery();

            var levels = new List<Level>();
            while (documentQuery.HasMoreResults)
            {
                var mlk = await documentQuery.ExecuteNextAsync<Level>();
                levels.AddRange(mlk);
            }

            return levels;
        }
    }
}