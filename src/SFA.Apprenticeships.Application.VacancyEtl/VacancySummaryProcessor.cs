namespace SFA.Apprenticeships.Application.VacancyEtl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Mappers;
    using Interfaces.Messaging;
    using Interfaces.Vacancy;
    using Entities;
    using Domain.Entities.Vacancy;

    public class VacancySummaryProcessor : IVacancySummaryProcessor
    {
        private readonly IMessageBus _bus;
        private readonly IVacancyProvider _vacancyProvider;
        private readonly IMessageService<StorageQueueMessage> _messagingService;
        private readonly IMapper _mapper;

        public VacancySummaryProcessor(IMessageBus bus, 
                                        IVacancyProvider vacancyProvider, 
                                        IMessageService<StorageQueueMessage> messagingService,
                                        IMapper mapper)
        {
            _bus = bus;
            _vacancyProvider = vacancyProvider;
            _messagingService = messagingService;
            _mapper = mapper;
        }

        public void QueueVacancyPages(StorageQueueMessage scheduledQueueMessage)
        {

            var nationalCount = _vacancyProvider.GetVacancyPageCount(VacancyLocationType.National);
            var nonNationalCount = _vacancyProvider.GetVacancyPageCount(VacancyLocationType.NonNational);
            var vacancySumaries = BuildVacancySummaryPages(Guid.Parse(scheduledQueueMessage.ClientRequestId), nationalCount, nonNationalCount);

            // Only delete from queue once we have all vacanies from the services without error.
            _messagingService.DeleteMessage(scheduledQueueMessage.MessageId, scheduledQueueMessage.PopReceipt);

            Parallel.ForEach(
                vacancySumaries,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                vacancySummaryPage => _bus.PublishMessage(vacancySummaryPage));
        }

        private IEnumerable<VacancySummaryPage> BuildVacancySummaryPages(Guid updateReferenceId, int nationalCount, int nonNationalCount)
        {
            var totalCount = nationalCount + nonNationalCount;
            var vacancySumaries = new List<VacancySummaryPage>(totalCount);

            for (var i = 1; i <= nationalCount; i++)
            {
                var vacancySummaryPage = new VacancySummaryPage
                {
                    PageNumber = i,
                    TotalPages = totalCount,
                    UpdateReference = updateReferenceId,
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
                    UpdateReference = updateReferenceId,
                    VacancyLocation = VacancyLocationType.NonNational
                };

                vacancySumaries.Add(vacancySummaryPage);
            }

            return vacancySumaries;
        }

        public void QueueVacancySummaries(VacancySummaryPage vacancySummaryPage)
        {
            try
            {
                var vacancies = _vacancyProvider.GetVacancySummary(vacancySummaryPage.VacancyLocation, vacancySummaryPage.PageNumber).ToList();
                var vacanciesExtended = _mapper.Map<IEnumerable<VacancySummary>, IEnumerable<VacancySummaryUpdate>>(vacancies);

                Parallel.ForEach(
                    vacanciesExtended,
                    new ParallelOptions() { MaxDegreeOfParallelism = 5 },
                    vacancySummaryExtended =>
                    {
                        vacancySummaryExtended.UpdateReference = vacancySummaryPage.UpdateReference;
                        _bus.PublishMessage(vacancySummaryExtended);
                    });
            }
            catch (Exception ex)
            {
                // TODO::High::Log this error
                throw;
            }
        }
    }
}
