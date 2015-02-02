namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Consumers
{
    using System;
    using System.Threading.Tasks;
    using Application.Interfaces.Communications;
    using EasyNetQ.AutoSubscribe;

    public class EmailRequestConsumerAsync : IConsumeAsync<EmailRequest>
    {
        private readonly IEmailDispatcher _dispatcher;

        public EmailRequestConsumerAsync(IEmailDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [SubscriptionConfiguration(PrefetchCount = 20)]
        [AutoSubscriberConsumer(SubscriptionId = "EmailRequestConsumerAsync")]
        public Task Consume(EmailRequest request)
        {
            return Task.Run(() => _dispatcher.SendEmail(request));
        }
    }
}
