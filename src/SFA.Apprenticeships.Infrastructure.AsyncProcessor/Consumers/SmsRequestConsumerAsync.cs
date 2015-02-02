namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Consumers
{
    using System;
    using System.Threading.Tasks;
    using Application.Interfaces.Communications;
    using EasyNetQ.AutoSubscribe;

    public class SmsRequestConsumerAsync : IConsumeAsync<SmsRequest>
    {
        private readonly ISmsDispatcher _dispatcher;

        public SmsRequestConsumerAsync(ISmsDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [SubscriptionConfiguration(PrefetchCount = 20)]
        [AutoSubscriberConsumer(SubscriptionId = "SmsRequestConsumerAsync")]
        public Task Consume(SmsRequest request)
        {
            return Task.Run(() => _dispatcher.SendSms(request));
        }
    }
}
