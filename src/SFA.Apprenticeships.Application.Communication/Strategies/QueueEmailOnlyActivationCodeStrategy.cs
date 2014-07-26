namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System.Collections.Generic;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Messaging;
    using Interfaces.Messaging;

    public class QueueEmailOnlyActivationCodeStrategy : ISendActivationCodeStrategy
    {
        private readonly IMessageBus _bus;

        public QueueEmailOnlyActivationCodeStrategy(IMessageBus bus)
        {
            _bus = bus;
        }

        public void Send(Candidate candidate, CandidateMessageTypes messageType, IEnumerable<KeyValuePair<CommunicationTokens, string>> tokens)
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