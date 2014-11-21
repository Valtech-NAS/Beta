namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using SFA.Apprenticeships.Application.Authentication;
    using SFA.Apprenticeships.Domain.Entities.Exceptions;

    public class CheckUserDirectory : IMonitorTask
    {
        private readonly IUserDirectoryProvider _userDirectoryProvider;

        public CheckUserDirectory(IUserDirectoryProvider userDirectoryProvider)
        {
            _userDirectoryProvider = userDirectoryProvider;
        }

        public string TaskName
        {
            get { return "Check Active Directory"; }
        }

        public void Run()
        {
            try
            {
                _userDirectoryProvider.AuthenticateUser(Guid.NewGuid().ToString(), "valtech");
            }
            catch (CustomException)
            {
                // It's ok to get a custom exception
            }
        }
    }
}
