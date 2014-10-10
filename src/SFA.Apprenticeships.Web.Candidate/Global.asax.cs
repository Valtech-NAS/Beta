namespace SFA.Apprenticeships.Web.Candidate
{
    using System;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Common.Binders;
    using Common.Framework;
    using Common.Validations;
    using Controllers;
    using FluentValidation.Mvc;
    using FluentValidation.Validators;
    using Microsoft.WindowsAzure;

    public class MvcApplication : HttpApplication
    {
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            bool isWebsiteOffline;

            if (!bool.TryParse(CloudConfigurationManager.GetSetting("IsWebsiteOffline"), out isWebsiteOffline))
            {
                return;
            }

            if (Request.Path.Contains("403.aspx")) { return;}

            if (isWebsiteOffline)
            {
                //Response.StatusCode = 403;
                Response.Redirect("403.aspx");
            }
        }

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

            ModelBinders.Binders.DefaultBinder = new TrimModelBinder();
            FluentValidationModelValidatorProvider.Configure(provider =>
            {
                provider.AddImplicitRequiredValidator = false;
                provider.Add(typeof (EqualValidator),
                    (metadata, context, description, validator) =>
                        new EqualToValueFluentValidationPropertyValidator(metadata, context, description, validator));
            });
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Server.HandleError<ErrorController>(((MvcApplication) sender).Context);
        }
    }
}