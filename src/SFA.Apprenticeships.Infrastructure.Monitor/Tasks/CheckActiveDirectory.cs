namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using Application.Authentication;

    public class CheckActiveDirectory : IMonitorTask
    {
        private readonly IUserDirectoryProvider _userDirectoryProvider;

        public CheckActiveDirectory(IUserDirectoryProvider userDirectoryProvider)
        {
            _userDirectoryProvider = userDirectoryProvider;
        }

        public string TaskName
        {
            get { return "Check Active Directory"; }
        }

        public void Run()
        {
            _userDirectoryProvider.AuthenticateUser("valtech", "valtech");
        }
    }
}
