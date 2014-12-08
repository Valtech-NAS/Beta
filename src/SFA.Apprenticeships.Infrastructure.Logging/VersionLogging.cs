namespace SFA.Apprenticeships.Infrastructure.Logging
{
    using System.Diagnostics;
    using System.Reflection;
    using NLog;

    public class VersionLogging
    {
        public static void SetVersion()
        {
            const string versionKey = "version";
            if (!GlobalDiagnosticsContext.Contains(versionKey))
            {
                GlobalDiagnosticsContext.Set(versionKey, GetVersion());
            }
        }

        public static string GetVersion()
        {
            var fileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetAssembly(typeof(VersionLogging)).Location).FileVersion;
            return fileVersion;
        }
    }
}