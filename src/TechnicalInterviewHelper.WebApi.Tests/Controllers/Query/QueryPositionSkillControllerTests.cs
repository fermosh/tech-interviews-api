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
    public class QueryPositionSkillControllerTests
    {
        #region Input Validations

        [Test]
        public void WhenPositionInputValueIsMissing_ReturnsBadRequestHttpStatusCode()
        {
            // Arrange
            var querySkillMock = new Mock<IQueryRepository<Skill, string>>();
            var queryPositionMock = new Mock<IQueryRepository<Position, string>>();

            var queryPositionSkillController = new QueryPositionSkillController(querySkillMock.Object, queryPositionMock.Object);

            // Act
            var actionResult = queryPositionSkillController.GetAll(null).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestErrorMessageResult>());
            Assert.That((actionResult as BadRequestErrorMessageResult).Message, Is.EqualTo("A position is required in order to get its skills."));
        }

        [TestCase(true, true, false)]
        [TestCase(true, false, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, true)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        public void WhenSomePositionInputValuesAreMissingAndNoDocumentMatchesThoseInputValues_ReturnsNotFoundMessage(bool competencyIsMissing, bool levelIsMissing, bool domainIsMissing)
        {
            // Arrange
            var positionInput = new PositionInputModel
            {
                CompetencyId = competencyIsMissing ? 0 : 2,
                LevelId = levelIsMissing ? 0 : 2,
                DomainId = domainIsMissing ? 0 : 2
            };

            var dataCollection = new List<Skill>
            {
                new Skill { Id = "83JD8992-652D-45ED-9AD8-8BACA37AC3E3", Description = "Behaviour Driven Design" }
            };

            var querySkillMock = new Mock<IQueryRepository<Skill, string>>();

            querySkillMock
                .Setup(m => m.FindBy(It.IsAny<Expression<Func<Skill, bool>>>()))
                .ReturnsAsync((Expression<Func<Skill, bool>> predicate) => { return dataCollection.Where(predicate.Compile()); });

            var queryPositionMock = new Mock<IQueryRepository<Position, string>>();

            var queryPositionSkillController = new QueryPositionSkillController(querySkillMock.Object, queryPositionMock.Object);

            // Act
            var actionResult = queryPositionSkillController.GetAll(positionInput).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
        }

        #endregion Input Validations

        [TestCase(10, 1, 1)]
        [TestCase(5, 1, 10)]
        public void WhenQueryForSkillsOfAnInvalidPosition_ReturnsNotFoundStatusCode(int competencyId, int levelId, int domainId)
        {
            // Arrange
            var positionInput = new PositionInputModel
            {
                CompetencyId = competencyId,
                DomainId = domainId,
                LevelId = levelId
            };

            var dataCollection = new List<Skill>
            {
                new Skill { Description = "Design Patterns" },
                new Skill { Description = "MVC Programming" },
                new Skill { Description = "OOP Knowledge" },
                new Skill { Description = "Generics and Lambda expressions" },
                new Skill { Description = "Test Driven Design" },
                new Skill { Description = "Behaviour Driven Design" }
            };

            var querySkillMock = new Mock<IQueryRepository<Skill, string>>();

            querySkillMock
                .Setup(m => m.FindBy(It.IsAny<Expression<Func<Skill, bool>>>()))
                .ReturnsAsync((Expression<Func<Skill, bool>> predicate) => { return dataCollection.Where(predicate.Compile()); });

            var queryPositionMock = new Mock<IQueryRepository<Position, string>>();

            var queryPositionSkillController = new QueryPositionSkillController(querySkillMock.Object, queryPositionMock.Object);

            // Act
            var actionResult = queryPositionSkillController.GetAll(positionInput).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
        }

        [TestCase(1, 1, 1, 2)]
        [TestCase(3, 1, 1, 1)]
        [TestCase(2, 2, 11, 3)]
        public void WhenQueryForSkillsOfAValidPosition_ReturnsExpectedNumberOfPositionSkills(int competencyId, int levelId, int domainId, int expectedSkillsRowsCount)
        {
            // Arrange
            var positionInput = new PositionInputModel
            {
                CompetencyId = competencyId,
                DomainId = domainId,
                LevelId = levelId
            };

            var savedSkills = new List<Skill>
            {
                new Skill { Id = "E9C96ABC-8D81-431A-B0C6-D884796710E8", SkillId = 1588, ParentSkillId = 899, PositionId = 1001, Description = "Design Patterns" },
                new Skill { Id = "68F27973-9A35-45C0-B3B0-F47C89194929", SkillId = 450,  ParentSkillId = 0,   PositionId = 13,   Description = "MVC Programming" },
                new Skill { Id = "21CEF7AB-0AE0-4D00-82F9-2695351673C4", SkillId = 654,  ParentSkillId = 0,   PositionId = 1001, Description = "OOP Knowledge" },
                new Skill { Id = "810F3B2E-7CA0-486C-8350-53BCF2166780", SkillId = 667,  ParentSkillId = 654, PositionId = 14,   Description = "Generics and Lambda expressions" },
                new Skill { Id = "9356718E-11E9-407A-8CAB-6926B9B6F532", SkillId = 899,  ParentSkillId = 0,   PositionId = 14,   Description = "Test Driven Design" },
                new Skill { Id = "F1E15940-9EF7-4E86-B10D-44F4D6D201D5", SkillId = 4752, ParentSkillId = 899, PositionId = 14,   Description = "Behaviour Driven Design" }
            };

            var savedPositions = new List<Position>
            {
                new Position { Id = "6DAAB649-15E9-44B1-8FC1-5241E09AA8A8", PositionId = 1001, CompetencyId = 1, LevelId = 1, DomainId = 1 },
                new Position { Id = "D2A796F4-484D-4196-AF12-9E7C82A74BFE", PositionId = 13,   CompetencyId = 3, LevelId = 1, DomainId = 1 },
                new Position { Id = "934887BB-9845-4A13-A8AC-C9941D0F11AD", PositionId = 14,   CompetencyId = 2, LevelId = 2, DomainId = 11 }
            };

            var querySkillMock = new Mock<IQueryRepository<Skill, string>>();

            querySkillMock
                .Setup(m => m.FindBy(It.IsAny<Expression<Func<Skill, bool>>>()))
                .ReturnsAsync((Expression<Func<Skill, bool>> predicate) => { return savedSkills.Where(predicate.Compile()); });

            var queryPositionMock = new Mock<IQueryRepository<Position, string>>();

            queryPositionMock
                .Setup(method => method.FindBy(It.IsAny<Expression<Func<Position, bool>>>()))
                .ReturnsAsync((Expression<Func<Position, bool>> predicate) => savedPositions.Where(predicate.Compile()));

            var queryPositionSkillController = new QueryPositionSkillController(querySkillMock.Object, queryPositionMock.Object);

            // Act
            var actionResult = queryPositionSkillController.GetAll(positionInput).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<PositionSkillViewModel>>());
            Assert.That((actionResult as OkNegotiatedContentResult<PositionSkillViewModel>).Content.Skills.Count, Is.EqualTo(expectedSkillsRowsCount));
        }

        [Test]
        public void WhenQuerySkillsOfAValidPosition_ReturnsCorrectPositionSkillValues()
        {
            // Arrange
            var positionInput = new PositionInputModel
            {
                CompetencyId = 2,
                LevelId = 2,
                DomainId = 11
            };

            var savedPositions = new List<Position>
            {
                new Position { Id = "6DAAB649-15E9-44B1-8FC1-5241E09AA8A8", PositionId = 1001, CompetencyId = 1, LevelId = 1, DomainId = 1 },
                new Position { Id = "D2A796F4-484D-4196-AF12-9E7C82A74BFE", PositionId = 13,   CompetencyId = 3, LevelId = 1, DomainId = 1 },
                new Position { Id = "934887BB-9845-4A13-A8AC-C9941D0F11AD", PositionId = 14,   CompetencyId = 2, LevelId = 2, DomainId = 11 }
            };

            var savedSkills = new List<Skill>
            {
                new Skill { Id = "E9C96ABC-8D81-431A-B0C6-D884796710E8", SkillId = 1588, ParentSkillId = 899, PositionId = 1001, Description = "Design Patterns" },
                new Skill { Id = "68F27973-9A35-45C0-B3B0-F47C89194929", SkillId = 450,  ParentSkillId = 0,   PositionId = 13,   Description = "MVC Programming" },
                new Skill { Id = "21CEF7AB-0AE0-4D00-82F9-2695351673C4", SkillId = 654,  ParentSkillId = 0,   PositionId = 1001, Description = "OOP Knowledge" },
                new Skill { Id = "810F3B2E-7CA0-486C-8350-53BCF2166780", SkillId = 667,  ParentSkillId = 654, PositionId = 14,   Description = "Generics and Lambda expressions" },
                new Skill { Id = "9356718E-11E9-407A-8CAB-6926B9B6F532", SkillId = 899,  ParentSkillId = 0,   PositionId = 14,   Description = "Test Driven Design" },
                new Skill { Id = "F1E15940-9EF7-4E86-B10D-44F4D6D201D5", SkillId = 4752, ParentSkillId = 899, PositionId = 14,   Description = "Behaviour Driven Design" }
            };

            var querySkillMock = new Mock<IQueryRepository<Skill, string>>();

            querySkillMock
                .Setup(m => m.FindBy(It.IsAny<Expression<Func<Skill, bool>>>()))
                .ReturnsAsync((Expression<Func<Skill, bool>> predicate) => { return savedSkills.Where(predicate.Compile()); });

            var queryPositionMock = new Mock<IQueryRepository<Position, string>>();

            queryPositionMock
                .Setup(method => method.FindBy(It.IsAny<Expression<Func<Position, bool>>>()))
                .ReturnsAsync((Expression<Func<Position, bool>> predicate) => savedPositions.Where(predicate.Compile()));

            var queryPositionSkillController = new QueryPositionSkillController(querySkillMock.Object, queryPositionMock.Object);

            // Act
            var actionResult = queryPositionSkillController.GetAll(positionInput).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<PositionSkillViewModel>>());
            Assert.That((actionResult as OkNegotiatedContentResult<PositionSkillViewModel>).Content.Skills.Count, Is.EqualTo(3));
            Assert.That((actionResult as OkNegotiatedContentResult<PositionSkillViewModel>).Content.Skills.ElementAt(0).SkillId, Is.EqualTo(667));
            Assert.That((actionResult as OkNegotiatedContentResult<PositionSkillViewModel>).Content.Skills.ElementAt(0).Name, Is.EqualTo("Generics and Lambda expressions"));
        }
    }
}