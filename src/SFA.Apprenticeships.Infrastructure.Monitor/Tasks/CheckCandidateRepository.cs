namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using Domain.Interfaces.Repositories;
    using NLog;

    public class CheckCandidateRepository : IMonitorTask
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ICandidateReadRepository _candidateReadRepository;
        private const string TaskName = "Check candidate repository";

        public CheckCandidateRepository(ICandidateReadRepository candidateReadRepository)
        {
            _candidateReadRepository = candidateReadRepository;
        }

        public void Run()
        {
            Logger.Debug(string.Format("Start running task {0}", TaskName));
            
            try
            {
                _candidateReadRepository.Get(Guid.NewGuid());
            }
            catch (Exception execption)
            {
                Logger.ErrorException("Error while accessing CandidateRepository", execption);
            }

            Logger.Debug(string.Format("Finished running task {0}", TaskName));
        }
    }
}