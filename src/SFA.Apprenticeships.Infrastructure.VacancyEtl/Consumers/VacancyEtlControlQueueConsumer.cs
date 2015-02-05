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
        }

        public Task CheckScheduleQueue()
        {
            return Task.Run(() =>
            {
                var latestScheduledMessage = GetLatestQueueMessage();

                if (latestScheduledMessage == null)
                {
                    return;
                }

                _apprenticeshipIndexer.CreateScheduledIndex(latestScheduledMessage.ExpectedExecutionTime);
                _trainseeshipIndexer.CreateScheduledIndex(latestScheduledMessage.ExpectedExecutionTime);

                _vacancySummaryProcessor.QueueVacancyPages(latestScheduledMessage);
            });
        }
    }
}
