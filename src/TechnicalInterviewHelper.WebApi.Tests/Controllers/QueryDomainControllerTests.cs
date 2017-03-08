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
    public class QueryDomainControllerTests
    {
        [Test]
        public void WhenThereAreNotDomains_ReturnsNotFoundStatusCode()
        {
            // Arrange
            int whateverCompetencyId = 1001;
            int whateverLevelId = 2001;

            var queryDomainMock = new Mock<IQueryRepository<Domain, string>>();

            queryDomainMock
                .Setup(method => method.FindBy(It.IsAny<Expression<Func<Domain, bool>>>()))
                .ReturnsAsync(new List<Domain>());

            var controllerUnderTest = new QueryDomainController(queryDomainMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetAllDomainsOfCompetencyAndLevel(whateverCompetencyId, whateverLevelId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            queryDomainMock.Verify(method => method.FindBy(It.IsAny<Expression<Func<Domain, bool>>>()), Times.Once);
            Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void WhenThereAreDomainsThatMatchInputCriteria_ReturnsOkStatusCodeAndListOfDomains()
        {
            // Arrange
            int competencyId = 1001;
            int levelId = 2001;

            var domains = new List<Domain>
            {
                new Domain { Id = "38CDE06E-DB3C-410C-872A-69AAAF0EA49A", CompetencyId = 1001, LevelId = 2001, Name = "FrontEnd Desktop" },
                new Domain { Id = "256EB7CF-0D0F-4E9F-800C-A931716BF3DD", CompetencyId = 1001, LevelId = 2001, Name = "FrontEnd Web" },
                new Domain { Id = "F2A5823F-7523-4EF3-9EF1-95812F685B12", CompetencyId = 1001, LevelId = 2002, Name = "BackEnd Desktop" },
                new Domain { Id = "731AE99A-5E20-4674-ACD8-77A70441EC12", CompetencyId = 1001, LevelId = 2002, Name = "BackEnd Web" },
                new Domain { Id = "2D5BE8E3-69D7-4F29-B27E-0EBE2100DF23", CompetencyId = 1001, LevelId = 2003, Name = "Azure" }
            };

            var queryDomainMock = new Mock<IQueryRepository<Domain, string>>();

            queryDomainMock
                .Setup(method => method.FindBy(It.IsAny<Expression<Func<Domain, bool>>>()))
                .ReturnsAsync((Expression<Func<Domain, bool>> predicate) => domains.Where(predicate.Compile()));

            var controllerUnderTest = new QueryDomainController(queryDomainMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetAllDomainsOfCompetencyAndLevel(competencyId, levelId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            queryDomainMock.Verify(method => method.FindBy(It.IsAny<Expression<Func<Domain, bool>>>()), Times.Once);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<List<DomainViewModel>>>());
            Assert.That((actionResult as OkNegotiatedContentResult<List<DomainViewModel>>).Content.Count(), Is.EqualTo(2));
            Assert.That((actionResult as OkNegotiatedContentResult<List<DomainViewModel>>).Content.First().DomainId, Is.EqualTo(1));
            Assert.That((actionResult as OkNegotiatedContentResult<List<DomainViewModel>>).Content.First().Name, Is.EqualTo("FrontEnd Desktop"));
        }
    }
}