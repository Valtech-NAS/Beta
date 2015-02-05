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

    public class CheckUnsetApprenticeshipApplicationLegacyId : IMonitorTask
    {
        private readonly IApprenticeshipApplicationDiagnosticsRepository _applicationDiagnosticsRepository;
        private readonly IMessageBus _messageBus;
        private readonly ILegacyApplicationStatusesProvider _legacyApplicationStatusesProvider;
        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;
        private readonly ILogService _logger;

        public CheckUnsetApprenticeshipApplicationLegacyId(IApprenticeshipApplicationDiagnosticsRepository applicationDiagnosticsRepository, IMessageBus messageBus, ILegacyApplicationStatusesProvider legacyApplicationStatusesProvider, IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository, ILogService logger)
        {
            _applicationDiagnosticsRepository = applicationDiagnosticsRepository;
            _messageBus = messageBus;
            _legacyApplicationStatusesProvider = legacyApplicationStatusesProvider;
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
            _logger = logger;
        }

        public string TaskName
        {
            get { return "Check Unset Apprenticeship Application Legacy Id"; }
        }

        public void Run()
        {
            var sb = new StringBuilder("The following actions were taken to resolve issues with apprenticeship applications:");
            sb.AppendLine();

            var applicationsToCheck = _applicationDiagnosticsRepository.GetSubmittedApplicationsWithUnsetLegacyId().ToList();

            foreach (var application in applicationsToCheck)
            {
                var applicationStatusSummaries = _legacyApplicationStatusesProvider.GetCandidateApplicationStatuses(application.Candidate);
                var applicationDetail = application.ApprenticeshipApplicationDetail;
                var applicationStatusSummary = applicationStatusSummaries.SingleOrDefault(s => s.LegacyVacancyId == applicationDetail.Vacancy.Id);
                if (applicationStatusSummary == null)
                {
                    applicationDetail.Status = ApplicationStatuses.Submitting;
                    _apprenticeshipApplicationWriteRepository.Save(applicationDetail);

                    var message = new SubmitApprenticeshipApplicationRequest
                    {
                        ApplicationId = applicationDetail.EntityId
                    };

                    _messageBus.PublishMessage(message);

                    _logger.Warn("Could not patch apprenticeship application id: {0} with legacy id as no matching application status summary was found. Requed instead", applicationDetail.EntityId);
                }
                else
                {
                    applicationDetail.LegacyApplicationId = applicationStatusSummary.LegacyApplicationId;
                    _apprenticeshipApplicationWriteRepository.Save(applicationDetail);
                    _logger.Info("Patching apprenticeship application id: {0} with legacy id: {1}", applicationDetail.EntityId, applicationDetail.LegacyApplicationId);
                }
            }
        }
    }
}