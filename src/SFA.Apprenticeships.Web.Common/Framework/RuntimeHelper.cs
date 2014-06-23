namespace SFA.Apprenticeships.Web.Common.Framework
{
    using System;
    using System.Reflection;
    using System.Web;

    public static class RuntimeHelper
    {
        public static void SetRuntimeName(string applicationName)
        {
            // Change the Application Name in runtime
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

        }
    }
}
