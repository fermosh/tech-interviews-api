namespace TechnicalInterviewHelper.WebApi.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using System.Web.Http;

    using NUnit.Framework;

    using TechnicalInterviewHelper.Model;
    using Moq;
    using TechnicalInterviewHelper.Tests.Common;
    using TechnicalInterviewHelper.WebApi.Controllers;

    [TestFixture]
    public class SkillMatrixControllerTests
    {
        private readonly Mock<IQueryRepository<Competency, string>> competencyRepositoryMock;
        private readonly Mock<IQueryRepository<Level, string>> levelRepositoryMock;
        private readonly Mock<IQueryRepository<Domain, string>> domainRepositoryMock;
        private readonly Mock<IQueryRepository<Skill, string>> skillRepositoryMock;
        

        #region Constructor

        public SkillMatrixControllerTests()
        {
            this.competencyRepositoryMock = new Mock<IQueryRepository<Competency, string>>();
            this.levelRepositoryMock = new Mock<IQueryRepository<Level, string>>();
            this.domainRepositoryMock = new Mock<IQueryRepository<Domain, string>>();
            this.skillRepositoryMock = new Mock<IQueryRepository<Skill, string>>();
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
        public async Task GetSkillMatrixTest()
        {
            this.competencyRepositoryMock.Setup(
                    x => x.FindById(It.Is((string compId) => Resources.TestCompetencyId.ToString().Equals(compId))))
                .ReturnsAsync(Resources.TestCompetency);
            this.levelRepositoryMock.Setup(
                    x => x.FindById(It.Is((string levelId) => Resources.TestLevelId.ToString().Equals(levelId))))
                .ReturnsAsync(Resources.TestLevel);
            this.domainRepositoryMock.Setup(
                    x => x.FindById(It.Is((string domainId) => Resources.TestDomainId.ToString().Equals(domainId))))
                .ReturnsAsync(Resources.TestDomain);
            this.skillRepositoryMock.Setup(x => x.FindBy(It.IsAny<Expression<Func<Skill, bool>>>()))
                .ReturnsAsync(new List<Skill> { Resources.TestSkill });
            var skillMatrixController = new SkillMatrixController(
                this.competencyRepositoryMock.Object,
                this.levelRepositoryMock.Object,
                this.domainRepositoryMock.Object,
                this.skillRepositoryMock.Object);
            var response = await skillMatrixController.GetSkillMatrix(Resources.TestCompetencyId,Resources.TestLevelId,Resources.TestDomainId);
            Assert.NotNull(response);
            Assert.True(response.Skills.Any());
        }

        #endregion Get
    }
}