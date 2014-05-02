using System.Web.Http;
using System.Reflection;
using System.Web;
using System.Web.Optimization;
using SFA.Apprenticeships.Web.Common.IoC.DependencyResolution;

namespace SFA.Apprenticeships.Web.Candidate
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            const string applicationName = "SFA.Apprenticeships.Web.Candidate";

            // Change the Application Name in runtime.
            var runtimeInfo = typeof(HttpRuntime).GetField("_theRuntime", BindingFlags.Static | BindingFlags.NonPublic);

            if (runtimeInfo != null)
            {
                var theRuntime = (HttpRuntime)runtimeInfo.GetValue(null);
                var appNameInfo = typeof(HttpRuntime).GetField("_appDomainAppId", BindingFlags.Instance | BindingFlags.NonPublic);
                if (appNameInfo != null)
                {
                    appNameInfo.SetValue(theRuntime, applicationName);
                }
            }


            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            ControllerBuilder.Current.SetControllerFactory(new NasControllerFactory());
        }
    }
}
