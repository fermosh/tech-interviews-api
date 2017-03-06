namespace TechnicalInterviewHelper.WebApi.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Model;
    using Moq;
    using NUnit.Framework;
    using WebApi.Controllers;    

    [TestFixture]
    public class QueryPositionSkillControllerTests
    {
        public void WhenQueryForPositionSkills_ReturnsExpectedPositionObjectReference()
        {

        }

        [TestCase(1, 1, 1, 2)]
        [TestCase(3, 1, 1, 1)]
        [TestCase(2, 2, 11, 3)]
        public void WhenQueryForTheSkillsOfValidPosition_ReturnsExpectedNumberOfPositionSkills(int competencyId, int levelId, int domainId, int expectedSkillsRowsCount)
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
            Assert.That(list.Skills.Count, Is.EqualTo(expectedSkillsRowsCount));
        }
    }
}