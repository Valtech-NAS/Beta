namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using System.Collections.Generic;
    using Interfaces.Messaging;

    public class QueuePasswordChangedStrategy : ISendPasswordChangedStrategy
    {
        private readonly IQueueCommunicationRequestStrategy _queueCommunicationRequestStrategy;

        public QueuePasswordChangedStrategy(IQueueCommunicationRequestStrategy queueCommunicationRequestStrategy)
        {
            _queueCommunicationRequestStrategy = queueCommunicationRequestStrategy;
        }

        public void Send(Guid candidateId, IEnumerable<KeyValuePair<CommunicationTokens, string>> tokens)
        {
            _queueCommunicationRequestStrategy.Queue(candidateId, MessageTypes.PasswordChanged, tokens);
        }
    }
}
