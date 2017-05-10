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
            Assert.That(actionResult, Is.TypeOf<BadRequestErrorMessageResult>());
            Assert.That((actionResult as BadRequestErrorMessageResult).Message, Is.EqualTo("Cannot save the interview because its reference is not valid."));
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
            Assert.That(actionResult, Is.TypeOf<BadRequestErrorMessageResult>());
            Assert.That((actionResult as BadRequestErrorMessageResult).Message, Is.EqualTo("Cannot save an interview without skills added to it."));
        }

        [Test]
        public void GivenNewInputInterview_WhenItIsValid_ThenAssignedDocumentTypeIdIsCorrectAndOkStatusCodeIsReturned()
        {
            // Arrange
            Interview savedInterview = null;

            var newInterviewDocumentGUID = "BB411DF9-B204-4FCF-BA90-8D5C8F52E414";

            var skillInputModels = new List<SkillInterviewInputModel>
            {
                new SkillInterviewInputModel
                {
                    SkillId = 1001, Description = "Documentation", Questions = new List<AnsweredQuestionInputModel>()
                },
                new SkillInterviewInputModel
                {
                    SkillId = 1001, Description = "Design Patterns", Questions = new List<AnsweredQuestionInputModel>()
                },
                new SkillInterviewInputModel
                {
                    SkillId = 1001, Description = "NET Best Practices", Questions = new List<AnsweredQuestionInputModel>()
                }
            };

            InterviewInputModel interviewInputModel = new InterviewInputModel
            {
                CompetencyId = 13,
                JobFunctionLevel = 1,
                TemplateId = "5E54B3E9-199A-4811-B67D-F17011FBF265",
                Skills = skillInputModels,
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
            Assert.That(savedInterview.DocumentTypeId, Is.EqualTo(DocumentType.Interviews));
        }

        [Test]
        public void GivenNewInputInterview_WhenItIsValidAndNoneOfItsSkillsHaveQuestions_ThenSkillsAreSavedCorrectlyAndOkStatusCodeIsReturned()
        {
            // Arrange
            Interview savedInterview = null;

            var newInterviewDocumentGUID = "BB411DF9-B204-4FCF-BA90-8D5C8F52E414";

            var skillInputModels = new List<SkillInterviewInputModel>
            {
                new SkillInterviewInputModel
                {
                    SkillId = 1001, Description = "Documentation", Questions = new List<AnsweredQuestionInputModel>()
                },
                new SkillInterviewInputModel
                {
                    SkillId = 1001, Description = "Design Patterns", Questions = new List<AnsweredQuestionInputModel>()
                },
                new SkillInterviewInputModel
                {
                    SkillId = 1001, Description = "NET Best Practices", Questions = new List<AnsweredQuestionInputModel>()
                }
            };

            InterviewInputModel interviewInputModel = new InterviewInputModel
            {
                CompetencyId = 13,
                JobFunctionLevel = 1,
                TemplateId = "5E54B3E9-199A-4811-B67D-F17011FBF265",
                Skills = skillInputModels,
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
            Assert.That(savedInterview.Skills.First().SkillId, Is.EqualTo(skillInputModels.First().SkillId));
            Assert.That(savedInterview.Skills.First().Description, Is.EqualTo(skillInputModels.First().Description));
            Assert.That(savedInterview.Skills.First().Questions, Is.Empty);
        }

        [Test]
        public void GivenNewInputInterview_WhenItIsValidAndAnyOfItsSkillsHaveQuestions_ThenSkillsAndItsQuestionsAreSavedCorrectlyAndOkStatusCodeIsReturned()
        {
            // Arrange
            Interview savedInterview = null;

            var newInterviewDocumentGUID = "BB411DF9-B204-4FCF-BA90-8D5C8F52E414";

            var skillInputModels = new List<SkillInterviewInputModel>
            {
                new SkillInterviewInputModel
                {
                    SkillId = 1001,
                    Description = "Documentation",
                    Questions = new List<AnsweredQuestionInputModel>
                    {
                        new AnsweredQuestionInputModel
                        {
                            CompetencyId = 13,
                            JobFunctionLevel = 1,
                            SkillId = 1001,
                            Description = "Explain differences about interface and abstract class",
                            Answer = "Both are contract but...",
                            Rating = 5
                        },
                        new AnsweredQuestionInputModel
                        {
                            CompetencyId = 13,
                            JobFunctionLevel = 1,
                            SkillId = 1001,
                            Description = "What's the role of the scrum master?.",
                            Answer = "Scrum master main role is to...",
                            Rating = 3
                        }
                    }
                },
                new SkillInterviewInputModel
                {
                    SkillId = 1099,
                    Description = "Design Patterns",
                    Questions = new List<AnsweredQuestionInputModel>
                    {
                        new AnsweredQuestionInputModel
                        {
                            CompetencyId = 13,
                            JobFunctionLevel = 1,
                            SkillId = 1099,
                            Description = "What's the difference between list, collection and enumation?.",
                            Answer = "List implements both interfaces but...",
                            Rating = 4
                        }
                    }
                },
                new SkillInterviewInputModel
                {
                    SkillId = 4562,
                    Description = "NET Best Practices",
                    Questions = new List<AnsweredQuestionInputModel>
                    {
                        new AnsweredQuestionInputModel
                        {
                            CompetencyId = 13,
                            JobFunctionLevel = 1,
                            SkillId = 4562,
                            Description = "What's the difference between an ApiController and a Controller class?.",
                            Answer = "ApiController is designed to...",
                            Rating = 2
                        },
                        new AnsweredQuestionInputModel
                        {
                            CompetencyId = 13,
                            JobFunctionLevel = 1,
                            SkillId = 4562,
                            Description = "What are the SOLID principles?.",
                            Answer = "SOLID stands for...",
                            Rating = 2
                        }
                    }
                }
            };

            InterviewInputModel interviewInputModel = new InterviewInputModel
            {
                CompetencyId = 13,
                JobFunctionLevel = 1,
                TemplateId = "5E54B3E9-199A-4811-B67D-F17011FBF265",
                Skills = skillInputModels,
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
            Assert.That(savedInterview.Skills.Sum(skill => skill.Questions.Count()), Is.EqualTo(skillInputModels.Sum(skill => skill.Questions.Count())));
            Assert.That(savedInterview.Skills.First().Questions.First().CompetencyId, Is.EqualTo(skillInputModels.First().Questions.First().CompetencyId));
            Assert.That(savedInterview.Skills.First().Questions.First().JobFunctionLevel, Is.EqualTo(skillInputModels.First().Questions.First().JobFunctionLevel));
            Assert.That(savedInterview.Skills.First().Questions.First().SkillId, Is.EqualTo(skillInputModels.First().Questions.First().SkillId));
            Assert.That(savedInterview.Skills.First().Questions.First().Description, Is.EqualTo(skillInputModels.First().Questions.First().Description));
            Assert.That(savedInterview.Skills.First().Questions.First().Answer, Is.EqualTo(skillInputModels.First().Questions.First().Answer));
            Assert.That(savedInterview.Skills.First().Questions.First().Rating, Is.EqualTo(skillInputModels.First().Questions.First().Rating));
        }

        [Test]
        public void GivenNewInputInterview_WhenItIsValidAndHasExercises_ThenExercisesAreSavedCorrectlyAndOkStatusCodeIsReturned()
        {
            // Arrange
            Interview savedInterview = null;

            var newInterviewDocumentGUID = "BB411DF9-B204-4FCF-BA90-8D5C8F52E414";

            var skillInputModels = new List<SkillInterviewInputModel>
            {
                new SkillInterviewInputModel
                {
                    SkillId = 1001,
                    Description = "Documentation",
                    Questions = new List<AnsweredQuestionInputModel>()
                },
                new SkillInterviewInputModel
                {
                    SkillId = 1099,
                    Description = "Design Patterns",
                    Questions = new List<AnsweredQuestionInputModel>()
                },
                new SkillInterviewInputModel
                {
                    SkillId = 4562,
                    Description = "NET Best Practices",
                    Questions = new List<AnsweredQuestionInputModel>()
                }
            };

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
                Skills = skillInputModels,
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
        }

        [Test]
        public void GivenNewInputInterview_WhenItIsValidButAnExceptionIsThrownAtSavingTime_ThenInternalServerErrorStatusCodeWithExceptionInformationIsReturned()
        {
            // Arrange
            InterviewInputModel interviewInputModel = new InterviewInputModel
            {
                CompetencyId = 13,
                JobFunctionLevel = 1,
                TemplateId = "5E54B3E9-199A-4811-B67D-F17011FBF265",
                Skills = new List<SkillInterviewInputModel>
                {
                    new SkillInterviewInputModel
                    {
                        Questions = new List<AnsweredQuestionInputModel>()
                    }
                },
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
        }
    }
}