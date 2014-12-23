namespace SFA.Apprenticeships.Infrastructure.ApplicationEtl.Consumers
{
    using System.Threading.Tasks;
    using Application.ApplicationUpdate;
    using Azure.Common.Messaging;
    using Domain.Interfaces.Messaging;

    public class ApplicationEtlControlQueueConsumer : AzureControlQueueConsumer
    {
        private readonly IApplicationStatusProcessor _applicationStatusProcessor;

        public ApplicationEtlControlQueueConsumer(IProcessControlQueue<StorageQueueMessage> messageService,
            IApplicationStatusProcessor applicationStatusProcessor) : base(messageService, "Application ETL")
        {
            _applicationStatusProcessor = applicationStatusProcessor;
        }

        public Task CheckScheduleQueue()
        {
            return Task.Run(() =>
            {
                var scheduleerNotification = GetLatestQueueMessage();
                if (scheduleerNotification != null)
                {
                    _applicationStatusProcessor.QueueApplicationStatusesPages();
                    _messageService.DeleteMessage(scheduleerNotification.MessageId, scheduleerNotification.PopReceipt);
                }
            });
        }
    }
}
