namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System.Collections.Generic;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Messaging;
    using Interfaces.Messaging;

    public class QueueEmailOnlyPasswordResetCodeStrategy : ISendPasswordResetCodeStrategy
    {
        private readonly IMessageBus _messageBus;

        public QueueEmailOnlyPasswordResetCodeStrategy(IMessageBus messageBus)
        {
            _messageBus = messageBus;
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

            _messageBus.PublishMessage(request);
        }
    }
}