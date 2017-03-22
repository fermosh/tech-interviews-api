namespace TechnicalInterviewHelper.WebApi.Tests.Controllers.Query
{
    using Model;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using System.Web.Http.Results;
    using TechnicalInterviewHelper.Model;
    using WebApi.Controllers;

    [TestFixture]
    public class QueryInterviewControllerTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public void WhenInputPositionSkillIdIsNullOrEmpty_ReturnsBadRequestStatusCode(string positionSkillId)
        {
            // Arrange
            var querySkillMock = new Mock<IQueryRepository<Skill, string>>();
            var queryQuestionMock = new Mock<IQueryRepository<Question, string>>();
            var queryPositionSkillMock = new Mock<IQueryRepository<PositionSkill, string>>();

            var controllerUnderTest = new QueryInterviewController(querySkillMock.Object,
                                                                   queryQuestionMock.Object,
                                                                   queryPositionSkillMock.Object);

            // Act
            var actionResult = controllerUnderTest.Get(positionSkillId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestErrorMessageResult>());
            Assert.That((actionResult as BadRequestErrorMessageResult).Message, Is.EqualTo("Cannot get an interview without an identifier of filtered skills for a position."));
            queryPositionSkillMock.Verify(method => method.FindById(It.IsAny<string>()), Times.Never);
            querySkillMock.Verify(method => method.FindBy(It.IsAny<Expression<Func<Skill, bool>>>()), Times.Never);
            queryQuestionMock.Verify(method => method.FindBy(It.IsAny<Expression<Func<Question, bool>>>()), Times.Never);
        }

        [Test]
        public void WhenPositionSkillDoesNotExist_ReturnsNotFoundStatusCode()
        {
            // Arrange
            var inputPositionSkillId = "7168AB98-5197-406A-A5DB-A5FF9548B32E";

            var querySkillMock = new Mock<IQueryRepository<Skill, string>>();
            var queryQuestionMock = new Mock<IQueryRepository<Question, string>>();
            var queryPositionSkillMock = new Mock<IQueryRepository<PositionSkill, string>>();

            queryPositionSkillMock
                .Setup(method => method.FindById(It.IsAny<string>()))
                .Returns(Task.FromResult<PositionSkill>(null));

            var controllerUnderTest = new QueryInterviewController(querySkillMock.Object,
                                                                   queryQuestionMock.Object,
                                                                   queryPositionSkillMock.Object);

            // Act
            var actionResult = controllerUnderTest.Get(inputPositionSkillId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
            queryPositionSkillMock.Verify(method => method.FindById(It.IsAny<string>()), Times.Once);
            querySkillMock.Verify(method => method.FindBy(It.IsAny<Expression<Func<Skill, bool>>>()), Times.Never);
            queryQuestionMock.Verify(method => method.FindBy(It.IsAny<Expression<Func<Question, bool>>>()), Times.Never);
        }

        [Test]
        public void WhenPositionSkillIsValid_ReturnsOneInterviewViewModelWithAllItsFieldsFilledOut()
        {
            // Arrange
            var positionToTest = new Position
            {
                CompetencyId = 1,
                LevelId = 10,
                DomainId = 5
            };

            var positionSkillIdToTest = "7168AB98-5197-406A-A5DB-A5FF9548B32E";

            var savedPositionSkill = new PositionSkill
            {
                EntityId = positionSkillIdToTest,
                Position = new Position
                {
                    CompetencyId = positionToTest.CompetencyId,
                    LevelId = 10,
                    DomainId = 5
                },
                SkillIdentifiers = new int[] { 1610, 56, 1779, 1899 }
            };

            var savedSkills = new List<Skill>
            {
                new Skill { SkillId = 1610, Position = new Position { CompetencyId = positionToTest.CompetencyId, LevelId = positionToTest.LevelId, DomainId = positionToTest.DomainId }, Description = "OOP Theory" },
                new Skill { SkillId = 56, Position = new Position { CompetencyId = positionToTest.CompetencyId, LevelId = positionToTest.LevelId, DomainId = positionToTest.DomainId }, Description = "Code Reviews" },
                new Skill { SkillId = 1779, Position = new Position { CompetencyId = positionToTest.CompetencyId, LevelId = positionToTest.LevelId, DomainId = positionToTest.DomainId }, Description = "Agile Teams" },
                new Skill { SkillId = 1899, Position = new Position { CompetencyId = positionToTest.CompetencyId, LevelId = positionToTest.LevelId, DomainId = positionToTest.DomainId }, Description = "Cloud Based Applications" }
            };

            var savedQuestions = new List<Question>
            {
                new Question { EntityId = "FB8BA409-D56D-4E92-AE15-2D25B757F3AA", SkillId = 1610, Description = "What's OOP purpose?." },
                new Question { EntityId = "8A7359B0-AA5D-406B-8124-1100AAF48C0A", SkillId = 1610, Description = "What are the OOP foundamentals?." },
                new Question { EntityId = "45092883-D65B-4004-B87F-8A00A19CCF7A", SkillId = 56, Description = "Can you describe the code review phase?." },
                new Question { EntityId = "EFFB19C6-7735-425C-B810-4D022DEFA600", SkillId = 1779, Description = "Can you describe all scrum's ceremonies?." },
                new Question { EntityId = "051E2FC1-56A1-4EFE-8375-D68FE1DEAFF3", SkillId = 1779, Description = "What's the role of the product owner?." },
                new Question { EntityId = "9DFCA635-C1E5-4EA8-89B2-5B41A10FD8E7", SkillId = 1779, Description = "Is scrum a philosophy or a framework?." },
            };

            var querySkillMock = new Mock<IQueryRepository<Skill, string>>();

            querySkillMock
                .Setup(method => method.FindBy(It.IsAny<Expression<Func<Skill, bool>>>()))
                .ReturnsAsync((Expression<Func<Skill, bool>> predicate) => savedSkills.Where(predicate.Compile()));

            var queryQuestionMock = new Mock<IQueryRepository<Question, string>>();

            queryQuestionMock
               .Setup(method => method.FindBy(It.IsAny<Expression<Func<Question, bool>>>()))
               .ReturnsAsync((Expression<Func<Question, bool>> predicate) => savedQuestions.Where(predicate.Compile()));

            var queryPositionSkillMock = new Mock<IQueryRepository<PositionSkill, string>>();

            queryPositionSkillMock
                .Setup(method => method.FindById(positionSkillIdToTest))
                .ReturnsAsync(savedPositionSkill);

            var controllerUnderTest = new QueryInterviewController(querySkillMock.Object,
                                                                   queryQuestionMock.Object,
                                                                   queryPositionSkillMock.Object);

            // Act
            var actionResult = controllerUnderTest.Get(positionSkillIdToTest).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<InterviewViewModel>>());
            Assert.That((actionResult as OkNegotiatedContentResult<InterviewViewModel>).Content.CompetencyId, Is.EqualTo(positionToTest.CompetencyId));
            // -- Skills --
            Assert.That((actionResult as OkNegotiatedContentResult<InterviewViewModel>).Content.Skills, Is.Not.Empty);
            Assert.That((actionResult as OkNegotiatedContentResult<InterviewViewModel>).Content.Skills.Count(), Is.EqualTo(4));
            // -- Questions --
            Assert.That((actionResult as OkNegotiatedContentResult<InterviewViewModel>).Content.Questions, Is.Not.Empty);
            Assert.That((actionResult as OkNegotiatedContentResult<InterviewViewModel>).Content.Questions.Count(), Is.EqualTo(6));
            // -- Exercises --
            Assert.That((actionResult as OkNegotiatedContentResult<InterviewViewModel>).Content.Exercises, Is.Null);
        }
    }
}