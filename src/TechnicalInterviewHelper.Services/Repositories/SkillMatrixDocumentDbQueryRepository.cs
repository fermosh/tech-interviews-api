namespace TechnicalInterviewHelper.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using Model;
    using TechnicalInterviewHelper.Model.Entities.Comparers;

    /// <summary>
    /// Repository for specific operation related to skill matrix catalog.
    /// </summary>
    /// <seealso cref="TechnicalInterviewHelper.Services.DocumentDbQueryRepository{TechnicalInterviewHelper.Model.SkillMatrix, System.String}" />
    /// <seealso cref="TechnicalInterviewHelper.Model.ISkillMatrixQueryRepository" />
    public class SkillMatrixDocumentDbQueryRepository : DocumentDbQueryRepository<SkillMatrix, string>, ISkillMatrixQueryRepository
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
                    .CreateDocumentQuery<SkillMatrix>(UriFactory.CreateDocumentCollectionUri(this.DatabaseId, this.CollectionId), new FeedOptions { MaxItemCount = -1 })
                    .Where(document => document.DocumentTypeId == DocumentType.Skills && document.CompetencyId == competencyId)
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

        /// <summary>
        /// Finds the within.
        /// </summary>
        /// <param name="competencyId">The competency identifier.</param>
        /// <param name="jobFunctionLevel">The job function level.</param>
        /// <param name="skillIds">The skill ids.</param>
        /// <returns>A bunch of tests.</returns>
        public async Task<IEnumerable<Skill>> FindWithinSkills(int competencyId, int jobFunctionLevel, int[] skillIds)
        {
            var documentQuery =
                    this.DocumentClient
                    .CreateDocumentQuery<SkillMatrix>(UriFactory.CreateDocumentCollectionUri(this.DatabaseId, this.CollectionId), new FeedOptions { MaxItemCount = -1 })
                    .Where(document => document.DocumentTypeId == DocumentType.Skills && document.CompetencyId == competencyId)
                    .SelectMany(catalog => catalog.Skills)
                    .Where(skill => skill.CompetencyId == competencyId && skill.JobFunctionLevel == jobFunctionLevel && skillIds.Contains(skill.Id))
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

        /// <summary>
        /// Return a skill collection
        /// </summary>
        /// <param name="competencyIds">Array of competencies</param>
        /// <param name="jobFunctionLevel">Job function level</param>
        /// <returns>skill collection</returns>
        public async Task<IEnumerable<Skill>> FindWithinSkills(IEnumerable<int> competencyIds, int jobFunctionLevel)
        {
            var documentQuery =
                    this.DocumentClient
                    .CreateDocumentQuery<SkillMatrix>(UriFactory.CreateDocumentCollectionUri(this.DatabaseId, this.CollectionId), new FeedOptions { MaxItemCount = -1 })
                    .Where(catalog => catalog.DocumentTypeId == DocumentType.Skills && competencyIds.Contains(catalog.CompetencyId))
                    .SelectMany(catalog => catalog.Skills)
                    .Where(skill => competencyIds.Contains(skill.CompetencyId) && skill.JobFunctionLevel == jobFunctionLevel)
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

        /// <summary>
        /// Queries the skills document and returns the skills by competency
        /// </summary>
        /// <param name="competencyId">receives the competency id</param>
        /// <returns>skill collection</returns>
        public async Task<IEnumerable<Skill>> FindWithinSkills(int competencyId)
        {
            var documentQuery =
            this.DocumentClient
            .CreateDocumentQuery<SkillMatrix>(UriFactory.CreateDocumentCollectionUri(this.DatabaseId, this.CollectionId), new FeedOptions { MaxItemCount = -1 })
            .Where(catalog => catalog.DocumentTypeId == DocumentType.Skills && catalog.CompetencyId == competencyId)
            .SelectMany(catalog => catalog.Skills)
            .Select(skill => skill)
            .AsDocumentQuery();

            var skillResult = new List<Skill>();
            var skillResult1 = new List<Skill>();
            while (documentQuery.HasMoreResults)
            {
                var skills = await documentQuery.ExecuteNextAsync<Skill>();
                skillResult.AddRange(skills);
            }

            skillResult.OrderBy(s => s.Name);

            // Load this list with only the skills for the job function level 1
            skillResult1.AddRange(skillResult.Where(s => s.JobFunctionLevel == 1));

            // the following routine is to create a new skill list but without duplicates
            int count = 0;
            // Loop through the original skill result which should contain all the skills for all job levels 
            foreach (var skill1 in skillResult)
            {
                count++;
                // check if skill exists in the skill result1
                if (!skillResult1.Any(s => s.Name == skill1.Name))
                {
                    skillResult1.Add(skill1);
                    continue;
                }
                else
                {
                    // this only checks if we reach the end of the skill result 1 which remember it only contains the skills related to job level 1
                    if (count == skillResult1.Count())
                    {
                        continue;
                    }
                }
            }

            return skillResult1;
        }
    }
}