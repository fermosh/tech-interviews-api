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
            var queryPositionSkillMock = new Mock<IQueryRepository<PositionSkill, string>>();

            var controllerUnderTest = new QueryInterviewController(querySkillMock.Object,
                                                                   queryPositionSkillMock.Object);

            // Act
            var actionResult = controllerUnderTest.Get(positionSkillId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestErrorMessageResult>());
            Assert.That((actionResult as BadRequestErrorMessageResult).Message, Is.EqualTo("Cannot get an interview without an identifier of filtered skills for a position."));
            queryPositionSkillMock.Verify(method => method.FindById(It.IsAny<string>()), Times.Never);
            querySkillMock.Verify(method => method.FindBy(It.IsAny<Expression<Func<Skill, bool>>>()), Times.Never);
        }

        [Test]
        public void WhenPositionSkillDoesNotExist_ReturnsNotFoundStatusCode()
        {
            // Arrange
            var inputPositionSkillId = "7168AB98-5197-406A-A5DB-A5FF9548B32E";

            var querySkillMock = new Mock<IQueryRepository<Skill, string>>();
            var queryPositionSkillMock = new Mock<IQueryRepository<PositionSkill, string>>();

            queryPositionSkillMock
                .Setup(method => method.FindById(It.IsAny<string>()))
                .Returns(Task.FromResult<PositionSkill>(null));

            var controllerUnderTest = new QueryInterviewController(querySkillMock.Object,
                                                                   queryPositionSkillMock.Object);

            // Act
            var actionResult = controllerUnderTest.Get(inputPositionSkillId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
            queryPositionSkillMock.Verify(method => method.FindById(It.IsAny<string>()), Times.Once);
            querySkillMock.Verify(method => method.FindBy(It.IsAny<Expression<Func<Skill, bool>>>()), Times.Never);
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
                Id = positionSkillIdToTest,
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
                new Skill { SkillId = 1610, Description = "OOP Theory" },
                new Skill { SkillId = 56, Description = "Code Reviews" },
                new Skill { SkillId = 1779, Description = "Agile Teams" },
                new Skill { SkillId = 1899, Description = "Cloud Based Applications" }
            };
            var querySkillMock = new Mock<IQueryRepository<Skill, string>>();

            querySkillMock
                .Setup(method => method.FindBy(It.IsAny<Expression<Func<Skill, bool>>>()))
                .ReturnsAsync((Expression<Func<Skill, bool>> predicate) => savedSkills.Where(predicate.Compile()));
            var queryPositionSkillMock = new Mock<IQueryRepository<PositionSkill, string>>();

            queryPositionSkillMock
                .Setup(method => method.FindById(positionSkillIdToTest))
                .ReturnsAsync(savedPositionSkill);

            var controllerUnderTest = new QueryInterviewController(querySkillMock.Object,
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
            Assert.That((actionResult as OkNegotiatedContentResult<InterviewViewModel>).Content.Questions, Is.Null);
            // -- Exercises --
            Assert.That((actionResult as OkNegotiatedContentResult<InterviewViewModel>).Content.Exercises, Is.Null);
        }
    }
}