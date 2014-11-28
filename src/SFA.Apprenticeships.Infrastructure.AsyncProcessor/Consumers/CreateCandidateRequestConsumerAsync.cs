namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Consumers
{
    using System;
    using System.Threading.Tasks;
    using Application.Candidate.Strategies;
    using Application.Interfaces.Messaging;
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
        private readonly ILegacyCandidateProvider _legacyCandidateProvider;
        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly IMessageBus _messageBus;

        public CreateCandidateRequestConsumerAsync(
            ICandidateReadRepository candidateReadRepository,
            IUserReadRepository userReadRepository,
            ILegacyCandidateProvider legacyCandidateProvider,
            ICandidateWriteRepository candidateWriteRepository,
            IMessageBus messageBus)
        {
            _candidateReadRepository = candidateReadRepository;
            _userReadRepository = userReadRepository;
            _legacyCandidateProvider = legacyCandidateProvider;
            _candidateWriteRepository = candidateWriteRepository;
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
            try
            {
                var user = _userReadRepository.Get(request.CandidateId);
                user.AssertState("User is in invalid state for creation in legacy", UserStatuses.Active);

                var candidate = _candidateReadRepository.Get(request.CandidateId, true);
                if (candidate.LegacyCandidateId == 0)
                {
                    var legacyCandidateId = _legacyCandidateProvider.CreateCandidate(candidate);
                    candidate.LegacyCandidateId = legacyCandidateId;
                    _candidateWriteRepository.Save(candidate);
                }
                else
                {
                    Logger.Warn("User has already been activated in legacy system: Candidate Id: \"{0}\"", request.CandidateId);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("Create candidate with id {0} request async process failed", request.CandidateId), ex);
                Requeue(request);
            }
        }

        private void Requeue(CreateCandidateRequest request)
        {
            var message = new CreateCandidateRequest
            {
                CandidateId = request.CandidateId
            };
            _messageBus.PublishMessage(message);
        }

        private static void Log(string narrative, CreateCandidateRequest request)
        {
            Logger.Debug("{0}: Candidate Id: \"{1}\"", narrative, request.CandidateId);
        }
    }
}
