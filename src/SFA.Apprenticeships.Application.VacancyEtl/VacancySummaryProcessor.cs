namespace SFA.Apprenticeships.Application.VacancyEtl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Entities.Vacancies.Traineeships;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Messaging;
    using Entities;
    using Interfaces.Logging;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Configuration;

    public class VacancySummaryProcessor : IVacancySummaryProcessor
    {
        private readonly ILogService _logger;

        private const string VacancyAboutToExpireNotificationHours = "VacancyAboutToExpireNotificationHours";
        private readonly int _vacancyAboutToExpireNotificationHours;

        private readonly IMessageBus _messageBus;
        private readonly IVacancyIndexDataProvider _vacancyIndexDataProvider;
        private readonly IMapper _mapper;
        private readonly IProcessControlQueue<StorageQueueMessage> _processControlQueue;

        public VacancySummaryProcessor(IMessageBus messageBus,
                                       IVacancyIndexDataProvider vacancyIndexDataProvider,
                                       IMapper mapper,
                                       IProcessControlQueue<StorageQueueMessage> processControlQueue, 
                                       IConfigurationManager configurationManager, ILogService logger)
        {
            _messageBus = messageBus;
            _vacancyIndexDataProvider = vacancyIndexDataProvider;
            _mapper = mapper;
            _processControlQueue = processControlQueue;
            _logger = logger;
            _vacancyAboutToExpireNotificationHours = configurationManager.GetAppSetting<int>(VacancyAboutToExpireNotificationHours);
        }

        public void QueueVacancyPages(StorageQueueMessage scheduledQueueMessage)
        {
            _logger.Debug("Retrieving vacancy summary page count");

            var vacancyPageCount = _vacancyIndexDataProvider.GetVacancyPageCount();

            _logger.Info("Retrieved vacancy summary page count of {0}", vacancyPageCount);

            if (vacancyPageCount == 0)
            {
                _logger.Warn("Expected vacancy page count to be greater than zero. Indexes will not be created successfully");
                _processControlQueue.DeleteMessage(scheduledQueueMessage.MessageId, scheduledQueueMessage.PopReceipt);
                return;
            }

            var vacancySumaries = BuildVacancySummaryPages(scheduledQueueMessage.ExpectedExecutionTime, vacancyPageCount).ToList();

            // Only delete from queue once we have all vacancies from the service without error.
            _processControlQueue.DeleteMessage(scheduledQueueMessage.MessageId, scheduledQueueMessage.PopReceipt);

            Parallel.ForEach(
                vacancySumaries,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                vacancySummaryPage => _messageBus.PublishMessage(vacancySummaryPage));

            _logger.Info("Queued {0} vacancy summary pages", vacancySumaries.Count());
        }
      
        public void QueueVacancySummaries(VacancySummaryPage vacancySummaryPage)
        {
            _logger.Info("Retrieving vacancy search page number: {0}/{1}", vacancySummaryPage.PageNumber, vacancySummaryPage.TotalPages);

            var vacancies = _vacancyIndexDataProvider.GetVacancySummaries(vacancySummaryPage.PageNumber);
            var apprenticeshipsExtended = _mapper.Map<IEnumerable<ApprenticeshipSummary>, IEnumerable<ApprenticeshipSummaryUpdate>>(vacancies.ApprenticeshipSummaries).ToList();
            var traineeshipsExtended = _mapper.Map<IEnumerable<TraineeshipSummary>, IEnumerable<TraineeshipSummaryUpdate>>(vacancies.TraineeshipSummaries).ToList();

            _logger.Info("Retrieved vacancy search page number: {0}/{1} with {2} apprenticeships and {3} traineeships", vacancySummaryPage.PageNumber, vacancySummaryPage.TotalPages, apprenticeshipsExtended.Count(), traineeshipsExtended.Count());

            Parallel.ForEach(
                apprenticeshipsExtended,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                apprenticeshipExtended =>
                {
                    apprenticeshipExtended.ScheduledRefreshDateTime = vacancySummaryPage.ScheduledRefreshDateTime;
                    _messageBus.PublishMessage(apprenticeshipExtended);
                });

            Parallel.ForEach(
                traineeshipsExtended,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                traineeshipExtended =>
                {
                    traineeshipExtended.ScheduledRefreshDateTime = vacancySummaryPage.ScheduledRefreshDateTime;
                    _messageBus.PublishMessage(traineeshipExtended);
                });
        }

        public void QueueVacancyIfExpiring(ApprenticeshipSummary vacancySummary)
        {
            try
            {
                if (vacancySummary.ClosingDate < DateTime.Now.AddHours(_vacancyAboutToExpireNotificationHours))
                {
                    _logger.Debug("Queueing expiring vacancy");

                    var vacancyAboutToExpireMessage = new VacancyAboutToExpire { Id = vacancySummary.Id };
                    _messageBus.PublishMessage(vacancyAboutToExpireMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.Warn("Failed queueing expiring vacancy {0}", ex, vacancySummary.Id);
            }
        }

        #region Helpers
        private static IEnumerable<VacancySummaryPage> BuildVacancySummaryPages(DateTime scheduledRefreshDateTime, int count)
        {           
            var vacancySumaries = new List<VacancySummaryPage>(count);

            for (var i = 1; i <= count; i++)
            {
                var vacancySummaryPage = new VacancySummaryPage
                {
                    PageNumber = i,
                    TotalPages = count,
                    ScheduledRefreshDateTime = scheduledRefreshDateTime
                };

                vacancySumaries.Add(vacancySummaryPage);
            }

            return vacancySumaries;
        }

        #endregion
    }
}