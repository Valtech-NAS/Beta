namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Consumers
{
    using System.Threading.Tasks;
    using Application.Candidate.Strategies;
    using Application.Interfaces.Messaging;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using EasyNetQ.AutoSubscribe;
    using NLog;

    public class SubmitApplicationRequestConsumerAsync : IConsumeAsync<SubmitApplicationRequest>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ILegacyApplicationProvider _legacyApplicationProvider;
        private readonly IApplicationReadRepository _applicationReadRepository;
        private readonly IApplicationWriteRepository _applicationWriteRepository;
        private readonly IMessageBus _messageBus;

        public SubmitApplicationRequestConsumerAsync(
            ILegacyApplicationProvider legacyApplicationProvider,
            IApplicationReadRepository applicationReadRepository,
            IApplicationWriteRepository applicationWriteRepository, 
            IMessageBus messageBus)
        {
            _legacyApplicationProvider = legacyApplicationProvider;
            _applicationReadRepository = applicationReadRepository;
            _applicationWriteRepository = applicationWriteRepository;
            _messageBus = messageBus;
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
            try
            {
                var application = _applicationReadRepository.Get(request.ApplicationId, true);

                // TODO: retrieve associated candidate and check the legacy candidate ID is already set. if not then log warning, requeue and exit

                EnsureApplicationCanBeCreated(application);

                Log("Creating", request);

                application.LegacyApplicationId = _legacyApplicationProvider.CreateApplication(application);

                Log("Created", request);

                Log("Updating", request);

                application.SetStateSubmitted();
                _applicationWriteRepository.Save(application);

                Log("Updated", request);
            }
            catch (CustomException ex)
            {
                if (ex.Code != ErrorCodes.ApplicationDuplicatedError)
                {
                    // re-queue application for submission
                    var message = new SubmitApplicationRequest
                    {
                        ApplicationId = request.ApplicationId
                    };
                    _messageBus.PublishMessage(message);
                }
            }
        }

        private static void EnsureApplicationCanBeCreated(ApplicationDetail applicationDetail)
        {
            var message = string.Format("Cannot create application with Id \"{0}\" for candidate with Id \"{1}\" in state: \"{2}\".",
                applicationDetail.EntityId, applicationDetail.CandidateId, applicationDetail.Status);

            applicationDetail.AssertState(message, ApplicationStatuses.Submitting);
        }

        private static void Log(string narrative, SubmitApplicationRequest request)
        {
            Logger.Debug("{0}: Application Id: \"{1}\"", narrative, request.ApplicationId);
        }
    }
}
