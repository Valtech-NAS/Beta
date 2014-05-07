namespace SFA.Apprenticeships.Web.Common.IntegrationTests
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
