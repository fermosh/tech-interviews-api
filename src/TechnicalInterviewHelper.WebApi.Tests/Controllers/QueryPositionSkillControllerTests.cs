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
    using WebApi.Controllers;

    [TestFixture]
    public class QueryPositionSkillControllerTests
    {
        [TestCase(1, 1, 1)]
        [TestCase(1, 5, 10)]
        public void WhenQueryForPositionSkills_ReturnsExpectedPositionObjectReference(int competencyId, int levelId, int domainId)
        {
            // Arrange
            var positionInput = new PositionInputModel
            {
                CompetencyId = competencyId,
                DomainId = domainId,
                LevelId = levelId
            };

            var dataCollection = new List<Skill>();

            var commandRepositoryMock = new Mock<IQueryRepository<Skill, string>>();

            commandRepositoryMock
                .Setup(m => m.FindBy(It.IsAny<Expression<Func<Skill, bool>>>()))
                .ReturnsAsync((Expression<Func<Skill, bool>> predicate) => { return dataCollection.Where(predicate.Compile()); });

            var queryPositionSkillController = new QueryPositionSkillController(commandRepositoryMock.Object);

            // Act
            var list = queryPositionSkillController.Get(positionInput).Result;

            // Assert
            Assert.That(list, Is.Not.Null);
            Assert.That(list, Is.TypeOf<OkNegotiatedContentResult<PositionSkillViewModel>>());
            Assert.That((list as OkNegotiatedContentResult<PositionSkillViewModel>).Content.Position, Is.SameAs(positionInput)); 
        }

        [TestCase(10, 1, 1)]
        [TestCase(5, 1, 10)]
        public void WhenQueryForSkillsOfAnInvalidPosition_ReturnsAnEmptyListOfSkills(int competencyId, int levelId, int domainId)
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

            var commandRepositoryMock = new Mock<IQueryRepository<Skill, string>>();

            commandRepositoryMock
                .Setup(m => m.FindBy(It.IsAny<Expression<Func<Skill, bool>>>()))
                .ReturnsAsync((Expression<Func<Skill, bool>> predicate) => { return dataCollection.Where(predicate.Compile()); });

            var queryPositionSkillController = new QueryPositionSkillController(commandRepositoryMock.Object);

            // Act
            var list = queryPositionSkillController.Get(positionInput).Result;

            // Assert
            Assert.That(list, Is.Not.Null);
            Assert.That((list as OkNegotiatedContentResult<PositionSkillViewModel>).Content.Skills, Is.Empty);
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

            var commandRepositoryMock = new Mock<IQueryRepository<Skill, string>>();

            commandRepositoryMock
                .Setup(m => m.FindBy(It.IsAny<Expression<Func<Skill, bool>>>()))
                .ReturnsAsync((Expression<Func<Skill, bool>> predicate) => { return dataCollection.Where(predicate.Compile()); });

            var queryPositionSkillController = new QueryPositionSkillController(commandRepositoryMock.Object);

            // Act
            var list = queryPositionSkillController.Get(positionInput).Result;

            // Assert
            Assert.That(list, Is.Not.Null);
            Assert.That((list as OkNegotiatedContentResult<PositionSkillViewModel>).Content.Skills.Count, Is.EqualTo(expectedSkillsRowsCount));
        }

        [Ignore("This method should be testing whether the position skill values are correct, this is work in progress.")]
        public void WhenQuerySkillsOfAValidPosition_ReturnsCorrectPositionSkillValues()
        {
            // Arrange
            var positionInput = new PositionInputModel
            {
                CompetencyId = 2,
                DomainId = 2,
                LevelId = 11
            };

            var dataCollection = new List<Skill>
            {
                new Skill { Id = "3A20A752-652D-45ED-9AD8-8BACA37AC3E3", CompetencyId = 1, LevelId = 1, DomainId = 1, Name = "Design Patterns", ParentId = 1, LevelSet = 1 },
                new Skill { Id = "38SKA752-652D-45ED-9AD8-8BACA37AC2F4", CompetencyId = 1, LevelId = 1, DomainId = 1, Name = "MVC Programming", ParentId = 1, LevelSet = 1 },
                new Skill { Id = "8SKKNA72-652D-45ED-9EGH-8BACA37AC3E3", CompetencyId = 3, LevelId = 1, DomainId = 1, Name = "OOP Knowledge", ParentId = 1, LevelSet = 1 },
                new Skill { Id = "3HGS7752-652D-45ED-99TT-8BACAUH7C3E3", CompetencyId = 2, LevelId = 2, DomainId = 11, Name = "Generics and Lambda expressions", ParentId = 1, LevelSet = 1 },
                new Skill { Id = "9KJ7SJ7N-65FF-45ED-9AD8-7GVF537AC3E3", CompetencyId = 2, LevelId = 2, DomainId = 11, Name = "Test Driven Design", ParentId = 1, LevelSet = 1 },
                new Skill { Id = "83JD8992-652D-45ED-9AD8-8BACA37AC3E3", CompetencyId = 2, LevelId = 2, DomainId = 11, Name = "Behaviour Driven Design", ParentId = 1, LevelSet = 1 }
            };

            var commandRepositoryMock = new Mock<IQueryRepository<Skill, string>>();

            commandRepositoryMock
                .Setup(m => m.FindBy(It.IsAny<Expression<Func<Skill, bool>>>()))
                .ReturnsAsync((Expression<Func<Skill, bool>> predicate) => { return dataCollection.Where(predicate.Compile()); });

            var queryPositionSkillController = new QueryPositionSkillController(commandRepositoryMock.Object);

            // Act
            var list = queryPositionSkillController.Get(positionInput).Result;

            // Assert
            Assert.That(list, Is.Not.Null);
        }
    }
}