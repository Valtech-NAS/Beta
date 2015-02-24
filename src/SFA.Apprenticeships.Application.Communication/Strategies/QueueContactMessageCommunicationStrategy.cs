namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using System.Collections.Generic;
    using Interfaces.Communications;
    using Domain.Interfaces.Messaging;
    
    public class QueueContactMessageCommunicationStrategy : ISendContactMessageStrategy
    {
        private readonly IMessageBus _messageBus;

        public QueueContactMessageCommunicationStrategy(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        public void Send(Guid? candidateId, MessageTypes messageType, IEnumerable<CommunicationToken> tokens)
        {
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
