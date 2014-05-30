namespace SFA.Apprenticeships.Application.VacancyEtl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SFA.Apprenticeships.Application.Interfaces.Messaging;
    using SFA.Apprenticeships.Application.Interfaces.Vacancy;
    using SFA.Apprenticeships.Application.VacancyEtl.Entities;
    using SFA.Apprenticeships.Domain.Entities.Vacancy;

    public class VacancySummaryProcessor : IVacancySummaryProcessor
    {
        private readonly IMessageBus _bus;
        private readonly IVacancySummaryService _vacancySummaryService;
        private readonly IMessageService<StorageQueueMessage> _azureMessageService;

        public VacancySummaryProcessor(IMessageBus bus, IVacancySummaryService vacancySummaryService, IMessageService<StorageQueueMessage> azureMessageService)
        {
            _bus = bus;
            _vacancySummaryService = vacancySummaryService;
            _azureMessageService = azureMessageService;
        }

        public void QueueVacancyPages(StorageQueueMessage scheduledQueueMessage)
        {

            var nationalCount = _vacancySummaryService.GetVacancyPageCount(VacancyLocationType.National);
            var nonNationalCount = _vacancySummaryService.GetVacancyPageCount(VacancyLocationType.NonNational);
            var vacancySumaries = BuildVacancySummaries(Guid.Parse(scheduledQueueMessage.ClientRequestId), nationalCount, nonNationalCount);

            // Only delete from queue once we have all vacanies from the services without error.
            _azureMessageService.DeleteMessage(scheduledQueueMessage.MessageId);

            Parallel.ForEach(
                vacancySumaries,
                new ParallelOptions { MaxDegreeOfParallelism = 10 },
                vacancy => _bus.PublishMessage(vacancy));
        }

        private IEnumerable<VacancySummaryPage> BuildVacancySummaries(Guid updateReferenceId, int nationalCount, int nonNationalCount)
        {
            var totalCount = nationalCount + nonNationalCount;
            var vacancySumaries = new List<VacancySummaryPage>(totalCount);

            for (int i = 1; i <= nationalCount; i++)
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

            for (int i = nationalCount + 1; i <= totalCount; i++)
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
                var vacancies = _vacancySummaryService.GetVacancySummary(vacancySummaryPage.VacancyLocation, vacancySummaryPage.PageNumber).ToList();
                //vacancies.ForEach(x => x.UpdateReference = vacancySummaryPage.UpdateReference);

                Parallel.ForEach(
                    vacancies,
                    new ParallelOptions() { MaxDegreeOfParallelism = 5 },
                    vacancySummary => _bus.PublishMessage(vacancySummary));
            }
            catch (Exception ex)
            {
                // TODO::High::Log this error
            }
        }
    }
}
