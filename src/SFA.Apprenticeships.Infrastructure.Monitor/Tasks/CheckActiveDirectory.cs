namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using Application.Interfaces.Locations;
    using NLog;
    using UserDirectory.ActiveDirectory;

    public class CheckActiveDirectory : IMonitorTask
    {
        private const string TaskName = "Check Active Directory";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ActiveDirectoryServer _server;

        public CheckActiveDirectory(ActiveDirectoryServer server)
        {
            _server = server;
        }

        public void Run()
        {
            Logger.Debug(string.Format("Start running task {0}", TaskName));

            try
            {
                using (var context = _server.Context)
                {
                    
                }
            }
            catch (Exception execption)
            {
                Logger.ErrorException("Error while accessing Active Directory", execption);
            }

            Logger.Debug(string.Format("Finished running task {0}", TaskName));
        }
    }
}