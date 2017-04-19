namespace TechnicalInterviewHelper.WebApi.Container
{
    using System.Configuration;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Services;    
    using TechnicalInterviewHelper.Model;

    public class QueryRepositoriesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IQueryRepository<CompetencyDocument, string>>()
                         .ImplementedBy<DocumentDbQueryRepository<CompetencyDocument, string>>()
                         .DependsOn(Dependency.OnValue("collectionId", ConfigurationManager.AppSettings["MainCollectionId"])),
                Component.For<IQueryRepository<Template, string>>()
                         .ImplementedBy<DocumentDbQueryRepository<Template, string>>()
                         .DependsOn(Dependency.OnValue("collectionId", ConfigurationManager.AppSettings["MainCollectionId"])),
                Component.For<IExerciseQueryRepository>()
                         .ImplementedBy<ExerciseDocumentDbQueryRepository>()
                         .DependsOn(Dependency.OnValue("collectionId", ConfigurationManager.AppSettings["MainCollectionId"])),
                Component.For<IQuestionQueryRepository>()
                         .ImplementedBy<QuestionDocumentDbQueryRepository>()
                         .DependsOn(Dependency.OnValue("collectionId", ConfigurationManager.AppSettings["MainCollectionId"])),
                Component.For<ISkillMatrixQueryRepository>()
                         .ImplementedBy<SkillMatrixDocumentDbQueryRepository>()
                         .DependsOn(Dependency.OnValue("collectionId", ConfigurationManager.AppSettings["MainCollectionId"])),
                Component.For<IJobFunctionQueryRepository>()
                         .ImplementedBy<JobFunctionQueryRepository>()
                         .DependsOn(Dependency.OnValue("collectionId", ConfigurationManager.AppSettings["MainCollectionId"])),
                Component.For<ICompetencyQueryRepository>()
                         .ImplementedBy<CompetencyDocumentDbQueryRepository>()
                         .DependsOn(Dependency.OnValue("collectionId", ConfigurationManager.AppSettings["MainCollectionId"])));
        }
    }
}