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
    public class QueryDomainControllerTests
    {
        [Test]
        public void WhenThereAreNotDomains_ReturnsNotFoundStatusCode()
        {
            // Arrange
            int whateverCompetencyId = 1001;
            int whateverLevelId = 2001;

            var queryDomainMock = new Mock<IDomainQueryRepository>();

            queryDomainMock
                .Setup(method => method.FindWithin(It.IsAny<Expression<Func<Domain, bool>>>()))
                .ReturnsAsync(new List<Domain>());

            var controllerUnderTest = new QueryDomainController(queryDomainMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetAll(whateverCompetencyId, whateverLevelId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            queryDomainMock.Verify(method => method.FindWithin(It.IsAny<Expression<Func<Domain, bool>>>()), Times.Once);
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
                new Domain { CompetencyId = 1001, LevelId = 2001, DomainId = 1, Name = "FrontEnd Desktop" },
                new Domain { CompetencyId = 1001, LevelId = 2001, DomainId = 1, Name = "FrontEnd Web" },
                new Domain { CompetencyId = 1001, LevelId = 2002, DomainId = 18, Name = "BackEnd Desktop" },
                new Domain { CompetencyId = 1001, LevelId = 2002, DomainId = 18, Name = "BackEnd Web" },
                new Domain { CompetencyId = 1001, LevelId = 2003, DomainId = 10, Name = "Azure" }
            };

            var queryDomainMock = new Mock<IDomainQueryRepository>();

            queryDomainMock
                .Setup(method => method.FindWithin(It.IsAny<Expression<Func<Domain, bool>>>()))
                .ReturnsAsync((Expression<Func<Domain, bool>> predicate) => domains.Where(predicate.Compile()));

            var controllerUnderTest = new QueryDomainController(queryDomainMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetAll(competencyId, levelId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            queryDomainMock.Verify(method => method.FindWithin(It.IsAny<Expression<Func<Domain, bool>>>()), Times.Once);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<List<DomainViewModel>>>());
            Assert.That((actionResult as OkNegotiatedContentResult<List<DomainViewModel>>).Content.Count(), Is.EqualTo(2));
            Assert.That((actionResult as OkNegotiatedContentResult<List<DomainViewModel>>).Content.First().CompetencyId, Is.EqualTo(1001));
            Assert.That((actionResult as OkNegotiatedContentResult<List<DomainViewModel>>).Content.First().LevelId, Is.EqualTo(2001));
            Assert.That((actionResult as OkNegotiatedContentResult<List<DomainViewModel>>).Content.First().DomainId, Is.EqualTo(1));
            Assert.That((actionResult as OkNegotiatedContentResult<List<DomainViewModel>>).Content.First().Name, Is.EqualTo("FrontEnd Desktop"));
        }
    }
}