namespace TechnicalInterviewHelper.WebApi.Tests.Controllers
{
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using System.Web.Http.Results;
    using TechnicalInterviewHelper.Model;
    using WebApi.Controllers;
    using System.Linq;
    using System.Collections.Generic;
    using System.Web.Http;
    using Model;

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
            var queryExerciseMock = new Mock<IQueryRepository< Exercise, string>>();
            var queryQuestionMock = new Mock<IQueryRepository< Question, string>>();
            var queryPositionSkillMock = new Mock<IQueryRepository<PositionSkill, string>>();

            var controllerUnderTest = new QueryInterviewController(querySkillMock.Object,
                                                                   queryExerciseMock.Object,
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
            queryExerciseMock.Verify(method => method.FindBy(It.IsAny<Expression<Func<Exercise, bool>>>()), Times.Never);
            queryQuestionMock.Verify(method => method.FindBy(It.IsAny<Expression<Func<Question, bool>>>()), Times.Never);
        }

        [Test]
        public void WhenPositionSkillDoesNotExist_ReturnsNotFoundStatusCode()
        {
            // Arrange
            var inputPositionSkillId = "7168AB98-5197-406A-A5DB-A5FF9548B32E";

            var querySkillMock = new Mock<IQueryRepository<Skill, string>>();
            var queryExerciseMock = new Mock<IQueryRepository<Exercise, string>>();
            var queryQuestionMock = new Mock<IQueryRepository<Question, string>>();
            var queryPositionSkillMock = new Mock<IQueryRepository<PositionSkill, string>>();

            queryPositionSkillMock
                .Setup(method => method.FindById(It.IsAny<string>()))
                .Returns(Task.FromResult<PositionSkill>(null));

            var controllerUnderTest = new QueryInterviewController(querySkillMock.Object,
                                                                   queryExerciseMock.Object,
                                                                   queryQuestionMock.Object,
                                                                   queryPositionSkillMock.Object);

            // Act
            var actionResult = controllerUnderTest.Get(inputPositionSkillId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
            queryPositionSkillMock.Verify(method => method.FindById(It.IsAny<string>()), Times.Once);
            querySkillMock.Verify(method => method.FindBy(It.IsAny<Expression<Func<Skill, bool>>>()), Times.Never);
            queryExerciseMock.Verify(method => method.FindBy(It.IsAny<Expression<Func<Exercise, bool>>>()), Times.Never);
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
                new Skill { SkillId = 1610, CompetencyId = positionToTest.CompetencyId, LevelId = positionToTest.LevelId, DomainId = positionToTest.DomainId, Name = "OOP Theory" },
                new Skill { SkillId = 56, CompetencyId = positionToTest.CompetencyId, LevelId = positionToTest.LevelId, DomainId = positionToTest.DomainId, Name = "Code Reviews" },
                new Skill { SkillId = 1779, CompetencyId = positionToTest.CompetencyId, LevelId = positionToTest.LevelId, DomainId = positionToTest.DomainId, Name = "Agile Teams" },
                new Skill { SkillId = 1899, CompetencyId = positionToTest.CompetencyId, LevelId = positionToTest.LevelId, DomainId = positionToTest.DomainId, Name = "Cloud Based Applications" }
            };

            var savedQuestions = new List<Question>
            {
                new Question { EntityId = "FB8BA409-D56D-4E92-AE15-2D25B757F3AA", SkillId = 1610, Text = "What's OOP purpose?." },
                new Question { EntityId = "8A7359B0-AA5D-406B-8124-1100AAF48C0A", SkillId = 1610, Text = "What are the OOP foundamentals?." },
                new Question { EntityId = "45092883-D65B-4004-B87F-8A00A19CCF7A", SkillId = 56, Text = "Can you describe the code review phase?." },
                new Question { EntityId = "EFFB19C6-7735-425C-B810-4D022DEFA600", SkillId = 1779, Text = "Can you describe all scrum's ceremonies?." },
                new Question { EntityId = "051E2FC1-56A1-4EFE-8375-D68FE1DEAFF3", SkillId = 1779, Text = "What's the role of the product owner?." },
                new Question { EntityId = "9DFCA635-C1E5-4EA8-89B2-5B41A10FD8E7", SkillId = 1779, Text = "Is scrum a philosophy or a framework?." },
            };

            var savedExercises = new List<Exercise>
            {
                new Exercise { EntityId = "DB909BFC-3030-4C0E-9B47-B13D3711336E", SkillId = 1610, Title = "OOP - Inheritance", Text = "Having a parent class named 'Father', write the code of a child class named 'Child' that descends from it.", ProposedSolution = "class Child : Parent { ... }" },
                new Exercise { EntityId = "F542C661-932F-4E54-A44F-74DD74DA2511", SkillId = 56, Title = "Fixing a code review", Text = "Suppose you're intended to write a hash function using SHA-1 algorithm, please, ask your partner to make a code review of it.", ProposedSolution = "public byte*[] GetHashValue(char* entrance){ ... }" },
                new Exercise { EntityId = "398086B0-1BE8-40AA-B18A-F6AE7D6B424D", SkillId = 1899, Title = "Writing a microservice", Text = "Using Azure Functions or AWS Lambda, propose a code that sums two digits and return its square root.", ProposedSolution = "public double GetSquareRootOfTheSum(int x, int y) { ... }" }
            };

            var querySkillMock = new Mock<IQueryRepository<Skill, string>>();

            querySkillMock
                .Setup(method => method.FindBy(It.IsAny<Expression<Func<Skill, bool>>>()))
                .ReturnsAsync((Expression<Func<Skill, bool>> predicate) => savedSkills.Where(predicate.Compile()));

            var queryExerciseMock = new Mock<IQueryRepository<Exercise, string>>();

            queryExerciseMock
               .Setup(method => method.FindBy(It.IsAny<Expression<Func<Exercise, bool>>>()))
               .ReturnsAsync((Expression<Func<Exercise, bool>> predicate) => savedExercises.Where(predicate.Compile()));

            var queryQuestionMock = new Mock<IQueryRepository<Question, string>>();

            queryQuestionMock
               .Setup(method => method.FindBy(It.IsAny<Expression<Func<Question, bool>>>()))
               .ReturnsAsync((Expression<Func<Question, bool>> predicate) => savedQuestions.Where(predicate.Compile()));

            var queryPositionSkillMock = new Mock<IQueryRepository<PositionSkill, string>>();

            queryPositionSkillMock
                .Setup(method => method.FindById(positionSkillIdToTest))
                .ReturnsAsync(savedPositionSkill);

            var controllerUnderTest = new QueryInterviewController(querySkillMock.Object,
                                                                   queryExerciseMock.Object,
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
            Assert.That((actionResult as OkNegotiatedContentResult<InterviewViewModel>).Content.Exercises, Is.Not.Empty);
            Assert.That((actionResult as OkNegotiatedContentResult<InterviewViewModel>).Content.Exercises.Count(), Is.EqualTo(3));
        }
    }
}