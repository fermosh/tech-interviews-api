namespace TechnicalInterviewHelper.WebApi.Tests.Controllers.Command
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
    public class CommandTemplateControllerTests
    {
        [Test]
        public void WhenTemplateInputIsNull_ReturnsBadRequestStatusCode()
        {
            // Arrange
            TemplateInputModel templateInput = null;
            var commandRepositoryMock = new Mock<ICommandRepository<TemplateCatalog>>();
            var controllerUnderTest = new CommandTemplateController(commandRepositoryMock.Object);

            // Act
            var actionResult = controllerUnderTest.Post(templateInput).Result;

            // Asserts
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestErrorMessageResult>());
            Assert.That((actionResult as BadRequestErrorMessageResult).Message, Is.EqualTo("Request doesn't have a valid template to save."));
        }

        [Test]
        public void WhenSkillsListIsNull_ReturnsBadRequestStatusCode()
        {
            // Arrange
            var templateInput = new TemplateInputModel
            {
                CompetencyId = 13,
                JobFunctionLevel = 1
            };

            var commandRepositoryMock = new Mock<ICommandRepository<TemplateCatalog>>();
            var controllerUnderTest = new CommandTemplateController(commandRepositoryMock.Object);

            // Act
            var actionResult = controllerUnderTest.Post(templateInput).Result;

            // Asserts
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestErrorMessageResult>());
            Assert.That((actionResult as BadRequestErrorMessageResult).Message, Is.EqualTo("Request doesn't have skills, add some ones in order to save the template."));
        }

        [Test]
        public void WhenSkillsListIsEmpty_ReturnsBadRequestStatusCode()
        {
            // Arrange
            var templateInput = new TemplateInputModel
            {
                CompetencyId = 13,
                JobFunctionLevel = 1,
                Skills = new List<int>()
            };

            var commandRepositoryMock = new Mock<ICommandRepository<TemplateCatalog>>();
            var controllerUnderTest = new CommandTemplateController(commandRepositoryMock.Object);

            // Act
            var actionResult = controllerUnderTest.Post(templateInput).Result;

            // Asserts
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestErrorMessageResult>());
            Assert.That((actionResult as BadRequestErrorMessageResult).Message, Is.EqualTo("Request doesn't have skills, add some ones in order to save the template."));
        }

        [Test]
        public void WhenRecordIsSavedSuccessfully_ReturnsOkStatusCodeAndTheIdOfNewRecord()
        {
            // Arrange
            TemplateCatalog savedTemplate = null;
            var filteredSkills = new List<int> { 22, 45, 667, 1008 };
            var newIdDocument = "3A20A752-652D-45ED-9AD8-8BACA37AC3E3";

            var templateInput = new TemplateInputModel
            {
                CompetencyId = 13,
                JobFunctionLevel = 1,
                Skills = filteredSkills
            };

            var commandRepositoryMock = new Mock<ICommandRepository<TemplateCatalog>>();

            commandRepositoryMock
                .Setup(method => method.Insert(It.IsAny<TemplateCatalog>()))
                .ReturnsAsync((TemplateCatalog templateToSave) =>
                {
                    templateToSave.Id = newIdDocument;
                    savedTemplate = templateToSave;
                    return templateToSave;
                });

            var controllerUnderTest = new CommandTemplateController(commandRepositoryMock.Object);

            // Act
            var actionResult = controllerUnderTest.Post(templateInput).Result;

            // Asserts
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<string>>());
            Assert.That((actionResult as OkNegotiatedContentResult<string>).Content, Is.EqualTo(newIdDocument));
            // Save method was called only once.
            commandRepositoryMock.Verify(method => method.Insert(It.IsAny<TemplateCatalog>()), Times.Once);
            // Checks the values of saved template.
            Assert.That(savedTemplate, Is.Not.Null);
            Assert.That(savedTemplate.Id, Is.EqualTo(newIdDocument));
            Assert.That(savedTemplate.CompetencyId, Is.EqualTo(templateInput.CompetencyId));
            Assert.That(savedTemplate.JobFunctionLevel, Is.EqualTo(templateInput.JobFunctionLevel));
            Assert.That(savedTemplate.Skills, Is.SameAs(filteredSkills));
        }

        [Test]
        public void WhenAnExceptionIsThrownAtSaveTime_ReturnsInternalServerErrorStatusCode()
        {
            // Arrange
            var templateInput = new TemplateInputModel
            {
                CompetencyId = 13,
                JobFunctionLevel = 1,
                Skills = new List<int> { 22, 45, 667, 1008 }
            };

            var commandRepositoryMock = new Mock<ICommandRepository<TemplateCatalog>>();

            commandRepositoryMock
                .Setup(method => method.Insert(It.IsAny<TemplateCatalog>()))
                .Throws(new Exception());

            var controllerUnderTest = new CommandTemplateController(commandRepositoryMock.Object);

            // Act
            var actionResult = controllerUnderTest.Post(templateInput).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<InternalServerErrorResult>());
        }
    }
}