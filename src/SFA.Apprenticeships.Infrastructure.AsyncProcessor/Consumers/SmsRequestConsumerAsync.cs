namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Consumers
{
    using System;
    using System.Threading.Tasks;
    using Application.Interfaces.Messaging;
    using EasyNetQ.AutoSubscribe;
    using NLog;

    public class SmsRequestConsumerAsync : IConsumeAsync<SmsRequest>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ISmsDispatcher _dispatcher;

        public SmsRequestConsumerAsync(ISmsDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [AutoSubscriberConsumer(SubscriptionId = "SmsRequestConsumerAsync")]
        public Task Consume(SmsRequest request)
        {
            return Task.Run(() =>
            {
                try
                {
                    Logger.Debug("Sending sms to dispatcher To:{0}, MessageType:{1}",
                        request.ToNumber, request.MessageType);
                    _dispatcher.SendSms(request);
                }
                catch (Exception ex)
                {
                    Logger.Error("Error sending sms", ex);
                }
            });
        }
    }
}
