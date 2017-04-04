namespace TechnicalInterviewHelper.WebApi.Container
{
    using System.Web.Mvc;
    using Castle.Windsor;
    using Castle.Windsor.Installer;

    public static class IocContainer
    {
        private static IWindsorContainer container;

        public static void Setup()
        {
            container = new WindsorContainer().Install(FromAssembly.This());

            WindsorControllerFactory controllerFactory = new WindsorControllerFactory(container.Kernel);

            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }
    }
}