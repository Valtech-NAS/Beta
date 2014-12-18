namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using Domain.Interfaces.Repositories;

    public class CheckApplicationRepository : IMonitorTask
    {
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;

        public CheckApplicationRepository(IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
        }

        public string TaskName
        {
            get { return "Check applications repository"; }
        }

        public void Run()
        {
            _apprenticeshipApplicationReadRepository.Get(Guid.NewGuid());
        }
    }
}
