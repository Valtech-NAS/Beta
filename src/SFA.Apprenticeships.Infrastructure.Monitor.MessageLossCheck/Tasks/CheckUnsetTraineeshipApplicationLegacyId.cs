namespace SFA.Apprenticeships.Infrastructure.Monitor.MessageLossCheck.Tasks
{
    using System.Linq;
    using System.Text;
    using Application.ApplicationUpdate;
    using Application.Candidate;
    using Application.Interfaces.Logging;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Monitor.Tasks;
    using Repository;

    public class CheckUnsetTraineeshipApplicationLegacyId : IMonitorTask
    {
        private readonly ITraineeshipApplicationDiagnosticsRepository _applicationDiagnosticsRepository;
        private readonly IMessageBus _messageBus;
        private readonly ILegacyApplicationStatusesProvider _legacyApplicationStatusesProvider;
        private readonly ITraineeshipApplicationWriteRepository _traineeshipApplicationWriteRepository;
        private readonly ILogService _logger;

        public CheckUnsetTraineeshipApplicationLegacyId(ITraineeshipApplicationDiagnosticsRepository applicationDiagnosticsRepository, IMessageBus messageBus, ILegacyApplicationStatusesProvider legacyApplicationStatusesProvider, ITraineeshipApplicationWriteRepository traineeshipApplicationWriteRepository, ILogService logger)
        {
            _applicationDiagnosticsRepository = applicationDiagnosticsRepository;
            _messageBus = messageBus;
            _legacyApplicationStatusesProvider = legacyApplicationStatusesProvider;
            _traineeshipApplicationWriteRepository = traineeshipApplicationWriteRepository;
            _logger = logger;
        }

        public string TaskName
        {
            get { return "Check Unset Traineeship Application Legacy Id"; }
        }

        public void Run()
        {
            var sb = new StringBuilder("The following actions were taken to resolve issues with traineeship applications:");
            sb.AppendLine();

            var applicationsToCheck = _applicationDiagnosticsRepository.GetSubmittedApplicationsWithUnsetLegacyId().ToList();

            foreach (var application in applicationsToCheck)
            {
                var applicationStatusSummaries = _legacyApplicationStatusesProvider.GetCandidateApplicationStatuses(application.Candidate);
                var applicationDetail = application.TraineeshipApplicationDetail;
                var applicationStatusSummary = applicationStatusSummaries.SingleOrDefault(s => s.LegacyVacancyId == applicationDetail.Vacancy.Id);
                if (applicationStatusSummary == null)
                {
                    var message = new SubmitTraineeshipApplicationRequest
                    {
                        ApplicationId = applicationDetail.EntityId
                    };

                    _messageBus.PublishMessage(message);

                    _logger.Warn("Could not patch traineeship application id: {0} with legacy id as no matching application status summary was found. Requed instead", applicationDetail.EntityId);
                }
                else
                {
                    applicationDetail.LegacyApplicationId = applicationStatusSummary.LegacyApplicationId;
                    _traineeshipApplicationWriteRepository.Save(applicationDetail);
                    _logger.Info("Patching traineeship application id: {0} with legacy id: {1}", applicationDetail.EntityId, applicationDetail.LegacyApplicationId);
                }
            }
        }
    }
}