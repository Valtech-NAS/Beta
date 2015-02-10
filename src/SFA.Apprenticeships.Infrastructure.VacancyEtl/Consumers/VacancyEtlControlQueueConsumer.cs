namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System.Threading.Tasks;
    using Application.Interfaces.Logging;
    using Application.VacancyEtl;
    using Application.VacancyEtl.Entities;
    using Azure.Common.Messaging;
    using Domain.Interfaces.Messaging;
    using Elastic.Common.Entities;
    using VacancyIndexer;

    public class VacancyEtlControlQueueConsumer : AzureControlQueueConsumer
    {
        private readonly IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary> _apprenticeshipIndexer;
        private readonly IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary> _trainseeshipIndexer;
        private readonly ILogService _logger;
        private readonly IVacancySummaryProcessor _vacancySummaryProcessor;

        public VacancyEtlControlQueueConsumer(IProcessControlQueue<StorageQueueMessage> messageService,
            IVacancySummaryProcessor vacancySummaryProcessor,
            IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary> apprenticeshipIndexer,
            IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary> trainseeshipIndexer, ILogService logger)
            : base(messageService, logger, "Vacancy ETL")
        {
            _vacancySummaryProcessor = vacancySummaryProcessor;
            _apprenticeshipIndexer = apprenticeshipIndexer;
            _trainseeshipIndexer = trainseeshipIndexer;
            _logger = logger;
        }

        public Task CheckScheduleQueue()
        {
            return Task.Run(() =>
            {
                var latestScheduledMessage = GetLatestQueueMessage();

                if (latestScheduledMessage == null)
                {
                    _logger.Debug("No scheduled message found on control queue");
                    return;
                }

                _logger.Info("Calling vacancy indexer service to create scheduled index");

                _apprenticeshipIndexer.CreateScheduledIndex(latestScheduledMessage.ExpectedExecutionTime);
                _trainseeshipIndexer.CreateScheduledIndex(latestScheduledMessage.ExpectedExecutionTime);

                _logger.Info("Calling vacancy summary processor to queue vacancy pages");
                
                _vacancySummaryProcessor.QueueVacancyPages(latestScheduledMessage);

                _logger.Info("Scheduled index created and vacancy pages queued");
            });
        }
    }
}
