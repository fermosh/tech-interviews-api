﻿namespace TechnicalInterviewHelper.WebApi.Tests.Controllers.Query
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
    public class QueryQuestionControllerTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public void WhenInputTemplateIdIsNullOrEmpty_ReturnsBadRequestStatusCode(string inputTemplateId)
        {
            // Arrange
            var queryQuestionMock = new Mock<IQuestionQueryRepository>();
            var queryTemplateCatalogMock = new Mock<IQueryRepository<TemplateCatalog, string>>();
            var controllerUnderTest = new QueryQuestionController(queryQuestionMock.Object, queryTemplateCatalogMock.Object);

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

            var controllerUnderTest = new QueryQuestionController(queryQuestionMock.Object, queryPositionSkillMock.Object);

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

            var controllerUnderTest = new QueryQuestionController(queryQuestionMock.Object, queryTemplateCatalogMock.Object);

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

            var controllerUnderTest = new QueryQuestionController(queryQuestionMock.Object, queryPositionSkillMock.Object);

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

            var savedQuestions = new List<Question>();

            var savedPositionSkill = new TemplateCatalog
            {
                Id = inputTemplateId,
                CompetencyId = 13,
                JobFunctionLevel = 1,
                Skills = new List<int> { 1001, 1912, 2000 }
            };

            var queryQuestionMock = new Mock<IQuestionQueryRepository>();
            queryQuestionMock
                .Setup(method => method.GetAll(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int[]>()))
                .ReturnsAsync(savedQuestions);

            var queryPositionSkillMock = new Mock<IQueryRepository<TemplateCatalog, string>>();
            queryPositionSkillMock
                .Setup(method => method.FindById(It.IsAny<string>()))
                .ReturnsAsync(savedPositionSkill);

            var controllerUnderTest = new QueryQuestionController(queryQuestionMock.Object, queryPositionSkillMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetQuestionsByTemplate(inputTemplateId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<List<QuestionViewModel>>>());
            Assert.That((actionResult as OkNegotiatedContentResult<List<QuestionViewModel>>).Content, Is.Empty);
            queryPositionSkillMock.Verify(method => method.FindById(It.IsAny<string>()), Times.Once);
            queryQuestionMock.Verify(method => method.GetAll(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int[]>()), Times.Once);
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

            var savedQuestions = new List<Question>
            {
                new Question { Id = "FB8BA409-D56D-4E92-AE15-2D25B757F3AA", Competency = { Id = 13 } , Skill = { Id = 1610 }, Body = "What's OOP purpose?." },
                new Question { Id = "8A7359B0-AA5D-406B-8124-1100AAF48C0A", Competency = { Id = 13 }, Skill = { Id = 1610 }, Body = "What are the OOP foundamentals?." },
                new Question { Id = "45092883-D65B-4004-B87F-8A00A19CCF7A", Competency = { Id = 1 }, Skill = { Id = 56 },   Body = "Can you describe the code review phase?." },
                new Question { Id = "EFFB19C6-7735-425C-B810-4D022DEFA600", Competency = { Id = 13 }, Skill = { Id = 1779 }, Body = "Can you describe all scrum's ceremonies?." },
                new Question { Id = "051E2FC1-56A1-4EFE-8375-D68FE1DEAFF3", Competency = { Id = 13 }, Skill = { Id = 1779 }, Body = "What's the role of the product owner?." },
                new Question { Id = "9DFCA635-C1E5-4EA8-89B2-5B41A10FD8E7", Competency = { Id = 1 },  Skill = { Id = 56 },   Body = "Is scrum a philosophy or a framework?." },
            };

            var queryQuestionMock = new Mock<IQuestionQueryRepository>();
            queryQuestionMock
                .Setup(method => method.GetAll(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int[]>()))
                .ReturnsAsync((int competencyId, int jobFunctionId, int[] skillIds) =>
                {
                    var result = new List<Question>();

                    foreach (var skillId in skillIds)
                    {
                        var filteredQuestions = savedQuestions.Where(item => item.Competency.Id == competencyId && item.Skill.Id == skillId);
                        result.AddRange(filteredQuestions);
                    }

                    return result;
                });

            var queryPositionSkillMock = new Mock<IQueryRepository<TemplateCatalog, string>>();
            queryPositionSkillMock
                .Setup(method => method.FindById(It.IsAny<string>()))
                .ReturnsAsync(savedPositionSkill);

            var controllerUnderTest = new QueryQuestionController(queryQuestionMock.Object, queryPositionSkillMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetQuestionsByTemplate(inputTemplateId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<List<QuestionViewModel>>>());
            // -- Checks that the methods were called as expected.
            queryPositionSkillMock.Verify(method => method.FindById(It.IsAny<string>()), Times.Once);
            queryQuestionMock.Verify(method => method.GetAll(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int[]>()), Times.Once);
            // -- Checks the quantity of records.
            Assert.That((actionResult as OkNegotiatedContentResult<List<QuestionViewModel>>).Content, Is.Not.Empty);
            Assert.That((actionResult as OkNegotiatedContentResult<List<QuestionViewModel>>).Content.Count, Is.EqualTo(4));
            // -- Checks the records' values.
            Assert.That((actionResult as OkNegotiatedContentResult<List<QuestionViewModel>>).Content[0].Id, Is.EqualTo("FB8BA409-D56D-4E92-AE15-2D25B757F3AA"));
            Assert.That((actionResult as OkNegotiatedContentResult<List<QuestionViewModel>>).Content[0].Body, Is.EqualTo("What's OOP purpose?."));
        }
    }
}