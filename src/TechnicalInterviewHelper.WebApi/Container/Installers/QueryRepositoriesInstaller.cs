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
                Component.For<IQueryRepository<CompetencyCatalog, string>>()
                         .ImplementedBy<DocumentDbQueryRepository<CompetencyCatalog, string>>()
                         .DependsOn(Dependency.OnValue("collectionId", ConfigurationManager.AppSettings["CompetencyCollectionId"])),
                Component.For<IQueryRepository<Template, string>>()
                         .ImplementedBy<DocumentDbQueryRepository<Template, string>>()
                         .DependsOn(Dependency.OnValue("collectionId", ConfigurationManager.AppSettings["TemplateCollectionId"])),
                Component.For<IExerciseQueryRepository>()
                         .ImplementedBy<ExerciseDocumentDbQueryRepository>()
                         .DependsOn(Dependency.OnValue("collectionId", ConfigurationManager.AppSettings["ExerciseCollectionId"])),
                Component.For<IQuestionQueryRepository>()
                         .ImplementedBy<QuestionDocumentDbQueryRepository>()
                         .DependsOn(Dependency.OnValue("collectionId", ConfigurationManager.AppSettings["QuestionCollectionId"])),
                Component.For<ISkillMatrixQueryRepository>()
                         .ImplementedBy<SkillMatrixDocumentDbQueryRepository>()
                         .DependsOn(Dependency.OnValue("collectionId", ConfigurationManager.AppSettings["SkillCollectionId"])));
        }
    }
}