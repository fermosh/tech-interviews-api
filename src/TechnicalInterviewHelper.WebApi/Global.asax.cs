﻿namespace TechnicalInterviewHelper.WebApi
{    
    using System.Web.Http;
    using System.Web.Http.Dispatcher;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Castle.Windsor;
    using Castle.Windsor.Installer;
    using Container;

    public class WebApiApplication : System.Web.HttpApplication
    {
        private IWindsorContainer container;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // Setup DocumentDB schema.
            DocumentDbConfig.Initialize();
            // Setup IoC container for ApiControllers.
            this.container = new WindsorContainer().Install(FromAssembly.This());
            WindsorControllerActivator activator = new WindsorControllerActivator(this.container.Kernel);
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), activator);
        }

        public override void Dispose()
        {
            this.container.Dispose();
            base.Dispose();
        }
    }
}