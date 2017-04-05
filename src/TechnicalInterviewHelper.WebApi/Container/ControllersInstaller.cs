namespace TechnicalInterviewHelper.WebApi.Container
{
    using System.Globalization;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;    

    public class ControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                .Pick().If(item => item.Name.EndsWith("controller", true, CultureInfo.InvariantCulture))
                .Configure(configurer => configurer.Named(configurer.Implementation.Name))
                .LifestyleTransient());
        }
    }
}