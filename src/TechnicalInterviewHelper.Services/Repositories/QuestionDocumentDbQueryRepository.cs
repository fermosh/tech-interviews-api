namespace TechnicalInterviewHelper.Services
{    
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic;
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
        /// <param name="template">The template.</param>
        /// <returns>
        /// An enumeration of questions.
        /// </returns>
        public async Task<IEnumerable<Question>> GetAll(Template template)
        {
            List<int> skillTemplateIds = template.Skills.Select(s => s.SkillId).ToList();

            var documentQuery =
                    this.DocumentClient
                    .CreateDocumentQuery<Question>(UriFactory.CreateDocumentCollectionUri(this.DatabaseId, this.CollectionId), new FeedOptions { MaxItemCount = -1 })
                    .Where(document =>
                        document.DocumentTypeId == DocumentType.Questions &&
                        document.Competency.Id == template.CompetencyId &&
                        skillTemplateIds.Contains(document.Skill.Id))
                    .AsDocumentQuery();

            var questionResult = new List<Question>();
            while (documentQuery.HasMoreResults)
            {
                var questions = await documentQuery.ExecuteNextAsync<Question>();
                questionResult.AddRange(questions);
            }

            return questionResult;
        }

        /// <summary>
        /// Select all those questions by Ids.
        /// </summary>
        /// <param name="ids">The Ids.</param>
        /// <returns>An enumeration of questions.</returns>
        public async Task<IEnumerable<Question>> FindByIds(List<string> ids)
        {
            var documentQuery =
                    this.DocumentClient
                    .CreateDocumentQuery<Question>(UriFactory.CreateDocumentCollectionUri(this.DatabaseId, this.CollectionId), new FeedOptions { MaxItemCount = -1 })
                    .Where(document =>
                        document.DocumentTypeId == DocumentType.Questions &&
                        ids.Contains(document.Id))
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