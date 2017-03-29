namespace TechnicalInterviewHelper.WebApi.Tests.Controllers.Query
{
    using Model;
    using Moq;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.Results;
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
                        new Competency { Id = 71, ParentId = 70,   Code = "SArchitect",  Name = "Solution Architect",    JobFunctions = new List<int>(),      IsSelectable = true },
                        new Competency { Id = 70, ParentId = null, Code = "SA",          Name = "Solution Architecture", JobFunctions = new List<int>(),      IsSelectable = false },
                        new Competency { Id = 82, ParentId = 79,   Code = "SAPHRCons",   Name = "SAP HR Consultant",     JobFunctions = new List<int>(),      IsSelectable = true },
                        new Competency { Id = 69, ParentId = 66,   Code = "SAP-BI-HANA", Name = "SAP HANA Consultant",   JobFunctions = new List<int>(),      IsSelectable = true },
                        new Competency { Id = 1,  ParentId = null, Code = "DotNET",      Name = ".NET",                  JobFunctions = new List<int> { 15 }, IsSelectable = false }
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
            Assert.That((actionResult as OkNegotiatedContentResult<List<CompetencyViewModel>>).Content.Last().Id, Is.EqualTo(competencies[0].Competencies.Last().Id));
            Assert.That((actionResult as OkNegotiatedContentResult<List<CompetencyViewModel>>).Content.Last().ParentId, Is.EqualTo(competencies[0].Competencies.Last().ParentId));
            Assert.That((actionResult as OkNegotiatedContentResult<List<CompetencyViewModel>>).Content.Last().Code, Is.EqualTo(competencies[0].Competencies.Last().Code));
            Assert.That((actionResult as OkNegotiatedContentResult<List<CompetencyViewModel>>).Content.Last().Name, Is.EqualTo(competencies[0].Competencies.Last().Name));
        }
    }
}