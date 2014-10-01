namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System;
    using System.Threading.Tasks;
    using Application.VacancyEtl;
    using Azure.Common.Messaging;
    using Domain.Interfaces.Messaging;
    using NLog;
    using VacancyIndexer;

    public class VacancyEtlControlQueueConsumer : AzureControlQueueConsumer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IVacancyIndexerService _vacancyIndexerService;
        private readonly IVacancySummaryProcessor _vacancySummaryProcessor;

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
                Logger.Debug("Called VacancyEtlControlQueueConsumer at {0} ",
                       DateTime.UtcNow);
                var latestScheduledMessage = GetLatestQueueMessage();

                if (latestScheduledMessage == null)
                {
                    Logger.Debug("No scheduled message found on control queue at {0} ",
                       DateTime.UtcNow);
                    return;
                }

                Logger.Debug("Calling vacancy indexer service to create scheduled index  at  {0} ",
                    latestScheduledMessage.ExpectedExecutionTime);

                _vacancyIndexerService.CreateScheduledIndex(latestScheduledMessage.ExpectedExecutionTime);

                Logger.Debug("Calling vacancy summary processor to queue vacancy pages  at  {0} ",
                    latestScheduledMessage.ExpectedExecutionTime);

                _vacancySummaryProcessor.QueueVacancyPages(latestScheduledMessage);

                Logger.Debug("Scheduled index created and vacancy pages queued at {0} ",
                    latestScheduledMessage.ExpectedExecutionTime);
            });
        }
    }
}