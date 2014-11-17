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

        [AutoSubscriberConsumer(SubscriptionId = "EmailRequestConsumerAsync")]
        public Task Consume(EmailRequest request)
        {
            Logger.Debug("Email request recieved from message bus, From:{0}, To:{1}, Subject:{2}", request.FromEmail, request.ToEmail, request.Subject);

            return Task.Run(() =>
            {
                try
                {
                    Logger.Debug("Sending email to dispatcher From:{0}, To:{1}, Subject:{2}, Template:{3}",
                        request.FromEmail, request.ToEmail, request.Subject, request.MessageType);
                    _dispatcher.SendEmail(request);
                    Logger.Debug("Sent email to dispatcher From:{0}, To:{1}, Subject:{2}, Template:{3}",
                        request.FromEmail, request.ToEmail, request.Subject, request.MessageType);
                }
                catch (Exception ex)
                {
                    var message =
                        string.Format(
                            "Error while sending email to dispatcher From:{0}, To:{1}, Subject:{2}, Template:{3}",
                            request.FromEmail, request.ToEmail, request.Subject, request.MessageType);
                    Logger.Error(message, ex);
                }
            });
        }
    }
}
