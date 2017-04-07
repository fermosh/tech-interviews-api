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
                Component.For<ICommandRepository<InterviewCatalog>>()
                         .ImplementedBy<DocumentDbCommandRepository<InterviewCatalog>>()
                         .DependsOn(Dependency.OnValue("collectionId", ConfigurationManager.AppSettings["InterviewCollection"])),
                Component.For<ICommandRepository<Template>>()
                         .ImplementedBy<DocumentDbCommandRepository<Template>>()
                         .DependsOn(Dependency.OnValue("collectionId", ConfigurationManager.AppSettings["TemplateCollectionId"])));
        }
    }
}