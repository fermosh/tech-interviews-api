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
    public class LevelControllerTests 
    {
        private readonly Mock<IQueryRepository<Level, string>> repositoryMock;

        #region Constructor

        public LevelControllerTests()
        {
            this.repositoryMock = new Mock<IQueryRepository<Level, string>>();
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
                    new List<Level>
                        {
                            new Level() { Id = Resources.TestLevelId.ToString(), Name = Resources.TestLevelName , CompetencyId = Resources.TestCompetencyId , Description = string.Empty}
                        });
            var levelController = new LevelController(this.repositoryMock.Object);
            var response = await levelController.GetAllLevels();
            Assert.NotNull(response);
            Assert.True(response.Any());
        }

        #endregion Get
    }
}