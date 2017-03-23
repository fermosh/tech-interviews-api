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
    public class QueryLevelControllerTests
    {
        [Test]
        public void WhenThereAreNoLevels_ReturnsNotFoundStatusCode()
        {
            // Arrange
            int whateverCompetencyId = 1001;

            var queryLevelMock = new Mock<IQueryRepository<Level, string>>();

            queryLevelMock
                .Setup(method => method.FindBy(It.IsAny<Expression<Func<Level, bool>>>()))
                .ReturnsAsync(new List<Level>());

            var controllerUnderTest = new QueryLevelController(queryLevelMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetAllLevelsOfCompetency(whateverCompetencyId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            queryLevelMock.Verify(method => method.FindBy(It.IsAny<Expression<Func<Level, bool>>>()), Times.Once);
            Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void WhenReceiveValidCompetencyIdAndThereAreCompetencies_ReturnsOkStatusCodeAndAListOfCompetencies()
        {
            // Arrange
            int validCompetencyId = 1001;

            var levels = new List<Level>
            {
                new Level { Id = "5FD42700-C9D8-46F8-97BB-7BB44B3312CA", CompetencyId = 1001, Name = "Level1" },
                new Level { Id = "506534E2-4A7A-4CA0-A0AD-31475C2B5120", CompetencyId = 1001, Name = "Level2" },
                new Level { Id = "A9B2C5C5-C7D0-403A-B1CB-61A4F077D10B", CompetencyId = 1001, Name = "Level3" },
                new Level { Id = "76E7E37F-BECB-4BE0-8880-33AF18288AE7", CompetencyId = 1522, Name = "Level1" },
                new Level { Id = "5F9478AC-9A1C-4A7D-BC22-67D762883DB9", CompetencyId = 1788, Name = "Level1" }
            };

            var queryLevelMock = new Mock<IQueryRepository<Level, string>>();

            queryLevelMock
                .Setup(method => method.FindBy(It.IsAny<Expression<Func<Level, bool>>>()))
                .ReturnsAsync((Expression<Func<Level, bool>> predicate) => levels.Where(predicate.Compile()));

            var controllerUnderTest = new QueryLevelController(queryLevelMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetAllLevelsOfCompetency(validCompetencyId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            queryLevelMock.Verify(method => method.FindBy(It.IsAny<Expression<Func<Level, bool>>>()), Times.Once);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<List<LevelViewModel>>>());
            Assert.That((actionResult as OkNegotiatedContentResult<List<LevelViewModel>>).Content.Count(), Is.EqualTo(3));
            Assert.That((actionResult as OkNegotiatedContentResult<List<LevelViewModel>>).Content.First().LevelId, Is.EqualTo(1));
            Assert.That((actionResult as OkNegotiatedContentResult<List<LevelViewModel>>).Content.First().Name, Is.EqualTo("Level1"));
        }
    }
}