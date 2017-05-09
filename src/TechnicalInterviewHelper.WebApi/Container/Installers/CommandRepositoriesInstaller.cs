namespace TechnicalInterviewHelper.WebApi.Container
{
    using System.Configuration;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Services;    
    using TechnicalInterviewHelper.Model;

    public class CommandRepositoriesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ICommandRepository<Interview>>()
                         .ImplementedBy<DocumentDbCommandRepository<Interview>>()
                         .DependsOn(Dependency.OnValue("collectionId", ConfigurationManager.AppSettings["InterviewCollectionId"])),
                Component.For<ICommandRepository<Template>>()
                         .ImplementedBy<DocumentDbCommandRepository<Template>>()
                         .DependsOn(Dependency.OnValue("collectionId", ConfigurationManager.AppSettings["TemplateCollectionId"])),
                Component.For<ICommandRepository<Question>>()
                             .ImplementedBy<DocumentDbCommandRepository<Question>>()
                             .DependsOn(Dependency.OnValue("collectionId", ConfigurationManager.AppSettings["QuestionCollectionId"])),
               Component.For<ICommandRepository<Exercise>>()
                             .ImplementedBy<DocumentDbCommandRepository<Exercise>>()
                             .DependsOn(Dependency.OnValue("collectionId", ConfigurationManager.AppSettings["ExerciseCollectionId"])));
        }
    }
}