namespace TechnicalInterviewHelper.WebApi.Tests.Container
{
    using Castle.Core.Internal;
    using Castle.MicroKernel;
    using Castle.Windsor;
    using NUnit.Framework;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using WebApi.Container;
    using WebApi.Controllers;

    [TestFixture]
    public class ControllersInstallerTests
    {
        private IWindsorContainer containerWithControllers;

        [SetUp]
        public void Setup()
        {
            containerWithControllers = new WindsorContainer().Install(new CommandRepositoriesInstaller(), new ApiControllersInstaller());
        }

        [Test]
        public void All_controllers_implement_IHttpController()
        {
            var allHandlers = GetAllHandlers(containerWithControllers);
            var controllerHandlers = GetHandlersFor(typeof(IHttpController), containerWithControllers);

            Assert.IsNotEmpty(allHandlers);
            Assert.AreEqual(allHandlers, controllerHandlers);
        }

        [Test]
        public void KK()
        {
            var mmk = containerWithControllers.Resolve<QueryCompetencyController>();
            var ok = mmk.GetAll().Result;

            Assert.IsNotNull(mmk);
        }

        [Test]
        public void All_controllers_are_registered()
        {
            // Is<TType> is an helper, extension method from Windsor in the Castle.Core.Internal namespace
            // which behaves like 'is' keyword in C# but at a Type, not instance level
            var allControllers = GetPublicClassesFromApplicationAssembly(c => c.Is<IHttpController>());
            var registeredControllers = GetImplementationTypesFor(typeof(IHttpController), containerWithControllers);

            Assert.AreEqual(allControllers, registeredControllers);
        }

        #region Helpers

        private IHandler[] GetAllHandlers(IWindsorContainer container)
        {
            return GetHandlersFor(typeof(object), container);
        }

        private IHandler[] GetHandlersFor(Type type, IWindsorContainer container)
        {
            return container.Kernel.GetAssignableHandlers(type);
        }

        private Type[] GetImplementationTypesFor(Type type, IWindsorContainer container)
        {
            return GetHandlersFor(type, container)
                .Select(h => h.ComponentModel.Implementation)
                .OrderBy(t => t.Name)
                .ToArray();
        }

        private Type[] GetPublicClassesFromApplicationAssembly(Predicate<Type> where)
        {
            return typeof(HomeController).Assembly.GetExportedTypes()
                .Where(t => t.IsClass)
                .Where(t => t.IsAbstract == false)
                .Where(where.Invoke)
                .OrderBy(t => t.Name)
                .ToArray();
        }

        #endregion Helpers
    }
}