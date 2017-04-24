namespace TechnicalInterviewHelper.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;    
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using Model;

    /// <summary>
    /// Repository for specific operation related to competencies.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Services.DocumentDbQueryRepository{TechnicalInterviewHelper.Model.CompetencyDocument, System.String}" />
    /// <seealso cref="TechnicalInterviewHelper.Model.ICompetencyQueryRepository" />
    public class CompetencyDocumentDbQueryRepository : DocumentDbQueryRepository<CompetencyDocument, string>, ICompetencyQueryRepository
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CompetencyDocumentDbQueryRepository"/> class.
        /// </summary>
        /// <param name="collectionId">The collection identifier.</param>
        public CompetencyDocumentDbQueryRepository(string collectionId)
            : base(collectionId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompetencyDocumentDbQueryRepository"/> class.
        /// </summary>
        /// <param name="documentClient">The document client.</param>
        public CompetencyDocumentDbQueryRepository(DocumentClient documentClient)
            : base(documentClient)
        {
        }

        #endregion Constructor

        /// <summary>
        /// Finds a unique competency through all the collection.
        /// </summary>
        /// <param name="competencyId">The competency identifier.</param>
        /// <returns>A competency that belongs to the passed identifier.</returns>
        public async Task<Competency> FindCompetency(int competencyId)
        {
            var documentQuery =
                    this.DocumentClient
                    .CreateDocumentQuery<CompetencyDocument>(UriFactory.CreateDocumentCollectionUri(this.DatabaseId, this.CollectionId), new FeedOptions { MaxItemCount = -1 })
                    .SelectMany(document => document.Competencies)
                    .Where(competency => competency.Id == competencyId)
                    .Select(competency => competency)
                    .AsDocumentQuery();

            var competencyList = new List<Competency>();
            while (documentQuery.HasMoreResults)
            {
                var competencies = await documentQuery.ExecuteNextAsync<Competency>();
                competencyList.AddRange(competencies);
            }

            return competencyList.SingleOrDefault();
        }

        /// <summary>
        /// Finds a competency collection by a given Parent collection Identifier
        /// </summary>
        /// <param name="parentCompetencyId">The parent competency identifier.</param>
        /// <returns>A competency collection that belongs to the passed parent competency identifier.</returns>
        public async Task<IEnumerable<Competency>> FindCompetenciesByParentId(int parentCompetencyId)
        {
            var documentQuery =
                    this.DocumentClient
                    .CreateDocumentQuery<CompetencyDocument>(UriFactory.CreateDocumentCollectionUri(this.DatabaseId, this.CollectionId), new FeedOptions { MaxItemCount = -1 })
                    .SelectMany(document => document.Competencies)
                    .Where(competency => competency.ParentId == parentCompetencyId)
                    .Select(competency => competency)
                    .AsDocumentQuery();

            var competencyList = new List<Competency>();
            while (documentQuery.HasMoreResults)
            {
                var competencies = await documentQuery.ExecuteNextAsync<Competency>();
                competencyList.AddRange(competencies);
            }

            return competencyList;
        }

        /// <summary>
        /// Finds a competency collection by a given Parent collection Identifier
        /// </summary>
        /// <param name="parentCompetencyId">The parent competency identifier.</param>
        /// <returns>A competency collection that belongs to the passed parent competency identifier.</returns>
        public async Task<IEnumerable<int>> FindCompetenciesIdByParentId(int parentCompetencyId)
        {
            var documentQuery =
                    this.DocumentClient
                    .CreateDocumentQuery<CompetencyDocument>(UriFactory.CreateDocumentCollectionUri(this.DatabaseId, this.CollectionId), new FeedOptions { MaxItemCount = -1 })
                    .SelectMany(document => document.Competencies)
                    .Where(competency => competency.ParentId == parentCompetencyId)
                    .Select(competency => competency.Id)
                    .AsDocumentQuery();

            var competencyList = new List<int>();
            while (documentQuery.HasMoreResults)
            {
                var competencies = await documentQuery.ExecuteNextAsync<int>();
                competencyList.AddRange(competencies);
            }

            return competencyList;
        }
    }
}