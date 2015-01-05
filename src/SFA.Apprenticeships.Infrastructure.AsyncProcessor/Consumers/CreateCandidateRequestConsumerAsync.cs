namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Consumers
{
    using System;
    using System.Threading.Tasks;
    using Application.Candidate;
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
            return Task.Run(() =>
            {
                if (request.ProcessTime.HasValue && request.ProcessTime > DateTime.Now)
                {
                    try
                    {
                        _messageBus.PublishMessage(request);
                        return;
                    }
                    catch
                    {
                        Logger.Error("Failed to re-queue deferred 'Create Candidate' request: {{ 'CandidateId': '{0}' }}", request.CandidateId);
                        throw;
                    }
                }

                Log("Creating", request);

                CreateCandidate(request);
            });
        }

        private void CreateCandidate(CreateCandidateRequest request)
        {
            try
            {
                var user = _userReadRepository.Get(request.CandidateId);
                user.AssertState("Create legacy user", UserStatuses.Active);

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
            request.ProcessTime = request.ProcessTime.HasValue ? DateTime.Now.AddMinutes(5) : DateTime.Now.AddSeconds(30);
            _messageBus.PublishMessage(request);
        }

        private static void Log(string narrative, CreateCandidateRequest request)
        {
            Logger.Debug("{0}: Candidate Id: \"{1}\"", narrative, request.CandidateId);
        }
    }
}
