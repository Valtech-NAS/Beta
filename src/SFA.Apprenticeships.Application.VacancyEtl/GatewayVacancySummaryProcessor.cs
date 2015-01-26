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
    using NLog;
    using Domain.Entities.Vacancies.Apprenticeships;

    public class GatewayVacancySummaryProcessor : IVacancySummaryProcessor
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IMessageBus _messageBus;
        private readonly IVacancyIndexDataProvider _vacancyIndexDataProvider;
        private readonly IMapper _mapper;
        private readonly IProcessControlQueue<StorageQueueMessage> _processControlQueue;

        public GatewayVacancySummaryProcessor(IMessageBus messageBus,
                                       IVacancyIndexDataProvider vacancyIndexDataProvider,
                                       IMapper mapper,
                                       IProcessControlQueue<StorageQueueMessage> processControlQueue)
        {
            _messageBus = messageBus;
            _vacancyIndexDataProvider = vacancyIndexDataProvider;
            _mapper = mapper;
            _processControlQueue = processControlQueue;
        }

        public void QueueVacancyPages(StorageQueueMessage scheduledQueueMessage)
        {
            Logger.Debug("Retrieving vacancy summary page count");

            var vacancyPageCount = _vacancyIndexDataProvider.GetVacancyPageCount();

            Logger.Info("Retrieved vacancy summary page count of {0}", vacancyPageCount);

            if (vacancyPageCount == 0)
            {
                Logger.Warn("Expected vacancy page count to be greater than zero. Indexes will not be created successfully");
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

            Logger.Info("Queued {0} vacancy summary pages", vacancySumaries.Count());
        }
      
        public void QueueVacancySummaries(VacancySummaryPage vacancySummaryPage)
        {
            Logger.Debug("Retrieving vacancy search page number: {0}/{1}", vacancySummaryPage.PageNumber, vacancySummaryPage.TotalPages);

            var vacancies = _vacancyIndexDataProvider.GetVacancySummaries(vacancySummaryPage.PageNumber);
            var apprenticeshipsExtended = _mapper.Map<IEnumerable<ApprenticeshipSummary>, IEnumerable<ApprenticeshipSummaryUpdate>>(vacancies.ApprenticeshipSummaries).ToList();
            var traineeshipsExtended = _mapper.Map<IEnumerable<TraineeshipSummary>, IEnumerable<TraineeshipSummaryUpdate>>(vacancies.TraineeshipSummaries).ToList();

            Logger.Debug("Retrieved vacancy search page number: {0}/{1} with {2} apprenticeships and {3} traineeships", vacancySummaryPage.PageNumber, vacancySummaryPage.TotalPages, apprenticeshipsExtended.Count(), traineeshipsExtended.Count());

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

        #region Helpers

        private IEnumerable<VacancySummaryPage> BuildVacancySummaryPages(DateTime scheduledRefreshDateTime, int count)
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