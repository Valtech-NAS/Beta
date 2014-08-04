namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System;
    using System.Threading.Tasks;
    using Domain.Interfaces.Messaging;
    using NLog;
    using Application.VacancyEtl;
    using Application.VacancyEtl.Entities;
    using VacancyIndexer.Services;

    public class VacancySchedulerConsumer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IProcessControlQueue<StorageQueueMessage> _messageService;
        private readonly IVacancySummaryProcessor _vacancySummaryProcessor;
        private readonly IVacancyIndexerService _vacancyIndexerService;

        public VacancySchedulerConsumer(IProcessControlQueue<StorageQueueMessage> messageService, 
                                        IVacancySummaryProcessor vacancySummaryProcessor,
                                        IVacancyIndexerService vacancyIndexerService)
        {
            _messageService = messageService;
            _vacancySummaryProcessor = vacancySummaryProcessor;
            _vacancyIndexerService = vacancyIndexerService;
        }

        public Task CheckScheduleQueue()
        {
            return Task.Run(() => CheckQueue());
        }

        private void CheckQueue()
        {
            var latestScheduledMessage = GetLatestQueueMessage();

            if (latestScheduledMessage != null)
            {
                _vacancyIndexerService.CreateScheduledIndex(latestScheduledMessage.ExpectedExecutionTime);
                // TODO: NOTIMPL: Check Rabbit procesing queue - should not still be processing messages or there maybe a potential issue.

                Logger.Debug("Scheduled VacancyEtl Message Received at: {0}", DateTime.Now);
                _vacancySummaryProcessor.QueueVacancyPages(latestScheduledMessage);
            }
        }

        private StorageQueueMessage GetLatestQueueMessage()
        {
            Logger.Debug("Checking Vacancy Etl control queue for message");
            var queueMessage = _messageService.GetMessage();

            if (queueMessage == null)
            {
                return null;
            }

            while (true)
            {
                var nextQueueMessage = _messageService.GetMessage();
                if (nextQueueMessage == null)
                {
                    // We have the latest message on the queue.
                    break;
                }

                Logger.Warn("More than 1 Vacancy Etl control message on queue");
                _messageService.DeleteMessage(queueMessage.MessageId, queueMessage.PopReceipt);
                queueMessage = nextQueueMessage;
            }

            Logger.Warn("New Vacancy Etl control message, start full re-index of vacancy data");
            return queueMessage;
        }
    }
}
