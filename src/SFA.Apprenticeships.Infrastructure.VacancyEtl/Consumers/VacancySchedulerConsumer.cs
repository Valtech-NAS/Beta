﻿namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System;
    using System.Threading.Tasks;
    using SFA.Apprenticeships.Application.Interfaces.Messaging;
    using SFA.Apprenticeships.Application.VacancyEtl;
    using SFA.Apprenticeships.Application.VacancyEtl.Entities;

    public class VacancySchedulerConsumer
    {
        private readonly static NLog.Logger Logger = NLog.LogManager.GetLogger(Constants.NamedLoggers.VacanyImporterLogger);

        private readonly IMessageService<StorageQueueMessage> _messageService;
        private readonly IVacancySummaryProcessor _vacancySummaryProcessor;

        public VacancySchedulerConsumer(IMessageService<StorageQueueMessage> messageService, IVacancySummaryProcessor vacancySummaryProcessor)
        {
            _messageService = messageService;
            _vacancySummaryProcessor = vacancySummaryProcessor;
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
                // TODO: Check Rabbit procesing queue - should not still be processing messages or there maybe a potential issue.
                Logger.Info("Scheduled VacancyEtl Message Received at: {0}", DateTime.Now);
                _vacancySummaryProcessor.QueueVacancyPages(latestScheduledMessage);
            }
        }

        private StorageQueueMessage GetLatestQueueMessage()
        {
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

                _messageService.DeleteMessage(queueMessage.MessageId);
                queueMessage = nextQueueMessage;
            }

            return queueMessage;
        }
    }
}
