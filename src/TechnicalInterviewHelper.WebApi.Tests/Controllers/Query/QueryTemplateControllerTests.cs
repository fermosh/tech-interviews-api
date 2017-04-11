namespace TechnicalInterviewHelper.WebApi.Tests.Controllers.Query
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.Results;
    using Model;
    using Moq;
    using NUnit.Framework;    
    using TechnicalInterviewHelper.Model;
    using WebApi.Controllers;

    [TestFixture]
    public class QueryTemplateControllerTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void WhenInputParamaterIsNullOrEmpty_ReturnsBadRequestStatusCode(string templateId)
        {
            // Arrange
            var querySkillMatrix = new Mock<ISkillMatrixQueryRepository>();
            var queryTemplate = new Mock<IQueryRepository<Template, string>>();
            var queryJobFunction = new Mock<IJobFunctionQueryRepository>();
            var queryCompetency = new Mock<ICompetencyQueryRepository>();

            var controllerUnderTest = new QueryTemplateController(querySkillMatrix.Object, queryTemplate.Object, queryJobFunction.Object, queryCompetency.Object);

            // Act
            var actionResult = controllerUnderTest.Get(templateId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestErrorMessageResult>());
            Assert.That((actionResult as BadRequestErrorMessageResult).Message, Is.EqualTo("Cannot get a template without a valid identifier."));
        }

        [Test]
        public void WhenCanNotFindTemplate_ReturnsNotFoundStatusCode()
        {
            // Arrange
            string templateId = "B7F16780-227C-46FC-A7A1-7AADF91BBECA";

            var querySkillMatrix = new Mock<ISkillMatrixQueryRepository>();
            var queryJobFunction = new Mock<IJobFunctionQueryRepository>();
            var queryCompetency = new Mock<ICompetencyQueryRepository>();

            var queryTemplate = new Mock<IQueryRepository<Template, string>>();
            queryTemplate
                .Setup(method => method.FindById(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            var controllerUnderTest = new QueryTemplateController(querySkillMatrix.Object, queryTemplate.Object, queryJobFunction.Object, queryCompetency.Object);

            // Act
            var actionResult = controllerUnderTest.Get(templateId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
        }

        [TestCase(true)]
        [TestCase(false)]
        public void WhenSkillIsEmptyOrNull_ReturnsBadRequestStatusCode(bool isNullAndEmpty)
        {
            // Arrange
            string templateId = "B7F16780-227C-46FC-A7A1-7AADF91BBECA";

            var savedTemplate = new Template
            {
                Id = "B7F16780-227C-46FC-A7A1-7AADF91BBECA",
                CompetencyId = 13,
                JobFunctionLevel = 1,
                Skills = isNullAndEmpty == true ? null : new List<int>()
            };

            var querySkillMatrix = new Mock<ISkillMatrixQueryRepository>();
            var queryJobFunction = new Mock<IJobFunctionQueryRepository>();
            var queryCompetency = new Mock<ICompetencyQueryRepository>();

            var queryTemplate = new Mock<IQueryRepository<Template, string>>();
            queryTemplate
                .Setup(method => method.FindById(It.IsAny<string>()))
                .ReturnsAsync(savedTemplate);

            var controllerUnderTest = new QueryTemplateController(querySkillMatrix.Object, queryTemplate.Object, queryJobFunction.Object, queryCompetency.Object);

            // Act
            var actionResult = controllerUnderTest.Get(templateId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestErrorMessageResult>());
            Assert.That((actionResult as BadRequestErrorMessageResult).Message, Is.EqualTo($"The template with Id '{templateId}' doesn't have saved skills."));
        }

        [Test]
        public void WhenCompetencyIsNull_ThenTemplateViewModelHasEmptyDescriptions()
        {
            // Arrange
            string templateId = "B7F16780-227C-46FC-A7A1-7AADF91BBECA";

            var savedTemplate = new Template
            {
                Id = "B7F16780-227C-46FC-A7A1-7AADF91BBECA",
                CompetencyId = 13,
                JobFunctionLevel = 1,
                Skills = new List<int> { 228, 289 }
            };

            var queryJobFunction = new Mock<IJobFunctionQueryRepository>();

            var querySkillMatrix = new Mock<ISkillMatrixQueryRepository>();
            querySkillMatrix
                .Setup(method => method.FindWithinSkills(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int[]>()))
                .ReturnsAsync(() => new List<Skill>());

            var queryCompetency = new Mock<ICompetencyQueryRepository>();
            queryCompetency
                .Setup(method => method.FindCompetency(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            var queryTemplate = new Mock<IQueryRepository<Template, string>>();
            queryTemplate
                .Setup(method => method.FindById(It.IsAny<string>()))
                .ReturnsAsync(savedTemplate);

            var controllerUnderTest = new QueryTemplateController(querySkillMatrix.Object, queryTemplate.Object, queryJobFunction.Object, queryCompetency.Object);

            // Act
            var actionResult = controllerUnderTest.Get(templateId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<TemplateViewModel>>());
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.Level.Description, Is.EqualTo(string.Empty));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.CompetencyName, Is.EqualTo(string.Empty));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.DomainName, Is.EqualTo(string.Empty));
        }

        [Test]
        public void WhenCompetencyHasJobFunctionsAtFirstLevel_ThenFindCompetencyIsCalledOnceAndViewModelHasExpectedValues()
        {
            // Arrange
            string templateId = "B7F16780-227C-46FC-A7A1-7AADF91BBECA";

            var savedTemplate = new Template
            {
                Id = "B7F16780-227C-46FC-A7A1-7AADF91BBECA",
                CompetencyId = 13,
                JobFunctionLevel = 3,
                Skills = new List<int> { 228, 289 }
            };

            var savedJobFunction = new JobFunction
            {
                Id = 15,
                MnemonicBase = "D",
                Track = 0,
                Name = "Software Engineering"
            };

            var savedJobLevels = new List<Level>
            {
                new Level { Id = 1, IsEligibleForAsmt = false, JobTitles = new List<string> { "Junior Software Engineer" } },
                new Level { Id = 2, IsEligibleForAsmt = false, JobTitles = new List<string> { "Software Engineer" } },
                new Level { Id = 3, IsEligibleForAsmt = true, JobTitles = new List<string> { "Senior Software Engineer" } }
            };

            var savedJobFunctionDocument = new JobFunctionDocument
            {
                Id = "B63BE6A3-64E3-43D9-A200-4577ADFE26E5",
                JobFunction = savedJobFunction,
                Levels = savedJobLevels
            };

            var savedCompetencies = new List<Competency>
            {
                new Competency { Id = 1, Code = "DotNET", ParentId = null, IsSelectable = false, JobFunctions = new List<int>(), Name = ".NET" },
                new Competency { Id = 13, Code = "DotNET-Azure", ParentId = 1, IsSelectable = true, JobFunctions = new List<int> { 15 }, Name = "Azure Development" }
            };

            var queryJobFunction = new Mock<IJobFunctionQueryRepository>();
            queryJobFunction
                .Setup(method => method.FindJobTitleByLevel(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((int jobFunctionId, int jobFunctionLevel) => savedJobFunctionDocument.Levels.Where(item => item.Id == jobFunctionLevel).First().JobTitles.FirstOrDefault());

            var querySkillMatrix = new Mock<ISkillMatrixQueryRepository>();
            querySkillMatrix
                .Setup(method => method.FindWithinSkills(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int[]>()))
                .ReturnsAsync(() => new List<Skill>());

            var queryCompetency = new Mock<ICompetencyQueryRepository>();
            queryCompetency
                .Setup(method => method.FindCompetency(It.IsAny<int>()))
                .ReturnsAsync(() => savedCompetencies.Where(item => item.Id == savedTemplate.CompetencyId).FirstOrDefault());

            var queryTemplate = new Mock<IQueryRepository<Template, string>>();
            queryTemplate
                .Setup(method => method.FindById(It.IsAny<string>()))
                .ReturnsAsync(savedTemplate);

            var controllerUnderTest = new QueryTemplateController(querySkillMatrix.Object, queryTemplate.Object, queryJobFunction.Object, queryCompetency.Object);

            // Act
            var actionResult = controllerUnderTest.Get(templateId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<TemplateViewModel>>());
            queryCompetency.Verify(method => method.FindCompetency(It.IsAny<int>()), Times.Once);
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.Level.Description, Is.EqualTo(savedJobFunctionDocument.Levels.Where(item => item.Id == savedTemplate.JobFunctionLevel).First().JobTitles.FirstOrDefault()));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.CompetencyName, Is.EqualTo(savedCompetencies.Where(item => item.Id == savedTemplate.CompetencyId).First().Name));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.DomainName, Is.EqualTo(savedJobFunctionDocument.Levels.Where(item => item.Id == savedTemplate.JobFunctionLevel).First().JobTitles.FirstOrDefault()));
        }

        [Test]
        public void WhenCompetencyHasJobFunctionsAtTopLevels_ThenFindCompetencyIsCalledOnceAndViewModelHasExpectedValues()
        {
            // Arrange
            Competency selectedCompetency = null;

            string templateId = "B7F16780-227C-46FC-A7A1-7AADF91BBECA";

            var savedTemplate = new Template
            {
                Id = "B7F16780-227C-46FC-A7A1-7AADF91BBECA",
                CompetencyId = 13,
                JobFunctionLevel = 3,
                Skills = new List<int> { 228, 289 }
            };

            var savedJobFunction = new JobFunction
            {
                Id = 15,
                MnemonicBase = "D",
                Track = 0,
                Name = "Software Engineering"
            };

            var savedJobLevels = new List<Level>
            {
                new Level { Id = 1, IsEligibleForAsmt = false, JobTitles = new List<string> { "Junior Software Engineer" } },
                new Level { Id = 2, IsEligibleForAsmt = false, JobTitles = new List<string> { "Software Engineer" } },
                new Level { Id = 3, IsEligibleForAsmt = true, JobTitles = new List<string> { "Senior Software Engineer" } }
            };

            var savedJobFunctionDocument = new JobFunctionDocument
            {
                Id = "B63BE6A3-64E3-43D9-A200-4577ADFE26E5",
                JobFunction = savedJobFunction,
                Levels = savedJobLevels
            };

            var savedCompetencies = new List<Competency>
            {
                new Competency { Id = 1, Code = "DotNET", ParentId = null, IsSelectable = false, JobFunctions = new List<int>() { 15 }, Name = ".NET" },
                new Competency { Id = 13, Code = "DotNET-Azure", ParentId = 1, IsSelectable = true, JobFunctions = new List<int>(), Name = "Azure Development" }
            };

            var queryJobFunction = new Mock<IJobFunctionQueryRepository>();
            queryJobFunction
                .Setup(method => method.FindJobTitleByLevel(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((int jobFunctionId, int jobFunctionLevel) => savedJobFunctionDocument.Levels.Where(item => item.Id == jobFunctionLevel).First().JobTitles.FirstOrDefault());

            var querySkillMatrix = new Mock<ISkillMatrixQueryRepository>();
            querySkillMatrix
                .Setup(method => method.FindWithinSkills(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int[]>()))
                .ReturnsAsync(() => new List<Skill>());

            var queryCompetency = new Mock<ICompetencyQueryRepository>();
            queryCompetency
                .Setup(method => method.FindCompetency(It.IsAny<int>()))
                .ReturnsAsync((int competencyId) =>
                {
                    selectedCompetency = savedCompetencies.Where(item => item.Id == competencyId).FirstOrDefault();
                    return selectedCompetency;
                });

            var queryTemplate = new Mock<IQueryRepository<Template, string>>();
            queryTemplate
                .Setup(method => method.FindById(It.IsAny<string>()))
                .ReturnsAsync(savedTemplate);

            var controllerUnderTest = new QueryTemplateController(querySkillMatrix.Object, queryTemplate.Object, queryJobFunction.Object, queryCompetency.Object);

            // Act
            var actionResult = controllerUnderTest.Get(templateId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<TemplateViewModel>>());
            queryCompetency.Verify(method => method.FindCompetency(It.IsAny<int>()), Times.Exactly(2));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.Level.Description, Is.EqualTo(savedJobFunctionDocument.Levels.Where(item => item.Id == savedTemplate.JobFunctionLevel).First().JobTitles.FirstOrDefault()));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.CompetencyName, Is.EqualTo(selectedCompetency.Name));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.DomainName, Is.EqualTo(savedJobFunctionDocument.Levels.Where(item => item.Id == savedTemplate.JobFunctionLevel).First().JobTitles.FirstOrDefault()));
        }

        [Test]
        public void WhenCompetencyDoesNotHaveJobFunctionAtAll_ThenFindCompetencyIsCalledOnceAndViewModelHasExpectedValues()
        {
            // Arrange
            Competency selectedCompetency = null;

            string templateId = "B7F16780-227C-46FC-A7A1-7AADF91BBECA";

            var savedTemplate = new Template
            {
                Id = "B7F16780-227C-46FC-A7A1-7AADF91BBECA",
                CompetencyId = 13,
                JobFunctionLevel = 3,
                Skills = new List<int> { 228, 289 }
            };

            var savedJobFunction = new JobFunction
            {
                Id = 15,
                MnemonicBase = "D",
                Track = 0,
                Name = "Software Engineering"
            };

            var savedJobLevels = new List<Level>
            {
                new Level { Id = 1, IsEligibleForAsmt = false, JobTitles = new List<string> { "Junior Software Engineer" } },
                new Level { Id = 2, IsEligibleForAsmt = false, JobTitles = new List<string> { "Software Engineer" } },
                new Level { Id = 3, IsEligibleForAsmt = true, JobTitles = new List<string> { "Senior Software Engineer" } }
            };

            var savedJobFunctionDocument = new JobFunctionDocument
            {
                Id = "B63BE6A3-64E3-43D9-A200-4577ADFE26E5",
                JobFunction = savedJobFunction,
                Levels = savedJobLevels
            };

            var savedCompetencies = new List<Competency>
            {
                new Competency { Id = 1, Code = "DotNET", ParentId = null, IsSelectable = false, JobFunctions = new List<int>(), Name = ".NET" },
                new Competency { Id = 13, Code = "DotNET-Azure", ParentId = 1, IsSelectable = true, JobFunctions = new List<int>(), Name = "Azure Development" }
            };

            var queryJobFunction = new Mock<IJobFunctionQueryRepository>();
            queryJobFunction
                .Setup(method => method.FindJobTitleByLevel(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((int jobFunctionId, int jobFunctionLevel) => savedJobFunctionDocument.Levels.Where(item => item.Id == jobFunctionLevel).First().JobTitles.FirstOrDefault());

            var querySkillMatrix = new Mock<ISkillMatrixQueryRepository>();
            querySkillMatrix
                .Setup(method => method.FindWithinSkills(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int[]>()))
                .ReturnsAsync(() => new List<Skill>());

            var queryCompetency = new Mock<ICompetencyQueryRepository>();
            queryCompetency
                .Setup(method => method.FindCompetency(It.IsAny<int>()))
                .ReturnsAsync((int competencyId) =>
                {
                    selectedCompetency = savedCompetencies.Where(item => item.Id == competencyId).FirstOrDefault();
                    return selectedCompetency;
                });

            var queryTemplate = new Mock<IQueryRepository<Template, string>>();
            queryTemplate
                .Setup(method => method.FindById(It.IsAny<string>()))
                .ReturnsAsync(savedTemplate);

            var controllerUnderTest = new QueryTemplateController(querySkillMatrix.Object, queryTemplate.Object, queryJobFunction.Object, queryCompetency.Object);

            // Act
            var actionResult = controllerUnderTest.Get(templateId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<TemplateViewModel>>());
            queryCompetency.Verify(method => method.FindCompetency(It.IsAny<int>()), Times.Exactly(2));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.Level.Description, Is.EqualTo(string.Empty));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.CompetencyName, Is.EqualTo(selectedCompetency.Name));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.DomainName, Is.EqualTo(string.Empty));
        }

        [Test]
        public void When_ThenFindCompetencyIsCalledOnceAndViewModelHasExpectedValues()
        {
            // Arrange
            Competency selectedCompetency = null;

            string templateId = "B7F16780-227C-46FC-A7A1-7AADF91BBECA";

            var savedTemplate = new Template
            {
                Id = "B7F16780-227C-46FC-A7A1-7AADF91BBECA",
                CompetencyId = 13,
                JobFunctionLevel = 3,
                Skills = new List<int> { 228, 289 }
            };

            var savedJobFunction = new JobFunction
            {
                Id = 15,
                MnemonicBase = "D",
                Track = 0,
                Name = "Software Engineering"
            };

            var savedJobLevels = new List<Level>
            {
                new Level { Id = 1, IsEligibleForAsmt = false, JobTitles = new List<string> { "Junior Software Engineer" } },
                new Level { Id = 2, IsEligibleForAsmt = false, JobTitles = new List<string> { "Software Engineer" } },
                new Level { Id = 3, IsEligibleForAsmt = true, JobTitles = new List<string> { "Senior Software Engineer" } }
            };

            var savedJobFunctionDocument = new JobFunctionDocument
            {
                Id = "B63BE6A3-64E3-43D9-A200-4577ADFE26E5",
                JobFunction = savedJobFunction,
                Levels = savedJobLevels
            };

            var savedCompetencies = new List<Competency>
            {
                new Competency { Id = 1, Code = "DotNET", ParentId = null, IsSelectable = false, JobFunctions = new List<int>(), Name = ".NET" },
                new Competency { Id = 13, Code = "DotNET-Azure", ParentId = 1, IsSelectable = true, JobFunctions = new List<int>(), Name = "Azure Development" }
            };

            var savedTopicsForSelecetedSkill = new List<Topic>
            {
                new Topic { IsRequired = false, Name = "NET" }
            };

            var savedSkills = new List<Skill>
            {
                new Skill
                {
                    RootId = 1001, DisplayOrder = 1, RequiredSkillLevel = 1, UserSkillLevel = 10, LevelsSet = 1, CompetencyId = 13,
                    JobFunctionLevel = 1, Topics = null, Id = 1001, ParentId = 1982, Name = "Documentation", IsSelectable = true
                },
                new Skill
                {
                    RootId = 1001, DisplayOrder = 1, RequiredSkillLevel = 1, UserSkillLevel = 10, LevelsSet = 1, CompetencyId = 13,
                    JobFunctionLevel = 2, Topics = null, Id = 7456, ParentId = 1982, Name = "Design Patterns", IsSelectable = true
                },
                new Skill
                {
                    RootId = 1001, DisplayOrder = 1, RequiredSkillLevel = 1, UserSkillLevel = 10, LevelsSet = 1, CompetencyId = 13,
                    JobFunctionLevel = 3, Topics = savedTopicsForSelecetedSkill, Id = 228, ParentId = 1982, Name = "NET Best Practices",
                    IsSelectable = true
                },
                new Skill
                {
                    RootId = 1001, DisplayOrder = 1, RequiredSkillLevel = 1, UserSkillLevel = 10, LevelsSet = 1, CompetencyId = 13,
                    JobFunctionLevel = 1, Topics = null, Id = 46, ParentId = 1982, Name = "Solid principles", IsSelectable = true
                },
                new Skill
                {
                    RootId = 1001, DisplayOrder = 1, RequiredSkillLevel = 1, UserSkillLevel = 10, LevelsSet = 1, CompetencyId = 13,
                    JobFunctionLevel = 2, Topics = null, Id = 965, ParentId = 1982, Name = "Parallel programming", IsSelectable = true
                }
            };

            var queryJobFunction = new Mock<IJobFunctionQueryRepository>();
            queryJobFunction
                .Setup(method => method.FindJobTitleByLevel(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((int jobFunctionId, int jobFunctionLevel) => savedJobFunctionDocument.Levels.Where(item => item.Id == jobFunctionLevel).First().JobTitles.FirstOrDefault());

            var querySkillMatrix = new Mock<ISkillMatrixQueryRepository>();
            querySkillMatrix
                .Setup(method => method.FindWithinSkills(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int[]>()))
                .ReturnsAsync((int competencyId, int jobFunctionLevel, int[] skillIds) =>
                {
                    var filteredSkills = new List<Skill>();
                    foreach (var skillId in skillIds)
                    {
                        filteredSkills.AddRange(savedSkills.Where(item => item.CompetencyId == competencyId && item.JobFunctionLevel == jobFunctionLevel && item.Id == skillId));
                    }

                    return filteredSkills;
                });

            var queryCompetency = new Mock<ICompetencyQueryRepository>();
            queryCompetency
                .Setup(method => method.FindCompetency(It.IsAny<int>()))
                .ReturnsAsync((int competencyId) =>
                {
                    selectedCompetency = savedCompetencies.Where(item => item.Id == competencyId).FirstOrDefault();
                    return selectedCompetency;
                });

            var queryTemplate = new Mock<IQueryRepository<Template, string>>();
            queryTemplate
                .Setup(method => method.FindById(It.IsAny<string>()))
                .ReturnsAsync(savedTemplate);

            var controllerUnderTest = new QueryTemplateController(querySkillMatrix.Object, queryTemplate.Object, queryJobFunction.Object, queryCompetency.Object);

            // Act
            var actionResult = controllerUnderTest.Get(templateId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<TemplateViewModel>>());
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.CompetencyId, Is.EqualTo(13));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.JobFunctionLevel, Is.EqualTo(3));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.Level.Id, Is.EqualTo(3));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.Level.Description, Is.EqualTo(string.Empty));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.Level.Name, Is.EqualTo("L3"));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.CompetencyName, Is.EqualTo(selectedCompetency.Name));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.DomainName, Is.EqualTo(string.Empty));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.Skills.Count, Is.EqualTo(1));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.Skills.First().RootId, Is.EqualTo(1001));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.Skills.First().DisplayOrder, Is.EqualTo(1));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.Skills.First().RequiredSkillLevel, Is.EqualTo(1));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.Skills.First().UserSkillLevel, Is.EqualTo(10));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.Skills.First().LevelsSet, Is.EqualTo(1));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.Skills.First().CompetencyId, Is.EqualTo(13));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.Skills.First().JobFunctionLevel, Is.EqualTo(3));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.Skills.First().Topics.Count(), Is.EqualTo(1));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.Skills.First().Topics.First().IsRequired, Is.EqualTo(false));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.Skills.First().Topics.First().Name, Is.EqualTo("NET"));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.Skills.First().Questions, Is.Empty);
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.Skills.First().Id, Is.EqualTo(228));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.Skills.First().ParentId, Is.EqualTo(1982));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.Skills.First().Name, Is.EqualTo("NET Best Practices"));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.Skills.First().IsSelectable, Is.EqualTo(true));
            Assert.That((actionResult as OkNegotiatedContentResult<TemplateViewModel>).Content.Exercises, Is.Empty);
        }
    }
}