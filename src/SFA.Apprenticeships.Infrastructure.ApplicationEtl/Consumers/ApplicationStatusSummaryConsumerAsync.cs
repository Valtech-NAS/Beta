namespace SFA.Apprenticeships.Infrastructure.ApplicationEtl.Consumers
{
    using System.Threading.Tasks;
    using Application.ApplicationUpdate;
    using Domain.Entities.Applications;
    using EasyNetQ.AutoSubscribe;

    public class ApplicationStatusSummaryConsumerAsync : IConsumeAsync<ApplicationStatusSummary>
    {
        private readonly IApplicationStatusProcessor _applicationStatusProcessor;

        public ApplicationStatusSummaryConsumerAsync(IApplicationStatusProcessor applicationStatusProcessor)
        {
            _applicationStatusProcessor = applicationStatusProcessor;
        }

        [AutoSubscriberConsumer(SubscriptionId = "ApplicationStatusSummaryConsumerAsync")]
        public Task Consume(ApplicationStatusSummary applicationStatusSummaryToProcess)
        {
            return Task.Run(() => _applicationStatusProcessor.ProcessApplicationStatuses(applicationStatusSummaryToProcess));
        }
    }
}
