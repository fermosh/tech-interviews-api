namespace TechnicalInterviewHelper.WebApi.Container
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Castle.MicroKernel;
    using System.Web;

    public class WindsorControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel kernel;

        public WindsorControllerFactory(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public override void ReleaseController(IController controller)
        {
            kernel.ReleaseComponent(controller);
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                throw new HttpException(404, $"Cannot find a controller for the path '{requestContext.HttpContext.Request.Path}'.");
            }

            return (IController)kernel.Resolve(controllerType);
        }
    }
}