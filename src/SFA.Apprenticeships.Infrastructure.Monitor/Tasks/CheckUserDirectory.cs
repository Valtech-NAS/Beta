namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using Apprenticeships.Application.Authentication;
    using Domain.Entities.Exceptions;

    public class CheckUserDirectory : IMonitorTask
    {
        private readonly IUserDirectoryProvider _userDirectoryProvider;

        public CheckUserDirectory(IUserDirectoryProvider userDirectoryProvider)
        {
            _userDirectoryProvider = userDirectoryProvider;
        }

        public string TaskName
        {
            get { return "Check user directory"; }
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
