namespace SFA.Apprenticeships.Web.Common.IoC
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class NasControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
	        if (controllerType == null)
	        {
		        throw new HttpException(404, string.Format("The controller for path '{0}' could not be found or it does not implement IController.", requestContext.HttpContext.Request.Path));
	        }

	        return base.GetControllerInstance(requestContext, controllerType);
        }
    }
}