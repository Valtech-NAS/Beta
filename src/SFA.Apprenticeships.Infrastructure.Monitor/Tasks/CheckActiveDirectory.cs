namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using Application.Authentication;
    using NLog;

    public class CheckActiveDirectory : IMonitorTask
    {
        private readonly IUserDirectoryProvider _userDirectoryProvider;
        private const string TaskName = "Check Active Directory";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public CheckActiveDirectory(IUserDirectoryProvider userDirectoryProvider)
        {
            _userDirectoryProvider = userDirectoryProvider;
        }

        public void Run()
        {
            Logger.Debug(string.Format("Start running task {0}", TaskName));

            try
            {
                _userDirectoryProvider.AuthenticateUser("valtech", "valtech");
            }
            catch (Exception exception)
            {
                Logger.ErrorException("Error while accessing Active Directory", exception);
            }

            Logger.Debug(string.Format("Finished running task {0}", TaskName));
        }
    }
}