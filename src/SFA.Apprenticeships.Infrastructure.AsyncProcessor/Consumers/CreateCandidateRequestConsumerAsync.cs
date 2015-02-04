namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Consumers
{
    using System;
    using System.Threading.Tasks;
    using Application.Candidate;
    using Application.Interfaces.Logging;
    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using EasyNetQ.AutoSubscribe;

    public class CreateCandidateRequestConsumerAsync : IConsumeAsync<CreateCandidateRequest>
    {
        private readonly ILogService _logger;
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
            IMessageBus messageBus, ILogService logger)
        {
            _candidateReadRepository = candidateReadRepository;
            _userReadRepository = userReadRepository;
            _legacyCandidateProvider = legacyCandidateProvider;
            _candidateWriteRepository = candidateWriteRepository;
            _messageBus = messageBus;
            _logger = logger;
        }

        [SubscriptionConfiguration(PrefetchCount = 2)]
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
                        _logger.Error("Failed to re-queue deferred 'Create Candidate' request: {{ 'CandidateId': '{0}' }}", request.CandidateId);
                        throw;
                    }
                }

                CreateCandidate(request);
            });
        }

        private void CreateCandidate(CreateCandidateRequest request)
        {
            try
            {
                _logger.Debug("Creating candidate Id: {0}", request.CandidateId);

                var user = _userReadRepository.Get(request.CandidateId);
                user.AssertState("Create legacy user", UserStatuses.Active, UserStatuses.Locked);

                var candidate = _candidateReadRepository.Get(request.CandidateId, true);
                if (candidate.LegacyCandidateId == 0)
                {
                    _logger.Info("Sending request to create candidate in legacy system: Candidate Id: \"{0}\"", request.CandidateId);
                    var legacyCandidateId = _legacyCandidateProvider.CreateCandidate(candidate);
                    candidate.LegacyCandidateId = legacyCandidateId;
                    _candidateWriteRepository.Save(candidate);
                    _logger.Info("Candidate created in legacy system: Candidate Id: \"{0}\", Legacy Candidate Id: \"{1}\"", request.CandidateId);
                }
                else
                {
                    _logger.Warn("User has already been activated in legacy system: Candidate Id: \"{0}\"", request.CandidateId);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Create candidate with id {0} request async process failed", request.CandidateId), ex);
                Requeue(request);
            }
        }

        private void Requeue(CreateCandidateRequest request)
        {
            request.ProcessTime = request.ProcessTime.HasValue ? DateTime.Now.AddMinutes(5) : DateTime.Now.AddSeconds(30);
            _messageBus.PublishMessage(request);
        }
    }
}
