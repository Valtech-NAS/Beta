namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using System.Collections.Generic;
    using Interfaces.Messaging;

    public interface ISendActivationCodeStrategy
    {
        //todo: remove this interface as all comm event strategies now use same signature
        void Send(Guid candidateId, IEnumerable<KeyValuePair<CommunicationTokens, string>> tokens);
    }
}