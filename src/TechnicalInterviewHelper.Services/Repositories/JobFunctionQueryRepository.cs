namespace TechnicalInterviewHelper.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using Model;

    /// <summary>
    /// Repository for specific operation related to JobFunctions.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Services.DocumentDbQueryRepository{TechnicalInterviewHelper.Model.JobFunctionDocument, System.String}" />
    /// <seealso cref="TechnicalInterviewHelper.Model.IJobFunctionQueryRepository" />
    public class JobFunctionQueryRepository : DocumentDbQueryRepository<JobFunctionDocument, string>, IJobFunctionQueryRepository
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="JobFunctionQueryRepository"/> class.
        /// </summary>
        /// <param name="collectionId">The collection identifier.</param>
        public JobFunctionQueryRepository(string collectionId)
            : base(collectionId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JobFunctionQueryRepository"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public JobFunctionQueryRepository(DocumentClient client)
            : base(client)
        {
        }

        #endregion Constructor

        /// <summary>
        /// Finds the title of an specific job and level among all jobfunction documents.
        /// </summary>
        /// <param name="competencyId">The competency identifier.</param>
        /// <param name="jobFunctionLevel">The job function level.</param>
        /// <returns>
        /// The position title of the level.
        /// </returns>
        public async Task<string> FindJobTitleByLevel(int competencyId, int jobFunctionLevel)
        {
            var documentQuery =
                    this.DocumentClient
                    .CreateDocumentQuery<JobFunctionDocument>(UriFactory.CreateDocumentCollectionUri(this.DatabaseId, this.CollectionId), new FeedOptions { MaxItemCount = -1 })
                    .Where(document => document.JobFunction.Id == competencyId)
                    .SelectMany(document => document.Levels)
                    .Where(level => level.Id == jobFunctionLevel)
                    .Select(level => level.JobTitles)
                    .AsDocumentQuery();

            var jobTitles = new List<string>();
            while (documentQuery.HasMoreResults)
            {
                var jobTitleArrays = await documentQuery.ExecuteNextAsync<string[]>();
                foreach (var jobTitleArray in jobTitleArrays)
                {
                    jobTitles.AddRange(jobTitleArray);
                }                    
            }

            return jobTitles.FirstOrDefault();
        }
    }
}