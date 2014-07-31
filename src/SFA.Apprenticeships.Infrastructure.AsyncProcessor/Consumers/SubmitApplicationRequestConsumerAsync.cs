namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Consumers
{
    using System.Threading.Tasks;
    using Application.Candidate.Strategies;
    using Application.Interfaces.Messaging;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;
    using EasyNetQ.AutoSubscribe;
    using NLog;

    public class SubmitApplicationRequestConsumerAsync : IConsumeAsync<SubmitApplicationRequest>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ILegacyApplicationProvider _legacyApplicationProvider;
        private readonly IApplicationReadRepository _applicationReadRepository;
        private readonly IApplicationWriteRepository _applicationWriteRepository;

        public SubmitApplicationRequestConsumerAsync(
            ILegacyApplicationProvider legacyApplicationProvider,
            IApplicationReadRepository applicationReadRepository,
            IApplicationWriteRepository applicationWriteRepository)
        {
            _legacyApplicationProvider = legacyApplicationProvider;
            _applicationReadRepository = applicationReadRepository;
            _applicationWriteRepository = applicationWriteRepository;
        }

        [AutoSubscriberConsumer(SubscriptionId = "SubmitApplicationRequestConsumerAsync")]
        public Task Consume(SubmitApplicationRequest request)
        {
            Log("Received", request);

            return Task.Run(() =>
            {
                Log("Submitting", request);

                CreateApplication(request);

                Log("Submitted", request);
            });
        }

        public void CreateApplication(SubmitApplicationRequest request)
        {
            var application = _applicationReadRepository.Get(request.ApplicationId);

            EnsureApplicationCanBeCreated(application);

            Log("Creating", request);

            application.LegacyApplicationId = _legacyApplicationProvider.CreateApplication(application);

            Log("Created", request);

            Log("Updating", request);

            application.SetStateSubmitted();
            _applicationWriteRepository.Save(application);

            Log("Updated", request);
        }

        private static void EnsureApplicationCanBeCreated(ApplicationDetail applicationDetail)
        {
            var message = string.Format("Cannot create application in state: \"{0}\".", applicationDetail.Status);

            applicationDetail.AssertState(message, ApplicationStatuses.Submitting);
        }

        private static void Log(string narrative, SubmitApplicationRequest request)
        {
            Logger.Debug("{0}: Application Id: \"{1}\"", narrative, request.ApplicationId);
        }
    }
}
