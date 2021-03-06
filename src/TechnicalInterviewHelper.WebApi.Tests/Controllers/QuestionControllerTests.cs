﻿namespace TechnicalInterviewHelper.WebApi.Tests.Controllers.Query
{
    using NUnit.Framework;

    [TestFixture]
    [Ignore("Tests need to be refactored due to last change in Question controller. lapch 04/10/2017")]
    public class QuestionControllerTests
    {
        /*
        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public void WhenInputTemplateIdIsNullOrEmpty_ReturnsBadRequestStatusCode(string inputTemplateId)
        {
            // Arrange
            var queryQuestionMock = new Mock<IQuestionQueryRepository>();
            var queryTemplateCatalogMock = new Mock<IQueryRepository<TemplateCatalog, string>>();
            var querySkillMatrixMock = new Mock<ISkillMatrixQueryRepository>();
            var controllerUnderTest = new QueryQuestionController(queryQuestionMock.Object, queryTemplateCatalogMock.Object, querySkillMatrixMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetQuestionsByTemplate(inputTemplateId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestErrorMessageResult>());
            Assert.That((actionResult as BadRequestErrorMessageResult).Message, Is.EqualTo("Cannot get questions without a valid template identifier."));
        }

        [Test]
        public void WhenInputTemplateIdDoesNotExist_ReturnsNotFoundStatusCode()
        {
            // Arrange
            var inputTemplateId = "07CFB7D0-5C3C-4433-8BAE-F79945B90376";

            var queryQuestionMock = new Mock<IQuestionQueryRepository>();

            var queryPositionSkillMock = new Mock<IQueryRepository<TemplateCatalog, string>>();
            queryPositionSkillMock
                .Setup(method => method.FindById(It.IsAny<string>()))
                .ReturnsAsync((string templateId) => null);

            var querySkillMatrixMock = new Mock<ISkillMatrixQueryRepository>();

            var controllerUnderTest = new QueryQuestionController(queryQuestionMock.Object, queryPositionSkillMock.Object, querySkillMatrixMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetQuestionsByTemplate(inputTemplateId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void WhenTemplateHasSkillIdsEqualToNull_ReturnsBadRequestStatusCode()
        {
            // Arrange
            var inputTemplateId = "07CFB7D0-5C3C-4433-8BAE-F79945B90376";

            var savedTemplateCatalog = new TemplateCatalog
            {
                Id = inputTemplateId,
                CompetencyId = 13,
                JobFunctionLevel = 1
            };

            var queryQuestionMock = new Mock<IQuestionQueryRepository>();

            var queryTemplateCatalogMock = new Mock<IQueryRepository<TemplateCatalog, string>>();
            queryTemplateCatalogMock
                .Setup(method => method.FindById(It.IsAny<string>()))
                .ReturnsAsync(savedTemplateCatalog);

            var querySkillMatrixMock = new Mock<ISkillMatrixQueryRepository>();

            var controllerUnderTest = new QueryQuestionController(queryQuestionMock.Object, queryTemplateCatalogMock.Object, querySkillMatrixMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetQuestionsByTemplate(inputTemplateId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestErrorMessageResult>());
            Assert.That((actionResult as BadRequestErrorMessageResult).Message, Is.EqualTo($"The template '{inputTemplateId}' doesn't have associated skills."));
        }

        [Test]
        public void WhenTemplateHasNoSkillIds_ReturnsBadRequestStatusCode()
        {
            // Arrange
            var inputTemplateId = "07CFB7D0-5C3C-4433-8BAE-F79945B90376";

            var savedPositionSkill = new TemplateCatalog
            {
                Id = inputTemplateId,
                CompetencyId = 13,
                JobFunctionLevel = 1,
                Skills = new List<int>()
            };

            var queryQuestionMock = new Mock<IQuestionQueryRepository>();

            var queryPositionSkillMock = new Mock<IQueryRepository<TemplateCatalog, string>>();
            queryPositionSkillMock
                .Setup(method => method.FindById(It.IsAny<string>()))
                .ReturnsAsync(savedPositionSkill);

            var querySkillMatrixMock = new Mock<ISkillMatrixQueryRepository>();

            var controllerUnderTest = new QueryQuestionController(queryQuestionMock.Object, queryPositionSkillMock.Object, querySkillMatrixMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetQuestionsByTemplate(inputTemplateId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestErrorMessageResult>());
            Assert.That((actionResult as BadRequestErrorMessageResult).Message, Is.EqualTo($"The template '{inputTemplateId}' doesn't have associated skills."));
        }

        [Test]
        public void WhenDoesNotExistAnyQuestion_ReturnsAnEmptyList()
        {
            // Arrange
            var inputTemplateId = "07CFB7D0-5C3C-4433-8BAE-F79945B90376";

            var savedQuestions = new List<QuestionCatalog>();

            var savedPositionSkill = new TemplateCatalog
            {
                Id = inputTemplateId,
                CompetencyId = 13,
                JobFunctionLevel = 1,
                Skills = new List<int> { 1001, 1912, 2000 }
            };

            var queryQuestionMock = new Mock<IQuestionQueryRepository>();
            queryQuestionMock
                .Setup(method => method.FindWithinQuestions(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int[]>()))
                .ReturnsAsync(savedQuestions);

            var queryPositionSkillMock = new Mock<IQueryRepository<TemplateCatalog, string>>();
            queryPositionSkillMock
                .Setup(method => method.FindById(It.IsAny<string>()))
                .ReturnsAsync(savedPositionSkill);

            var querySkillMatrixMock = new Mock<ISkillMatrixQueryRepository>();

            var controllerUnderTest = new QueryQuestionController(queryQuestionMock.Object, queryPositionSkillMock.Object, querySkillMatrixMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetQuestionsByTemplate(inputTemplateId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<List<QuestionViewModel>>>());
            Assert.That((actionResult as OkNegotiatedContentResult<List<QuestionViewModel>>).Content, Is.Empty);
            queryPositionSkillMock.Verify(method => method.FindById(It.IsAny<string>()), Times.Once);
            queryQuestionMock.Verify(method => method.FindWithinQuestions(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int[]>()), Times.Once);
        }

        [Test]
        public void WhenQuestionsExistForInputSkillIdentifiers_ReturnsAListOfQuestionViewModels()
        {
            // Arrange
            var inputTemplateId = "07CFB7D0-5C3C-4433-8BAE-F79945B90376";

            var savedPositionSkill = new TemplateCatalog
            {
                Id = inputTemplateId,
                CompetencyId = 13,
                JobFunctionLevel = 1,
                Skills = new List<int> { 1610, 1779 }
            };

            var savedQuestions = new List<QuestionCatalog>
            {
                new QuestionCatalog { Id = "FB8BA409-D56D-4E92-AE15-2D25B757F3AA", CompetencyId = 13, JobFunctionLevel = 1, SkillId = 1610, Description = "What's OOP purpose?.", Answer = string.Empty },
                new QuestionCatalog { Id = "8A7359B0-AA5D-406B-8124-1100AAF48C0A", CompetencyId = 13, JobFunctionLevel = 1, SkillId = 1610, Description = "What are the OOP foundamentals?.", Answer = string.Empty },
                new QuestionCatalog { Id = "45092883-D65B-4004-B87F-8A00A19CCF7A", CompetencyId = 1,  JobFunctionLevel = 1, SkillId = 56,   Description = "Can you describe the code review phase?.", Answer = string.Empty },
                new QuestionCatalog { Id = "EFFB19C6-7735-425C-B810-4D022DEFA600", CompetencyId = 13, JobFunctionLevel = 1, SkillId = 1779, Description = "Can you describe all scrum's ceremonies?.", Answer = string.Empty },
                new QuestionCatalog { Id = "051E2FC1-56A1-4EFE-8375-D68FE1DEAFF3", CompetencyId = 13, JobFunctionLevel = 1, SkillId = 1779, Description = "What's the role of the product owner?.", Answer = string.Empty },
                new QuestionCatalog { Id = "9DFCA635-C1E5-4EA8-89B2-5B41A10FD8E7", CompetencyId = 1,  JobFunctionLevel = 1, SkillId = 56,   Description = "Is scrum a philosophy or a framework?.", Answer = string.Empty },
            };

            var savedSkills = new List<Skill>
            {
               new Skill
               {
                   RootId = null, DisplayOrder = -100500, RequiredSkillLevel = 0, UserSkillLevel = 0, LevelsSet = 0, CompetencyId = 13,
                   JobFunctionLevel = 1, Topics = new List<Topic>(), Id = 1610, ParentId = null, Name = "Hard skills", IsSelectable = true
               },
               new Skill
               {
                   RootId = null, DisplayOrder = 35, RequiredSkillLevel = 0, UserSkillLevel = 0, LevelsSet = 0, CompetencyId = 13,
                   JobFunctionLevel = 1, Topics = new List<Topic>(), Id = 1779, ParentId = null, Name = "Soft skills", IsSelectable = true
               },
               new Skill
               {
                   RootId = 472, DisplayOrder = 54, RequiredSkillLevel = 10, UserSkillLevel = 0, LevelsSet = 0, CompetencyId = 13,
                   JobFunctionLevel = 1, Topics = new List<Topic> { new Topic { Name = "Stress tolerance - Advanced", IsRequired = false } }, Id = 564, ParentId = 477, Name = "Stress tolerance", IsSelectable = true
               },
               new Skill
               {
                   RootId = null, DisplayOrder = -100500, RequiredSkillLevel = 0, UserSkillLevel = 0, LevelsSet = 0, CompetencyId = 12,
                   JobFunctionLevel = 1, Topics = new List<Topic>(), Id = 7, ParentId = null, Name = "Hard skills", IsSelectable = true
               }
            };

            var queryQuestionMock = new Mock<IQuestionQueryRepository>();
            queryQuestionMock
                .Setup(method => method.FindWithinQuestions(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int[]>()))
                .ReturnsAsync((int competencyId, int jobFunctionId, int[] skillIds) =>
                {
                    var result = new List<QuestionCatalog>();

                    foreach (var skillId in skillIds)
                    {
                        var filteredQuestions = savedQuestions.Where(item => item.CompetencyId == competencyId && item.JobFunctionLevel == jobFunctionId && item.SkillId == skillId);
                        result.AddRange(filteredQuestions);
                    }

                    return result;
                });

            var queryPositionSkillMock = new Mock<IQueryRepository<TemplateCatalog, string>>();
            queryPositionSkillMock
                .Setup(method => method.FindById(It.IsAny<string>()))
                .ReturnsAsync(savedPositionSkill);

            var querySkillMatrixMock = new Mock<ISkillMatrixQueryRepository>();
            querySkillMatrixMock
                .Setup(method => method.FindWithinSkills(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int[]>()))
                .ReturnsAsync((int competencyId, int jobFunctionId, int[] skillIds) =>
                {
                    var result = new List<Skill>();
                    foreach (var skillId in skillIds)
                    {
                        var filteredSkills = savedSkills.Where(item => item.CompetencyId == competencyId && item.JobFunctionLevel == jobFunctionId && item.Id == skillId);
                        result.AddRange(filteredSkills);
                    }
                    return result;
                });

            var controllerUnderTest = new QueryQuestionController(queryQuestionMock.Object, queryPositionSkillMock.Object, querySkillMatrixMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetQuestionsByTemplate(inputTemplateId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<List<QuestionViewModel>>>());
            // -- Checks that the methods were called as expected.
            queryPositionSkillMock.Verify(method => method.FindById(It.IsAny<string>()), Times.Once);
            queryQuestionMock.Verify(method => method.FindWithinQuestions(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int[]>()), Times.Once);
            // -- Checks the quantity of records.
            Assert.That((actionResult as OkNegotiatedContentResult<List<QuestionViewModel>>).Content, Is.Not.Empty);
            Assert.That((actionResult as OkNegotiatedContentResult<List<QuestionViewModel>>).Content.Count, Is.EqualTo(4));
            // -- Checks the records' values.
            Assert.That((actionResult as OkNegotiatedContentResult<List<QuestionViewModel>>).Content[0].QuestionId, Is.EqualTo("FB8BA409-D56D-4E92-AE15-2D25B757F3AA"));
            Assert.That((actionResult as OkNegotiatedContentResult<List<QuestionViewModel>>).Content[0].Description, Is.EqualTo("What's OOP purpose?."));
        }
        */
    }
}