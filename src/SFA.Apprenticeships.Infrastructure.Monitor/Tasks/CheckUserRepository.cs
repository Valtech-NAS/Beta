namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using Domain.Interfaces.Repositories;
    using NLog;

    public class CheckUserRepository : IMonitorTask
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IUserReadRepository _userReadRepository;

        public CheckUserRepository(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}
