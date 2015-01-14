namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using System.Collections.Generic;
    using Interfaces.Messaging;

    public class QueuePasswordResetCodeStrategy : ISendPasswordResetCodeStrategy
    {
        private readonly IQueueCommunicationRequestStrategy _queueCommunicationRequestStrategy;

        public QueuePasswordResetCodeStrategy(IQueueCommunicationRequestStrategy queueCommunicationRequestStrategy)
        {
            _queueCommunicationRequestStrategy = queueCommunicationRequestStrategy;
        }

        public void Send(Guid candidateId, IEnumerable<KeyValuePair<CommunicationTokens, string>> tokens)
        {
            _queueCommunicationRequestStrategy.Queue(candidateId, MessageTypes.SendPasswordResetCode, tokens);
        }
    }
}
