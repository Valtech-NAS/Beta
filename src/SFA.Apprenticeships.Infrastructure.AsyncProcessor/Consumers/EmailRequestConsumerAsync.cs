namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Consumers
{
    using System;
    using System.Threading.Tasks;
    using Application.Interfaces.Messaging;
    using EasyNetQ.AutoSubscribe;
    using NLog;

    public class EmailRequestConsumerAsync : IConsumeAsync<EmailRequest>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IEmailDispatcher _dispatcher;

        public EmailRequestConsumerAsync(IEmailDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [SubscriptionConfiguration(PrefetchCount = 20)]
        [AutoSubscriberConsumer(SubscriptionId = "EmailRequestConsumerAsync")]
        public Task Consume(EmailRequest request)
        {
            return Task.Run(() =>
            {
                try
                {
                    Logger.Debug("Sending email to dispatcher To:{0}, Template:{1}",
                        request.ToEmail, request.MessageType);
                    _dispatcher.SendEmail(request);
                }
                catch (Exception ex)
                {
                    Logger.Error("Error sending email", ex);
                }
            });
        }
    }
}
