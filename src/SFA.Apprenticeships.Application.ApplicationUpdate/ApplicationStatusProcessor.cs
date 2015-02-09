namespace SFA.Apprenticeships.Application.ApplicationUpdate
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Interfaces.Logging;
    using Strategies;

    public class ApplicationStatusProcessor : IApplicationStatusProcessor
    {
        private readonly ILogService _logger;

        private readonly ILegacyApplicationStatusesProvider _legacyApplicationStatusesProvider;
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly IApplicationStatusUpdateStrategy _applicationStatusUpdateStrategy;
        private readonly IMessageBus _messageBus;

        public ApplicationStatusProcessor(ILegacyApplicationStatusesProvider legacyApplicationStatusesProvider,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository,
            IApplicationStatusUpdateStrategy applicationStatusUpdateStrategy, 
            IMessageBus messageBus, ILogService logger)
        {
            _legacyApplicationStatusesProvider = legacyApplicationStatusesProvider;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
            _applicationStatusUpdateStrategy = applicationStatusUpdateStrategy;
            _messageBus = messageBus;
            _logger = logger;
        }

        public void QueueApplicationStatusesPages()
        {
            _logger.Debug("Starting to queue application summary status update pages");

            var pageCount = _legacyApplicationStatusesProvider.GetApplicationStatusesPageCount();

            if (pageCount == 0)
            {
                _logger.Debug("No application status update pages to queue");
                return;
            }

            var pages = Enumerable.Range(1, pageCount)
                .Select(i => new ApplicationUpdatePage {PageNumber = i, TotalPages = pageCount});

            _logger.Debug("Queueing {0} application summary status update pages", pageCount);

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
            var applicationStatusSummaries = _legacyApplicationStatusesProvider.GetAllApplicationStatuses(applicationStatusSummaryPage.PageNumber).ToList();

            if (!applicationStatusSummaries.Any())
            {
                _logger.Debug("No application status updates to queue");
                return;
            }

            _logger.Debug("Queueing {0} application status updates for page {1} of {2}", applicationStatusSummaries.Count(), applicationStatusSummaryPage.PageNumber, applicationStatusSummaryPage.TotalPages);

            Parallel.ForEach(
                applicationStatusSummaries,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                applicationStatusSummary => _messageBus.PublishMessage(applicationStatusSummary));

            _logger.Debug("Queued {0} application status updates for page {1} of {2}", applicationStatusSummaries.Count(), applicationStatusSummaryPage.PageNumber, applicationStatusSummaryPage.TotalPages);
        }

        public void ProcessApplicationStatuses(ApplicationStatusSummary applicationStatusSummary)
        {
            _logger.Debug("Processing application summary status update for application with legacy application ID '{0}'", applicationStatusSummary.LegacyApplicationId);
            
            if (!ProcessApprenticeshipsApplication(applicationStatusSummary) && !ProcessTraineeshipsApplication(applicationStatusSummary))
            {
                _logger.Warn("Unable to find/update apprenticeship or traineeship application status for application with legacy application ID '{0}'", applicationStatusSummary.LegacyApplicationId);
            }
        }

        public void ProcessApplicationStatuses(int legacyVacancyId, VacancyStatuses vacancyStatus, DateTime closingDate)
        {
            try
            {
                QueueApprenticeshipApplicationStatusSummaries(legacyVacancyId, vacancyStatus, closingDate);
                QueueTraineeshipApplicationStatusSummaries(legacyVacancyId, vacancyStatus, closingDate);
            }
            catch (Exception ex)
            {
                _logger.Error("Error processing application statuses", ex);
            }
        }

        private void QueueApprenticeshipApplicationStatusSummaries(int legacyVacancyId, VacancyStatuses vacancyStatus, DateTime closingDate)
        {
            var applicationSummaries = _apprenticeshipApplicationReadRepository.GetApplicationSummaries(legacyVacancyId);
            var applicationStatusSummaries =
                applicationSummaries.Select(
                    x =>
                        new ApplicationStatusSummary
                        {
                            ApplicationId = x.ApplicationId,
                            LegacyApplicationId = x.LegacyApplicationId,
                            ApplicationStatus = x.Status,
                            VacancyStatus = vacancyStatus,
                            LegacyVacancyId = x.LegacyVacancyId,
                            ClosingDate = closingDate,
                            UnsuccessfulReason = x.UnsuccessfulReason
                        });

            //TODO: Think how to reduce applications that need processed based on their status.
            Parallel.ForEach(
                applicationStatusSummaries,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                applicationStatusSummary => _messageBus.PublishMessage(applicationStatusSummary));
        }

        private void QueueTraineeshipApplicationStatusSummaries(int legacyVacancyId, VacancyStatuses vacancyStatus, DateTime closingDate)
        {
            var applicationSummaries = _traineeshipApplicationReadRepository.GetApplicationSummaries(legacyVacancyId);
            var applicationStatusSummaries =
                applicationSummaries.Select(
                    x =>
                        new ApplicationStatusSummary
                        {
                            ApplicationId = x.ApplicationId,
                            LegacyApplicationId = x.LegacyApplicationId,
                            ApplicationStatus = ApplicationStatuses.Submitted,
                            VacancyStatus = vacancyStatus,
                            LegacyVacancyId = x.LegacyVacancyId,
                            ClosingDate = closingDate
                        });

            //TODO: Think how to reduce applications that need processed based on their status.
            Parallel.ForEach(
                applicationStatusSummaries,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                applicationStatusSummary => _messageBus.PublishMessage(applicationStatusSummary));
        }

        private bool ProcessApprenticeshipsApplication(ApplicationStatusSummary applicationStatusSummary)
        {
            var apprenticeshipApplicationDetail = _apprenticeshipApplicationReadRepository.Get(applicationStatusSummary.LegacyApplicationId);

            if (apprenticeshipApplicationDetail == null)
            {
                _logger.Debug("Unable to find/update apprenticeship application status for application with legacy application ID '{0}'", applicationStatusSummary.LegacyApplicationId);
                return false;
            }
            return _applicationStatusUpdateStrategy.Update(apprenticeshipApplicationDetail, applicationStatusSummary);
        }

        private bool ProcessTraineeshipsApplication(ApplicationStatusSummary applicationStatusSummary)
        {
            var traineeshipApplicationDetail = _traineeshipApplicationReadRepository.Get(applicationStatusSummary.LegacyApplicationId);

            if (traineeshipApplicationDetail == null)
            {
                _logger.Debug("Unable to find/update traineeship application status for application with legacy application ID '{0}'", applicationStatusSummary.LegacyApplicationId);
                return false;
            }

            return _applicationStatusUpdateStrategy.Update(traineeshipApplicationDetail, applicationStatusSummary);
        }
    }
}
