namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using Domain.Interfaces.Repositories;

    public class CheckTraineeshipApplicationRepository : IMonitorTask
    {
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;

        public CheckTraineeshipApplicationRepository(ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository)
        {
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
        }

        public string TaskName
        {
            get { return "Check traineeship applications repository"; }
        }

        public void Run()
        {
            _traineeshipApplicationReadRepository.Get(Guid.NewGuid());
        }
    }
}
