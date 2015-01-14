namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SFA.Apprenticeships.Application.Interfaces.Messaging;
    using SFA.Apprenticeships.Domain.Interfaces.Messaging;
    using SFA.Apprenticeships.Domain.Interfaces.Repositories;

    public class QueueCommunicationRequestStrategy : IQueueCommunicationRequestStrategy
    {
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IMessageBus _messageBus;

        public QueueCommunicationRequestStrategy(ICandidateReadRepository candidateReadRepository, IMessageBus messageBus)
        {
            _candidateReadRepository = candidateReadRepository;
            _messageBus = messageBus;
        }

        public void Queue(Guid candidateId, MessageTypes messageType, IEnumerable<KeyValuePair<CommunicationTokens, string>> tokens)
        {
            var candidate = _candidateReadRepository.Get(candidateId);

            tokens = tokens.Union(new[]
                {
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.CandidateEmailAddress, candidate.RegistrationDetails.EmailAddress),
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.CandidateMobileNumber, candidate.RegistrationDetails.PhoneNumber) //TODO: change when we have the mobile number
                });

            var request = new CommunicationRequest
            {
                EntityId = candidateId,
                MessageType = messageType,
                Tokens = tokens
            };

            _messageBus.PublishMessage(request);
        }
    }
}