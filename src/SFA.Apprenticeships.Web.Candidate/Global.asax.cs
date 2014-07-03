namespace SFA.Apprenticeships.Web.Candidate
{
    using System;
    using System.Web.Http;
    using System.Web;
    using System.Web.Optimization;
    using FluentValidation.Mvc;
    using Common.Framework;
    using Controllers;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            RuntimeHelper.SetRuntimeName("SFA.Apprenticeships.Web.Candidate");

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            ControllerBuilder.Current.SetControllerFactory(new DefaultControllerFactory());

            FluentValidationModelValidatorProvider.Configure();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //Server.HandleError<ErrorController>(((MvcApplication)sender).Context);
        }
    }
}
