namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using Domain.Interfaces.Repositories;
    using NLog;

    public class CheckUserRepository : IMonitorTask
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IUserReadRepository _userReadRepository;
        private const string TaskName = "Check user repository";

        public CheckUserRepository(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public void Run()
        {
            Logger.Debug(string.Format("Start running task {0}", TaskName));
            
            try
            {
                _userReadRepository.Get(Guid.NewGuid());
            }
            catch (Exception execption)
            {
                Logger.ErrorException("Error while accessing UserRepository", execption);
            }

            Logger.Debug(string.Format("Finished running task {0}", TaskName));
        }
    }
}