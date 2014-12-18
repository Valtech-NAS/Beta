namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using Domain.Interfaces.Repositories;

    public class CheckApprenticeshipApplicationRepository : IMonitorTask
    {
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;

        public CheckApprenticeshipApplicationRepository(IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
        }

        public string TaskName
        {
            get { return "Check apprenticeship applications repository"; }
        }

        public void Run()
        {
            _apprenticeshipApplicationReadRepository.Get(Guid.NewGuid());
        }
    }
}
