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
    /// Repository for specific operation related to questions.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Services.DocumentDbQueryRepository{TechnicalInterviewHelper.Model.Question, System.String}" />
    /// <seealso cref="TechnicalInterviewHelper.Model.IQuestionQueryRepository" />
    public class QuestionDocumentDbQueryRepository : DocumentDbQueryRepository<Question, string>, IQuestionQueryRepository
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionDocumentDbQueryRepository"/> class.
        /// </summary>
        /// <param name="collectionId">The collection identifier.</param>
        public QuestionDocumentDbQueryRepository(string collectionId)
            : base(collectionId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionDocumentDbQueryRepository"/> class.
        /// </summary>
        /// <param name="documentClient">The document client.</param>
        public QuestionDocumentDbQueryRepository(DocumentClient documentClient)
            : base(documentClient)
        {
        }

        #endregion Constructor

        /// <summary>
        /// Select all those questions that have skill id as one of its values.
        /// </summary>
        /// <param name="competencyId">The competency identifier.</param>
        /// <param name="jobFunctionLevel">The job function level.</param>
        /// <param name="skillIds">Skill identifiers to query.</param>
        /// <returns>
        /// An enumeration of questions.
        /// </returns>
        public async Task<IEnumerable<Question>> GetAll(int competencyId, int jobFunctionLevel, int[] skillIds)
        {
            var predicate = new StringBuilder();

            foreach (var skillId in skillIds)
            {
                predicate.Append($"(CompetencyId = {competencyId} AND JobFunctionLevel = {jobFunctionLevel} AND SkillId = {skillId}) OR ");
            }

            predicate.Remove(predicate.Length - 4, 4);

            var documentQuery =
                    this.DocumentClient
                    .CreateDocumentQuery<Question>(UriFactory.CreateDocumentCollectionUri(this.DatabaseId, this.CollectionId), new FeedOptions { MaxItemCount = -1 })
                    .Where(predicate.ToString())
                    .AsDocumentQuery();

            var questionResult = new List<Question>();
            while (documentQuery.HasMoreResults)
            {
                var questions = await documentQuery.ExecuteNextAsync<Question>();
                questionResult.AddRange(questions);
            }

            return questionResult;
        }
    }
}