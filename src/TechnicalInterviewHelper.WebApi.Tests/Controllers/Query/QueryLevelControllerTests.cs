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

            var queryLevelCatalogMock = new Mock<ILevelQueryRepository>();  //new Mock<IQueryRepository<LevelCatalog, string>>();

            queryLevelCatalogMock
                .Setup(method => method.FindOnInternalCollection(It.IsAny<Expression<Func<Level, bool>>>()))
                .ReturnsAsync(new List<Level>());

            var controllerUnderTest = new QueryLevelController(queryLevelCatalogMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetAll(whateverCompetencyId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            queryLevelCatalogMock.Verify(method => method.FindOnInternalCollection(It.IsAny<Expression<Func<Level, bool>>>()), Times.Once);
            Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void WhenReceiveValidCompetencyIdAndThereAreCompetencies_ReturnsOkStatusCodeAndAListOfCompetencies()
        {
            // Arrange
            int validCompetencyId = 1001;

            var savedLevels = new List<Level>
            {
                new Level { LevelId = 10, CompetencyId = 1001, Name = "Level1", Description = "" },
                new Level { LevelId = 2,  CompetencyId = 1001, Name = "Level2", Description = "" },
                new Level { LevelId = 22, CompetencyId = 1001, Name = "Level3", Description = "" },
                new Level { LevelId = 25, CompetencyId = 1522, Name = "Level1", Description = "" },
                new Level { LevelId = 89, CompetencyId = 1788, Name = "Level1", Description = "" }
            };

            var savedLevelCatalog = new List<LevelCatalog>
            {
                new LevelCatalog
                {
                    Id = "8D4ED75F-1E40-4A43-918C-16753B0AA85C",
                    Levels = savedLevels
                }
            };

            var queryLevelCatalogMock = new Mock<ILevelQueryRepository>();

            queryLevelCatalogMock
                .Setup(method => method.FindOnInternalCollection(It.IsAny<Expression<Func<Level, bool>>>()))
                .ReturnsAsync((Expression<Func<Level, bool>> predicate) => savedLevels.Where(predicate.Compile()));

            var controllerUnderTest = new QueryLevelController(queryLevelCatalogMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetAll(validCompetencyId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            queryLevelCatalogMock.Verify(method => method.FindOnInternalCollection(It.IsAny<Expression<Func<Level, bool>>>()), Times.Once);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<List<LevelViewModel>>>());
            Assert.That((actionResult as OkNegotiatedContentResult<List<LevelViewModel>>).Content.Count(), Is.EqualTo(3));
            Assert.That((actionResult as OkNegotiatedContentResult<List<LevelViewModel>>).Content.First().LevelId, Is.EqualTo(savedLevels[0].LevelId));
            Assert.That((actionResult as OkNegotiatedContentResult<List<LevelViewModel>>).Content.First().CompetencyId, Is.EqualTo(savedLevels[0].CompetencyId));
            Assert.That((actionResult as OkNegotiatedContentResult<List<LevelViewModel>>).Content.First().Name, Is.EqualTo(savedLevels[0].Name));
            Assert.That((actionResult as OkNegotiatedContentResult<List<LevelViewModel>>).Content.First().Description, Is.EqualTo(savedLevels[0].Description));
        }
    }
}