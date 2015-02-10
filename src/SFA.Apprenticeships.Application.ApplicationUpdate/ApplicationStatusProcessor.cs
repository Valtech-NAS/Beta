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
                _logger.Warn("Unable to find/update apprenticeship or traineeship application status for application with legacy application ID '{0}' and application ID '{1}'", applicationStatusSummary.LegacyApplicationId, applicationStatusSummary.ApplicationId);
            }
        }

        public void ProcessApplicationStatuses(VacancyStatusSummary vacancyStatusSummary)
        {
            try
            {
                QueueApprenticeshipApplicationStatusSummaries(vacancyStatusSummary);
                QueueTraineeshipApplicationStatusSummaries(vacancyStatusSummary);
            }
            catch (Exception ex)
            {
                _logger.Error("Error processing application statuses", ex);
            }
        }

        private void QueueApprenticeshipApplicationStatusSummaries(VacancyStatusSummary vacancyStatusSummary)
        {
            var applicationSummaries = _apprenticeshipApplicationReadRepository.GetApplicationSummaries(vacancyStatusSummary.LegacyVacancyId);
            var applicationSummaryStates = new[] { ApplicationStatuses.Draft, ApplicationStatuses.Submitting, ApplicationStatuses.Submitted };

            var applicationStatusSummaries = applicationSummaries
                .Select(applicationSummary =>
                    new ApplicationStatusSummary
                    {
                        ApplicationId = applicationSummary.ApplicationId,
                        LegacyApplicationId = applicationSummary.LegacyApplicationId,
                        LegacyVacancyId = applicationSummary.LegacyVacancyId,
                        ApplicationStatus = applicationSummary.Status,
                        VacancyStatus = vacancyStatusSummary.VacancyStatus,
                        ClosingDate = vacancyStatusSummary.ClosingDate,
                        UnsuccessfulReason = applicationSummary.UnsuccessfulReason
                    })
                .Where(applicationSummary => applicationSummaryStates.Contains(applicationSummary.ApplicationStatus));

            Parallel.ForEach(
                applicationStatusSummaries,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                applicationStatusSummary => _messageBus.PublishMessage(applicationStatusSummary));
        }

        private void QueueTraineeshipApplicationStatusSummaries(VacancyStatusSummary vacancyStatusSummary)
        {
            var applicationSummaries = _traineeshipApplicationReadRepository.GetApplicationSummaries(vacancyStatusSummary.LegacyVacancyId);

            var applicationStatusSummaries = applicationSummaries
                .Select(applicationSummary =>
                    new ApplicationStatusSummary
                    {
                        ApplicationId = applicationSummary.ApplicationId,
                        LegacyApplicationId = applicationSummary.LegacyApplicationId,
                        LegacyVacancyId = applicationSummary.LegacyVacancyId,
                        ApplicationStatus = ApplicationStatuses.Submitted,
                        VacancyStatus = vacancyStatusSummary.VacancyStatus,
                        ClosingDate = vacancyStatusSummary.ClosingDate
                    });

            // TODO: Think how to reduce applications that need processing based on their status.
            // TODO: Think about why we are processing traineeship application status updates.
            Parallel.ForEach(
                applicationStatusSummaries,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                applicationStatusSummary => _messageBus.PublishMessage(applicationStatusSummary));
        }

        private bool ProcessApprenticeshipsApplication(ApplicationStatusSummary applicationStatusSummary)
        {
            // TODO: get application by Legacy Candidate and Legacy Vacancy Id. This will enable Legacy Application Id to be 'back-filled'.
            var apprenticeshipApplicationDetail = default(ApprenticeshipApplicationDetail);

            if (applicationStatusSummary.ApplicationId != Guid.Empty)
            {
                apprenticeshipApplicationDetail = _apprenticeshipApplicationReadRepository.Get(applicationStatusSummary.ApplicationId);
            }
            else if (applicationStatusSummary.LegacyApplicationId != 0)
            {
                apprenticeshipApplicationDetail = _apprenticeshipApplicationReadRepository.Get(applicationStatusSummary.LegacyApplicationId);
            }

            if (apprenticeshipApplicationDetail == null)
            {
                return false;
            }

            return _applicationStatusUpdateStrategy.Update(apprenticeshipApplicationDetail, applicationStatusSummary);
        }

        private bool ProcessTraineeshipsApplication(ApplicationStatusSummary applicationStatusSummary)
        {
            // TODO: get application by Legacy Candidate and Legacy Vacancy Id. This will enable Legacy Application Id to be 'back-filled'.
            var traineeshipApplicationDetail = _traineeshipApplicationReadRepository.Get(applicationStatusSummary.LegacyApplicationId);

            if (traineeshipApplicationDetail == null)
            {
                return false;
            }

            return _applicationStatusUpdateStrategy.Update(traineeshipApplicationDetail, applicationStatusSummary);
        }
    }
}
