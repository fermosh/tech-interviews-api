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
    /// Repository for specific operation related to exercises.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Services.DocumentDbQueryRepository{TechnicalInterviewHelper.Model.Exercise, System.String}" />
    /// <seealso cref="TechnicalInterviewHelper.Model.IExerciseQueryRepository" />
    public class ExerciseDocumentDbQueryRepository : DocumentDbQueryRepository<Exercise, string>, IExerciseQueryRepository
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
        /// <param name="template">The template.</param>
        /// <returns>
        /// An enumeration of exercises.
        /// </returns>
        public async Task<IEnumerable<Exercise>> GetAll(Template template)
        {
            var documentQuery =
                    this.DocumentClient
                    .CreateDocumentQuery<Exercise>(UriFactory.CreateDocumentCollectionUri(this.DatabaseId, this.CollectionId), new FeedOptions { MaxItemCount = -1 })
                    .Where(document => document.Competency.Id == template.CompetencyId)
                    .SelectMany(document => document.Skills
                    .Where(skill => template.Skills.Contains(skill.Id))
                    .Select(skill => new Exercise
                    {
                        Id = document.Id,
                        Competency = document.Competency,
                        Skills = document.Skills,
                        Description = document.Description,
                        Solution = document.Solution,
                        Title = document.Title
                    }))
                    .AsDocumentQuery();

            var queryResult = new List<Exercise>();
            while (documentQuery.HasMoreResults)
            {
                var exercises = await documentQuery.ExecuteNextAsync<Exercise>();
                queryResult.AddRange(exercises);
            }

            return queryResult;
        }
    }
}