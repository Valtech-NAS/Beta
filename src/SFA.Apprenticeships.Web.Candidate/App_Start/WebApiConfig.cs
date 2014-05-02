using System.Web.Http;

namespace SFA.Apprenticeships.Web.Candidate
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {         
            config.MapHttpAttributeRoutes();
        }
    }
}
