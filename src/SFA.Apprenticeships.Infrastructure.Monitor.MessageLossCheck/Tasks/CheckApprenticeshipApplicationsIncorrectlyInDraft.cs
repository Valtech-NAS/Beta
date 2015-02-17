namespace SFA.Apprenticeships.Infrastructure.Monitor.MessageLossCheck.Tasks
{
    using System.Linq;
    using System.Text;
    using Application.ApplicationUpdate;
    using Application.Candidate;
    using Application.Interfaces.Logging;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Messaging;
    using Monitor.Tasks;
    using Repository;

    public class CheckApprenticeshipApplicationsIncorrectlyInDraft : IMonitorTask
    {
        private readonly IApprenticeshipApplicationDiagnosticsRepository _applicationDiagnosticsRepository;
        private readonly IMessageBus _messageBus;
        private readonly ILegacyApplicationStatusesProvider _legacyApplicationStatusesProvider;
        private readonly ILogService _logger;

        public CheckApprenticeshipApplicationsIncorrectlyInDraft(IApprenticeshipApplicationDiagnosticsRepository applicationDiagnosticsRepository, IMessageBus messageBus, ILegacyApplicationStatusesProvider legacyApplicationStatusesProvider, ILogService logger)
        {
            _applicationDiagnosticsRepository = applicationDiagnosticsRepository;
            _messageBus = messageBus;
            _legacyApplicationStatusesProvider = legacyApplicationStatusesProvider;
            _logger = logger;
        }

        public string TaskName
        {
            get { return "Check Draft Applications With Applied Date"; }
        }

        public void Run()
        {
            var sb = new StringBuilder("The following actions were taken to resolve issues with apprenticeship applications:");
            sb.AppendLine();

            var applicationsToCheck = _applicationDiagnosticsRepository.GetDraftApplicationsWithAppliedDate().ToList();

            foreach (var application in applicationsToCheck)
            {
                var applicationStatusSummaries = _legacyApplicationStatusesProvider.GetCandidateApplicationStatuses(application.Candidate);
                var applicationDetail = application.ApprenticeshipApplicationDetail;
                var applicationStatusSummary = applicationStatusSummaries.SingleOrDefault(s => s.LegacyVacancyId == applicationDetail.Vacancy.Id);
                if (applicationStatusSummary == null)
                {
                    if (applicationDetail.Status != ApplicationStatuses.Submitting)
                    {
                        _applicationDiagnosticsRepository.UpdateApplicationStatus(applicationDetail, ApplicationStatuses.Submitting);
                    }

                    var message = new SubmitApprenticeshipApplicationRequest
                    {
                        ApplicationId = applicationDetail.EntityId
                    };

                    _messageBus.PublishMessage(message);

                    _logger.Warn("Could not patch apprenticeship application id: {0} with legacy id as no matching application status summary was found. Re-queued instead", applicationDetail.EntityId);
                }
                else
                {
                    if (applicationDetail.Status != ApplicationStatuses.Submitted)
                    {
                        _applicationDiagnosticsRepository.UpdateApplicationStatus(applicationDetail, ApplicationStatuses.Submitted);
                    }
                    _applicationDiagnosticsRepository.UpdateLegacyApplicationId(applicationDetail, applicationStatusSummary.LegacyApplicationId);
                    _logger.Info("Patching apprenticeship application id: {0} with legacy id: {1}", applicationDetail.EntityId, applicationStatusSummary.LegacyApplicationId);
                }
            }
        }
    }
}