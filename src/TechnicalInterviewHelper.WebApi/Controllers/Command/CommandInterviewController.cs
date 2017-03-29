namespace TechnicalInterviewHelper.WebApi.Controllers
{
    using Model;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading.Tasks;
    using System.Web.Http;
    using TechnicalInterviewHelper.Model;

    [Route("interview")]
    public class CommandInterviewController : ApiController
    {
        #region Repository

        private readonly ICommandRepository<Interview> commandInterview;

        #endregion Repository

        #region Constructor

        public CommandInterviewController()
        {
            this.commandInterview = new DocumentDbCommandRepository<Interview>(ConfigurationManager.AppSettings["InterviewCollection"]);
        }

        public CommandInterviewController(ICommandRepository<Interview> commandInterview)
        {
            this.commandInterview = commandInterview;
        }

        #endregion Constructor

        /*
        [HttpPost]
        [Route("save")]
        public async Task<IHttpActionResult> SaveInterview(InterviewInputModel interviewInputModel)
        {
            // Exit early from method when any of next validations are not meet.
            if (interviewInputModel == null
                ||
                interviewInputModel.Skills == null
                ||
                interviewInputModel.Questions == null
                ||
                interviewInputModel.Exercises == null)
            {
                return BadRequest();
            }

            try
            {
                var interviewToSave = new Interview
                {
                    PositionId = interviewInputModel.PositionId
                };

                var skills = new List<Skill>();
                foreach (var skill in interviewInputModel.Skills)
                {
                    skills.Add(new Skill
                    {
                        SkillId = skill.SkillId,
                        Description = skill.Description
                    });
                }

                interviewToSave.Skills = skills;

                var questions = new List<Question>();
                foreach (var question in interviewInputModel.Questions)
                {
                    questions.Add(new Question
                    {
                        Id = question.QuestionId,
                        Description = question.Description,
                        CapturedAnswer = question.CapturedAnswer,
                        CapturedRating = question.CapturedRating
                    });
                }

                interviewToSave.Questions = questions;

                var exercises = new List<Exercise>();
                foreach (var exercise in interviewInputModel.Exercises)
                {
                    exercises.Add(new Exercise
                    {
                        Id = exercise.ExerciseId,
                        Title = exercise.Title,
                        Description = exercise.Description,
                        CapturedSolution = exercise.CapturedSolution,
                        CapturedRating = exercise.CapturedRating
                    });
                }

                interviewToSave.Exercises = exercises;

                var savedInterview = await this.commandInterview.Insert(interviewToSave);

                return Ok("interview.saved");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        */
    }
}