namespace TechnicalInterviewHelper.WebApi.Container
{    
    using System;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;
    using Castle.MicroKernel;

    public class WindsorControllerActivator : IHttpControllerActivator
    {
        private readonly IKernel kernel;

        public WindsorControllerActivator(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            var controller = (IHttpController)this.kernel.Resolve(controllerType);
            request.RegisterForDispose(new Release(() => this.kernel.ReleaseComponent(controller)));
            return controller;
        }

        private class Release : IDisposable
        {
            private readonly Action release;

            public Release(Action release)
            {
                this.release = release;
            }

            public void Dispose()
            {
                this.release();
            }
        }
    }
}