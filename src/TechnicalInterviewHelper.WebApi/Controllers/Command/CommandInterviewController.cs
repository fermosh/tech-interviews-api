namespace TechnicalInterviewHelper.WebApi.Controllers
{    
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using Model;
    using TechnicalInterviewHelper.Model;

    [RoutePrefix("api/interviews")]
    [EnableCors(origins: "*", headers: "*", methods: "POST")]
    public class CommandInterviewController : ApiController
    {
        #region Repository

        /// <summary>
        /// The command for interview.
        /// </summary>
        private readonly ICommandRepository<Interview> commandInterview;

        #endregion Repository

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandInterviewController"/> class.
        /// </summary>
        /// <param name="commandInterview">The command interview.</param>
        public CommandInterviewController(ICommandRepository<Interview> commandInterview)
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
            // Exit early from method when validation is not meet.
            if (interviewInputModel == null)
            {
                return BadRequest("Cannot save the interview because its reference is not valid.");
            }

            if (interviewInputModel.Skills == null)
            {
                return BadRequest("Cannot save an interview without skills added to it.");
            }

            try
            {
                var interviewToSave = new Interview
                {
                    CompetencyId = interviewInputModel.CompetencyId,
                    JobFunctionLevel = interviewInputModel.JobFunctionLevel,
                    TemplateId = interviewInputModel.TemplateId
                };

                var skills = new List<SkillInterview>();
                foreach (var skill in interviewInputModel.Skills)
                {
                    var questions = new List<AnsweredQuestion>();
                    foreach (var question in skill.Questions)
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

                    skills.Add(new SkillInterview
                    {
                        SkillId = skill.SkillId,
                        Description = skill.Description,
                        Questions = questions
                    });
                }

                interviewToSave.Skills = skills;

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