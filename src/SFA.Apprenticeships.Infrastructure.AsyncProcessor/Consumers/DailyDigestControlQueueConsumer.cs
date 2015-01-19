namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Consumers
{
    using System.Threading.Tasks;
    using Application.Interfaces.Messaging;
    using Azure.Common.Messaging;
    using Domain.Interfaces.Messaging;

    public class DailyDigestControlQueueConsumer : AzureControlQueueConsumer
    {
        private readonly ICommunicationProcessor _dailyDigestProcessor;

        public DailyDigestControlQueueConsumer(IProcessControlQueue<StorageQueueMessage> messageService,
            ICommunicationProcessor dailyDigestProcessor) : base(messageService, "Daily Digest")
        {
            _dailyDigestProcessor = dailyDigestProcessor;
        }

        public Task CheckScheduleQueue()
        {
            return Task.Run(() =>
            {
                var scheduleerNotification = GetLatestQueueMessage();
                if (scheduleerNotification != null)
                {
                    _dailyDigestProcessor.SendDailyDigests();
                    _messageService.DeleteMessage(scheduleerNotification.MessageId, scheduleerNotification.PopReceipt);
                }
            });
        }
    }
}
