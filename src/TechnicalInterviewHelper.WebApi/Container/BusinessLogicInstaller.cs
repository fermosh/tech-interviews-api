namespace TechnicalInterviewHelper.WebApi.Container
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Services;
    using System.Configuration;
    using TechnicalInterviewHelper.Model;

    public class BusinessLogicInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IQueryRepository<CompetencyCatalog, string>>()
                         .ImplementedBy<DocumentDbQueryRepository<CompetencyCatalog, string>>()
                         .DependsOn(Dependency.OnValue("collectionId", ConfigurationManager.AppSettings["CompetencyCollectionId"])),
                Component.For<IQueryRepository<TemplateCatalog, string>>()
                         .ImplementedBy<DocumentDbQueryRepository<TemplateCatalog, string>>()
                         .DependsOn(Dependency.OnValue("collectionId", ConfigurationManager.AppSettings["TemplateCollectionId"]))
                );
        }
    }
}