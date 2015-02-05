﻿namespace SFA.Apprenticeships.Web.Candidate
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
    using Infrastructure.Logging;
    using StructureMap;

    public class MvcApplication : HttpApplication
    {
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            bool isWebsiteOffline;

            if (!bool.TryParse(CloudConfigurationManager.GetSetting("IsWebsiteOffline"), out isWebsiteOffline))
            {
                return;
            }

            var context = HttpContext.Current;
            var path = context.Request.Path.ToLower();

            var lastExtension = path.LastIndexOf(".aspx", StringComparison.Ordinal);
            if (lastExtension != -1)
            {
                return;
            }
            var lastContent = path.LastIndexOf("/content/", StringComparison.Ordinal);
            if (lastContent != -1)
            {
                return;
            }

            if (isWebsiteOffline)
            {               
                context.RewritePath("~/403.aspx", false);
            }
        }

        protected void Application_Start()
        {
            VersionLogging.SetVersion();
            RuntimeHelper.SetRuntimeName("SFA.Apprenticeships.Web.Candidate");

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);

            var container = (IContainer) DependencyResolver.Current.GetService(typeof(IContainer));
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters, container);
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

            // This header cannot be removed using web.config --> http://www.codeproject.com/Tips/785867/ASP-NET-MVC-Remove-IIS-Header-Bloat
            MvcHandler.DisableMvcResponseHeader = true;
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Server.HandleError<ErrorController>(((MvcApplication) sender).Context);
        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            // This header cannot be removed using web.config --> http://www.codeproject.com/Tips/785867/ASP-NET-MVC-Remove-IIS-Header-Bloat
            var application = sender as HttpApplication;
            if (application != null && application.Context != null)
            {
                application.Context.Response.Headers.Remove("Server");
            }
        }
    }
}