namespace TechnicalInterviewHelper.WebApi.Tests.Controllers
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

            var queryPositionSkillController = new QueryPositionSkillController(querySkillMock.Object);

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
                new Skill { EntityId = "83JD8992-652D-45ED-9AD8-8BACA37AC3E3", CompetencyId = 2, LevelId = 2, DomainId = 11, Name = "Behaviour Driven Design", ParentId = 1, LevelSet = 1 }
            };

            var querySkillMock = new Mock<IQueryRepository<Skill, string>>();

            querySkillMock
                .Setup(m => m.FindBy(It.IsAny<Expression<Func<Skill, bool>>>()))
                .ReturnsAsync((Expression<Func<Skill, bool>> predicate) => { return dataCollection.Where(predicate.Compile()); });

            var queryPositionSkillController = new QueryPositionSkillController(querySkillMock.Object);

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
                new Skill { CompetencyId = 1, LevelId = 1, DomainId = 1, Name = "Design Patterns" },
                new Skill { CompetencyId = 1, LevelId = 1, DomainId = 1, Name = "MVC Programming" },
                new Skill { CompetencyId = 3, LevelId = 1, DomainId = 1, Name = "OOP Knowledge" },
                new Skill { CompetencyId = 2, LevelId = 2, DomainId = 11, Name = "Generics and Lambda expressions" },
                new Skill { CompetencyId = 2, LevelId = 2, DomainId = 11, Name = "Test Driven Design" },
                new Skill { CompetencyId = 2, LevelId = 2, DomainId = 11, Name = "Behaviour Driven Design" }
            };

            var querySkillMock = new Mock<IQueryRepository<Skill, string>>();

            querySkillMock
                .Setup(m => m.FindBy(It.IsAny<Expression<Func<Skill, bool>>>()))
                .ReturnsAsync((Expression<Func<Skill, bool>> predicate) => { return dataCollection.Where(predicate.Compile()); });

            var queryPositionSkillController = new QueryPositionSkillController(querySkillMock.Object);

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

            var dataCollection = new List<Skill>
            {
                new Skill { CompetencyId = 1, LevelId = 1, DomainId = 1, Name = "Design Patterns" },
                new Skill { CompetencyId = 1, LevelId = 1, DomainId = 1, Name = "MVC Programming" },
                new Skill { CompetencyId = 3, LevelId = 1, DomainId = 1, Name = "OOP Knowledge" },
                new Skill { CompetencyId = 2, LevelId = 2, DomainId = 11, Name = "Generics and Lambda expressions" },
                new Skill { CompetencyId = 2, LevelId = 2, DomainId = 11, Name = "Test Driven Design" },
                new Skill { CompetencyId = 2, LevelId = 2, DomainId = 11, Name = "Behaviour Driven Design" }
            };

            var querySkillMock = new Mock<IQueryRepository<Skill, string>>();

            querySkillMock
                .Setup(m => m.FindBy(It.IsAny<Expression<Func<Skill, bool>>>()))
                .ReturnsAsync((Expression<Func<Skill, bool>> predicate) => { return dataCollection.Where(predicate.Compile()); });

            var queryPositionSkillController = new QueryPositionSkillController(querySkillMock.Object);

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

            var dataCollection = new List<Skill>
            {
                new Skill { EntityId = "3A20A752-652D-45ED-9AD8-8BACA37AC3E3", CompetencyId = 1, LevelId = 1, DomainId = 1, Name = "Design Patterns", ParentId = 1, LevelSet = 1 },
                new Skill { EntityId = "38SKA752-652D-45ED-9AD8-8BACA37AC2F4", CompetencyId = 1, LevelId = 1, DomainId = 1, Name = "MVC Programming", ParentId = 1, LevelSet = 1 },
                new Skill { EntityId = "8SKKNA72-652D-45ED-9EGH-8BACA37AC3E3", CompetencyId = 3, LevelId = 1, DomainId = 1, Name = "OOP Knowledge", ParentId = 1, LevelSet = 1 },
                new Skill { EntityId = "3HGS7752-652D-45ED-99TT-8BACAUH7C3E3", CompetencyId = 2, LevelId = 2, DomainId = 11, Name = "Generics and Lambda expressions", ParentId = 2, LevelSet = 1 },
                new Skill { EntityId = "9KJ7SJ7N-65FF-45ED-9AD8-7GVF537AC3E3", CompetencyId = 2, LevelId = 2, DomainId = 11, Name = "Test Driven Design", ParentId = 2, LevelSet = 2 },
                new Skill { EntityId = "83JD8992-652D-45ED-9AD8-8BACA37AC3E3", CompetencyId = 2, LevelId = 2, DomainId = 11, Name = "Behaviour Driven Design", ParentId = 1, LevelSet = 1 }
            };

            var querySkillMock = new Mock<IQueryRepository<Skill, string>>();

            querySkillMock
                .Setup(m => m.FindBy(It.IsAny<Expression<Func<Skill, bool>>>()))
                .ReturnsAsync((Expression<Func<Skill, bool>> predicate) => { return dataCollection.Where(predicate.Compile()); });

            var queryPositionSkillController = new QueryPositionSkillController(querySkillMock.Object);

            // Act
            var actionResult = queryPositionSkillController.GetAll(positionInput).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<PositionSkillViewModel>>());
            Assert.That((actionResult as OkNegotiatedContentResult<PositionSkillViewModel>).Content.Skills.Count, Is.EqualTo(3));
            Assert.That((actionResult as OkNegotiatedContentResult<PositionSkillViewModel>).Content.Skills.ElementAt(0).SkillId, Is.EqualTo("3HGS7752-652D-45ED-99TT-8BACAUH7C3E3"));
            Assert.That((actionResult as OkNegotiatedContentResult<PositionSkillViewModel>).Content.Skills.ElementAt(0).ParentSkillId, Is.EqualTo(2));
            Assert.That((actionResult as OkNegotiatedContentResult<PositionSkillViewModel>).Content.Skills.ElementAt(0).Name, Is.EqualTo("Generics and Lambda expressions"));
            Assert.That((actionResult as OkNegotiatedContentResult<PositionSkillViewModel>).Content.Skills.ElementAt(0).SkillLevel, Is.EqualTo(1));
            Assert.That((actionResult as OkNegotiatedContentResult<PositionSkillViewModel>).Content.Skills.ElementAt(0).HasChildren, Is.EqualTo(false));
        }
    }
}