namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System.Threading.Tasks;
    using Application.VacancyEtl;
    using Application.VacancyEtl.Entities;
    using Azure.Common.Messaging;
    using Domain.Interfaces.Messaging;
    using Elastic.Common.Entities;
    using NLog;
    using VacancyIndexer;

    public class VacancyEtlControlQueueConsumer : AzureControlQueueConsumer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary> _apprenticeshipIndexer;
        private readonly IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary> _trainseeshipIndexer;
        private readonly IVacancySummaryProcessor _vacancySummaryProcessor;

        public VacancyEtlControlQueueConsumer(IProcessControlQueue<StorageQueueMessage> messageService,
            IVacancySummaryProcessor vacancySummaryProcessor,
            IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary> apprenticeshipIndexer, 
            IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary> trainseeshipIndexer) : base(messageService, "Vacancy ETL")
        {
            _vacancySummaryProcessor = vacancySummaryProcessor;
            _apprenticeshipIndexer = apprenticeshipIndexer;
            _trainseeshipIndexer = trainseeshipIndexer;
        }

        public Task CheckScheduleQueue()
        {
            return Task.Run(() =>
            {
                var latestScheduledMessage = GetLatestQueueMessage();

                if (latestScheduledMessage == null)
                {
                    Logger.Debug("No scheduled message found on control queue");
                    return;
                }

                Logger.Info("Calling vacancy indexer service to create scheduled index");

                _apprenticeshipIndexer.CreateScheduledIndex(latestScheduledMessage.ExpectedExecutionTime);
                _trainseeshipIndexer.CreateScheduledIndex(latestScheduledMessage.ExpectedExecutionTime);

                Logger.Info("Calling vacancy summary processor to queue vacancy pages");

                _vacancySummaryProcessor.QueueVacancyPages(latestScheduledMessage);

                Logger.Info("Scheduled index created and vacancy pages queued");
            });
        }
    }
}