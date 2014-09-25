namespace SFA.Apprenticeships.Application.VacancyEtl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Messaging;
    using Entities;
    using NLog;

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
            Logger.Debug("Loading vacancy counts from NAS Gateway");

            var vacancyPageCount = _vacancyIndexDataProvider.GetVacancyPageCount(VacancyLocationType.National);
            Logger.Debug("Loaded vacancy page count of: {0} from NAS Gateway", vacancyPageCount);
         
            var vacancySumaries = BuildVacancySummaryPages(scheduledQueueMessage.ExpectedExecutionTime, vacancyPageCount).ToList();

            // Only delete from queue once we have all vacancies from the service without error.
            _processControlQueue.DeleteMessage(scheduledQueueMessage.MessageId, scheduledQueueMessage.PopReceipt);

            Parallel.ForEach(
                vacancySumaries,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                vacancySummaryPage => _messageBus.PublishMessage(vacancySummaryPage));

            Logger.Debug("Posted {0} vacancy summary pages from NAS Gateway to queue", vacancySumaries.Count());
        }
      
        public void QueueVacancySummaries(VacancySummaryPage vacancySummaryPage)
        {
            Logger.Debug("Loading NAS Gateway vacancy search page number: {0}", vacancySummaryPage.PageNumber);

            var vacancies = _vacancyIndexDataProvider.GetVacancySummaries(vacancySummaryPage.VacancyLocation, vacancySummaryPage.PageNumber).ToList();
            var vacanciesExtended = _mapper.Map<IEnumerable<VacancySummary>, IEnumerable<VacancySummaryUpdate>>(vacancies);

            Logger.Debug("Loaded and transformed NAS Gateway vacancy search page number: {0}", vacancySummaryPage.PageNumber);

            Parallel.ForEach(
                vacanciesExtended,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                vacancySummaryExtended =>
                {
                    vacancySummaryExtended.ScheduledRefreshDateTime = vacancySummaryPage.ScheduledRefreshDateTime;
                    _messageBus.PublishMessage(vacancySummaryExtended);
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
                    ScheduledRefreshDateTime = scheduledRefreshDateTime,
                    VacancyLocation = VacancyLocationType.National
                };

                vacancySumaries.Add(vacancySummaryPage);
            }

            return vacancySumaries;
        }

        #endregion
    }
}