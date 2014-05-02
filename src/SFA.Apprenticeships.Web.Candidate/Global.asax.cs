using SFA.Apprenticeships.Web.Common.IoC.DependencyResolution;

namespace SFA.Apprenticeships.Web.Candidate
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            StructuremapMvc.StartIoC();
            ControllerBuilder.Current.SetControllerFactory(new NasControllerFactory());
        }
    }
}
