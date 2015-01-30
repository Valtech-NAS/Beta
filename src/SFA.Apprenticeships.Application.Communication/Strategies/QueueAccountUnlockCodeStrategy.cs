namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using System.Collections.Generic;
    using Interfaces.Messaging;

    public class QueueAccountUnlockCodeStrategy : ISendAccountUnlockCodeStrategy
    {
        private readonly IQueueCommunicationRequestStrategy _queueCommunicationRequestStrategy;

        public QueueAccountUnlockCodeStrategy(IQueueCommunicationRequestStrategy queueCommunicationRequestStrategy)
        {
            _queueCommunicationRequestStrategy = queueCommunicationRequestStrategy;
        }

        public void Send(Guid candidateId, IEnumerable<CommunicationToken> tokens)
        {
            _queueCommunicationRequestStrategy.Queue(candidateId, MessageTypes.SendAccountUnlockCode, tokens);
        }
    }
}
