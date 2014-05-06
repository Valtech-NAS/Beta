using System;
using System.Web.Http;
using System.Reflection;
using System.Web;
using System.Web.Optimization;
using SFA.Apprenticeships.Web.Candidate.Controllers;
using SFA.Apprenticeships.Web.Common.IoC.DependencyResolution;
using SFA.Apprenticeships.Web.Common.Models.Errors;

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

        /// <summary>
        /// source: http://www.codeproject.com/Articles/422572/Exception-Handling-in-ASP-NET-MVC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            //var ex = Server.GetLastError();
            var httpContext = ((MvcApplication)sender).Context;
            var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));
            var currentController = string.Empty;
            var currentAction = string.Empty;

            if (currentRouteData != null)
            {
                if (currentRouteData.Values["controller"] != null &&
                    !String.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
                {
                    currentController = currentRouteData.Values["controller"].ToString();
                }

                if (currentRouteData.Values["action"] != null &&
                    !String.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
                {
                    currentAction = currentRouteData.Values["action"].ToString();
                }
            }

            //var resolver = DependencyResolver.Current;
            //var logger = resolver.GetService<ILogger>();
            //logger.Error(ex, "Unhandled exception in action {0} of controller {1}", currentAction, currentController);

            // if the request is AJAX return JSON else view.
            if (httpContext.Request.Headers["X-Requested-With"].Equals("XMLHttpRequest", StringComparison.InvariantCultureIgnoreCase))
            {
                ReturnExceptionJson(httpContext);
            }
            else
            {
                ShowErrorPage(httpContext, currentController, currentAction);
            }
        }

        protected void ReturnExceptionJson(HttpContext context)
        {
            context.Response.Clear();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;

            var lastException = Server.GetLastError();
            context.Response.Write(lastException.Message);

            context.ClearError();
        }

        protected void ShowErrorPage(HttpContext context, string currentController, string currentAction)
        {
            var ex = Server.GetLastError();

            using (var controller = new ErrorController())
            {
                var routeData = new RouteData();
                var action = "Index";

                if (ex is HttpException)
                {
                    var httpEx = ex as HttpException;

                    switch (httpEx.GetHttpCode())
                    {
                        case 404:
                            action = "NotFound";
                            break;

                        // others if any

                        default:
                            action = "Index";
                            break;
                    }
                }

                context.ClearError();
                context.Response.Clear();
                context.Response.StatusCode = ex is HttpException ? ((HttpException)ex).GetHttpCode() : 500;
                context.Response.TrySkipIisCustomErrors = true;
                routeData.Values["controller"] = "Error";
                routeData.Values["action"] = action;

                controller.ViewData.Model = new ErrorViewModel
                {
                    HandleErrorInfo = new HandleErrorInfo(ex, currentController, currentAction)
                };

                ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(context), routeData));
            }
        }
    }
}
