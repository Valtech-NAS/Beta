namespace SFA.Apprenticeships.Infrastructure.ApplicationEtl.Consumers
{
    using System.Threading.Tasks;
    using Application.ApplicationUpdate;
    using Application.ApplicationUpdate.Entities;
    using EasyNetQ.AutoSubscribe;

    public class ApplicationStatusSummaryPageConsumerAsync : IConsumeAsync<ApplicationUpdatePage>
    {
        private readonly IApplicationStatusProcessor _applicationStatusProcessor;

        public ApplicationStatusSummaryPageConsumerAsync(IApplicationStatusProcessor applicationStatusProcessor)
        {
            _applicationStatusProcessor = applicationStatusProcessor;
        }

        [AutoSubscriberConsumer(SubscriptionId = "ApplicationStatusSummaryPageConsumerAsync")]
        public Task Consume(ApplicationUpdatePage message)
        {
            return Task.Run(() => _applicationStatusProcessor.QueueApplicationStatuses(message));
        }
    }
}
