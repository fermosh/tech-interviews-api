namespace TechnicalInterviewHelper.WebApi.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Model;
    using Moq;
    using NUnit.Framework;
    using WebApi.Controllers;
    using TechnicalInterviewHelper.Tests.Common;

    /// <summary>
    /// The competency controller tests.
    /// </summary>
    [TestFixture]
    public class CompetencyControllerTests {
        private readonly Mock< IQueryRepository<Competency, string> > repositoryMock;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompetencyControllerTests"/> class.
        /// </summary>
        public CompetencyControllerTests()
        {
            this.repositoryMock = new Mock<IQueryRepository<Competency, string>>();
        }

        /// <summary>
        /// The init.
        /// </summary>
        [SetUp]
        public void Init()
        {
        }

        #region Constructor

        /// <summary>
        /// The competency controller constructor.
        /// </summary>
        [Test]
        public void CompetencyControllerConstructor()
        {
            var competencyController = new CompetencyController(this.repositoryMock.Object);
            Assert.IsNotNull(competencyController);
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
                    new List<Competency>
                        {
                            new Competency { Id = Resources.TestCompetencyId.ToString(), Name = Resources.TestCompetencyName }
                        });
            var competencyController = new CompetencyController(this.repositoryMock.Object);
            var response = await competencyController.GetAll();
            Assert.NotNull(response);
            Assert.True(response.Any());
        }

        #endregion Get
    }
}