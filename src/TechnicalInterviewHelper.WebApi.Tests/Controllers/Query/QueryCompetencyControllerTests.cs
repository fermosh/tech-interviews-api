namespace TechnicalInterviewHelper.WebApi.Tests.Controllers.Query
{    
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.Results;    
    using Model;
    using Moq;
    using NUnit.Framework;
    using TechnicalInterviewHelper.Model;
    using WebApi.Controllers;

    /// <summary>
    /// The competency controller tests.
    /// </summary>
    [TestFixture]
    public class QueryCompetencyControllerTests
    {
        [Test]
        public void WhenThereAreNoCompetencies_ReturnsNotFoundStatusCode()
        {
            // Arrange
            var competencies = new List<CompetencyCatalog>();

            var queryCompetencyCatalogMock = new Mock<IQueryRepository<CompetencyCatalog, string>>();

            queryCompetencyCatalogMock
                .Setup(method => method.GetAll())
                .ReturnsAsync(competencies);

            var controllerUnderTest = new QueryCompetencyController(queryCompetencyCatalogMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetAll().Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            queryCompetencyCatalogMock.Verify(method => method.GetAll(), Times.Once);
            Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void WhenGetAllCompetencies_ReturnsAnEnumerationWithAllAvailableCompetencyViewModels()
        {
            // Arrange
            var competencies = new List<CompetencyCatalog>
            {
                new CompetencyCatalog
                {
                    Id = "3FB6E6CC-4505-45AF-BC5F-73F45E33CC76",
                    Competencies = new List<Competency>
                    {
                        new Competency { CompentencyId = 1,  Name = "NET Architect" },
                        new Competency { CompentencyId = 10, Name = "NET Developer" },
                        new Competency { CompentencyId = 78, Name = "Azure Architect" },
                        new Competency { CompentencyId = 98, Name = "DevOp Agent" },
                        new Competency { CompentencyId = 25, Name = "Account Manager Staff" }
                    }
                }
            };

            var queryCompetencyCatalogMock = new Mock<IQueryRepository<CompetencyCatalog, string>>();

            queryCompetencyCatalogMock
                .Setup(method => method.GetAll())
                .ReturnsAsync(competencies);

            var controllerUnderTest = new QueryCompetencyController(queryCompetencyCatalogMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetAll().Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<List<CompetencyViewModel>>>());
            queryCompetencyCatalogMock.Verify(method => method.GetAll(), Times.Once);
            Assert.That((actionResult as OkNegotiatedContentResult<List<CompetencyViewModel>>).Content.Count(), Is.EqualTo(5));
            Assert.That((actionResult as OkNegotiatedContentResult<List<CompetencyViewModel>>).Content.First().CompetencyId, Is.EqualTo(competencies[0].Competencies.First().CompentencyId));
            Assert.That((actionResult as OkNegotiatedContentResult<List<CompetencyViewModel>>).Content.First().Name, Is.EqualTo(competencies[0].Competencies.First().Name));
        }
    }
}