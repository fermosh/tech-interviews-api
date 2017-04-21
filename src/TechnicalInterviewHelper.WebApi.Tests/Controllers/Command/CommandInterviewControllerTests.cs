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
        [Test]
        public void GivenNewInputInterview_WhenInputInterviewIsValidatedAndItIsNull_ThenBadRequestStatusCodeIsReturned()
        {
            // Arrange
            InterviewInputModel interviewInputModel = null;
            var commandInterviewMock = new Mock<ICommandRepository<Interview>>();
            var controllerUnderTest = new CommandInterviewController(commandInterviewMock.Object);

            // Act
            var actionResult = controllerUnderTest.PostInterview(interviewInputModel).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        public void GivenNewInputInterview_WhenInputInterviewIsValidatedAndInputSkillsAreNull_ThenBadRequestStatusCodeIsReturned()
        {
            // Arrange
            InterviewInputModel interviewInputModel = new InterviewInputModel
            {
                CompetencyId = 13,
                JobFunctionLevel = 1,
                TemplateId = "5E54B3E9-199A-4811-B67D-F17011FBF265"
            };

            var commandInterviewMock = new Mock<ICommandRepository<Interview>>();
            var controllerUnderTest = new CommandInterviewController(commandInterviewMock.Object);

            // Act
            var actionResult = controllerUnderTest.PostInterview(interviewInputModel).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Ignore("Needs to be fixed to reflect changes applied to the model. -lapch- 04/02/2017")]
        public void WhenInputQuestionsIsNull_ReturnsBadRequestStatusCode()
        {
            /*
            // Arrange
            InterviewInputModel interviewInputModel = new InterviewInputModel
            {
                CompetencyId = 13,
                JobFunctionLevel = 1,
                TemplateId = "5E54B3E9-199A-4811-B67D-F17011FBF265",
                Skills = new List<Skill>()
            };

            var commandInterviewMock = new Mock<ICommandRepository<Interview>>();
            var controllerUnderTest = new CommandInterviewController(commandInterviewMock.Object);

            // Act
            var actionResult = controllerUnderTest.PostInterview(interviewInputModel).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestResult>());
            */
        }

        [Test]
        [Ignore("Needs to be fixed to reflect changes applied to the model. -lapch- 04/02/2017")]
        public void WhenInputExercisesIsNull_ReturnsBadRequestStatusCode()
        {
            /*
            // Arrange
            InterviewInputModel interviewInputModel = new InterviewInputModel
            {
                CompetencyId = 13,
                JobFunctionLevel = 1,
                TemplateId = "5E54B3E9-199A-4811-B67D-F17011FBF265",
                Skills = new List<Skill>(),
                Questions = new List<AnsweredQuestionInputModel>()
            };

            var commandInterviewMock = new Mock<ICommandRepository<Interview>>();
            var controllerUnderTest = new CommandInterviewController(commandInterviewMock.Object);

            // Act
            var actionResult = controllerUnderTest.PostInterview(interviewInputModel).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestResult>());
            */
        }

        [Test]
        [Ignore("Needs to be fixed to reflect changes applied to the model. -lapch- 04/02/2017")]
        public void WhenInputInterviewIsValid_ReturnsOkStatusCodeAndAllSkillsWereSaved()
        {
            /*
            // Arrange
            Interview savedInterview = null;

            var newInterviewDocumentGUID = "BB411DF9-B204-4FCF-BA90-8D5C8F52E414";

            var skillInputModels = new List<Skill>
            {
                new Skill
                {
                    RootId = 1001, DisplayOrder = 1, RequiredSkillLevel = 1, UserSkillLevel = 10, LevelsSet = 1, CompetencyId = 13,
                    JobFunctionLevel = 1, Topics = null, Id = 1001, ParentId = 1982, Name = "Documentation", IsSelectable = true
                },
                new Skill
                {
                    RootId = 1001, DisplayOrder = 1, RequiredSkillLevel = 1, UserSkillLevel = 10, LevelsSet = 1, CompetencyId = 13,
                    JobFunctionLevel = 1, Topics = null, Id = 1001, ParentId = 1982, Name = "Design Patterns", IsSelectable = true
                },
                new Skill
                {
                    RootId = 1001, DisplayOrder = 1, RequiredSkillLevel = 1, UserSkillLevel = 10, LevelsSet = 1, CompetencyId = 13,
                    JobFunctionLevel = 1, Topics = null, Id = 1001, ParentId = 1982, Name = "NET Best Practices", IsSelectable = true
                }
            };

            InterviewInputModel interviewInputModel = new InterviewInputModel
            {
                CompetencyId = 13,
                JobFunctionLevel = 1,
                TemplateId = "5E54B3E9-199A-4811-B67D-F17011FBF265",
                Skills = skillInputModels,
                Questions = new List<AnsweredQuestionInputModel>(),
                Exercises = new List<AnsweredExerciseInputModel>()
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
            var actionResult = controllerUnderTest.PostInterview(interviewInputModel).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkResult>());
            // -- Check that the save method was called once.
            commandInterviewMock.Verify(method => method.Insert(It.IsAny<Interview>()), Times.Once);
            // -- Check that the data skills were  mapped correctly in the interview object.
            Assert.That(savedInterview, Is.Not.Null);
            Assert.That(savedInterview.Id, Is.EqualTo(newInterviewDocumentGUID));
            Assert.That(savedInterview.Skills.Count(), Is.EqualTo(skillInputModels.Count()));
            Assert.That(savedInterview.Skills.First().RootId, Is.EqualTo(skillInputModels.First().RootId));
            Assert.That(savedInterview.Skills.First().DisplayOrder, Is.EqualTo(skillInputModels.First().DisplayOrder));
            Assert.That(savedInterview.Skills.First().RequiredSkillLevel, Is.EqualTo(skillInputModels.First().RequiredSkillLevel));
            Assert.That(savedInterview.Skills.First().UserSkillLevel, Is.EqualTo(skillInputModels.First().UserSkillLevel));
            Assert.That(savedInterview.Skills.First().LevelsSet, Is.EqualTo(skillInputModels.First().LevelsSet));
            Assert.That(savedInterview.Skills.First().CompetencyId, Is.EqualTo(skillInputModels.First().CompetencyId));
            Assert.That(savedInterview.Skills.First().JobFunctionLevel, Is.EqualTo(skillInputModels.First().JobFunctionLevel));
            Assert.That(savedInterview.Skills.First().Topics, Is.Null);
            Assert.That(savedInterview.Skills.First().Id, Is.EqualTo(skillInputModels.First().Id));
            Assert.That(savedInterview.Skills.First().ParentId, Is.EqualTo(skillInputModels.First().ParentId));
            Assert.That(savedInterview.Skills.First().Name, Is.EqualTo(skillInputModels.First().Name));
            Assert.That(savedInterview.Skills.First().IsSelectable, Is.EqualTo(skillInputModels.First().IsSelectable));
            */
        }

        [Test]
        [Ignore("Needs to be fixed to reflect changes applied to the model. -lapch- 04/02/2017")]
        public void WhenInputInterviewIsValid_ReturnsOkStatusCodeAndAllQuestionsWereSaved()
        {
            /*
            // Arrange
            Interview savedInterview = null;

            var newInterviewDocumentGUID = "BB411DF9-B204-4FCF-BA90-8D5C8F52E414";

            var questionInputModels = new List<AnsweredQuestionInputModel>
            {
                new AnsweredQuestionInputModel
                {
                    CompetencyId = 13, JobFunctionLevel = 1, SkillId = 1025,
                    Description = "Explain differences about interface and abstract class", Answer = "Both are contract but...", Rating = 5
                },
                new AnsweredQuestionInputModel
                {
                    CompetencyId = 13, JobFunctionLevel = 1, SkillId = 1025,
                    Description = "What's the role of the scrum master?.", Answer = "Scrum master main role is to...", Rating = 3
                },
                new AnsweredQuestionInputModel
                {
                    CompetencyId = 13, JobFunctionLevel = 1, SkillId = 1025,
                    Description = "What's the difference between list, collection and enumation?.", Answer = "List implements both interfaces but...", Rating = 4
                },
                new AnsweredQuestionInputModel
                {
                    CompetencyId = 13, JobFunctionLevel = 1, SkillId = 1025,
                    Description = "What's the difference between an ApiController and a Controller class?.", Answer = "ApiController is designed to...", Rating = 2
                },
                new AnsweredQuestionInputModel
                {
                    CompetencyId = 13, JobFunctionLevel = 1, SkillId = 1025,
                    Description = "What are the SOLID principles?.", Answer = "SOLID stands for...", Rating = 2
                }
            };

            InterviewInputModel interviewInputModel = new InterviewInputModel
            {
                CompetencyId = 13,
                JobFunctionLevel = 1,
                TemplateId = "5E54B3E9-199A-4811-B67D-F17011FBF265",
                Skills = new List<Skill>(),
                Questions = questionInputModels,
                Exercises = new List<AnsweredExerciseInputModel>()
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
            var actionResult = controllerUnderTest.PostInterview(interviewInputModel).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkResult>());
            // -- Check that the save method was called once.
            commandInterviewMock.Verify(method => method.Insert(It.IsAny<Interview>()), Times.Once);
            // -- Check that the data skills were  mapped correctly in the interview object.
            Assert.That(savedInterview, Is.Not.Null);
            Assert.That(savedInterview.Id, Is.EqualTo(newInterviewDocumentGUID));
            Assert.That(savedInterview.Questions.Count(), Is.EqualTo(questionInputModels.Count()));
            Assert.That(savedInterview.Questions.First().CompetencyId, Is.EqualTo(questionInputModels.First().CompetencyId));
            Assert.That(savedInterview.Questions.First().JobFunctionLevel, Is.EqualTo(questionInputModels.First().JobFunctionLevel));
            Assert.That(savedInterview.Questions.First().SkillId, Is.EqualTo(questionInputModels.First().SkillId));
            Assert.That(savedInterview.Questions.First().Description, Is.EqualTo(questionInputModels.First().Description));
            Assert.That(savedInterview.Questions.First().Answer, Is.EqualTo(questionInputModels.First().Answer));
            Assert.That(savedInterview.Questions.First().Rating, Is.EqualTo(questionInputModels.First().Rating));
            */
        }

        [Test]
        [Ignore("Needs to be fixed to reflect changes applied to the model. -lapch- 04/02/2017")]
        public void WhenInputInterviewIsValid_ReturnsOkStatusCodeAndAllExercisesWereSaved()
        {
            /*
            // Arrange
            Interview savedInterview = null;

            var newInterviewDocumentGUID = "BB411DF9-B204-4FCF-BA90-8D5C8F52E414";

            var exerciseInputModels = new List<AnsweredExerciseInputModel>
            {
                new AnsweredExerciseInputModel
                {
                    CompetencyId = 13, JobFunctionLevel = 1, SkillId = 1002,
                    Title = "Palindrom", Description = "Build a C# program that detects palindroms.",
                    Answer = "public void main() { ... }", Rating = 1f
                },
                new AnsweredExerciseInputModel
                {
                    CompetencyId = 13, JobFunctionLevel = 1, SkillId = 1298,
                    Title = "Balanced brackets", Description = "Build a C# program that detects whether a string has balanced brackets.",
                    Answer = "public void main() { ... }", Rating = 3.5f
                },
                new AnsweredExerciseInputModel
                {
                    CompetencyId = 13, JobFunctionLevel = 1, SkillId = 1342,
                    Title = "Balanced tree B+", Description = "Write a pseudo-code to detect when a tree B+ is well balanced.",
                    Answer = "public void main() { ... }", Rating = 0f
                }
            };

            InterviewInputModel interviewInputModel = new InterviewInputModel
            {
                CompetencyId = 13,
                JobFunctionLevel = 1,
                TemplateId = "5E54B3E9-199A-4811-B67D-F17011FBF265",
                Skills = new List<Skill>(),
                Questions = new List<AnsweredQuestionInputModel>(),
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
            var actionResult = controllerUnderTest.PostInterview(interviewInputModel).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkResult>());
            // -- Check that the save method was called once.
            commandInterviewMock.Verify(method => method.Insert(It.IsAny<Interview>()), Times.Once);
            // -- Check that the data skills were  mapped correctly in the interview object.
            Assert.That(savedInterview, Is.Not.Null);
            Assert.That(savedInterview.Id, Is.EqualTo(newInterviewDocumentGUID));
            Assert.That(savedInterview.Exercises.Count(), Is.EqualTo(exerciseInputModels.Count()));
            Assert.That(savedInterview.Exercises.First().CompetencyId, Is.EqualTo(exerciseInputModels.First().CompetencyId));
            Assert.That(savedInterview.Exercises.First().JobFunctionLevel, Is.EqualTo(exerciseInputModels.First().JobFunctionLevel));
            Assert.That(savedInterview.Exercises.First().SkillId, Is.EqualTo(exerciseInputModels.First().SkillId));
            Assert.That(savedInterview.Exercises.First().Title, Is.EqualTo(exerciseInputModels.First().Title));
            Assert.That(savedInterview.Exercises.First().Description, Is.EqualTo(exerciseInputModels.First().Description));
            Assert.That(savedInterview.Exercises.First().Complexity, Is.EqualTo(exerciseInputModels.First().Complexity));
            Assert.That(savedInterview.Exercises.First().ProposedSolution, Is.EqualTo(exerciseInputModels.First().ProposedSolution));
            Assert.That(savedInterview.Exercises.First().Answer, Is.EqualTo(exerciseInputModels.First().Answer));
            Assert.That(savedInterview.Exercises.First().Rating, Is.EqualTo(exerciseInputModels.First().Rating));
            */
        }

        [Test]
        [Ignore("Needs to be fixed to reflect changes applied to the model. -lapch- 04/02/2017")]
        public void WhenExceptionIsThrownAtSavingTime_ReturnsInternalServerErrorStatusCodeWithExceptionInformation()
        {
            /*
            // Arrange
            InterviewInputModel interviewInputModel = new InterviewInputModel
            {
                CompetencyId = 13,
                JobFunctionLevel = 1,
                TemplateId = "5E54B3E9-199A-4811-B67D-F17011FBF265",
                Skills = new List<Skill>(),
                Questions = new List<AnsweredQuestionInputModel>(),
                Exercises = new List<AnsweredExerciseInputModel>()
            };

            var commandInterviewMock = new Mock<ICommandRepository<Interview>>();

            commandInterviewMock
                .Setup(method => method.Insert(It.IsAny<Interview>()))
                .ThrowsAsync(new Exception("DocumentDb was not properly initialized."));

            var controllerUnderTest = new CommandInterviewController(commandInterviewMock.Object);

            // Act
            var actionResult = controllerUnderTest.PostInterview(interviewInputModel).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<ExceptionResult>());
            */
        }
    }
}