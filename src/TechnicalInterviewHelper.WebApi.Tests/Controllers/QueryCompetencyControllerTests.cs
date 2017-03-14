namespace TechnicalInterviewHelper.WebApi.Tests.Controllers
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
            var competencies = new List<Competency>();

            var queryCompetencyMock = new Mock<IQueryRepository<Competency, string>>();

            queryCompetencyMock
                .Setup(method => method.GetAll())
                .ReturnsAsync(competencies);

            var controllerUnderTest = new QueryCompetencyController(queryCompetencyMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetAll().Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            queryCompetencyMock.Verify(method => method.GetAll(), Times.Once);
            Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void WhenGetAllCompetencies_ReturnsAnEnumerationWithAllAvailableCompetencyViewModels()
        {
            // Arrange
            var competencies = new List<Competency>
            {
                new Competency { EntityId = "7B21643E-A8B5-4FE0-B691-B45191FB2F30", Name = "NET Architect" },
                new Competency { EntityId = "883KJKDF-A8B5-4FE0-B691-B45191FB2F30", Name = "NET Developer" },
                new Competency { EntityId = "6MAMAZ8S-92KK-4FE0-B691-B45191FB2F30", Name = "Azure Architect" },
                new Competency { EntityId = "729AA93E-A8B5-92SA-0MA3-B45191FB2F30", Name = "DevOp Agent" },
                new Competency { EntityId = "7B22982K-92LL-92AA-MAR2-02KA82JAT521", Name = "Account Manager Staff" }
            };

            var queryCompetencyMock = new Mock<IQueryRepository<Competency, string>>();

            queryCompetencyMock
                .Setup(method => method.GetAll())
                .ReturnsAsync(competencies);

            var controllerUnderTest = new QueryCompetencyController(queryCompetencyMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetAll().Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<List<CompetencyViewModel>>>());
            queryCompetencyMock.Verify(method => method.GetAll(), Times.Once);
            Assert.That((actionResult as OkNegotiatedContentResult<List<CompetencyViewModel>>).Content.Count(), Is.EqualTo(5));
            Assert.That((actionResult as OkNegotiatedContentResult<List<CompetencyViewModel>>).Content.First().CompetencyId, Is.EqualTo(1));
            Assert.That((actionResult as OkNegotiatedContentResult<List<CompetencyViewModel>>).Content.First().Name, Is.EqualTo("NET Architect"));
        }
    }
}