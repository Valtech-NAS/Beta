namespace SFA.Apprenticeships.Application.VacancyEtl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Interfaces.Mapping;
    using Interfaces.Messaging;
    using Entities;
    using Domain.Entities.Vacancy;

    public class VacancySummaryProcessor : IVacancySummaryProcessor
    {
        private readonly IMessageBus _bus;
        private readonly IVacancyIndexDataProvider _vacancyIndexDataProvider;
        private readonly IMapper _mapper;
        private readonly IProcessControlQueue<StorageQueueMessage> _processControlQueue;

        public VacancySummaryProcessor(IMessageBus bus, 
                                       IVacancyIndexDataProvider vacancyIndexDataProvider, 
                                       IMapper mapper,
                                       IProcessControlQueue<StorageQueueMessage> processControlQueue)
        {
            _bus = bus;
            _vacancyIndexDataProvider = vacancyIndexDataProvider;
            _mapper = mapper;
            _processControlQueue = processControlQueue;
        }

        public void QueueVacancyPages(StorageQueueMessage scheduledQueueMessage)
        {

            var nationalCount = _vacancyIndexDataProvider.GetVacancyPageCount(VacancyLocationType.National);
            var nonNationalCount = _vacancyIndexDataProvider.GetVacancyPageCount(VacancyLocationType.NonNational);
            var vacancySumaries = BuildVacancySummaryPages(scheduledQueueMessage.ExpectedExecutionTime, nationalCount, nonNationalCount);

            // Only delete from queue once we have all vacancies from the service without error.
            _processControlQueue.DeleteMessage(scheduledQueueMessage.MessageId, scheduledQueueMessage.PopReceipt);

            Parallel.ForEach(
                vacancySumaries,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                vacancySummaryPage => _bus.PublishMessage(vacancySummaryPage));
        }

        private IEnumerable<VacancySummaryPage> BuildVacancySummaryPages(DateTime scheduledRefreshDateTime, int nationalCount, int nonNationalCount)
        {
            var totalCount = nationalCount + nonNationalCount;
            var vacancySumaries = new List<VacancySummaryPage>(totalCount);

            for (var i = 1; i <= nationalCount; i++)
            {
                var vacancySummaryPage = new VacancySummaryPage
                {
                    PageNumber = i,
                    TotalPages = totalCount,
                    ScheduledRefreshDateTime = scheduledRefreshDateTime,
                    VacancyLocation = VacancyLocationType.National
                };

                vacancySumaries.Add(vacancySummaryPage);
            }

            for (var i = nationalCount + 1; i <= totalCount; i++)
            {
                var vacancySummaryPage = new VacancySummaryPage
                {
                    PageNumber = i,
                    TotalPages = totalCount,
                    ScheduledRefreshDateTime = scheduledRefreshDateTime,
                    VacancyLocation = VacancyLocationType.NonNational
                };

                vacancySumaries.Add(vacancySummaryPage);
            }

            return vacancySumaries;
        }

        public void QueueVacancySummaries(VacancySummaryPage vacancySummaryPage)
        {
            var vacancies = _vacancyIndexDataProvider.GetVacancySummaries(vacancySummaryPage.VacancyLocation, vacancySummaryPage.PageNumber).ToList();
            var vacanciesExtended = _mapper.Map<IEnumerable<VacancySummary>, IEnumerable<VacancySummaryUpdate>>(vacancies);

            Parallel.ForEach(
                vacanciesExtended,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                vacancySummaryExtended =>
                {
                    vacancySummaryExtended.ScheduledRefreshDateTime = vacancySummaryPage.ScheduledRefreshDateTime;
                    _bus.PublishMessage(vacancySummaryExtended);
                });
        }
    }
}
