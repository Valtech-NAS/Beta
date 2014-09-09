namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using Domain.Interfaces.Repositories;
    using NLog;

    public class CheckApplicationRepository : IMonitorTask
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IApplicationReadRepository _applicationReadRepository;
        private const string TaskName = "Check applications repository";

        public CheckApplicationRepository(IApplicationReadRepository applicationReadRepository)
        {
            _applicationReadRepository = applicationReadRepository;
        }

        public void Run()
        {
            Logger.Debug(string.Format("Start running task {0}", TaskName));
            
            try
            {
                _applicationReadRepository.Get(Guid.NewGuid());
            }
            catch (Exception execption)
            {
                Logger.ErrorException("Error while accessing ApplicationsRepository", execption);
            }

            Logger.Debug(string.Format("Finished running task {0}", TaskName));
        }
    }
}