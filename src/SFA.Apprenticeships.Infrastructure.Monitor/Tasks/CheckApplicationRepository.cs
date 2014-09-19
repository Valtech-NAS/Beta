namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using Domain.Interfaces.Repositories;

    public class CheckApplicationRepository : IMonitorTask
    {
        private readonly IApplicationReadRepository _applicationReadRepository;

        public CheckApplicationRepository(IApplicationReadRepository applicationReadRepository)
        {
            _applicationReadRepository = applicationReadRepository;
        }

        public string TaskName
        {
            get { return "Check applications repository"; }
        }

        public void Run()
        {
            _applicationReadRepository.Get(Guid.NewGuid());
        }
    }
}
