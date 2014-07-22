namespace SFA.Apprenticeships.Web.Candidate
{
    using System;
    using System.Security.Principal;
    using System.Web.Http;
    using System.Web;
    using System.Web.Optimization;
    using System.Web.Security;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Users;
    using Common.Validations;
    using Domain.Entities.Users;
    using FluentValidation.Mvc;
    using Common.Framework;
    using System.Web.Mvc;
    using System.Web.Routing;
    using FluentValidation.Validators;
    using StructureMap;

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

            FluentValidationModelValidatorProvider.Configure(provider =>
            {
                provider.AddImplicitRequiredValidator = false;
                provider.Add(typeof(EqualValidator), (metadata, context, description, validator) => new EqualToValueFluentValidationPropertyValidator(metadata, context, description, validator));
            });
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //Server.HandleError<ErrorController>(((MvcApplication)sender).Context);
        }
    }
}
