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
            var queryExerciseMock = new Mock<IQueryRepository<Exercise, string>>();
            var queryPositionSkillMock = new Mock<IQueryRepository<PositionSkill, string>>();

            var controllerUnderTest = new QueryInterviewController(querySkillMock.Object,
                                                                   queryExerciseMock.Object,
                                                                   queryPositionSkillMock.Object);

            // Act
            var actionResult = controllerUnderTest.Get(positionSkillId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestErrorMessageResult>());
            Assert.That((actionResult as BadRequestErrorMessageResult).Message, Is.EqualTo("Cannot get an interview without an identifier of filtered skills for a position."));
            queryPositionSkillMock.Verify(method => method.FindById(It.IsAny<string>()), Times.Never);
            querySkillMock.Verify(method => method.FindBy(It.IsAny<Expression<Func<Skill, bool>>>()), Times.Never);
            queryExerciseMock.Verify(method => method.FindBy(It.IsAny<Expression<Func<Exercise, bool>>>()), Times.Never);
        }

        [Test]
        public void WhenPositionSkillDoesNotExist_ReturnsNotFoundStatusCode()
        {
            // Arrange
            var inputPositionSkillId = "7168AB98-5197-406A-A5DB-A5FF9548B32E";

            var querySkillMock = new Mock<IQueryRepository<Skill, string>>();
            var queryExerciseMock = new Mock<IQueryRepository<Exercise, string>>();
            var queryPositionSkillMock = new Mock<IQueryRepository<PositionSkill, string>>();

            queryPositionSkillMock
                .Setup(method => method.FindById(It.IsAny<string>()))
                .Returns(Task.FromResult<PositionSkill>(null));

            var controllerUnderTest = new QueryInterviewController(querySkillMock.Object,
                                                                   queryExerciseMock.Object,
                                                                   queryPositionSkillMock.Object);

            // Act
            var actionResult = controllerUnderTest.Get(inputPositionSkillId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
            queryPositionSkillMock.Verify(method => method.FindById(It.IsAny<string>()), Times.Once);
            querySkillMock.Verify(method => method.FindBy(It.IsAny<Expression<Func<Skill, bool>>>()), Times.Never);
            queryExerciseMock.Verify(method => method.FindBy(It.IsAny<Expression<Func<Exercise, bool>>>()), Times.Never);
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

            var savedExercises = new List<Exercise>
            {
                new Exercise { EntityId = "DB909BFC-3030-4C0E-9B47-B13D3711336E", SkillId = 1610, Title = "OOP - Inheritance", Description = "Having a parent class named 'Father', write the code of a child class named 'Child' that descends from it.", ProposedSolution = "class Child : Parent { ... }" },
                new Exercise { EntityId = "F542C661-932F-4E54-A44F-74DD74DA2511", SkillId = 56, Title = "Fixing a code review", Description = "Suppose you're intended to write a hash function using SHA-1 algorithm, please, ask your partner to make a code review of it.", ProposedSolution = "public byte*[] GetHashValue(char* entrance){ ... }" },
                new Exercise { EntityId = "398086B0-1BE8-40AA-B18A-F6AE7D6B424D", SkillId = 1899, Title = "Writing a microservice", Description = "Using Azure Functions or AWS Lambda, propose a code that sums two digits and return its square root.", ProposedSolution = "public double GetSquareRootOfTheSum(int x, int y) { ... }" }
            };

            var querySkillMock = new Mock<IQueryRepository<Skill, string>>();

            querySkillMock
                .Setup(method => method.FindBy(It.IsAny<Expression<Func<Skill, bool>>>()))
                .ReturnsAsync((Expression<Func<Skill, bool>> predicate) => savedSkills.Where(predicate.Compile()));

            var queryExerciseMock = new Mock<IQueryRepository<Exercise, string>>();

            queryExerciseMock
               .Setup(method => method.FindBy(It.IsAny<Expression<Func<Exercise, bool>>>()))
               .ReturnsAsync((Expression<Func<Exercise, bool>> predicate) => savedExercises.Where(predicate.Compile()));

            var queryPositionSkillMock = new Mock<IQueryRepository<PositionSkill, string>>();

            queryPositionSkillMock
                .Setup(method => method.FindById(positionSkillIdToTest))
                .ReturnsAsync(savedPositionSkill);

            var controllerUnderTest = new QueryInterviewController(querySkillMock.Object,
                                                                   queryExerciseMock.Object,
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
            Assert.That((actionResult as OkNegotiatedContentResult<InterviewViewModel>).Content.Exercises, Is.Not.Empty);
            Assert.That((actionResult as OkNegotiatedContentResult<InterviewViewModel>).Content.Exercises.Count(), Is.EqualTo(3));
        }
    }
}