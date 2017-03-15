namespace TechnicalInterviewHelper.WebApi.Tests.Controllers
{
    using Model;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Web.Http.Results;
    using TechnicalInterviewHelper.Model;
    using WebApi.Controllers;

    [TestFixture]
    public class CommandPositionSkillControllerTests
    {
        [Test]
        public void WhenPositionFieldIsMissedWithinPositionSkillInputModel_ReturnsBadRequestStatusCode()
        {
            // Arrange
            var positionSkillInput = new PositionSkillInputModel
            {
                Position = null,
                SkillIdentifiers = null
            };

            var commandRepositoryMock = new Mock<ICommandRepository<PositionSkill>>();
            var controllerUnderTest = new CommandPositionSkillController(commandRepositoryMock.Object);

            // Act
            var actionResult = controllerUnderTest.Post(positionSkillInput).Result;

            // Asserts
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestErrorMessageResult>());
            Assert.That((actionResult as BadRequestErrorMessageResult).Message, Is.EqualTo("Request doesn't have a position to link with the skills."));
        }

        [Test]
        public void WhenTheListOfIdentifiersIsEmpty_ReturnsBadRequestStatusCode()
        {
            // Arrange
            var positionSkillInput = new PositionSkillInputModel
            {
                Position = new PositionInputModel(),
                SkillIdentifiers = new List<int>()
            };

            var commandRepositoryMock = new Mock<ICommandRepository<PositionSkill>>();
            var controllerUnderTest = new CommandPositionSkillController(commandRepositoryMock.Object);

            // Act
            var actionResult = controllerUnderTest.Post(positionSkillInput).Result;

            // Asserts
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestErrorMessageResult>());
            Assert.That((actionResult as BadRequestErrorMessageResult).Message, Is.EqualTo("Cannot save a position without skills, add at least one of them."));
        }

        [Test]
        public void WhenDocumentIsSavedSuccessfully_ReturnsOkStatusCodeAndTheIdOfNewDocument()
        {
            // Arrange
            PositionSkill newPositionSkill = null;

            var newIdDocument = "3A20A752-652D-45ED-9AD8-8BACA37AC3E3";

            var positionInput = new PositionInputModel
            {
                CompetencyId = 1,
                LevelId = 5,
                DomainId = 11
            };

            var skillIdentifiers = new List<int> { 22, 45, 667, 1008 };

            var positionSkillInput = new PositionSkillInputModel
            {
                Position = positionInput,
                SkillIdentifiers = skillIdentifiers
            };

            var commandRepositoryMock = new Mock<ICommandRepository<PositionSkill>>();

            commandRepositoryMock
                .Setup(method => method.Insert(It.IsAny<PositionSkill>()))
                .Callback<PositionSkill>((positionSkill) =>
                {
                    positionSkill.EntityId = newIdDocument;
                    newPositionSkill = positionSkill;
                })
                .ReturnsAsync((PositionSkill positionSkill) => positionSkill);

            var controllerUnderTest = new CommandPositionSkillController(commandRepositoryMock.Object);

            // Act
            var actionResult = controllerUnderTest.Post(positionSkillInput).Result;

            // Asserts
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(newPositionSkill, Is.Not.Null);

            Assert.That(newPositionSkill.EntityId, Is.EqualTo(newIdDocument));
            Assert.That(newPositionSkill.Position.CompetencyId, Is.EqualTo(positionInput.CompetencyId));
            Assert.That(newPositionSkill.Position.LevelId, Is.EqualTo(positionInput.LevelId));
            Assert.That(newPositionSkill.Position.DomainId, Is.EqualTo(positionInput.DomainId));
            Assert.That(newPositionSkill.SkillIdentifiers, Is.SameAs(skillIdentifiers));

            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<string>>());
            Assert.That((actionResult as OkNegotiatedContentResult<string>).Content, Is.EqualTo(newIdDocument));
            commandRepositoryMock.Verify(method => method.Insert(It.IsAny<PositionSkill>()), Times.Once);
        }

        [Test]
        public void WhenAnExceptionIsThrownAtSaveTime_ReturnsInternalServerErrorStatusCode()
        {
            // Arrange
            var positionSkillInput = new PositionSkillInputModel
            {
                Position = new PositionInputModel
                {
                    CompetencyId = 1,
                    LevelId = 5,
                    DomainId = 11
                },
                SkillIdentifiers = new List<int>
                {
                    1999, 2229, 2911, 9123
                }
            };

            var commandRepositoryMock = new Mock<ICommandRepository<PositionSkill>>();

            commandRepositoryMock
                .Setup(method => method.Insert(It.IsAny<PositionSkill>()))
                .Throws(new Exception());

            var controllerUnderTest = new CommandPositionSkillController(commandRepositoryMock.Object);

            // Act
            var actionResult = controllerUnderTest.Post(positionSkillInput).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<InternalServerErrorResult>());
        }
    }
}