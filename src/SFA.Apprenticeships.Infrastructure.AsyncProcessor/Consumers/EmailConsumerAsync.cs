namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Consumers
{
    using System.Threading.Tasks;
    using Application.Interfaces.Messaging;
    using EasyNetQ.AutoSubscribe;

    public class EmailConsumerAsync : IConsumeAsync<EmailRequest>
    {
        private readonly IEmailDispatcher _dispatcher;

        public EmailConsumerAsync(IEmailDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [AutoSubscriberConsumer(SubscriptionId = "EmailConsumerAsync")]
        public Task Consume(EmailRequest request)
        {
            return Task.Run(() => _dispatcher.SendEmail(request));
        }
    }
}
