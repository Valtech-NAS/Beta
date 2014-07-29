namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Messaging;
    using Interfaces.Messaging;

    public class QueueEmailOnlyPasswordChangedStrategy : ISendPasswordChangedStrategy
    {
        private readonly IMessageBus _bus;

        public QueueEmailOnlyPasswordChangedStrategy(IMessageBus bus)
        {
            _bus = bus;
        }

        public void Send(Candidate candidate, CandidateMessageTypes messageType,
            IEnumerable<KeyValuePair<CommunicationTokens, string>> tokens)
        {
            var request = new EmailRequest
            {
                ToEmail = candidate.RegistrationDetails.EmailAddress,
                MessageType = messageType,
                Tokens = tokens,
            };

            _bus.PublishMessage(request);
        }
    }
}