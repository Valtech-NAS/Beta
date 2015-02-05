namespace SFA.Apprenticeships.Infrastructure.Communications.Consumers
{
    using System.Threading.Tasks;
    using Application.Communications;
    using Application.Interfaces.Logging;
    using Azure.Common.Messaging;
    using Domain.Interfaces.Messaging;

    public class CommunicationsControlQueueConsumer : AzureControlQueueConsumer
    {
        private readonly ICommunicationProcessor _communicationProcessor;

        public CommunicationsControlQueueConsumer(IProcessControlQueue<StorageQueueMessage> messageService,
            ICommunicationProcessor communicationProcessor, ILogService logger)
            : base(messageService, logger, "Communications")
        {
            _communicationProcessor = communicationProcessor;
        }

        public Task CheckScheduleQueue()
        {
            return Task.Run(() =>
            {
                var scheduleerNotification = GetLatestQueueMessage();
                if (scheduleerNotification != null)
                {
                    _communicationProcessor.SendDailyDigests(scheduleerNotification.ClientRequestId);
                    MessageService.DeleteMessage(scheduleerNotification.MessageId, scheduleerNotification.PopReceipt);
                }
            });
        }
    }
}
