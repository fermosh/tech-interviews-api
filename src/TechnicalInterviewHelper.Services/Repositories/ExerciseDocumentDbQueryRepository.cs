namespace TechnicalInterviewHelper.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using Model;

    /// <summary>
    /// Repository for specific operation related to exercises.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Services.DocumentDbQueryRepository{TechnicalInterviewHelper.Model.ExerciseCatalog, System.String}" />
    /// <seealso cref="TechnicalInterviewHelper.Model.IExerciseQueryRepository" />
    public class ExerciseDocumentDbQueryRepository : DocumentDbQueryRepository<ExerciseCatalog, string>, IExerciseQueryRepository
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ExerciseDocumentDbQueryRepository"/> class.
        /// </summary>
        /// <param name="collectionId">The collection identifier.</param>
        public ExerciseDocumentDbQueryRepository(string collectionId)
            : base(collectionId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExerciseDocumentDbQueryRepository"/> class.
        /// </summary>
        /// <param name="documentClient">The document client.</param>
        public ExerciseDocumentDbQueryRepository(DocumentClient documentClient)
            : base(documentClient)
        {
        }

        #endregion Constructor

        /// <summary>
        /// Finds the within exercises.
        /// </summary>
        /// <param name="competencyId">The competency identifier.</param>
        /// <param name="jobFunctionLevel">The job function level.</param>
        /// <param name="skillIds">Skill identifiers to query.</param>
        /// <returns>
        /// An enumeration of exercises.
        /// </returns>
        public async Task<IEnumerable<ExerciseCatalog>> FindWithinExercises(int competencyId, int jobFunctionLevel, int[] skillIds)
        {
            var predicate = new StringBuilder();

            foreach (var skillId in skillIds)
            {
                predicate.Append($"(CompetencyId = {competencyId} AND JobFunctionLevel = {jobFunctionLevel} AND SkillId = {skillId}) OR ");
            }

            predicate.Remove(predicate.Length - 4, 4);

            var documentQuery =
                    this.DocumentClient
                    .CreateDocumentQuery<ExerciseCatalog>(UriFactory.CreateDocumentCollectionUri(this.DatabaseId, this.CollectionId), new FeedOptions { MaxItemCount = -1 })
                    .Where(predicate.ToString())
                    .AsDocumentQuery();

            var queryResult = new List<ExerciseCatalog>();
            while (documentQuery.HasMoreResults)
            {
                var exercises = await documentQuery.ExecuteNextAsync<ExerciseCatalog>();
                queryResult.AddRange(exercises);
            }

            return queryResult;
        }
    }
}