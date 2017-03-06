namespace TechnicalInterviewHelper.WebApi.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;

    using NUnit.Framework;

    using TechnicalInterviewHelper.Model;
    using Moq;
    using TechnicalInterviewHelper.Tests.Common;
    using TechnicalInterviewHelper.WebApi.Controllers;

    [TestFixture]
    public class DomainControllerTests 
    {
        private readonly Mock<IQueryRepository<Domain, string>> repositoryMock;

        #region Constructor

        public DomainControllerTests()
        {
            this.repositoryMock = new Mock<IQueryRepository<Domain, string>>();
        }

        #endregion Constructor

        #region Get

        /// <summary>
        /// The get all test.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Test]
        public async Task GetAllTest()
        {
            this.repositoryMock.Setup(x => x.GetAll())
                .ReturnsAsync(
                    new List<Domain>
                        {
                            new Domain() { Id = Resources.TestDomainId.ToString(), Name = Resources.TestDomainName , CompetencyId = Resources.TestCompetencyId , LevelId = Resources.TestLevelId}
                        });
            var domainController = new DomainController(this.repositoryMock.Object);
            var response = await domainController.GetAll();
            Assert.NotNull(response);
            Assert.True(response.Any());
        }

        #endregion Get
    }
}