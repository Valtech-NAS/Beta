namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Consumers
{
    using System.Threading.Tasks;
    using Application.Interfaces.Messaging;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using EasyNetQ.AutoSubscribe;
    using NLog;

    public class CreateCandidateRequestConsumerAsync : IConsumeAsync<CreateCandidateRequest>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IMessageBus _messageBus;

        public CreateCandidateRequestConsumerAsync(
            ICandidateReadRepository candidateReadRepository,
            IUserReadRepository userReadRepository,
            IMessageBus messageBus)
        {
            _candidateReadRepository = candidateReadRepository;
            _userReadRepository = userReadRepository;
            _messageBus = messageBus;
        }

        [AutoSubscriberConsumer(SubscriptionId = "CreateCandidateRequestConsumerAsync")]
        public Task Consume(CreateCandidateRequest request)
        {
            Log("Received", request);

            return Task.Run(() => CreateCandidate(request));
        }

        private void CreateCandidate(CreateCandidateRequest request)
        {
            //try
            //{
                //todo: user account status check (should be active)
                var user = _userReadRepository.Get(request.CandidateId);
                user.AssertState("User is in invalid state for creation in legacy", UserStatuses.Active);

                // TODO: check legacy id not already set, debug log and bail out if so
                var candidate = _candidateReadRepository.Get(request.CandidateId, true);

                // TODO: invoke candidate creation on nas gateway
                //var legacyCandidateId = _legacyCandidateProvider.CreateCandidate(candidate);

                // TODO: update candidate
                //candidate.LegacyCandidateId = legacyCandidateId;
                //_candidateWriteRepository.Save(candidate);
            //{
            catch (CustomException)
                // TODO: think about which exceptions should result in a re-queue
                //if (ex.Code != ErrorCodes.ApplicationDuplicatedError)
                {
                    // re-queue
                    var message = new CreateCandidateRequest
                    {
                        CandidateId = request.CandidateId
                    };
                    _messageBus.PublishMessage(message);
                }
            //}
        }

        private static void Log(string narrative, CreateCandidateRequest request)
        {
            Logger.Debug("{0}: Candidate Id: \"{1}\"", narrative, request.CandidateId);
        }
    }
}
