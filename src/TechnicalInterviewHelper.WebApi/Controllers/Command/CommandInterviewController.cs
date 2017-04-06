namespace TechnicalInterviewHelper.WebApi.Controllers
{    
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using Model;
    using TechnicalInterviewHelper.Model;

    [RoutePrefix("api/interview")]
    [EnableCors(origins: "*", headers: "*", methods: "POST")]
    public class CommandInterviewController : ApiController
    {
        #region Repository

        /// <summary>
        /// The command for interview.
        /// </summary>
        private readonly ICommandRepository<InterviewCatalog> commandInterview;

        #endregion Repository

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandInterviewController"/> class.
        /// </summary>
        /// <param name="commandInterview">The command interview.</param>
        public CommandInterviewController(ICommandRepository<InterviewCatalog> commandInterview)
        {
            this.commandInterview = commandInterview;
        }

        #endregion Constructor

        /// <summary>
        /// Posts the interview.
        /// </summary>
        /// <param name="interviewInputModel">The interview input model.</param>
        /// <returns></returns>
        [Route("")]
        public async Task<IHttpActionResult> PostInterview(InterviewInputModel interviewInputModel)
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
                var interviewToSave = new InterviewCatalog
                {
                    CompetencyId = interviewInputModel.CompetencyId,
                    JobFunctionLevel = interviewInputModel.JobFunctionLevel,
                    TemplateId = interviewInputModel.TemplateId,
                    Skills = interviewInputModel.Skills
                };

                var questions = new List<AnsweredQuestion>();
                foreach (var question in interviewInputModel.Questions)
                {
                    questions.Add(new AnsweredQuestion
                    {
                        CompetencyId = question.CompetencyId,
                        JobFunctionLevel = question.JobFunctionLevel,
                        SkillId = question.SkillId,
                        Description = question.Description,
                        Answer = question.Answer,
                        Rating = question.Rating
                    });
                }

                interviewToSave.Questions = questions;

                var exercises = new List<AnsweredExercise>();
                foreach (var exercise in interviewInputModel.Exercises)
                {
                    exercises.Add(new AnsweredExercise
                    {
                        CompetencyId = exercise.CompetencyId,
                        JobFunctionLevel = exercise.JobFunctionLevel,
                        SkillId = exercise.SkillId,
                        Title = exercise.Title,
                        Description = exercise.Description,
                        Complexity = exercise.Complexity,
                        ProposedSolution = exercise.ProposedSolution,
                        Answer = exercise.Answer,
                        Rating = exercise.Rating
                    });
                }

                interviewToSave.Exercises = exercises;

                var savedInterview = await this.commandInterview.Insert(interviewToSave);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}