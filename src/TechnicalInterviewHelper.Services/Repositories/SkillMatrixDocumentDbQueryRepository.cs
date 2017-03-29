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
    /// Repository for specific operation related to skill matrix catalog.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Services.DocumentDbQueryRepository{TechnicalInterviewHelper.Model.SkillMatrixCatalog, System.String}" />
    /// <seealso cref="TechnicalInterviewHelper.Model.ISkillMatrixQueryRepository" />
    public class SkillMatrixDocumentDbQueryRepository : DocumentDbQueryRepository<SkillMatrixCatalog, string>, ISkillMatrixQueryRepository
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillMatrixDocumentDbQueryRepository"/> class.
        /// </summary>
        /// <param name="collectionId">The collection identifier.</param>
        public SkillMatrixDocumentDbQueryRepository(string collectionId)
            : base(collectionId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillMatrixDocumentDbQueryRepository"/> class.
        /// </summary>
        /// <param name="documentClient">The document client.</param>
        public SkillMatrixDocumentDbQueryRepository(DocumentClient documentClient)
            : base(documentClient)
        {
        }

        #endregion Constructor

        /// <summary>
        /// Find all skills that match a predicate within an specific document of competency.
        /// </summary>
        /// <param name="competencyId">The competency identifier.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>An enumeration of skills.</returns>
        public async Task<IEnumerable<Skill>> FindWithin(int competencyId, Expression<Func<Skill, bool>> predicate)
        {
            var documentQuery =
                    this.DocumentClient
                    .CreateDocumentQuery<SkillMatrixCatalog>(UriFactory.CreateDocumentCollectionUri(this.DatabaseId, this.CollectionId), new FeedOptions { MaxItemCount = -1 })
                    .Where(catalog => catalog.CompetencyId == competencyId)
                    .SelectMany(catalog => catalog.Skills)
                    .Where(predicate)
                    .Select(skill => skill)
                    .AsDocumentQuery();

            var skillResult = new List<Skill>();
            while (documentQuery.HasMoreResults)
            {
                var skills = await documentQuery.ExecuteNextAsync<Skill>();
                skillResult.AddRange(skills);
            }

            return skillResult;
        }
    }
}