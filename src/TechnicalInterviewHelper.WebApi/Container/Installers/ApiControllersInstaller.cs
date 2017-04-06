namespace TechnicalInterviewHelper.WebApi.Container
{
    using System.Web.Http;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;    

    public class ApiControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly()
                       .Pick()
                       .If(item => item.BaseType == typeof(ApiController))
                       .Configure(configurer => configurer.Named(configurer.Implementation.Name))
                       .LifestyleTransient());
        }
    }
}