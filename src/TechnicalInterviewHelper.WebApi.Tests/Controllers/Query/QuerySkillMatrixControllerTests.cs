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
    public class QuerySkillMatrixControllerTests
    {
        [TestCase(10, 1)]
        [TestCase(5, 5)]
        public void WhenQueryForSkillsOfAnInvalidPosition_ReturnsNotFoundStatusCode(int competencyId, int jobFunctionLevel)
        {
            // Arrange
            var savedSkills = new List<Skill>
            {
               new Skill {
                   RootId = null, DisplayOrder = -100500, RequiredSkillLevel = 0, UserSkillLevel = 0, LevelsSet = 0, CompetencyId = 13,
                   JobFunctionLevel = 1, Topics = new List<Topic>(), Id = 7, ParentId = null, Name = "Hard skills", IsSelectable = true },
               new Skill {
                   RootId = null, DisplayOrder = 35, RequiredSkillLevel = 0, UserSkillLevel = 0, LevelsSet = 0, CompetencyId = 13,
                   JobFunctionLevel = 1, Topics = new List<Topic>(), Id = 472, ParentId = null, Name = "Soft skills", IsSelectable = true },
               new Skill {
                   RootId = 472, DisplayOrder = 54, RequiredSkillLevel = 10, UserSkillLevel = 0, LevelsSet = 0, CompetencyId = 13,
                   JobFunctionLevel = 1, Topics = new List<Topic> { new Topic { Name = "Stress tolerance - Advanced", IsRequired = false } }, Id = 564, ParentId = 477, Name = "Stress tolerance", IsSelectable = true }
            };

            var querySkillMatrixMock = new Mock<ISkillMatrixQueryRepository>();

            querySkillMatrixMock
                .Setup(m => m.FindWithin(It.IsAny<int>(), It.IsAny<Expression<Func<Skill, bool>>>()))
                .ReturnsAsync((int aCompetencyId, Expression<Func<Skill, bool>> predicate) => { return savedSkills.Where(predicate.Compile()); });

            var controllerUnderTest = new QuerySkillMatrixController(querySkillMatrixMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetAll(competencyId, jobFunctionLevel).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<SkillMatrixViewModel>>());
            Assert.That((actionResult as OkNegotiatedContentResult<SkillMatrixViewModel>).Content, Is.Not.Null);
            Assert.That((actionResult as OkNegotiatedContentResult<SkillMatrixViewModel>).Content.CompetencyId, Is.EqualTo(competencyId));
            Assert.That((actionResult as OkNegotiatedContentResult<SkillMatrixViewModel>).Content.HasContent, Is.EqualTo(false));
        }

        [TestCase(13, 1, 3)]
        [TestCase(12, 1, 1)]
        public void WhenQueryForSkillsOfAValidPosition_ReturnsExpectedNumberOfPositionSkills(int competencyId, int jobFunctionLevel, int expectedSkillsRowsCount)
        {
            // Arrange
            var savedSkills = new List<Skill>
            {
               new Skill
               {
                   RootId = null, DisplayOrder = -100500, RequiredSkillLevel = 0, UserSkillLevel = 0, LevelsSet = 0, CompetencyId = 13,
                   JobFunctionLevel = 1, Topics = new List<Topic>(), Id = 7, ParentId = null, Name = "Hard skills", IsSelectable = true
               },
               new Skill
               {
                   RootId = null, DisplayOrder = 35, RequiredSkillLevel = 0, UserSkillLevel = 0, LevelsSet = 0, CompetencyId = 13,
                   JobFunctionLevel = 1, Topics = new List<Topic>(), Id = 472, ParentId = null, Name = "Soft skills", IsSelectable = true
               },
               new Skill
               {
                   RootId = 472, DisplayOrder = 54, RequiredSkillLevel = 10, UserSkillLevel = 0, LevelsSet = 0, CompetencyId = 13,
                   JobFunctionLevel = 1, Topics = new List<Topic> { new Topic { Name = "Stress tolerance - Advanced", IsRequired = false } }, Id = 564, ParentId = 477, Name = "Stress tolerance", IsSelectable = true
               },
               new Skill
               {
                   RootId = null, DisplayOrder = -100500, RequiredSkillLevel = 0, UserSkillLevel = 0, LevelsSet = 0, CompetencyId = 12,
                   JobFunctionLevel = 1, Topics = new List<Topic>(), Id = 7, ParentId = null, Name = "Hard skills", IsSelectable = true
               }
            };

            var querySkillMatrixMock = new Mock<ISkillMatrixQueryRepository>();

            querySkillMatrixMock
                .Setup(m => m.FindWithin(It.IsAny<int>(), It.IsAny<Expression<Func<Skill, bool>>>()))
                .ReturnsAsync((int aCompetencyId, Expression<Func<Skill, bool>> predicate) => { return savedSkills.Where(predicate.Compile()); });

            var controllerUnderTest = new QuerySkillMatrixController(querySkillMatrixMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetAll(competencyId, jobFunctionLevel).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<SkillMatrixViewModel>>());
            Assert.That((actionResult as OkNegotiatedContentResult<SkillMatrixViewModel>).Content.Skills.Count, Is.EqualTo(expectedSkillsRowsCount));
        }

        [Test]
        public void WhenQuerySkillsOfAValidPosition_ReturnsCorrectPositionSkillValues()
        {
            // Arrange
            var validCompetencyId = 13;
            var validJobFunctionLevel = 1;

            var savedSkills = new List<Skill>
            {
               new Skill
               {
                   RootId = null, DisplayOrder = -100500, RequiredSkillLevel = 0, UserSkillLevel = 0, LevelsSet = 0, CompetencyId = 13,
                   JobFunctionLevel = 1, Topics = new List<Topic>(), Id = 7, ParentId = null, Name = "Hard skills", IsSelectable = true
               },
               new Skill
               {
                   RootId = null, DisplayOrder = 35, RequiredSkillLevel = 0, UserSkillLevel = 0, LevelsSet = 0, CompetencyId = 13,
                   JobFunctionLevel = 1, Topics = new List<Topic>(), Id = 472, ParentId = null, Name = "Soft skills", IsSelectable = true
               },
               new Skill
               {
                   RootId = 472, DisplayOrder = 54, RequiredSkillLevel = 10, UserSkillLevel = 0, LevelsSet = 0, CompetencyId = 13,
                   JobFunctionLevel = 1, Topics = new List<Topic> { new Topic { Name = "Stress tolerance - Advanced", IsRequired = false } }, Id = 564, ParentId = 477, Name = "Stress tolerance", IsSelectable = true
               },
               new Skill
               {
                   RootId = null, DisplayOrder = -100500, RequiredSkillLevel = 0, UserSkillLevel = 0, LevelsSet = 0, CompetencyId = 12,
                   JobFunctionLevel = 1, Topics = new List<Topic>(), Id = 7, ParentId = null, Name = "Hard skills", IsSelectable = true
               }
            };

            var querySkillMatrixMock = new Mock<ISkillMatrixQueryRepository>();

            querySkillMatrixMock
                .Setup(m => m.FindWithin(It.IsAny<int>(), It.IsAny<Expression<Func<Skill, bool>>>()))
                .ReturnsAsync((int aCompetencyId, Expression<Func<Skill, bool>> predicate) => { return savedSkills.Where(predicate.Compile()); });

            var controllerUnderTest = new QuerySkillMatrixController(querySkillMatrixMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetAll(validCompetencyId, validJobFunctionLevel).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<SkillMatrixViewModel>>());
            Assert.That((actionResult as OkNegotiatedContentResult<SkillMatrixViewModel>).Content.Skills.Count, Is.EqualTo(3));
            // Checks the values from the root element.
            Assert.That((actionResult as OkNegotiatedContentResult<SkillMatrixViewModel>).Content.HasContent, Is.EqualTo(true));
            Assert.That((actionResult as OkNegotiatedContentResult<SkillMatrixViewModel>).Content.CompetencyId, Is.EqualTo(validCompetencyId));
            // Checks the values from the skills list.
            Assert.That((actionResult as OkNegotiatedContentResult<SkillMatrixViewModel>).Content.Skills.Last().RootId, Is.EqualTo(savedSkills[2].RootId));
            Assert.That((actionResult as OkNegotiatedContentResult<SkillMatrixViewModel>).Content.Skills.Last().DisplayOrder, Is.EqualTo(savedSkills[2].DisplayOrder));
            Assert.That((actionResult as OkNegotiatedContentResult<SkillMatrixViewModel>).Content.Skills.Last().RequiredSkillLevel, Is.EqualTo(savedSkills[2].RequiredSkillLevel));
            Assert.That((actionResult as OkNegotiatedContentResult<SkillMatrixViewModel>).Content.Skills.Last().UserSkillLevel, Is.EqualTo(savedSkills[2].UserSkillLevel));
            Assert.That((actionResult as OkNegotiatedContentResult<SkillMatrixViewModel>).Content.Skills.Last().LevelsSet, Is.EqualTo(savedSkills[2].LevelsSet));
            Assert.That((actionResult as OkNegotiatedContentResult<SkillMatrixViewModel>).Content.Skills.Last().CompetencyId, Is.EqualTo(savedSkills[2].CompetencyId));
            Assert.That((actionResult as OkNegotiatedContentResult<SkillMatrixViewModel>).Content.Skills.Last().JobFunctionLevel, Is.EqualTo(savedSkills[2].JobFunctionLevel));
            Assert.That((actionResult as OkNegotiatedContentResult<SkillMatrixViewModel>).Content.Skills.Last().Id, Is.EqualTo(savedSkills[2].Id));
            Assert.That((actionResult as OkNegotiatedContentResult<SkillMatrixViewModel>).Content.Skills.Last().ParentId, Is.EqualTo(savedSkills[2].ParentId));
            Assert.That((actionResult as OkNegotiatedContentResult<SkillMatrixViewModel>).Content.Skills.Last().Name, Is.EqualTo(savedSkills[2].Name));
            Assert.That((actionResult as OkNegotiatedContentResult<SkillMatrixViewModel>).Content.Skills.Last().IsSelectable, Is.EqualTo(savedSkills[2].IsSelectable));
            Assert.That((actionResult as OkNegotiatedContentResult<SkillMatrixViewModel>).Content.Skills.Last().Topics, Is.Not.Null);
            Assert.That((actionResult as OkNegotiatedContentResult<SkillMatrixViewModel>).Content.Skills.Last().Topics[0].Name, Is.EqualTo(savedSkills[2].Topics[0].Name));
            Assert.That((actionResult as OkNegotiatedContentResult<SkillMatrixViewModel>).Content.Skills.Last().Topics[0].IsRequired, Is.EqualTo(savedSkills[2].Topics[0].IsRequired));
        }
    }
}