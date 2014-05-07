namespace SFA.Apprenticeships.Tests.Web.Common
{
    using System.Configuration;

    public static class SiteConfig
    {
        static SiteConfig()
        {
            WebRoot = ConfigurationManager.AppSettings["WebRoot"] ?? "http://localhost/";
        }

        public static string WebRoot { get; private set; }
    }
}
