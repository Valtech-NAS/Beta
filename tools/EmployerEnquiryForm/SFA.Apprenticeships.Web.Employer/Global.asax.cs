using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SFA.Apprenticeships.Web.Employer
{
    using FluentValidation.Mvc;
    using FluentValidation.Validators;
    using SFA.Apprenticeships.Web.Employer.Framework.Binders;
    using SFA.Apprenticeships.Web.Employer.Framework.Validations;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
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
                provider.Add(typeof(EqualValidator),
                    (metadata, context, description, validator) =>
                        new EqualToValueFluentValidationPropertyValidator(metadata, context, description, validator));
            });
        }
    }
}
