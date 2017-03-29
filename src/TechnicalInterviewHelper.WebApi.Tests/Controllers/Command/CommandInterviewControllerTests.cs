namespace TechnicalInterviewHelper.WebApi.Tests.Controllers.Command
{
    using Model;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.Results;
    using TechnicalInterviewHelper.Model;
    using WebApi.Controllers;

    [TestFixture]
    public class CommandInterviewControllerTests
    {
        /*
        [Test]
        public void WhenInputInterviewIsNull_ReturnsBadRequestStatusCode()
        {
            // Arrange
            InterviewInputModel interviewInputModel = null;
            var commandInterviewMock = new Mock<ICommandRepository<Interview>>();
            var controllerUnderTest = new CommandInterviewController(commandInterviewMock.Object);

            // Act
            var actionResult = controllerUnderTest.SaveInterview(interviewInputModel).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        public void WhenInputSkillIsNull_ReturnsBadRequestStatusCode()
        {
            // Arrange
            InterviewInputModel interviewInputModel = new InterviewInputModel
            {
                PositionId = 1001
            };

            var commandInterviewMock = new Mock<ICommandRepository<Interview>>();
            var controllerUnderTest = new CommandInterviewController(commandInterviewMock.Object);

            // Act
            var actionResult = controllerUnderTest.SaveInterview(interviewInputModel).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        public void WhenInputQuestionsIsNull_ReturnsBadRequestStatusCode()
        {
            // Arrange
            InterviewInputModel interviewInputModel = new InterviewInputModel
            {
                PositionId = 1001,
                Skills = new List<SkillInterviewInputModel>()
            };

            var commandInterviewMock = new Mock<ICommandRepository<Interview>>();
            var controllerUnderTest = new CommandInterviewController(commandInterviewMock.Object);

            // Act
            var actionResult = controllerUnderTest.SaveInterview(interviewInputModel).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        public void WhenInputExercisesIsNull_ReturnsBadRequestStatusCode()
        {
            // Arrange
            InterviewInputModel interviewInputModel = new InterviewInputModel
            {
                PositionId = 1001,
                Skills = new List<SkillInterviewInputModel>(),
                Questions = new List<QuestionInterviewInputModel>()
            };

            var commandInterviewMock = new Mock<ICommandRepository<Interview>>();
            var controllerUnderTest = new CommandInterviewController(commandInterviewMock.Object);

            // Act
            var actionResult = controllerUnderTest.SaveInterview(interviewInputModel).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        public void WhenInputInterviewIsValid_ReturnsOkStatusCodeAndAllSkillsWereSaved()
        {
            // Arrange
            Interview savedInterview = null;

            var newInterviewDocumentGUID = "BB411DF9-B204-4FCF-BA90-8D5C8F52E414";

            var skillInputModels = new List<SkillInterviewInputModel>
            {
                new SkillInterviewInputModel { SkillId = 1001, Description = "Documentation" },
                new SkillInterviewInputModel { SkillId = 2031, Description = "Design Patterns" },
                new SkillInterviewInputModel { SkillId = 5052, Description = "NET Best Practices" }
            };

            InterviewInputModel interviewInputModel = new InterviewInputModel
            {
                PositionId = 1001,
                Skills = skillInputModels,
                Questions = new List<QuestionInterviewInputModel>(),
                Exercises = new List<ExerciseInterviewInputModel>()
            };

            var commandInterviewMock = new Mock<ICommandRepository<Interview>>();

            commandInterviewMock
                .Setup(method => method.Insert(It.IsAny<Interview>()))
                .ReturnsAsync((Interview interview) =>
                {
                    interview.Id = newInterviewDocumentGUID;
                    savedInterview = interview;
                    return interview;
                });

            var controllerUnderTest = new CommandInterviewController(commandInterviewMock.Object);

            // Act
            var actionResult = controllerUnderTest.SaveInterview(interviewInputModel).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<string>>());
            Assert.That((actionResult as OkNegotiatedContentResult<string>).Content, Is.EqualTo("interview.saved"));

            // -- Check that the save method was called once.
            commandInterviewMock.Verify(method => method.Insert(It.IsAny<Interview>()), Times.Once);

            // -- Check that the data skills were  mapped correctly in the interview object.
            Assert.That(savedInterview, Is.Not.Null);
            Assert.That(savedInterview.Id, Is.EqualTo(newInterviewDocumentGUID));
            Assert.That(savedInterview.Skills, Is.Not.Empty);
            Assert.That(savedInterview.Skills.First().SkillId, Is.EqualTo(skillInputModels.First().SkillId));
            Assert.That(savedInterview.Skills.First().Description, Is.EqualTo(skillInputModels.First().Description));
            Assert.That(savedInterview.Skills.First().Id, Is.Null);
            Assert.That(savedInterview.Skills.First().Topics, Is.Null);            
        }

        [Test]
        public void WhenInputInterviewIsValid_ReturnsOkStatusCodeAndAllQuestionsWereSaved()
        {
            // Arrange
            Interview savedInterview = null;

            var newInterviewDocumentGUID = "BB411DF9-B204-4FCF-BA90-8D5C8F52E414";

            var questionInputModels = new List<QuestionInterviewInputModel>
            {
                new QuestionInterviewInputModel { QuestionId = "50EF5673-7FE9-4D6F-87C1-726335B7EA9C", Description = "Explain differences about interface and abstract class", CapturedAnswer = "Both are contract but...", CapturedRating = 5 },
                new QuestionInterviewInputModel { QuestionId = "196FE11E-AE1E-442D-AC0F-C1B386F69DFE", Description = "What's the role of the scrum master?.", CapturedAnswer = "Scrum master main role is to...", CapturedRating = 3 },
                new QuestionInterviewInputModel { QuestionId = "E3C01446-FAE5-4DBE-9497-941A7C270D4B", Description = "What's the difference between list, collection and enumation?.", CapturedAnswer = "List implements both interfaces but...", CapturedRating = 4 },
                new QuestionInterviewInputModel { QuestionId = "38F79003-DADB-4439-A921-1CD7E3EA0B86", Description = "What's the difference between an ApiController and a Controller class?.", CapturedAnswer = "ApiController is designed to...", CapturedRating = 2 },
                new QuestionInterviewInputModel { QuestionId = "EBA12B5C-D374-422B-BA5B-90B4CA571952", Description = "What are the SOLID principles?.", CapturedAnswer = "SOLID stands for...", CapturedRating = 2 }
            };

            InterviewInputModel interviewInputModel = new InterviewInputModel
            {
                PositionId = 1001,
                Skills = new List<SkillInterviewInputModel>(),
                Questions = questionInputModels,
                Exercises = new List<ExerciseInterviewInputModel>()
            };

            var commandInterviewMock = new Mock<ICommandRepository<Interview>>();

            commandInterviewMock
                .Setup(method => method.Insert(It.IsAny<Interview>()))
                .ReturnsAsync((Interview interview) =>
                {
                    interview.Id = newInterviewDocumentGUID;
                    savedInterview = interview;
                    return interview;
                });

            var controllerUnderTest = new CommandInterviewController(commandInterviewMock.Object);

            // Act
            var actionResult = controllerUnderTest.SaveInterview(interviewInputModel).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<string>>());
            Assert.That((actionResult as OkNegotiatedContentResult<string>).Content, Is.EqualTo("interview.saved"));

            // -- Check that the save method was called once.
            commandInterviewMock.Verify(method => method.Insert(It.IsAny<Interview>()), Times.Once);

            // -- Check that the data skills were  mapped correctly in the interview object.
            Assert.That(savedInterview, Is.Not.Null);
            Assert.That(savedInterview.Id, Is.EqualTo(newInterviewDocumentGUID));
            Assert.That(savedInterview.Questions, Is.Not.Empty);
            Assert.That(savedInterview.Questions.First().Id, Is.EqualTo(questionInputModels.First().QuestionId));
            Assert.That(savedInterview.Questions.First().Description, Is.EqualTo(questionInputModels.First().Description));
            Assert.That(savedInterview.Questions.First().CapturedAnswer, Is.EqualTo(questionInputModels.First().CapturedAnswer));
            Assert.That(savedInterview.Questions.First().CapturedRating, Is.EqualTo(questionInputModels.First().CapturedRating));
            Assert.That(savedInterview.Questions.First().SkillId, Is.EqualTo(0));
        }

        [Test]
        public void WhenInputInterviewIsValid_ReturnsOkStatusCodeAndAllExercisesWereSaved()
        {
            // Arrange
            Interview savedInterview = null;

            var newInterviewDocumentGUID = "BB411DF9-B204-4FCF-BA90-8D5C8F52E414";

            var exerciseInputModels = new List<ExerciseInterviewInputModel>
            {
                new ExerciseInterviewInputModel { ExerciseId = "50EF5673-7FE9-4D6F-87C1-726335B7EA9C", Title = "Palindrom", Description = "Build a C# program that detects palindroms.", CapturedSolution = "public void main() { ... }", CapturedRating = 1f },
                new ExerciseInterviewInputModel { ExerciseId = "E3C01446-FAE5-4DBE-9497-941A7C270D4B", Title = "Balanced brackets", Description = "Build a C# program that detects whether a string has balanced brackets.", CapturedSolution = "public void main() { ... }", CapturedRating = 3.5f },
                new ExerciseInterviewInputModel { ExerciseId = "EBA12B5C-D374-422B-BA5B-90B4CA571952", Title = "Balanced tree B+", Description = "Write a pseudo-code to detect when a tree B+ is well balanced.", CapturedSolution = "public void main() { ... }", CapturedRating = 0f }
            };

            InterviewInputModel interviewInputModel = new InterviewInputModel
            {
                PositionId = 1001,
                Skills = new List<SkillInterviewInputModel>(),
                Questions = new List<QuestionInterviewInputModel>(),
                Exercises = exerciseInputModels
            };

            var commandInterviewMock = new Mock<ICommandRepository<Interview>>();

            commandInterviewMock
                .Setup(method => method.Insert(It.IsAny<Interview>()))
                .ReturnsAsync((Interview interview) =>
                {
                    interview.Id = newInterviewDocumentGUID;
                    savedInterview = interview;
                    return interview;
                });

            var controllerUnderTest = new CommandInterviewController(commandInterviewMock.Object);

            // Act
            var actionResult = controllerUnderTest.SaveInterview(interviewInputModel).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<string>>());
            Assert.That((actionResult as OkNegotiatedContentResult<string>).Content, Is.EqualTo("interview.saved"));

            // -- Check that the save method was called once.
            commandInterviewMock.Verify(method => method.Insert(It.IsAny<Interview>()), Times.Once);

            // -- Check that the data skills were  mapped correctly in the interview object.
            Assert.That(savedInterview, Is.Not.Null);
            Assert.That(savedInterview.Id, Is.EqualTo(newInterviewDocumentGUID));
            Assert.That(savedInterview.Exercises, Is.Not.Empty);
            Assert.That(savedInterview.Exercises.First().Id, Is.EqualTo(exerciseInputModels.First().ExerciseId));
            Assert.That(savedInterview.Exercises.First().Title, Is.EqualTo(exerciseInputModels.First().Title));
            Assert.That(savedInterview.Exercises.First().Description, Is.EqualTo(exerciseInputModels.First().Description));
            Assert.That(savedInterview.Exercises.First().CapturedSolution, Is.EqualTo(exerciseInputModels.First().CapturedSolution));
            Assert.That(savedInterview.Exercises.First().CapturedRating, Is.EqualTo(exerciseInputModels.First().CapturedRating));
            Assert.That(savedInterview.Exercises.First().SkillId, Is.EqualTo(0));
            Assert.That(savedInterview.Exercises.First().Complexity, Is.Null);
            Assert.That(savedInterview.Exercises.First().ProposedSolution, Is.Null);
        }

        [Test]
        public void WhenExceptionIsThrownAtSavingTime_ReturnsInternalServerErrorStatusCodeWithExceptionInformation()
        {
            // Arrange
            InterviewInputModel interviewInputModel = new InterviewInputModel
            {
                PositionId = 1001,
                Skills = new List<SkillInterviewInputModel>(),
                Questions = new List<QuestionInterviewInputModel>(),
                Exercises = new List<ExerciseInterviewInputModel>()
            };

            var commandInterviewMock = new Mock<ICommandRepository<Interview>>();

            commandInterviewMock
                .Setup(method => method.Insert(It.IsAny<Interview>()))
                .ThrowsAsync(new Exception("DocumentDb was not properly initialized."));

            var controllerUnderTest = new CommandInterviewController(commandInterviewMock.Object);

            // Act
            var actionResult = controllerUnderTest.SaveInterview(interviewInputModel).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<ExceptionResult>());
        }
        */
    }
}