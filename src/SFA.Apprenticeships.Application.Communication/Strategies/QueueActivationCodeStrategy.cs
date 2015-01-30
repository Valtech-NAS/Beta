namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using System.Collections.Generic;
    using Interfaces.Messaging;

    public class QueueActivationCodeStrategy : ISendActivationCodeStrategy
    {
        private readonly IQueueCommunicationRequestStrategy _queueCommunicationRequestStrategy;

        public QueueActivationCodeStrategy(IQueueCommunicationRequestStrategy queueCommunicationRequestStrategy)
        {
            _queueCommunicationRequestStrategy = queueCommunicationRequestStrategy;
        }

        public void Send(Guid candidateId, IEnumerable<CommunicationToken> tokens)
        {
            _queueCommunicationRequestStrategy.Queue(candidateId, MessageTypes.SendActivationCode, tokens);
        }
    }
}
