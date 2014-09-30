namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System;
    using System.Threading.Tasks;
    using Azure.Common.Messaging;
    using Domain.Interfaces.Messaging;
    using Application.VacancyEtl;
    using VacancyIndexer;

    public class VacancyEtlControlQueueConsumer : AzureControlQueueConsumer
    {
        private readonly IVacancySummaryProcessor _vacancySummaryProcessor;
        private readonly IVacancyIndexerService _vacancyIndexerService;

        public VacancyEtlControlQueueConsumer(IProcessControlQueue<StorageQueueMessage> messageService, 
                                        IVacancySummaryProcessor vacancySummaryProcessor,
                                        IVacancyIndexerService vacancyIndexerService) : base(messageService, "Vacancy ETL")
        {
            _vacancySummaryProcessor = vacancySummaryProcessor;
            _vacancyIndexerService = vacancyIndexerService;
        }

        public Task CheckScheduleQueue()
        {
            return Task.Run(() =>
            {
                var latestScheduledMessage = GetLatestQueueMessage();
              
                if (latestScheduledMessage != null)
                {                    
                    _vacancyIndexerService.CreateScheduledIndex(latestScheduledMessage.ExpectedExecutionTime);
                    _vacancySummaryProcessor.QueueVacancyPages(latestScheduledMessage);
                }
            });
        }
    }
}
