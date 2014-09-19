namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using Domain.Interfaces.Repositories;

    public class CheckCandidateRepository : IMonitorTask
    {
        private readonly ICandidateReadRepository _candidateReadRepository;

        public CheckCandidateRepository(ICandidateReadRepository candidateReadRepository)
        {
            _candidateReadRepository = candidateReadRepository;
        }

        public string TaskName
        {
            get { return "Check candidate repository"; }
        }

        public void Run()
        {
            _candidateReadRepository.Get(Guid.NewGuid());
        }
    }
}
