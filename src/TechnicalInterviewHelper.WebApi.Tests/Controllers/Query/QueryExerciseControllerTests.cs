﻿namespace TechnicalInterviewHelper.WebApi.Tests.Controllers.Query
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
    public class QueryExerciseControllerTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public void WhenInputTemplateIdIsNullOrEmpty_ReturnsBadRequestStatusCode(string inputTemplateId)
        {
            // Arrange
            var queryExerciseMock = new Mock<IExerciseQueryRepository>();
            var queryPositionSkillMock = new Mock<IQueryRepository<TemplateCatalog, string>>();
            var controllerUnderTest = new QueryExerciseController(queryExerciseMock.Object, queryPositionSkillMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetAll(inputTemplateId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestErrorMessageResult>());
            Assert.That((actionResult as BadRequestErrorMessageResult).Message, Is.EqualTo("Cannot get exercises without a valid template identifier."));
        }

        [Test]
        public void WhenInputTemplateIdDoesNotExist_ReturnsNotFoundStatusCode()
        {
            // Arrange
            var inputTemplateId = "07CFB7D0-5C3C-4433-8BAE-F79945B90376";

            var queryExerciseMock = new Mock<IExerciseQueryRepository>();

            var queryPositionSkillMock = new Mock<IQueryRepository<TemplateCatalog, string>>();
            queryPositionSkillMock
                .Setup(method => method.FindById(It.IsAny<string>()))
                .ReturnsAsync((string templateId) => null);

            var controllerUnderTest = new QueryExerciseController(queryExerciseMock.Object, queryPositionSkillMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetAll(inputTemplateId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void WhenTemplateHasSkillIdsEqualToNull_ReturnsBadRequestStatusCode()
        {
            // Arrange
            var inputTemplateId = "07CFB7D0-5C3C-4433-8BAE-F79945B90376";

            var savedPositionSkill = new TemplateCatalog
            {
                Id = inputTemplateId,
                CompetencyId = 13,
                JobFunctionLevel = 1
            };

            var queryExerciseMock = new Mock<IExerciseQueryRepository>();

            var queryPositionSkillMock = new Mock<IQueryRepository<TemplateCatalog, string>>();
            queryPositionSkillMock
                .Setup(method => method.FindById(It.IsAny<string>()))
                .ReturnsAsync(savedPositionSkill);

            var controllerUnderTest = new QueryExerciseController(queryExerciseMock.Object, queryPositionSkillMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetAll(inputTemplateId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestErrorMessageResult>());
            Assert.That((actionResult as BadRequestErrorMessageResult).Message, Is.EqualTo($"The template '{inputTemplateId}' doesn't have associated skills."));
        }

        [Test]
        public void WhenTemplateHasNoSkillIds_ReturnsBadRequestStatusCode()
        {
            // Arrange
            var inputTemplateId = "07CFB7D0-5C3C-4433-8BAE-F79945B90376";

            var savedPositionSkill = new TemplateCatalog
            {
                Id = inputTemplateId,
                CompetencyId = 13,
                JobFunctionLevel = 1,
                Skills = new List<int>()
            };

            var queryExerciseMock = new Mock<IExerciseQueryRepository>();

            var queryPositionSkillMock = new Mock<IQueryRepository<TemplateCatalog, string>>();
            queryPositionSkillMock
                .Setup(method => method.FindById(It.IsAny<string>()))
                .ReturnsAsync(savedPositionSkill);

            var controllerUnderTest = new QueryExerciseController(queryExerciseMock.Object, queryPositionSkillMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetAll(inputTemplateId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestErrorMessageResult>());
            Assert.That((actionResult as BadRequestErrorMessageResult).Message, Is.EqualTo($"The template '{inputTemplateId}' doesn't have associated skills."));
        }

        [Test]
        public void WhenDoesNotExistAnyExercise_ReturnsAnEmptyList()
        {
            // Arrange
            var inputTemplateId = "07CFB7D0-5C3C-4433-8BAE-F79945B90376";

            var savedExercises = new List<Exercise>();

            var savedPositionSkill = new TemplateCatalog
            {
                Id = inputTemplateId,
                CompetencyId = 13,
                JobFunctionLevel = 1,
                Skills = new List<int> { 1001, 1912, 2000 }
            };

            var queryExerciseMock = new Mock<IExerciseQueryRepository>();
            queryExerciseMock
                .Setup(method => method.FindWithinExercises(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int[]>()))
                .ReturnsAsync(savedExercises);

            var queryPositionSkillMock = new Mock<IQueryRepository<TemplateCatalog, string>>();
            queryPositionSkillMock
                .Setup(method => method.FindById(It.IsAny<string>()))
                .ReturnsAsync(savedPositionSkill);

            var controllerUnderTest = new QueryExerciseController(queryExerciseMock.Object, queryPositionSkillMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetAll(inputTemplateId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<List<ExerciseViewModel>>>());
            Assert.That((actionResult as OkNegotiatedContentResult<List<ExerciseViewModel>>).Content, Is.Empty);
            queryPositionSkillMock.Verify(method => method.FindById(It.IsAny<string>()), Times.Once);
            queryExerciseMock.Verify(method => method.FindWithinExercises(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int[]>()), Times.Once);
        }

        [Test]
        public void WhenExercisesExistForInputSkillIdentifiers_ReturnsAListOfExerciseViewModels()
        {
            // Arrange
            var inputTemplateId = "07CFB7D0-5C3C-4433-8BAE-F79945B90376";

            var savedPositionSkill = new TemplateCatalog
            {
                Id = inputTemplateId,
                CompetencyId = 13,
                JobFunctionLevel = 1,
                Skills = new List<int> { 1001, 1912, 2000 }
            };

            var savedExercises = new List<Exercise>
            {
                new Exercise { Id = "592F493B-E974-499F-9A49-F9A889D89EC8", CompetencyId = 13, JobFunctionLevel = 1, SkillId = 1001, Title = "Fix the code following SOLID principles", Description = "If any, please, fix next code to meet the SOLID principles.", ProposedSolution = "public void main() { ... }" },
                new Exercise { Id = "5336A7B8-09B3-4437-A53B-751107212A8B", CompetencyId = 13, JobFunctionLevel = 1, SkillId = 1001, Title = "Bubble sort", Description = "Complete next code of bubble sort algorithm.", ProposedSolution = "public void main() { ... }" },
                new Exercise { Id = "807D3020-ACB5-4BD1-94FA-A995866E4609", CompetencyId = 13, JobFunctionLevel = 1, SkillId = 2002, Title = "OOP - Inheritance", Description = "Having a parent class named 'Father', write the code of a child class named 'Child' that descends from it.", ProposedSolution = "class Child : Parent { ... }" },
                new Exercise { Id = "2FE75BAA-7C83-4E82-AC1C-CC0D81ED0725", CompetencyId = 13, JobFunctionLevel = 1, SkillId = 1912, Title = "Fixing a code review", Description = "Suppose you're intended to write a hash function using SHA-1 algorithm, please, ask your partner to make a code review of it.", ProposedSolution = "public byte*[] GetHashValue(char* entrance){ ... }" },
                new Exercise { Id = "EDFE0DC2-281B-4FCF-A20B-13BFDCDC6D43", CompetencyId = 13, JobFunctionLevel = 1, SkillId = 2000, Title = "Writing a microservice", Description = "Using Azure Functions or AWS Lambda, propose a code that sums two digits and return its square root.", ProposedSolution = "public double GetSquareRootOfTheSum(int x, int y) { ... }" },
                new Exercise { Id = "F120A901-0950-4F74-BAD3-218B5F980494", CompetencyId = 13, JobFunctionLevel = 1, SkillId = 100,  Title = "Palindrome", Description = "Write a program to detect whether a phrase is a palindrome.", ProposedSolution = "public bool IsPalindrome(string phrase) { ... }" }
            };

            var queryExerciseMock = new Mock<IExerciseQueryRepository>();
            queryExerciseMock
                .Setup(method => method.FindWithinExercises(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int[]>()))
                .ReturnsAsync((int competencyId, int jobFunctionId, int[] skillIds) =>
                {
                    var result = new List<Exercise>();

                    foreach (var skillId in skillIds)
                    {
                        var filteredExercises = savedExercises.Where(item => item.CompetencyId == competencyId && item.JobFunctionLevel == jobFunctionId && item.SkillId == skillId);
                        result.AddRange(filteredExercises);
                    }

                    return result;
                });

            var queryPositionSkillMock = new Mock<IQueryRepository<TemplateCatalog, string>>();
            queryPositionSkillMock
                .Setup(method => method.FindById(It.IsAny<string>()))
                .ReturnsAsync(savedPositionSkill);

            var controllerUnderTest = new QueryExerciseController(queryExerciseMock.Object, queryPositionSkillMock.Object);

            // Act
            var actionResult = controllerUnderTest.GetAll(inputTemplateId).Result;

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkNegotiatedContentResult<List<ExerciseViewModel>>>());
            // -- Checks that the methods were called as expected.
            queryPositionSkillMock.Verify(method => method.FindById(It.IsAny<string>()), Times.Once);
            queryExerciseMock.Verify(method => method.FindWithinExercises(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int[]>()), Times.Once);
            // -- Checks the quantity of records.
            Assert.That((actionResult as OkNegotiatedContentResult<List<ExerciseViewModel>>).Content, Is.Not.Empty);
            Assert.That((actionResult as OkNegotiatedContentResult<List<ExerciseViewModel>>).Content.Count, Is.EqualTo(4));
            // -- Checks the records' values.
            Assert.That((actionResult as OkNegotiatedContentResult<List<ExerciseViewModel>>).Content[0].ExerciseId, Is.EqualTo("592F493B-E974-499F-9A49-F9A889D89EC8"));
            Assert.That((actionResult as OkNegotiatedContentResult<List<ExerciseViewModel>>).Content[0].Title, Is.EqualTo("Fix the code following SOLID principles"));
            Assert.That((actionResult as OkNegotiatedContentResult<List<ExerciseViewModel>>).Content[0].Description, Is.EqualTo("If any, please, fix next code to meet the SOLID principles."));
            Assert.That((actionResult as OkNegotiatedContentResult<List<ExerciseViewModel>>).Content[0].ProposedSolution, Is.EqualTo("public void main() { ... }"));
        }
    }
}