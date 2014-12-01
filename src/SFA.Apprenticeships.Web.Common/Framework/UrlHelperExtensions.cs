namespace SFA.Apprenticeships.Web.Common.Framework
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Web.Mvc;

    public static class UrlHelperExtensions
    {
        public static string CdnContent(this UrlHelper urlHelper, string contentName, string localContentPath)
        {
            const string separator = "/";

            return IsLocalEnvironment ?
                urlHelper.Content(string.Join(separator, localContentPath, contentName)) :
                string.Join(separator, CdnUrl, EnvironmentName, contentName).ToLower();
        }

        #region Helpers

        private static string EnvironmentName
        {
            get
            {
                return ConfigurationManager.AppSettings["Environment"];
            }
        }

        private static string CdnUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["CdnUrl"];
            }
        }

        private static bool IsLocalEnvironment
        {
            get
            {
                var localEnvironmentNames = new [] { "debug", "local" };

                return
                    localEnvironmentNames.Any(each =>
                        each.Equals(EnvironmentName, StringComparison.InvariantCultureIgnoreCase));
            }
        }

        #endregion
    }
}
