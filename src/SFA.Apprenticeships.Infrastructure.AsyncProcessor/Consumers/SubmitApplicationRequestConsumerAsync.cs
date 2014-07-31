namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Consumers
{
    using System.Threading.Tasks;
    using Application.Interfaces.Messaging;
    using EasyNetQ.AutoSubscribe;
    using NLog;

    public class SubmitApplicationRequestConsumerAsync : IConsumeAsync<SubmitApplicationRequest>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ISubmitApplicationRequestDispatcher _dispatcher;

        public SubmitApplicationRequestConsumerAsync(ISubmitApplicationRequestDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [AutoSubscriberConsumer(SubscriptionId = "SubmitApplicationRequestConsumerAsync")]
        public Task Consume(SubmitApplicationRequest request)
        {
            Log("Received", request);

            return Task.Run(() =>
            {
                Log("Submitting", request);

                _dispatcher.SubmitApplication(request);

                Log("Submitted", request);
            });
        }

        private static void Log(string narrative, SubmitApplicationRequest request)
        {
            Logger.Debug("{0}: Application Id: \"{1}\"", narrative, request.ApplicationId);
        }
    }
}
