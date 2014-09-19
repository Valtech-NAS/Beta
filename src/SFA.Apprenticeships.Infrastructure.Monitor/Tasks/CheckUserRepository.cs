namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using Domain.Interfaces.Repositories;

    public class CheckUserRepository : IMonitorTask
    {
        private readonly IUserReadRepository _userReadRepository;

        public CheckUserRepository(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public string TaskName
        {
            get { return "Check user repository"; }
        }

        public void Run()
        {
            _userReadRepository.Get(Guid.NewGuid());
        }
    }
}
