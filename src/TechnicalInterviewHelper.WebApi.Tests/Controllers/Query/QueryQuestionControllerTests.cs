namespace TechnicalInterviewHelper.WebApi.Tests.Controllers.Query
{
    using Model;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Http.Results;
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
            var queryQuestionMock = new Mock<IQueryRepository<Question, string>>();
            var queryPositionSkillMock = new Mock<IQueryRepository<PositionSkill, string>>();
            var controllerUnderTest = new QueryQuestionController(queryQuestionMock.Object, queryPositionSkillMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetAll(inputTemplateId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestErrorMessageResult>());
            Assert.That((actionResult as BadRequestErrorMessageResult).Message, Is.EqualTo("Cannot get questions without a valid identifier"));
        }

        [Test]
        public void WhenInputTemplateIdDoesNotExist_ReturnsNotFoundStatusCode()
        {
            // Arrange
            var inputTemplateId = "07CFB7D0-5C3C-4433-8BAE-F79945B90376";

            var queryQuestionMock = new Mock<IQueryRepository<Question, string>>();

            var queryPositionSkillMock = new Mock<IQueryRepository<PositionSkill, string>>();
            queryPositionSkillMock
                .Setup(method => method.FindById(It.IsAny<string>()))
                .ReturnsAsync((string templateId) => null);

            var controllerUnderTest = new QueryQuestionController(queryQuestionMock.Object, queryPositionSkillMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetAll(inputTemplateId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void WhenTemplateHasSkillIdsEqualToNull_ReturnsBadRequestStatusCode()
        {
            // Arrange
            var inputTemplateId = "07CFB7D0-5C3C-4433-8BAE-F79945B90376";

            var savedPositionSkill = new PositionSkill
            {
                Id = inputTemplateId,
                Position = new Position { CompetencyId = 1, LevelId = 2, DomainId = 1 }
            };

            var queryQuestionMock = new Mock<IQueryRepository<Question, string>>();

            var queryPositionSkillMock = new Mock<IQueryRepository<PositionSkill, string>>();
            queryPositionSkillMock
                .Setup(method => method.FindById(It.IsAny<string>()))
                .ReturnsAsync(savedPositionSkill);

            var controllerUnderTest = new QueryQuestionController(queryQuestionMock.Object, queryPositionSkillMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetAll(inputTemplateId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestErrorMessageResult>());
            Assert.That((actionResult as BadRequestErrorMessageResult).Message, Is.EqualTo("There are no existing skill identifiers associated with the template '{templateId}'"));
        }

        [Test]
        public void WhenTemplateHasNoSkillIds_ReturnsBadRequestStatusCode()
        {
            // Arrange
            var inputTemplateId = "07CFB7D0-5C3C-4433-8BAE-F79945B90376";

            var savedPositionSkill = new PositionSkill
            {
                Id = inputTemplateId,
                Position = new Position { CompetencyId = 1, LevelId = 2, DomainId = 1 },
                SkillIdentifiers = new List<int>()
            };

            var queryQuestionMock = new Mock<IQueryRepository<Question, string>>();

            var queryPositionSkillMock = new Mock<IQueryRepository<PositionSkill, string>>();
            queryPositionSkillMock
                .Setup(method => method.FindById(It.IsAny<string>()))
                .ReturnsAsync(savedPositionSkill);

            var controllerUnderTest = new QueryQuestionController(queryQuestionMock.Object, queryPositionSkillMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetAll(inputTemplateId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestErrorMessageResult>());
            Assert.That((actionResult as BadRequestErrorMessageResult).Message, Is.EqualTo("There are no existing skill identifiers associated with the template '{templateId}'"));
        }

        [Test]
        public void WhenDoesNotExistAnyQuestion_ReturnsAnEmptyList()
        {
            // Arrange
            var inputTemplateId = "07CFB7D0-5C3C-4433-8BAE-F79945B90376";

            var savedPositionSkill = new PositionSkill
            {
                Id = inputTemplateId,
                Position = new Position { CompetencyId = 1, LevelId = 2, DomainId = 1 },
                SkillIdentifiers = new List<int> { 1001, 1912, 2000 }
            };

            var queryQuestionMock = new Mock<IQueryRepository<Question, string>>();
            queryQuestionMock
                .Setup(method => method.FindBy(It.IsAny<Expression<Func<Question, bool>>>()))
                .ReturnsAsync((Expression<Func<Question, bool>> predicate) => new List<Question>());

            var queryPositionSkillMock = new Mock<IQueryRepository<PositionSkill, string>>();
            queryPositionSkillMock
                .Setup(method => method.FindById(It.IsAny<string>()))
                .ReturnsAsync(savedPositionSkill);

            var controllerUnderTest = new QueryQuestionController(queryQuestionMock.Object, queryPositionSkillMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetAll(inputTemplateId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<List<QuestionViewModel>>>());
            Assert.That((actionResult as OkNegotiatedContentResult<List<QuestionViewModel>>).Content, Is.Empty);
            queryPositionSkillMock.Verify(method => method.FindById(It.IsAny<string>()), Times.Once);
            queryQuestionMock.Verify(method => method.FindBy(It.IsAny<Expression<Func<Question, bool>>>()), Times.Once);
        }

        [Test]
        public void WhenQuestionsExistForInputSkillIdentifiers_ReturnsAListOfQuestionViewModels()
        {
            // Arrange
            var inputTemplateId = "07CFB7D0-5C3C-4433-8BAE-F79945B90376";

            var savedPositionSkill = new PositionSkill
            {
                Id = inputTemplateId,
                Position = new Position { CompetencyId = 1, LevelId = 2, DomainId = 1 },
                SkillIdentifiers = new List<int> { 1610, 1779 }
            };

            var savedQuestions = new List<Question>
            {
                new Question { Id = "FB8BA409-D56D-4E92-AE15-2D25B757F3AA", SkillId = 1610, Description = "What's OOP purpose?." },
                new Question { Id = "8A7359B0-AA5D-406B-8124-1100AAF48C0A", SkillId = 1610, Description = "What are the OOP foundamentals?." },
                new Question { Id = "45092883-D65B-4004-B87F-8A00A19CCF7A", SkillId = 56,   Description = "Can you describe the code review phase?." },
                new Question { Id = "EFFB19C6-7735-425C-B810-4D022DEFA600", SkillId = 1779, Description = "Can you describe all scrum's ceremonies?." },
                new Question { Id = "051E2FC1-56A1-4EFE-8375-D68FE1DEAFF3", SkillId = 1779, Description = "What's the role of the product owner?." },
                new Question { Id = "9DFCA635-C1E5-4EA8-89B2-5B41A10FD8E7", SkillId = 56,   Description = "Is scrum a philosophy or a framework?." },
            };

            var queryQuestionMock = new Mock<IQueryRepository<Question, string>>();
            queryQuestionMock
                .Setup(method => method.FindBy(It.IsAny<Expression<Func<Question, bool>>>()))
                .ReturnsAsync((Expression<Func<Question, bool>> predicate) => savedQuestions.Where(predicate.Compile()));

            var queryPositionSkillMock = new Mock<IQueryRepository<PositionSkill, string>>();
            queryPositionSkillMock
                .Setup(method => method.FindById(It.IsAny<string>()))
                .ReturnsAsync(savedPositionSkill);

            var controllerUnderTest = new QueryQuestionController(queryQuestionMock.Object, queryPositionSkillMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetAll(inputTemplateId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<List<QuestionViewModel>>>());
            // -- Checks that the methods were called as expected.
            queryPositionSkillMock.Verify(method => method.FindById(It.IsAny<string>()), Times.Once);
            queryQuestionMock.Verify(method => method.FindBy(It.IsAny<Expression<Func<Question, bool>>>()), Times.Once);
            // -- Checks the quantity of records.
            Assert.That((actionResult as OkNegotiatedContentResult<List<QuestionViewModel>>).Content, Is.Not.Empty);
            Assert.That((actionResult as OkNegotiatedContentResult<List<QuestionViewModel>>).Content.Count, Is.EqualTo(4));
            // -- Checks the records' values.
            Assert.That((actionResult as OkNegotiatedContentResult<List<QuestionViewModel>>).Content[0].QuestionId, Is.EqualTo("FB8BA409-D56D-4E92-AE15-2D25B757F3AA"));
            Assert.That((actionResult as OkNegotiatedContentResult<List<QuestionViewModel>>).Content[0].Description, Is.EqualTo("What's OOP purpose?."));
        }
    }
}