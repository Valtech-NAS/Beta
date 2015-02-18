namespace SFA.Apprenticeships.Application.ApplicationUpdate
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Interfaces.Logging;
    using Strategies;

    public class ApplicationStatusProcessor : IApplicationStatusProcessor
    {
        private readonly ILogService _logger;
        private readonly IConfigurationManager _configurationManager;

        private readonly ILegacyApplicationStatusesProvider _legacyApplicationStatusesProvider;
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IApplicationStatusUpdateStrategy _applicationStatusUpdateStrategy;
        private readonly IMessageBus _messageBus;
        private int? _applicationStatusExtractWindow;

        public ApplicationStatusProcessor(ILegacyApplicationStatusesProvider legacyApplicationStatusesProvider,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository,
            ICandidateReadRepository candidateReadRepository,
            IApplicationStatusUpdateStrategy applicationStatusUpdateStrategy,
            IMessageBus messageBus, ILogService logger, IConfigurationManager configurationManager)
        {
            _legacyApplicationStatusesProvider = legacyApplicationStatusesProvider;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
            _candidateReadRepository = candidateReadRepository;
            _applicationStatusUpdateStrategy = applicationStatusUpdateStrategy;
            _messageBus = messageBus;
            _logger = logger;
            _configurationManager = configurationManager;
        }

        private int ApplicationStatusExtractWindow
        {
            get
            {
                if (!_applicationStatusExtractWindow.HasValue)
                {
                    _applicationStatusExtractWindow = _configurationManager.GetCloudAppSetting<int>("ApplicationStatusExtractWindow");
                }

                return _applicationStatusExtractWindow.Value;
            }
        }

        public void QueueApplicationStatusesPages()
        {
            _logger.Debug("Starting to queue application summary status update pages");

            var pageCount = _legacyApplicationStatusesProvider.GetApplicationStatusesPageCount(ApplicationStatusExtractWindow);

            if (pageCount == 0)
            {
                _logger.Debug("No application status update pages to queue");
                return;
            }

            var pages = Enumerable.Range(1, pageCount)
                .Select(i => new ApplicationUpdatePage {PageNumber = i, TotalPages = pageCount});

            _logger.Debug("Queuing {0} application summary status update pages", pageCount);

            Parallel.ForEach(
                pages,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                page => _messageBus.PublishMessage(page));

            _logger.Debug("Queued {0} application status update pages", pageCount);
        }

        public void QueueApplicationStatuses(ApplicationUpdatePage applicationStatusSummaryPage)
        {
            _logger.Debug("Starting to queue application status updates for page {0} of {1}", applicationStatusSummaryPage.PageNumber, applicationStatusSummaryPage.TotalPages);

            // retrieve page of status updates from legacy... then queue each one for subsequent processing
            var applicationStatusSummaries = _legacyApplicationStatusesProvider.GetAllApplicationStatuses(applicationStatusSummaryPage.PageNumber, ApplicationStatusExtractWindow).ToList();

            if (!applicationStatusSummaries.Any())
            {
                _logger.Debug("No application status updates to queue");
                return;
            }

            _logger.Debug("Queuing {0} application status updates for page {1} of {2}", applicationStatusSummaries.Count(), applicationStatusSummaryPage.PageNumber, applicationStatusSummaryPage.TotalPages);

            Parallel.ForEach(
                applicationStatusSummaries,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                applicationStatusSummary => _messageBus.PublishMessage(applicationStatusSummary));

            _logger.Debug("Queued {0} application status updates for page {1} of {2}", applicationStatusSummaries.Count(), applicationStatusSummaryPage.PageNumber, applicationStatusSummaryPage.TotalPages);
        }

        public void ProcessApplicationStatuses(ApplicationStatusSummary applicationStatusSummary)
        {
            _logger.Debug("Processing application summary status update for application with legacy application ID '{0}'", applicationStatusSummary.LegacyApplicationId);
            
            if (!ProcessApprenticeshipApplication(applicationStatusSummary) && !ProcessTraineeshipApplication(applicationStatusSummary))
            {
                _logger.Warn("Unable to find/update apprenticeship or traineeship application status for application with legacy application ID '{0}' and application ID '{1}'", applicationStatusSummary.LegacyApplicationId, applicationStatusSummary.ApplicationId);
            }
        }

        public void ProcessApplicationStatuses(VacancyStatusSummary vacancyStatusSummary)
        {
            //TODO: 1.6: extract to strategy
            // propagate current vacancy state to all draft applications for the vacancy
            var applicationSummaries = _apprenticeshipApplicationReadRepository.GetApplicationSummaries(vacancyStatusSummary.LegacyVacancyId);

            var applicationStatusSummaries = applicationSummaries
                .Where(applicationSummary => applicationSummary.Status == ApplicationStatuses.Draft)
                .Select(applicationSummary =>
                    new ApplicationStatusSummary
                    {
                        ApplicationId = applicationSummary.ApplicationId,
                        ApplicationStatus = applicationSummary.Status,
                        LegacyApplicationId = applicationSummary.LegacyApplicationId,
                        LegacyVacancyId = applicationSummary.LegacyVacancyId,
                        VacancyStatus = vacancyStatusSummary.VacancyStatus,
                        ClosingDate = vacancyStatusSummary.ClosingDate,
                        UnsuccessfulReason = applicationSummary.UnsuccessfulReason
                    });

            Parallel.ForEach(
                applicationStatusSummaries,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                applicationStatusSummary => _messageBus.PublishMessage(applicationStatusSummary));
        }

        private bool ProcessApprenticeshipApplication(ApplicationStatusSummary applicationStatusSummary)
        {
            var apprenticeshipApplicationDetail = default(ApprenticeshipApplicationDetail);

            if (applicationStatusSummary.ApplicationId != Guid.Empty)
            {
                apprenticeshipApplicationDetail = _apprenticeshipApplicationReadRepository.Get(applicationStatusSummary.ApplicationId);
            }

            if (apprenticeshipApplicationDetail == null && applicationStatusSummary.LegacyApplicationId != 0)
            {
                apprenticeshipApplicationDetail = _apprenticeshipApplicationReadRepository.Get(applicationStatusSummary.LegacyApplicationId);
            }

            if (apprenticeshipApplicationDetail == null && applicationStatusSummary.LegacyCandidateId != 0)
            {
                // in some cases the application can't be found using the application IDs so use legacy candidate and vacancy IDs
                var candidate = _candidateReadRepository.Get(applicationStatusSummary.LegacyCandidateId);
                apprenticeshipApplicationDetail = _apprenticeshipApplicationReadRepository.GetForCandidate(candidate.EntityId, applicationStatusSummary.LegacyVacancyId);
            } 

            if (apprenticeshipApplicationDetail == null)
            {
                return false; // not necessarily an error as may be a traineeship
            }

            _applicationStatusUpdateStrategy.Update(apprenticeshipApplicationDetail, applicationStatusSummary);

            return true;
        }

        private bool ProcessTraineeshipApplication(ApplicationStatusSummary applicationStatusSummary)
        {
            var traineeshipApplicationDetail = _traineeshipApplicationReadRepository.Get(applicationStatusSummary.LegacyApplicationId);

            if (traineeshipApplicationDetail == null && applicationStatusSummary.LegacyCandidateId != 0)
            {
                // in some cases the application can't be found using the application IDs so use legacy candidate and vacancy IDs
                var candidate = _candidateReadRepository.Get(applicationStatusSummary.LegacyCandidateId);
                traineeshipApplicationDetail = _traineeshipApplicationReadRepository.GetForCandidate(candidate.EntityId, applicationStatusSummary.LegacyVacancyId);
            }

            if (traineeshipApplicationDetail == null)
            {
                return false;
            }

            _applicationStatusUpdateStrategy.Update(traineeshipApplicationDetail, applicationStatusSummary);

            return true;
        }
    }
}
